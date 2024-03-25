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
using LinqKit;
using System.Text.Json;

namespace Collectium.Service
{
    public class RestructureService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<RestructureService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration conf;

        public RestructureService(CollectiumDBContext ctx,
                                ILogger<RestructureService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                IMapper mapper,
                                IHttpContextAccessor httpContextAccessor,
                                IConfiguration conf)
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

            IQueryable<Restructure> q = this.ctx.Set<Restructure>().Include(i => i.Status).Include(i => i.Loan).ThenInclude(i => i!.Customer)
                                            .Include(i => i.Branch).Include(i => i.BranchPembukuan)
                                            .Include(i => i.BranchProses).Include(i => i.PolaRestruktur);

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
                dto.PolaRestruktur = it.PolaRestruktur?.Name;

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

            IQueryable<Restructure> q = this.ctx.Set<Restructure>().Include(i => i.Status).Include(i => i.Loan).ThenInclude(i => i!.Customer)
                                            .Include(i => i.Branch).Include(i => i.BranchPembukuan)
                                            .Include(i => i.BranchProses).Include(i => i.PolaRestruktur);

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
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

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
                dto.PolaRestruktur = it.PolaRestruktur?.Name;

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

            var dc = this.ctx.DocumentRestruktur.Find(filter.DocTypeId);
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
            path = path + "/restructdok";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var cp = new RestructureDocument();
            cp.CreateDate = DateTime.Today;
            cp.UserId = reqUser.Id;
            cp.Mime = filter.File.ContentType.ToString();
            cp.DocumentResutrukturId = filter.DocTypeId;
            this.ctx.RestructureDocument.Add(cp);
            this.ctx.SaveChanges();

            string ext = Path.GetExtension(filter.File.FileName);

            var nm = path + "/" + cp.Id.ToString() + ext;

            using (FileStream filestream = System.IO.File.Create(nm))
            {
                filter.File.CopyTo(filestream);
                filestream.Flush();
                //  return "\\Upload\\" + objFile.files.FileName;
            }

            var url = "restructdok/" + cp.Id.ToString() + ext;
            cp.Url = url;
            this.ctx.RestructureDocument.Update(cp);
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

            var prev = this.ctx.RestructureDocument.Find(filter.Id);
            if (prev == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }

            this.ctx.RestructureDocument.Remove(prev);
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

            var prev = this.ctx.RestructureDocument.Find(filter.Id);
            if (prev == null)
            {
                return new BadRequestResult();
            }

            var file = conf["PhotoPath"] + "/" + prev.Url;
            this.logger.LogInformation("filename >>> " + file);
            if (File.Exists(file) == false)
            {
                return new BadRequestResult();
            }

            var bytes = File.ReadAllBytes(file);
            MemoryStream ms = new MemoryStream(bytes);
            return new FileStreamResult(ms, prev.Mime!);
        }

        public GenericResponse<CreateRestructure> Create(CreateRestructure bean)
        {
            var wrap = new GenericResponse<CreateRestructure>
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
                //.Pick(nameof(bean.PolaRestrukId)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            /*
            if (bean.CashFlow == null)
            {
                wrap.Message = "Data CashFlow adalah mandatory";
                return wrap;
            }
            
            var cf = bean.CashFlow;
            pr = Validation.IlKeiValidator.Instance.WithPoCo(cf)
                .Pick(nameof(cf.BiayaPendidikan)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.BiayaTransportasi)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.BiayaLainnya)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.BiayaBelanja)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.BiayaListrikAirTelp)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.CicilanLainnya)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.HutangDiBank)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.PenghasilanLainnya)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.PenghasilanPasangan)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.PenghasilanNasabah)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.TotalPenghasilan)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }
            */

            var vli = this.ctx.MasterLoan.Find(bean.LoanId);
            if (vli == null)
            {
                wrap.Message = "Data Loan tidak ditemukan di sistem";
                return wrap;
            }

            if (bean.PolaRestrukId != null)
            {
                var vpr = this.ctx.PolaRestruktur.Find(bean.PolaRestrukId);
                if (vpr == null)
                {
                    wrap.Message = "Data Pola Restruktur tidak ditemukan di sistem";
                    return wrap;
                }
            }

            if (bean.PembayaranGpId != null) { 
                var vpg = this.ctx.PembayaranGp.Find(bean.PembayaranGpId);
                if (vpg != null)
                {
                    wrap.Message = "Data Pembayaran Grace Periode tidak ditemukan di sistem";
                    return wrap;
                }
            }

            this.ctx.Entry(vli).Reference(r => r.Customer).Load();

            var ne = new Restructure();
            ne.Denda = bean.Denda;
            ne.DiskonTunggakanDenda = bean.DiskonTunggakanDenda;
            ne.DiskonTunggakanMargin = bean.DiskonTunggakanMargin;
            ne.JenisPenguranganId = bean.JenisPenguranganId;
            ne.Keterangan = bean.Keterangan;
            ne.LoanId = bean.LoanId;
            ne.MarginAmount = bean.MarginAmount;
            ne.Margin = bean.Margin;
            ne.MarginPembayaran = bean.MarginPembayaran;
            ne.MarginPinalty = bean.MarginPinalty;
            ne.PembayaranGpId = bean.PembayaranGpId;
            ne.PenguranganNilaiMargin = bean.PenguranganNilaiMargin;
            ne.PeriodeDiskon = bean.PeriodeDiskon;
            ne.PolaRestrukId = bean.PolaRestrukId;
            ne.PrincipalPembayaran = bean.PrincipalPembayaran;
            ne.PrincipalPinalty = bean.PrincipalPinalty;
            ne.TglJatuhTempoBaru = bean.TglJatuhTempoBaru;
            ne.TotalDiskonMargin = bean.TotalDiskonMargin;
            ne.ValueDate = bean.ValueDate;
            ne.BranchId = vli.Customer?.BranchId;
            ne.BranchPembukuanId = vli.Customer?.BranchId;
            ne.BranchProsesId = vli.Customer?.BranchId;

            //var json = JsonSerializer.Deserialize<List<string>>(bean.Permasalahan!);

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(bean.Permasalahan, serializeOptions);
            ne.Permasalahan = json;

            ne.CreateBy = reqUser;
            ne.CreateDate = DateTime.Now;
            ne.Status = this.statusService.GetStatusRestruktur("DRAFT");            
            this.ctx.Restructure.Add(ne);
            this.ctx.SaveChanges();

            if (bean.CashFlow != null)
            {
                var cf = bean.CashFlow;
                var nrcf = new RestructureCashFlow();
                nrcf.PenghasilanNasabah = cf.PenghasilanNasabah;
                nrcf.PenghasilanPasangan = cf.PenghasilanPasangan;
                nrcf.PenghasilanLainnya = cf.PenghasilanLainnya;
                nrcf.TotalPenghasilan = cf.TotalPenghasilan;
                nrcf.BiayaPendidikan = cf.BiayaPendidikan;
                nrcf.BiayaListrikAirTelp = cf.BiayaListrikAirTelp;
                nrcf.BiayaBelanja = cf.BiayaBelanja;
                nrcf.BiayaTransportasi = cf.BiayaTransportasi;
                nrcf.BiayaLainnya = cf.BiayaLainnya;
                nrcf.TotalPengeluaran = cf.TotalPengeluaran;
                nrcf.HutangDiBank = cf.HutangDiBank;
                nrcf.CicilanLainnya = cf.CicilanLainnya;
                nrcf.TotalKewajiban = cf.TotalKewajiban;
                nrcf.PenghasilanBersih = cf.PenghasilanBersih;
                nrcf.Rpc70Persen = cf.Rpc70Persen;
                nrcf.RestructureId = ne.Id;
                this.ctx.RestructureCashFlow.Add(nrcf);
                this.ctx.SaveChanges();
            }


            var appr = new RestructureApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("DRAFT");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.RestructureId = ne.Id;
            this.ctx.RestructureApproval.Add(appr);
            this.ctx.SaveChanges();


            if (bean.Document != null)
            {
                foreach (var item in bean.Document)
                {
                    var doc = this.ctx.RestructureDocument.Find(item);
                    if (doc != null)
                    {
                        doc.RestructureId = ne.Id;
                        this.ctx.RestructureDocument.Update(doc);
                        this.ctx.SaveChanges();
                    }
                }
            }

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<SubmitRestructure> Submit(SubmitRestructure bean)
        {
            var wrap = new GenericResponse<SubmitRestructure>
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
                .Pick(nameof(bean.PolaRestrukId)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            
            if (bean.CashFlow == null)
            {
                wrap.Message = "Data CashFlow adalah mandatory";
                return wrap;
            }
            
            var cf = bean.CashFlow;
            pr = Validation.IlKeiValidator.Instance.WithPoCo(cf)
                .Pick(nameof(cf.BiayaPendidikan)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.BiayaTransportasi)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.BiayaLainnya)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.BiayaBelanja)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.BiayaListrikAirTelp)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.CicilanLainnya)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.HutangDiBank)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.PenghasilanLainnya)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.PenghasilanPasangan)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.PenghasilanNasabah)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(cf.TotalPenghasilan)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }
            

            if (bean.PolaRestrukId != null)
            {
                var vpr = this.ctx.PolaRestruktur.Find(bean.PolaRestrukId);
                if (vpr == null)
                {
                    wrap.Message = "Data Pola Restruktur tidak ditemukan di sistem";
                    return wrap;
                }
            }

            if (bean.PembayaranGpId != null)
            {
                var vpg = this.ctx.PembayaranGp.Find(bean.PembayaranGpId);
                if (vpg != null)
                {
                    wrap.Message = "Data Pembayaran Grace Periode tidak ditemukan di sistem";
                    return wrap;
                }
            }

            var ne = this.ctx.Restructure.Find(bean.Id);
            if (ne == null)
            {
                wrap.Message = "Data Resktruktur tidak ditemukan di sistem";
                return wrap;
            }

            this.ctx.Entry(ne).Reference(r => r.Status).Load();
            var allowedStatus = new string[] { "DRAFT" };

            if (allowedStatus.Contains(ne.Status!.Name) == false)
            {
                wrap.Message = "Data Resktruktur tidak dapat ajukan (STAT)";
                return wrap;
            }

            ne.Denda = bean.Denda;
            ne.DiskonTunggakanDenda = bean.DiskonTunggakanDenda;
            ne.DiskonTunggakanMargin = bean.DiskonTunggakanMargin;
            ne.JenisPenguranganId = bean.JenisPenguranganId;
            ne.Keterangan = bean.Keterangan;
            ne.MarginAmount = bean.MarginAmount;
            ne.Margin = bean.Margin;
            ne.MarginPembayaran = bean.MarginPembayaran;
            ne.MarginPinalty = bean.MarginPinalty;
            ne.PembayaranGpId = bean.PembayaranGpId;
            ne.PenguranganNilaiMargin = bean.PenguranganNilaiMargin;
            ne.PeriodeDiskon = bean.PeriodeDiskon;
            ne.PolaRestrukId = bean.PolaRestrukId;
            ne.PrincipalPembayaran = bean.PrincipalPembayaran;
            ne.PrincipalPinalty = bean.PrincipalPinalty;
            ne.TglJatuhTempoBaru = bean.TglJatuhTempoBaru;
            ne.TotalDiskonMargin = bean.TotalDiskonMargin;
            ne.ValueDate = bean.ValueDate;
            ne.CreateBy = reqUser;
            ne.CreateDate = DateTime.Now;
            ne.Status = this.statusService.GetStatusRestruktur("PENGAJUAN");
            this.ctx.Restructure.Update(ne);
            this.ctx.SaveChanges();

            if (ne.CashFlow != null)
            {
                var nrcf = ne.CashFlow.ToArray()[0];
                nrcf.PenghasilanNasabah = cf.PenghasilanNasabah;
                nrcf.PenghasilanPasangan = cf.PenghasilanPasangan;
                nrcf.PenghasilanLainnya = cf.PenghasilanLainnya;
                nrcf.TotalPenghasilan = cf.TotalPenghasilan;
                nrcf.BiayaPendidikan = cf.BiayaPendidikan;
                nrcf.BiayaListrikAirTelp = cf.BiayaListrikAirTelp;
                nrcf.BiayaBelanja = cf.BiayaBelanja;
                nrcf.BiayaTransportasi = cf.BiayaTransportasi;
                nrcf.BiayaLainnya = cf.BiayaLainnya;
                nrcf.TotalPengeluaran = cf.TotalPengeluaran;
                nrcf.HutangDiBank = cf.HutangDiBank;
                nrcf.CicilanLainnya = cf.CicilanLainnya;
                nrcf.TotalKewajiban = cf.TotalKewajiban;
                nrcf.PenghasilanBersih = cf.PenghasilanBersih;
                nrcf.Rpc70Persen = cf.Rpc70Persen;
                nrcf.RestructureId = ne.Id;
                this.ctx.RestructureCashFlow.Update(nrcf);
                this.ctx.SaveChanges();
            } else
            {
                var nrcf = new RestructureCashFlow();
                nrcf.PenghasilanNasabah = cf.PenghasilanNasabah;
                nrcf.PenghasilanPasangan = cf.PenghasilanPasangan;
                nrcf.PenghasilanLainnya = cf.PenghasilanLainnya;
                nrcf.TotalPenghasilan = cf.TotalPenghasilan;
                nrcf.BiayaPendidikan = cf.BiayaPendidikan;
                nrcf.BiayaListrikAirTelp = cf.BiayaListrikAirTelp;
                nrcf.BiayaBelanja = cf.BiayaBelanja;
                nrcf.BiayaTransportasi = cf.BiayaTransportasi;
                nrcf.BiayaLainnya = cf.BiayaLainnya;
                nrcf.TotalPengeluaran = cf.TotalPengeluaran;
                nrcf.HutangDiBank = cf.HutangDiBank;
                nrcf.CicilanLainnya = cf.CicilanLainnya;
                nrcf.TotalKewajiban = cf.TotalKewajiban;
                nrcf.PenghasilanBersih = cf.PenghasilanBersih;
                nrcf.Rpc70Persen = cf.Rpc70Persen;
                nrcf.RestructureId = ne.Id;
                this.ctx.RestructureCashFlow.Add(nrcf);
                this.ctx.SaveChanges();
            }

            if (bean.Document != null)
            {
                foreach (var item in bean.Document)
                {
                    var doc = this.ctx.RestructureDocument.Find(item);
                    if (doc != null)
                    {
                        doc.RestructureId = ne.Id;
                        this.ctx.RestructureDocument.Update(doc);
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


            var q = this.ctx.Set<Restructure>().Include(i => i.Status)
                                            .Include(i => i.BranchProses).Include(i => i.PolaRestruktur)
                                            .Include(i => i.PembayaranGp).Include(i => i.JenisPengurangan)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Resktruktur tidak ditemukan di dalam sistem";
                return wrap;
            }

            var res = q[0];
            var allowedStatus = new string[] { "PENGAJUAN", "VERIFIKASI"};

            if (allowedStatus.Contains(res.Status!.Name) == false)
            {
                wrap.Message = "Data Resktruktur tidak dapat di kembalikan (STAT)";
                return wrap;
            }

            if (res.Status!.Name == "PENGAJUAN")
            {
                res.Status = this.statusService.GetStatusRestruktur("DRAFT");
            } else if (res.Status!.Name == "VERIFIKASI")
            {
                res.Status = this.statusService.GetStatusRestruktur("PENGAJUAN");
            }

            this.ctx.Restructure.Update(res);
            this.ctx.SaveChanges();

            var appr = new RestructureApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("SENT BACK");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.RestructureId = res.Id;
            this.ctx.RestructureApproval.Add(appr);
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


            var q = this.ctx.Set<Restructure>().Include(i => i.Status)
                                            .Include(i => i.BranchProses).Include(i => i.PolaRestruktur)
                                            .Include(i => i.PembayaranGp).Include(i => i.JenisPengurangan)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Resktruktur tidak ditemukan di dalam sistem";
                return wrap;
            }

            var res = q[0];
            var allowedStatus = new string[] { "PENGAJUAN" };

            if (allowedStatus.Contains(res.Status!.Name) == false)
            {
                wrap.Message = "Data Resktruktur tidak dapat di verifikasi (STAT)";
                return wrap;
            }

            res.Status = this.statusService.GetStatusRestruktur("VERIFIKASI");

            this.ctx.Restructure.Update(res);
            this.ctx.SaveChanges();

            var appr = new RestructureApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("APPROVED");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.RestructureId = res.Id;
            this.ctx.RestructureApproval.Add(appr);
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


            var q = this.ctx.Set<Restructure>().Include(i => i.Status)
                                            .Include(i => i.BranchProses).Include(i => i.PolaRestruktur)
                                            .Include(i => i.PembayaranGp).Include(i => i.JenisPengurangan)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Resktruktur tidak ditemukan di dalam sistem";
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
                res.Status = this.statusService.GetStatusRestruktur("VERIFIKASI");
            } else if (res.Status!.Name == "VERIFIKASI")
            {
                res.Status = this.statusService.GetStatusRestruktur("DISETUJUI");
            }


            this.ctx.Restructure.Update(res);
            this.ctx.SaveChanges();

            var appr = new RestructureApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("APPROVED");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.RestructureId = res.Id;
            this.ctx.RestructureApproval.Add(appr);
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


            var q = this.ctx.Set<Restructure>().Include(i => i.Status)
                                            .Include(i => i.BranchProses).Include(i => i.PolaRestruktur)
                                            .Include(i => i.PembayaranGp).Include(i => i.JenisPengurangan)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Resktruktur tidak ditemukan di dalam sistem";
                return wrap;
            }

            var res = q[0];
            var allowedStatus = new string[] { "DRAFT", "PENGAJUAN", "VERIFIKASI" };

            if (allowedStatus.Contains(res.Status!.Name) == false)
            {
                wrap.Message = "Data Resktruktur tidak dapat di reject (STAT)";
                return wrap;
            }

            res.Status = this.statusService.GetStatusRestruktur("DITOLAK");

            this.ctx.Restructure.Update(res);
            this.ctx.SaveChanges();

            var appr = new RestructureApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("REJECTED");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.RestructureId = res.Id;
            this.ctx.RestructureApproval.Add(appr);
            this.ctx.SaveChanges();

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<RestructureDetailResponseBean> Detail(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<RestructureDetailResponseBean>
            {
                Status = false,
                Message = ""
            };


            var q = this.ctx.Set<Restructure>().Include(i => i.Status).Include(i => i.Loan).ThenInclude(i => i!.Customer)
                                            .Include(i => i.Branch).Include(i => i.BranchPembukuan)
                                            .Include(i => i.BranchProses).Include(i => i.PolaRestruktur)
                                            .Include(i => i.PembayaranGp).Include(i => i.JenisPengurangan)
                                            .Include(i => i.CashFlow)
                                            .Where(q => q.Id.Equals(filter.Id))
                                            .ToList();

            var ldata = new List<RestructureDetailResponseBean>();
            foreach (var it in q)
            {
                this.ctx.Entry(it.Loan!).Reference(r => r.ProductSegment!).Load();
                this.ctx.Entry(it.Loan!).Reference(r => r.Product).Load();

                var dto = this.mapper.Map<RestructureDetailResponseBean>(it);

                var json = JsonSerializer.Deserialize<List<string>>(it.Permasalahan!);
                dto.Masalah = json;

                if (it.CashFlow != null)
                {
                    var cf = it.CashFlow.ToList()[0];
                    var dfcf = this.mapper.Map<RestructureCashFlowBean>(cf);
                    dto.CashFlowBean = dfcf;
                }

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }



        public GenericResponse<CollResponseBean> ListCollection(CollListRequestBean filter)
        {
            var wrap = new GenericResponse<CollResponseBean>
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


            IQueryable<MasterLoan> q = this.ctx.Set<MasterLoan>().Include(i => i.Customer)
                                        .Include(i => i.Product)
                                        .Include(i => i.Call!.CallBy)
                                        .Include(i => i.Call!.Branch)
                                        .Include(i => i.Call!.Branch!.Area);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<MasterLoan>();
                predicate = predicate.Or(p => p.Cif!.Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.AccNo!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.Customer!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            if (filter.Name != null && filter.Name.Length > 0)
            {
                q = q.Where(q => q.Customer!.Name!.Contains(filter.Name));
            }

            if (filter.AccountNo != null && filter.AccountNo!.Length > 0)
            {
                q = q.Where(q => q.AccNo!.Contains(filter.AccountNo));
            }

            if (filter.Dpd != null)
            {
                q = q.Where(q => q.Dpd.Equals(filter.Dpd));
            }

            if (filter.OfficerId != null && filter.OfficerId > 0)
            {
                q = q.Where(q => q.Call!.CallBy!.Id.Equals(filter.OfficerId));
            }

            if (filter.CallResultCode != null && filter.CallResultCode.Equals("PTP"))
            {
                if (filter.PtpDate != null)
                {
                    q = q.Where(q => q.Call!.CallResultDate.Equals(filter.PtpDate));
                }
            }

            q = q.Where(q => q.Status.Equals(1));

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<CollResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CollResponseBean>(it);
                dto.Branch = it.Call!.Branch!.Name;
                if (it.Call != null && it.Call.Branch != null && it.Call.Branch.Area != null)
                {
                    dto.Area = it.Call!.Branch!.Area!.Name;
                }
                dto.LastActivityDate = it.Call!.CallDate!;
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<RecoveryField> ListRecoveryField()
        {
            var wrap = new GenericResponse<RecoveryField>
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


            var q = this.ctx.Set<RecoveryField>().OrderByDescending(q => q.Id).ToList();
            wrap.Data = q;

            wrap.Status = true;

            return wrap;
        }
    }
}
