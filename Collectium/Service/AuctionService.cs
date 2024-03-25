using AutoMapper;
using Collectium.Model.Helper;
using Collectium.Model;
using Microsoft.EntityFrameworkCore;
using Collectium.Model.Entity;
using System.Globalization;
using Collectium.Model.Bean.Request;
using System.Diagnostics.Metrics;
using DocumentFormat.OpenXml.Office.CustomUI;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Response;
using Collectium.Model.Bean;
using static Collectium.Model.Bean.ListRequest.RecoveryListRequest;
using static Collectium.Model.Bean.Response.RestructureResponse;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;

namespace Collectium.Service
{
    public class AuctionService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<AuctionService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration conf;

        public AuctionService(CollectiumDBContext ctx,
                                ILogger<AuctionService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                IMapper mapper,
                                IConfiguration conf,
                                IHttpContextAccessor httpContextAccessor)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.pagination = pagination;
            this.statusService = statusService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.conf = conf;
        }

        public GenericResponse<RestructureListResponseBean> ListMonitoring(RestructureListBean filter)
        {
            var wrap = new GenericResponse<RestructureListResponseBean>
            {
                Status = false,
                Message = ""
            };

            /*
            if (filter.StartDate == null || filter.EndDate == null)
            {
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);
            */

            IQueryable<Auction> q = this.ctx.Set<Auction>().Include(i => i.Status).Include(i => i.Loan).ThenInclude(i => i!.Customer)
                                            .Include(i => i.Branch).Include(i => i.BranchPembukuan)
                                            .Include(i => i.BranchProses);

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Loan!.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccNo != null && filter.AccNo!.Length > 0)
            {
                q = q.Where(q => q.Loan!.AccNo!.Contains(filter.AccNo));
            }

            if (filter.BranchId != null)
            {
                q = q.Where(o => o.BranchId == filter.BranchId);
            }


            var cnt = q.Count();
            wrap.DataCount = cnt;

            q = q.OrderByDescending(q => q.Id);
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<RestructureListResponseBean>();
            foreach (var it in data)
            {
                var dto = new RestructureListResponseBean();
                dto.Id = it.Id;
                dto.AccoNo = it.Loan?.AccNo;
                dto.Cif = it.Loan?.Cif;
                dto.BranchName = it.Branch?.Name;
                dto.Name = it.Loan?.Customer?.Name;
                dto.BranchPembukuanName = it.BranchPembukuan?.Name;
                dto.BranchProsesName = it.BranchProses?.Name;

                var sb = new StatusGeneralBean();
                sb.Id = it.Status?.Id;
                sb.Name = it.Status?.Name;
                dto.Status = sb;

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<RestructureListResponseBean> ListApprove(RestructureListBean filter)
        {
            var wrap = new GenericResponse<RestructureListResponseBean>
            {
                Status = false,
                Message = ""
            };

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            /*
            if (filter.StartDate == null || filter.EndDate == null)
            {
                return wrap;
            }

            var dst = ((DateTime)filter.StartDate);
            var ded = ((DateTime)filter.EndDate).AddHours(23).AddMinutes(59);
            */

            this.ctx.Entry(reqUser).Collection(r => r.Branch!).Load();

            var myBranch = new List<int>();
            foreach (var item in reqUser.Branch!)
            {
                myBranch.Add(item.Id!.Value);
            }

            IQueryable<Auction> q = this.ctx.Set<Auction>().Include(i => i.Status).Include(i => i.Loan).ThenInclude(i => i!.Customer)
                                            .Include(i => i.Branch).Include(i => i.BranchPembukuan)
                                            .Include(i => i.BranchProses).Include(i => i.AlasanLelang);

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Loan!.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccNo != null && filter.AccNo!.Length > 0)
            {
                q = q.Where(q => q.Loan!.AccNo!.Contains(filter.AccNo));
            }

            if (filter.BranchId != null)
            {
                q = q.Where(o => o.BranchId == filter.BranchId);
            }

            if (reqUser.Role?.Name == "ADMIN")
            {
                q = q.Where(o => o.Status!.Name == "DRAFT");
            }

            if (reqUser.Role?.Name == "ADMIN2")
            {
                q = q.Where(o => o.Status!.Name == "PENGAJUAN");
            }

            if (reqUser.Role?.Name == "MANAJEMEN")
            {
                q = q.Where(o => o.Status!.Name == "VERIFIKASI");
            }

            //q = q.Where(o => myBranch.Contains(o.BranchId!.Value));

            var cnt = q.Count();
            wrap.DataCount = cnt;

            q = q.OrderByDescending(q => q.Id);
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<RestructureListResponseBean>();
            foreach (var it in data)
            {
                var dto = new RestructureListResponseBean();
                dto.Id = it.Id;
                dto.AccoNo = it.Loan?.AccNo;
                dto.Cif = it.Loan?.Cif;
                dto.BranchName = it.Branch?.Name;
                dto.Name = it.Loan?.Customer?.Name;
                dto.BranchPembukuanName = it.BranchPembukuan?.Name;
                dto.BranchProsesName = it.BranchProses?.Name;
                dto.AlasanLelang = it.AlasanLelang?.Name;

                var sb = new StatusGeneralBean();
                sb.Id = it.Status?.Id;
                sb.Name = it.Status?.Name;
                dto.Status = sb;

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<int> UploadDocument(UploadDocumentRestructure filter)
        {

            var wrap = new GenericResponse<int>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                wrap.Message = "Data tidak ditemukan";
                return wrap;
            }

            if (filter.File == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }

            if (filter.DocTypeId == null || filter.DocTypeId < 0)
            {
                wrap.Message = "Tipe Dokumen tidak ditemukan";
                return wrap;
            }

            var dc = this.ctx.DocumentAuction.Find(filter.DocTypeId);
            if (dc == null)
            {
                wrap.Message = "Tipe Dokumen tidak ditemukan di sistem";
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var path = conf["PhotoPath"];
            path = path + "/auctiondok";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var cp = new AuctionDocument();
            cp.CreateDate = DateTime.Today;
            cp.UserId = reqUser.Id;
            cp.Mime = filter.File.ContentType.ToString();
            cp.DocumentAuctionId = filter.DocTypeId;

            this.logger.LogInformation("cp >>>> " + cp.Id);

            this.ctx.AuctionDocument.Add(cp);
            this.ctx.SaveChanges();

            string ext = Path.GetExtension(filter.File.FileName);

            var nm = path + "/" + cp.Id.ToString() + ext;

            using (FileStream filestream = System.IO.File.Create(nm))
            {
                filter.File.CopyTo(filestream);
                filestream.Flush();
                //  return "\\Upload\\" + objFile.files.FileName;
            }

            var url = "auctiondok/" + cp.Id.ToString() + ext;
            cp.Url = url;
            this.ctx.AuctionDocument.Update(cp);
            this.ctx.SaveChanges(true);

            wrap.Status = true;
            wrap.AddData(cp.Id!.Value);

            return wrap;
        }

        public GenericResponse<UserReqApproveBean> DeleteDocument(UserReqApproveBean filter)
        {

            var wrap = new GenericResponse<UserReqApproveBean>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                wrap.Message = "Data tidak ditemukan";
                return wrap;
            }

            if (filter.Id == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }

            var prev = this.ctx.AuctionDocument.Find(filter.Id);
            if (prev == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }

            this.ctx.AuctionDocument.Remove(prev);
            this.ctx.SaveChanges(true);

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public IActionResult ViewDocument(UserReqApproveBean filter)
        {

            var wrap = new GenericResponse<UserReqApproveBean>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                return new BadRequestResult();
            }

            if (filter.Id == null)
            {
                return new BadRequestResult();
            }

            var prev = this.ctx.AuctionDocument.Find(filter.Id);
            if (prev == null)
            {
                return new BadRequestResult();
            }

            var file = conf["PhotoPath"] + "/" + prev.Url;
            if (File.Exists(file) == false)
            {
                return new BadRequestResult();
            }

            var bytes = File.ReadAllBytes(file);
            MemoryStream ms = new MemoryStream(bytes);
            return new FileStreamResult(ms, prev.Mime!);
        }

        public GenericResponse<int> UploadDocumentResult(UploadDocumentRestructure filter)
        {

            var wrap = new GenericResponse<int>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                wrap.Message = "Data tidak ditemukan";
                return wrap;
            }

            if (filter.File == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }

            if (filter.DocTypeId == null || filter.DocTypeId < 0)
            {
                wrap.Message = "Tipe Dokumen tidak ditemukan";
                return wrap;
            }

            var dc = this.ctx.DocumentAuctionResult.Find(filter.DocTypeId);
            if (dc == null)
            {
                wrap.Message = "Tipe Dokumen tidak ditemukan di sistem";
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var path = conf["PhotoPath"];
            path = path + "/auctionresultdok";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var cp = new AuctionResultDocument();
            cp.CreateDate = DateTime.Today;
            cp.UserId = reqUser.Id;
            cp.Mime = filter.File.ContentType.ToString();
            cp.DocumentResultAuctionId = filter.DocTypeId;
            this.ctx.AuctionResultDocument.Add(cp);
            this.ctx.SaveChanges();

            string ext = Path.GetExtension(filter.File.FileName);

            var nm = path + "/" + cp.Id.ToString() + ext;

            using (FileStream filestream = System.IO.File.Create(nm))
            {
                filter.File.CopyTo(filestream);
                filestream.Flush();
                //  return "\\Upload\\" + objFile.files.FileName;
            }

            var url = "auctionresultdok/" + cp.Id.ToString() + ext;
            cp.Url = url;
            this.ctx.AuctionResultDocument.Update(cp);
            this.ctx.SaveChanges(true);

            wrap.Status = true;
            wrap.AddData(cp.Id!.Value);

            return wrap;
        }

        public GenericResponse<UserReqApproveBean> DeleteDocumentResult(UserReqApproveBean filter)
        {

            var wrap = new GenericResponse<UserReqApproveBean>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                wrap.Message = "Data tidak ditemukan";
                return wrap;
            }

            if (filter.Id == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }

            var prev = this.ctx.AuctionResultDocument.Find(filter.Id);
            if (prev == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }

            this.ctx.AuctionResultDocument.Remove(prev);
            this.ctx.SaveChanges(true);

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public IActionResult ViewDocumentResult(UserReqApproveBean filter)
        {

            var wrap = new GenericResponse<UserReqApproveBean>
            {
                Status = false,
                Message = ""
            };

            if (filter == null)
            {
                return new BadRequestResult();
            }

            if (filter.Id == null)
            {
                return new BadRequestResult();
            }

            var prev = this.ctx.AuctionResultDocument.Find(filter.Id);
            if (prev == null)
            {
                return new BadRequestResult();
            }

            var file = conf["PhotoPath"] + "/" + prev.Url;
            if (File.Exists(file) == false)
            {
                return new BadRequestResult();
            }

            var bytes = File.ReadAllBytes(file);
            MemoryStream ms = new MemoryStream(bytes);
            return new FileStreamResult(ms, prev.Mime!);
        }

        public GenericResponse<CreateLelang> Create(CreateLelang bean)
        {
            var wrap = new GenericResponse<CreateLelang>
            {
                Status = false,
                Message = ""
            };

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var pr = Validation.IlKeiValidator.Instance.WithPoCo(bean)
                .Pick(nameof(bean.LoanId)).IsMandatory().AsInteger().Pack()
                //.Pick(nameof(bean.AlasanLelangId)).IsMandatory().AsInteger().Pack()
                //.Pick(nameof(bean.NoPK)).IsMandatory().AsString().WithMinLen(2).Pack()
                //.Pick(nameof(bean.NilaiLimitLelang)).IsMandatory().AsDouble().Pack()
                //.Pick(nameof(bean.UangJaminan)).IsMandatory().AsDouble().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var vli = this.ctx.MasterLoan.Find(bean.LoanId);
            if (vli == null)
            {
                wrap.Message = "Data Loan tidak ditemukan di sistem";
                return wrap;
            }

            if (bean.AlasanLelangId != null)
            {
                var vpr = this.ctx.AlasanLelang.Find(bean.AlasanLelangId);
                if (vpr == null)
                {
                    wrap.Message = "Data Alasan Lelang tidak ditemukan di sistem";
                    return wrap;
                }
            }


            this.ctx.Entry(vli).Reference(r => r.Customer).Load();

            var ne = new Auction();
            ne.AlasanLelangId = bean.AlasanLelangId;
            ne.Keterangan = bean.Keterangan;
            ne.LoanId = bean.LoanId;
            ne.NilaiLimitLelang = bean.NilaiLimitLelang;
            ne.NoPK = bean.NoPK;
            ne.UangJaminan = bean.UangJaminan;
            ne.ObjekLelang = bean.ObjekLelang;
            ne.BranchId = vli.Customer?.BranchId;
            ne.BranchPembukuanId = vli.Customer?.BranchId;
            ne.BranchProsesId = vli.Customer?.BranchId;
            ne.Status = this.statusService.GetStatusLeLang("DRAFT");
            this.ctx.Auction.Add(ne);
            this.ctx.SaveChanges();


            var appr = new AuctionApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("DRAFT");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.AuctionId = ne.Id;
            this.ctx.AuctionApproval.Add(appr);
            this.ctx.SaveChanges();

            if (bean.Document != null)
            {
                foreach (var item in bean.Document)
                {
                    var doc = this.ctx.AuctionDocument.Find(item);
                    if (doc != null)
                    {
                        doc.AuctionId = ne.Id;
                        this.ctx.AuctionDocument.Update(doc);
                        this.ctx.SaveChanges();
                    }
                }
            }

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<SubmitLelang> Submit(SubmitLelang bean)
        {
            var wrap = new GenericResponse<SubmitLelang>
            {
                Status = false,
                Message = ""
            };

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var pr = Validation.IlKeiValidator.Instance.WithPoCo(bean)
                .Pick(nameof(bean.Id)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(bean.AlasanLelangId)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(bean.NoPK)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(bean.NilaiLimitLelang)).IsMandatory().AsDouble().Pack()
                .Pick(nameof(bean.UangJaminan)).IsMandatory().AsDouble().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }


            var vpr = this.ctx.AlasanLelang.Find(bean.AlasanLelangId);
            if (vpr == null)
            {
                wrap.Message = "Data Alasan Lelang tidak ditemukan di sistem";
                return wrap;
            }

            var ne = this.ctx.Auction.Find(bean.Id);
            if (ne == null)
            {
                wrap.Message = "Data Lelang tidak ditemukan di sistem";
                return wrap;
            }

            this.ctx.Entry(ne).Reference(r => r.Status).Load();
            var allowedStatus = new string[] { "DRAFT" };

            if (allowedStatus.Contains(ne.Status!.Name) == false)
            {
                wrap.Message = "Data Lelang tidak dapat ajukan (STAT)";
                return wrap;
            }

            ne.AlasanLelangId = bean.AlasanLelangId;
            ne.Keterangan = bean.Keterangan;
            ne.NilaiLimitLelang = bean.NilaiLimitLelang;
            ne.NoPK = bean.NoPK;
            ne.UangJaminan = bean.UangJaminan;
            ne.ObjekLelang = bean.ObjekLelang;
            ne.Status = this.statusService.GetStatusLeLang("PENGAJUAN");
            this.ctx.Auction.Update(ne);
            this.ctx.SaveChanges();

            if (bean.Document != null)
            {
                foreach (var item in bean.Document)
                {
                    var doc = this.ctx.AuctionDocument.Find(item);
                    if (doc != null)
                    {
                        doc.AuctionId = ne.Id;
                        this.ctx.AuctionDocument.Update(doc);
                        this.ctx.SaveChanges();
                    }
                }
            }

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<ApprovalLevelRestructure> SendBack(ApprovalLevelRestructure bean)
        {
            var wrap = new GenericResponse<ApprovalLevelRestructure>
            {
                Status = false,
                Message = ""
            };

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }
            this.ctx.Entry(reqUser).Reference(r => r.Role!).Load();


            var pr = Validation.IlKeiValidator.Instance.WithPoCo(bean)
                .Pick(nameof(bean.Id)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(bean.Comment)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }


            var q = this.ctx.Set<Auction>().Include(i => i.Status)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Resktruktur tidak ditemukan di dalam sistem";
                return wrap;
            }

            var res = q[0];
            var allowedStatus = new string[] { "PENGAJUAN", "VERIFIKASI" };

            if (allowedStatus.Contains(res.Status!.Name) == false)
            {
                wrap.Message = "Data Resktruktur tidak dapat di kembalikan (STAT)";
                return wrap;
            }

            if (res.Status!.Name == "PENGAJUAN")
            {
                res.Status = this.statusService.GetStatusLeLang("DRAFT");
            }
            else if (res.Status!.Name == "VERIFIKASI")
            {
                res.Status = this.statusService.GetStatusLeLang("PENGAJUAN");
            }

            this.ctx.Auction.Update(res);
            this.ctx.SaveChanges();

            var appr = new AuctionApproval  ();
            appr.Execution = this.statusService.GetRecoveryExecution("SENT BACK");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.AuctionId = res.Id;
            this.ctx.AuctionApproval.Add(appr);
            this.ctx.SaveChanges();

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<ApprovalLevelRestructure> Verify(ApprovalLevelRestructure bean)
        {
            var wrap = new GenericResponse<ApprovalLevelRestructure>
            {
                Status = false,
                Message = ""
            };

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }
            this.ctx.Entry(reqUser).Reference(r => r.Role!).Load();

            var pr = Validation.IlKeiValidator.Instance.WithPoCo(bean)
                .Pick(nameof(bean.Id)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(bean.Comment)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }


            var q = this.ctx.Set<Auction>().Include(i => i.Status)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Lelang tidak ditemukan di dalam sistem";
                return wrap;
            }

            var res = q[0];
            var allowedStatus = new string[] { "PENGAJUAN" };

            if (allowedStatus.Contains(res.Status!.Name) == false)
            {
                wrap.Message = "Data Lelang tidak dapat di verifikasi (STAT)";
                return wrap;
            }

            res.Status = this.statusService.GetStatusLeLang("VERIFIKASI");

            this.ctx.Auction.Update(res);
            this.ctx.SaveChanges();

            var appr = new AuctionApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("APPROVED");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.AuctionId = res.Id;
            this.ctx.AuctionApproval.Add(appr);
            this.ctx.SaveChanges();

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<ApprovalLevelRestructure> Approve(ApprovalLevelRestructure bean)
        {
            var wrap = new GenericResponse<ApprovalLevelRestructure>
            {
                Status = false,
                Message = ""
            };

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }
            this.ctx.Entry(reqUser).Reference(r => r.Role!).Load();

            var pr = Validation.IlKeiValidator.Instance.WithPoCo(bean)
                .Pick(nameof(bean.Id)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(bean.Comment)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }


            var q = this.ctx.Set<Auction>().Include(i => i.Status)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Lelang tidak ditemukan di dalam sistem";
                return wrap;
            }

            var res = q[0];
            var allowedStatus = new string[] { "VERIFIKASI", "PENGAJUAN" };

            if (allowedStatus.Contains(res.Status!.Name) == false)
            {
                wrap.Message = "Data Resktruktur tidak dapat di setujui (STAT)";
                return wrap;
            }
            if (res.Status!.Name == "PENGAJUAN")
            {
                res.Status = this.statusService.GetStatusLeLang("VERIFIKASI");
            }
            else if (res.Status!.Name == "VERIFIKASI")
            {
                res.Status = this.statusService.GetStatusLeLang("DISETUJUI");
            }

            this.ctx.Auction.Update(res);
            this.ctx.SaveChanges();

            var appr = new AuctionApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("APPROVED");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.AuctionId = res.Id;
            this.ctx.AuctionApproval.Add(appr);
            this.ctx.SaveChanges();

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<ApprovalLevelRestructure> Reject(ApprovalLevelRestructure bean)
        {
            var wrap = new GenericResponse<ApprovalLevelRestructure>
            {
                Status = false,
                Message = ""
            };

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }
            this.ctx.Entry(reqUser).Reference(r => r.Role!).Load();

            var pr = Validation.IlKeiValidator.Instance.WithPoCo(bean)
                .Pick(nameof(bean.Id)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(bean.Comment)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }


            var q = this.ctx.Set<Auction>().Include(i => i.Status)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Lelang tidak ditemukan di dalam sistem";
                return wrap;
            }

            var res = q[0];
            var allowedStatus = new string[] { "DRAFT", "PENGAJUAN", "VERIFIKASI" };

            if (allowedStatus.Contains(res.Status!.Name) == false)
            {
                wrap.Message = "Data Resktruktur tidak dapat di reject (STAT)";
                return wrap;
            }

            res.Status = this.statusService.GetStatusLeLang("DITOLAK");

            this.ctx.Auction.Update(res);
            this.ctx.SaveChanges();

            var appr = new AuctionApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("REJECTED");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.AuctionId = res.Id;
            this.ctx.AuctionApproval.Add(appr);
            this.ctx.SaveChanges();

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<LelangDetailResponseBean> Detail(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<LelangDetailResponseBean>
            {
                Status = false,
                Message = ""
            };


            var q = this.ctx.Set<Auction>().Include(i => i.Status).Include(i => i.Loan).ThenInclude(i => i!.Customer)
                                            .Include(i => i.Branch).Include(i => i.BranchPembukuan)
                                            .Include(i => i.BranchProses).Include(i => i.AlasanLelang)
                                            .Include(i => i.BalaiLelang).Include(i => i.JenisLelang)
                                            .Where(q => q.Id.Equals(filter.Id))
                                            .ToList();

            var ldata = new List<LelangDetailResponseBean>();
            foreach (var it in q)
            {
                this.ctx.Entry(it.Loan!).Reference(r => r.ProductSegment!).Load();
                this.ctx.Entry(it.Loan!).Reference(r => r.Product).Load();
                var dto = this.mapper.Map<LelangDetailResponseBean>(it);
                dto.NoPK = it.NoPK;
                dto.CatatanLelang = it.CatatanLelang;
                dto.TglPenetapanLelang = it.TglPenetapanLelang;
                dto.TataCaraLelang = it.TataCaraLelang;
                dto.BiayaLelang = it.BiayaLelang;
                dto.UangJaminan = it.UangJaminan;
                dto.Keterangan = it.Keterangan;
                dto.NoRekening = it.NoRekening;
                dto.NamaRekening = it.NamaRekening;

                if (it.AlasanLelang != null)
                {
                    var jl = new GenericParameterBean();
                    jl.Id = it.AlasanLelang!.Id;
                    jl.Name = it.AlasanLelang!.Name;
                    dto.AlasanLelang = jl;
                }

                if (it.JenisLelang != null)
                {
                    var jl = new GenericParameterBean();
                    jl.Id = it.JenisLelang!.Id;
                    jl.Name = it.JenisLelang!.Name;
                    dto.JenisLelang = jl;
                }

                if (it.BalaiLelang != null)
                {
                    var jl = new GenericParameterBean();
                    jl.Id = it.BalaiLelang!.Id;
                    jl.Name = it.BalaiLelang!.Name;
                    dto.BalaiLelang = jl;
                }



                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }
    }
}
