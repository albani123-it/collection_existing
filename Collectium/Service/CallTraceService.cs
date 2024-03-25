using AutoMapper;
using ClosedXML.Excel;
using Collectium.Model;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Model.Helper;
using Collectium.Validation;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Action = Collectium.Model.Entity.Action;

namespace Collectium.Service
{
    public class CallTraceService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<ActionGroupService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CallTraceService(CollectiumDBContext ctx,
                                ILogger<ActionGroupService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                ToolService toolService,
                                IMapper mapper,
                                IHttpContextAccessor httpContextAccessor)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.pagination = pagination;
            this.statusService = statusService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.toolService = toolService;
        }

        public GenericResponse<PayRecordResponseBean> ListPayRecord(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<PayRecordResponseBean>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<PaymentRecord> q = this.ctx.Set<PaymentRecord>().Include(i => i.CallBy)
                                                .Include(i => i.Call)
                                                    .ThenInclude(i => i!.Loan!)
                                                        .ThenInclude(i => i.Customer!);

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Call!.Loan!.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccNo != null && filter.AccNo!.Length > 0)
            {
                q = q.Where(q => q.AccNo!.Contains(filter.AccNo));
            }

            q = q.Where(q => q.RecordDate!.Value.Date >= dst && q.RecordDate!.Value.Date <= ded);

            q = q.OrderByDescending(q => q.Id);

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<PayRecordResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<PayRecordResponseBean>(it);
                var xspv = this.getMySPV(it.CallBy);
                if (xspv != null)
                {
                    var spv = mapper.Map<UserTraceResponseBean>(xspv);
                    dto.Spv = spv;
                }
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollTraceResponseBean> ListCallTrace(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<CollTraceResponseBean>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<CollectionTrace> q = this.ctx.Set<CollectionTrace>().Include(i => i.CallBy)
                                                .Include(i => i.Result)
                                                .Include(i => i.Call)
                                                    .ThenInclude(i => i!.Loan!)
                                                        .ThenInclude(i => i.Customer!);

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Call!.Loan!.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccNo != null && filter.AccNo!.Length > 0)
            {
                q = q.Where(q => q.AccNo!.Contains(filter.AccNo));
            }

            if (filter.DPDmin != null && filter.DPDmin > 0)
            {
                q = q.Where(o => o.DPD >= filter.DPDmin);

                if (filter.DPDmax != null && filter.DPDmax > 0)
                {
                    q = q.Where(o => o.DPD <= filter.DPDmax);
                }
            }

            if (filter.DPDmin > 0 && filter.DPDmax > 0)
            {
                q = q.Where(o => o.DPD >= filter.DPDmin).Where(o => o.DPD <= filter.DPDmax);
            }

            if (filter.BranchId != null)
            {
                q = q.Where(o => o.Call!.BranchId == filter.BranchId);
            }

            if (filter.AgentId != null)
            {
                q = q.Where(o => o.CallById == filter.AgentId);
            }

            q = q.Where(q => q.TraceDate!.Value.Date >= dst && q.TraceDate!.Value.Date <= ded);

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<CollTraceResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CollTraceResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public IActionResult ListCallTracePrint(ExportTraceBean filter)
        {
            var wrap = new GenericResponse<CollTraceResponseBean>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                return new BadRequestResult();
            }

            var dst = DateTime.Parse(filter.StartDate!);
            var ded = DateTime.Parse(filter.EndDate!);

            IQueryable<CollectionTrace> q = this.ctx.Set<CollectionTrace>().Include(i => i.CallBy)
                                                .Include(i => i.Result)
                                                .Include(i => i.Call)
                                                    .ThenInclude(i => i!.Loan!)
                                                        .ThenInclude(i => i.Customer!);
            q = q.Where(q => q.TraceDate!.Value.Date >= dst && q.TraceDate!.Value.Date <= ded);

            var data = q.ToList();

            var ldata = new List<CollTraceResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CollTraceResponseBean>(it);
                ldata.Add(dto);
            }

            var returnStream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {

                var wsm = "Histori Distribusi";
                var worksheet = workbook.Worksheets.Add(wsm);
                //worksheet.Range("A2:H3").Row(1).Merge();
                worksheet.Cell("A2").Value = "Account No";
                worksheet.Cell("B2").Value = "Debitur";
                worksheet.Cell("C2").Value = "Amount";
                worksheet.Cell("D2").Value = "Tanggal";
                worksheet.Cell("E2").Value = "Kolek";
                worksheet.Cell("F2").Value = "DPD";
                worksheet.Cell("G2").Value = "CallBy";
                worksheet.Cell("H2").Value = "Result";

                this.DrawTable(worksheet, "A2:H2");

                int i = 3;
                foreach (var x in ldata)
                {
                    worksheet.Cell("A" + i).Value = x.AccNo;
                    worksheet.Cell("B" + i).Value = x.Name;
                    worksheet.Cell("C" + i).Value = x.Amount;
                    worksheet.Cell("D" + i).Value = x.TraceDate;
                    worksheet.Cell("E" + i).Value = x.Kolek;
                    worksheet.Cell("F" + i).Value = x.DPD;
                    if (x.CallBy != null)
                    {
                        worksheet.Cell("G" + i).Value = x.CallBy!.Username;
                    } else
                    {
                        worksheet.Cell("G" + i).Value = "";
                    }

                    worksheet.Cell("H" + i).Value = x.Result!.Code;

                    this.DrawTable(worksheet, "A" + i + ":H" + i);
                    i++;
                }


                workbook.SaveAs(returnStream);
            }

            returnStream.Position = 0;
            returnStream.Flush();

            //template.SaveAs(returnStream);

            return new FileStreamResult(returnStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public IActionResult ListCallTraceRecordPrint(ExportTraceBean filter)
        {
            var wrap = new GenericResponse<PayRecordResponseBean>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                return new BadRequestResult();
            }

            var dst = DateTime.Parse(filter.StartDate!);
            var ded = DateTime.Parse(filter.EndDate!);

            IQueryable<PaymentRecord> q = this.ctx.Set<PaymentRecord>().Include(i => i.CallBy)
                                                .Include(i => i.Call)
                                                    .ThenInclude(i => i!.Loan!)
                                                        .ThenInclude(i => i.Customer!);
            q = q.Where(q => q.RecordDate!.Value.Date >= dst && q.RecordDate!.Value.Date <= ded);

            var data = q.ToList();

            var dict = new Dictionary<int, UserTraceResponseBean>();
            var ldata = new List<PayRecordResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<PayRecordResponseBean>(it);
                if (it.CallById != null)
                {
                    if (dict.ContainsKey(it.CallById!.Value) == false)
                    {
                        var myspv = this.getMySPV(it.CallBy);
                        var sp = mapper.Map<UserTraceResponseBean>(myspv);
                        dto.Spv = sp;

                        dict.Add(it.CallById.Value, sp);
                    }
                    else
                    {
                        var sp = dict[it.CallById!.Value];
                        dto.Spv = sp;
                    }
                }


                ldata.Add(dto);
            }

            var returnStream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {

                var wsm = "Histori Pembayaran";
                var worksheet = workbook.Worksheets.Add(wsm);
               // worksheet.Range("A2:D3").Row(1).Merge();
                worksheet.Cell("A2").Value = "Account No";
                worksheet.Cell("B2").Value = "Debitur";
                worksheet.Cell("C2").Value = "Tanggal";
                worksheet.Cell("D2").Value = "Amount";
                worksheet.Cell("E2").Value = "Officer";
                worksheet.Cell("F2").Value = "Spv";

                this.DrawTable(worksheet, "A3:F3");

                int i = 3;
                foreach (var x in ldata)
                {
                    if (x.Name == null || x.Name.Length < 1)
                    {
                        continue;
                    }
                    worksheet.Cell("A" + i).Value = x.AccNo;
                    worksheet.Cell("B" + i).Value = x.Name;
                    worksheet.Cell("C" + i).Value = x.RecordDate;
                    worksheet.Cell("D" + i).Value = x.Amount;
                    if (x.CallBy != null)
                    {
                        worksheet.Cell("E" + i).Value = x.CallBy!.Username;
                    }
                    else
                    {
                        worksheet.Cell("E" + i).Value = "";
                    }

                    if (x.Spv != null)
                    {
                        worksheet.Cell("F" + i).Value = x.Spv!.Username;
                    }
                    else
                    {
                        worksheet.Cell("F" + i).Value = "";
                    }
                    this.DrawTable(worksheet, "A" + i + ":F" + i);
                    i++;
                }


                workbook.SaveAs(returnStream);
            }

            returnStream.Position = 0;
            returnStream.Flush();

            //template.SaveAs(returnStream);

            return new FileStreamResult(returnStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public GenericResponse<AddressLatLonResponseBean> ListAddress(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<AddressLatLonResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<CollectionAddContact> q = this.ctx.Set<CollectionAddContact>()
                .Where(q => q.Lat != null)
                .Where(q => q.Lon != null)
                .OrderByDescending(q => q.Id);
            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<AddressLatLonResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<AddressLatLonResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public IActionResult AddressPrint(ExportTraceBean filter)
        {
            var wrap = new GenericResponse<AddressLatLonResponseBean>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                return new BadRequestResult();
            }

            var dst = DateTime.Parse(filter.StartDate!);
            var ded = DateTime.Parse(filter.EndDate!);

            IQueryable<CollectionAddContact> q = this.ctx.Set<CollectionAddContact>().Where(q => q.Lat != null).Where(q => q.Lon != null);

            var data = q.ToList();

            var ldata = new List<AddressLatLonResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<AddressLatLonResponseBean>(it);
                ldata.Add(dto);
            }

            var returnStream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {

                var wsm = "Data Geolocation";
                var worksheet = workbook.Worksheets.Add(wsm);
                // worksheet.Range("A2:D3").Row(1).Merge();
                worksheet.Cell("A2").Value = "Account No";
                worksheet.Cell("B2").Value = "CIF";
                worksheet.Cell("C2").Value = "Latitude";
                worksheet.Cell("D2").Value = "Longitude";

                this.DrawTable(worksheet, "A3:D3");

                int i = 3;
                foreach (var x in ldata)
                {
                    worksheet.Cell("A" + i).Value = x.AccNo;
                    worksheet.Cell("B" + i).Value = x.CuCif;
                    worksheet.Cell("C" + i).Value = x.Lat;
                    worksheet.Cell("D" + i).Value = x.Lon;

                    this.DrawTable(worksheet, "A" + i + ":D" + i);
                    i++;
                }


                workbook.SaveAs(returnStream);
            }

            returnStream.Position = 0;
            returnStream.Flush();

            //template.SaveAs(returnStream);

            return new FileStreamResult(returnStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public GenericResponse<NewDailyResponse> ListNewDaily(SpvMonListBean filter)
        {
            var wrap = new GenericResponse<NewDailyResponse>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);

            IQueryable<MasterLoanHistory> q = this.ctx.Set<MasterLoanHistory>().Include(i => i.Customer!).Include(i => i.Loan).Include(i => i.CallBy);

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccNo != null && filter.AccNo!.Length > 0)
            {
                q = q.Where(q => q.AccNo!.Contains(filter.AccNo));
            }



            q = q.Where(q => q.Dpd ==1).Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded).OrderByDescending(q => q.Id);

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<NewDailyResponse>();
            foreach (var it in data)
            {
                var dto = mapper.Map<NewDailyResponse>(it);

                dto.Name = it.Customer!.Name;

                var cb = mapper.Map<UserTraceResponseBean>(it.CallBy);
                dto.CallBy = cb;

                var xspv = this.getMySPV(it.CallBy);
                if (xspv != null)
                {
                    var spv = mapper.Map<UserTraceResponseBean>(xspv);
                    dto.Spv = spv;
                }

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public IActionResult NewDailyPrint(ExportTraceBean filter)
        {
            var wrap = new GenericResponse<AddressLatLonResponseBean>
            {
                Status = false,
                Message = ""
            };

            if (filter.StartDate == null || filter.EndDate == null)
            {
                return new BadRequestResult();
            }

            var dst = DateTime.Parse(filter.StartDate!);
            var ded = DateTime.Parse(filter.EndDate!);

            IQueryable<MasterLoanHistory> q = this.ctx.Set<MasterLoanHistory>().Where(q => q.Dpd == 1);
            q = q.Where(q => q.STG_DATE!.Value.Date >= dst && q.STG_DATE!.Value.Date <= ded);

            var data = q.ToList();

            var dict = new Dictionary<int, UserTraceResponseBean>();
            var ldata = new List<NewDailyResponse>();
            foreach (var it in data)
            {
                var dto = mapper.Map<NewDailyResponse>(it);

                dto.Name = it.Customer!.Name;

                var cb = mapper.Map<UserTraceResponseBean>(it.CallBy);
                dto.CallBy = cb;


                if (it.CallById != null)
                {
                    if (dict.ContainsKey(it.CallById!.Value) == false)
                    {
                        var myspv = this.getMySPV(it.CallBy);
                        var sp = mapper.Map<UserTraceResponseBean>(myspv);
                        dto.Spv = sp;

                        dict.Add(it.CallById.Value, sp);
                    }
                    else
                    {
                        var sp = dict[it.CallById!.Value];
                        dto.Spv = sp;
                    }
                }

                ldata.Add(dto);
            }

            var returnStream = new MemoryStream();
            using (var workbook = new XLWorkbook())
            {

                var wsm = "Data Nasabah Baru";
                var worksheet = workbook.Worksheets.Add(wsm);
                // worksheet.Range("A2:D3").Row(1).Merge();
                worksheet.Cell("A2").Value = "Tanggal";
                worksheet.Cell("B2").Value = "AccNo";
                worksheet.Cell("C2").Value = "Dpd";
                worksheet.Cell("D2").Value = "Tunggakan Total";
                worksheet.Cell("E2").Value = "Officer";
                worksheet.Cell("F2").Value = "SPV";

                this.DrawTable(worksheet, "A3:D3");

                int i = 3;
                foreach (var x in ldata)
                {
                    worksheet.Cell("A" + i).Value = x.STG_DATE;
                    worksheet.Cell("B" + i).Value = x.AccNo;
                    worksheet.Cell("C" + i).Value = x.Dpd;
                    worksheet.Cell("D" + i).Value = x.TunggakanTotal;
                    worksheet.Cell("E" + i).Value = x.CallBy.Name;
                    worksheet.Cell("F" + i).Value = x.Spv.Name;

                    this.DrawTable(worksheet, "A" + i + ":F" + i);
                    i++;
                }


                workbook.SaveAs(returnStream);
            }

            returnStream.Position = 0;
            returnStream.Flush();

            //template.SaveAs(returnStream);

            return new FileStreamResult(returnStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        private User getMySPV(User me)
        {
            if (me == null)
            {
                return null;
            }

            var tm = this.ctx.TeamMember.Where(q => q.MemberId == me.Id).Include(i => i.Team).ThenInclude(i => i.Spv).ToList();
            if (tm == null || tm.Count < 1)
            {
                return null;
            }

            var a = tm[0];
            var b = a.Team;
            var c = b.Spv;
            return c;
        }
        private void DrawTable(IXLWorksheet ws, string range)
        {
            ws.Range(range).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Range(range).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws.Range(range).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(range).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Range(range).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ws.Range(range).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        }

        public GenericResponse<UserResponseBean> ListTeamAll()
        {
            var wrap = new GenericResponse<UserResponseBean>
            {
                Status = false,
                Message = ""
            };

            var ld = new List<UserResponseBean>();
            var bid = new List<int?>();
            bid.Add(3);
            bid.Add(4);
            var ls = this.ctx.User.Where(q => bid.Contains(q.Role!.Id))
                                        .Where(q => q.Status!.Name!.Equals("AKTIF"))
                                        .ToList();
            if (ls != null && ls.Count > 0)
            {
                foreach (var o in ls)
                {
                    var n = this.mapper.Map<UserResponseBean>(o);
                    ld.Add(n);
                }

            }

            wrap.Data = ld;

            wrap.Status = true;

            return wrap;
        }
    }
}
