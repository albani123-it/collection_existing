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

namespace Collectium.Service
{
    public class InsuranceService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<InsuranceService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration conf;

        public InsuranceService(CollectiumDBContext ctx,
                                ILogger<InsuranceService> logger,
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

            IQueryable<Insurance> q = this.ctx.Set<Insurance>().Include(i => i.Status).Include(i => i.Loan).ThenInclude(i => i!.Customer)
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

            IQueryable<Insurance> q = this.ctx.Set<Insurance>().Include(i => i.Status).Include(i => i.Loan).ThenInclude(i => i!.Customer)
                                            .Include(i => i.Branch).Include(i => i.BranchPembukuan)
                                            .Include(i => i.BranchProses).Include(i => i.Asuransi);

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
                dto.Asuransi = it.Asuransi?.Name;

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

            var dc = this.ctx.DocumentInsurance.Find(filter.DocTypeId);
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
            path = path + "/insurancedok";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var cp = new InsuranceDocument();
            cp.CreateDate = DateTime.Today;
            cp.UserId = reqUser.Id;
            cp.Mime = filter.File.ContentType.ToString();
            cp.DocumentInsuranceId = filter.DocTypeId;
            this.ctx.InsuranceDocument.Add(cp);
            this.ctx.SaveChanges();

            string ext = Path.GetExtension(filter.File.FileName);

            var nm = path + "/" + cp.Id.ToString() + ext;

            using (FileStream filestream = System.IO.File.Create(nm))
            {
                filter.File.CopyTo(filestream);
                filestream.Flush();
                //  return "\\Upload\\" + objFile.files.FileName;
            }

            var url = "insurancedok/" + cp.Id.ToString() + ext;
            cp.Url = url;
            this.ctx.InsuranceDocument.Update(cp);
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

            var prev = this.ctx.InsuranceDocument.Find(filter.Id);
            if (prev == null)
            {
                wrap.Message = "Data File tidak ditemukan";
                return wrap;
            }

            this.ctx.InsuranceDocument.Remove(prev);
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

            var prev = this.ctx.InsuranceDocument.Find(filter.Id);
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

        public GenericResponse<CreateAsuransi> Create(CreateAsuransi bean)
        {
            var wrap = new GenericResponse<CreateAsuransi>
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
                //.Pick(nameof(bean.NamaPejabat)).IsMandatory().AsString().WithMinLen(2).Pack()
                //.Pick(nameof(bean.Jabatan)).IsMandatory().AsString().WithMinLen(2).Pack()
                //.Pick(nameof(bean.NoSertifikat)).IsMandatory().AsString().WithMinLen(2).Pack()
                //.Pick(nameof(bean.TglSertifikat)).IsMandatory().AsDate().Pack()
                //.Pick(nameof(bean.AsuransiId)).IsMandatory().AsInteger().Pack()
                //.Pick(nameof(bean.NoPolis)).IsMandatory().AsString().WithMinLen(2).Pack()
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

            if (bean.AsuransiId != null)
            {
                var vpr = this.ctx.Asuransi.Find(bean.AsuransiId);
                if (vpr == null)
                {
                    wrap.Message = "Data Asuransi tidak ditemukan di sistem";
                    return wrap;
                }
            }


            this.ctx.Entry(vli).Reference(r => r.Customer).Load();

            var ne = new Insurance();
            ne.Keterangan = bean.Keterangan;
            ne.LoanId = bean.LoanId;
            ne.BranchId = vli.Customer?.BranchId;
            ne.BranchPembukuanId = vli.Customer?.BranchId;
            ne.BranchProsesId = vli.Customer?.BranchId;
            ne.NamaPejabat = bean.NamaPejabat;
            ne.Jabatan = bean.Jabatan;
            ne.NoSertifikat = bean.NoSertifikat;
            ne.TglSertifikat = bean.TglSertifikat;
            ne.AsuransiId = bean.AsuransiId;
            ne.NoPolis = bean.NoPolis;
            ne.TglPolis = bean.TglPolis;
            ne.NoPk = bean.NoPk;
            ne.TunggakanPokok70Persen = bean.TunggakanPokok70Persen;
            ne.TunggakanBunga70Persen = bean.TunggakanBunga70Persen;
            ne.CatatanPolis = bean.CatatanPolis;
            ne.Keterangan = bean.Keterangan;
            ne.Status = this.statusService.GetStatusAsuransi("DRAFT");
            this.ctx.Insurance.Add(ne);
            this.ctx.SaveChanges();


            var appr = new InsuranceApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("DRAFT");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.InsuranceId = ne.Id;
            this.ctx.InsuranceApproval.Add(appr);
            this.ctx.SaveChanges();


            if (bean.Document != null)
            {
                foreach (var item in bean.Document)
                {
                    var doc = this.ctx.InsuranceDocument.Find(item);
                    if (doc != null)
                    {
                        doc.InsuranceId = ne.Id;
                        this.ctx.InsuranceDocument.Update(doc);
                        this.ctx.SaveChanges();
                    }
                }
            }

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<SubmitAsuransi> Submit(SubmitAsuransi bean)
        {
            var wrap = new GenericResponse<SubmitAsuransi>
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
                .Pick(nameof(bean.NamaPejabat)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(bean.Jabatan)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(bean.NoSertifikat)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(bean.TglSertifikat)).IsMandatory().AsString().Pack()
                .Pick(nameof(bean.AsuransiId)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(bean.NoPolis)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }


            var vpr = this.ctx.Asuransi.Find(bean.AsuransiId);
            if (vpr == null)
            {
                wrap.Message = "Data Asuransi Provider tidak ditemukan di sistem";
                return wrap;
            }

            var ne = this.ctx.Insurance.Find(bean.Id);
            if (ne == null)
            {
                wrap.Message = "Data Asuransi tidak ditemukan di sistem";
                return wrap;
            }

            this.ctx.Entry(ne).Reference(r => r.Status).Load();
            var allowedStatus = new string[] { "DRAFT" };

            if (allowedStatus.Contains(ne.Status!.Name) == false)
            {
                wrap.Message = "Data Asuransi tidak dapat ajukan (STAT)";
                return wrap;
            }

            ne.Keterangan = bean.Keterangan;
            ne.NamaPejabat = bean.NamaPejabat;
            ne.Jabatan = bean.Jabatan;
            ne.NoSertifikat = bean.NoSertifikat;
            ne.TglSertifikat = bean.TglSertifikat;
            ne.AsuransiId = bean.AsuransiId;
            ne.NoPolis = bean.NoPolis;
            ne.TglPolis = bean.TglPolis;
            ne.NoPk = bean.NoPk;
            ne.TunggakanPokok70Persen = bean.TunggakanPokok70Persen;
            ne.TunggakanBunga70Persen = bean.TunggakanBunga70Persen;
            ne.CatatanPolis = bean.CatatanPolis;
            ne.Keterangan = bean.Keterangan;
            ne.Status = this.statusService.GetStatusAsuransi("PENGAJUAN");
            this.ctx.Insurance.Update(ne);
            this.ctx.SaveChanges();

            if (bean.Document != null)
            {
                foreach (var item in bean.Document)
                {
                    var doc = this.ctx.InsuranceDocument.Find(item);
                    if (doc != null)
                    {
                        doc.InsuranceId = ne.Id;
                        this.ctx.InsuranceDocument.Update(doc);
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


            var q = this.ctx.Set<Insurance>().Include(i => i.Status)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Asuransi tidak ditemukan di dalam sistem";
                return wrap;
            }

            var res = q[0];
            var allowedStatus = new string[] { "PENGAJUAN", "VERIFIKASI" };

            if (allowedStatus.Contains(res.Status!.Name) == false)
            {
                wrap.Message = "Data Asuransi tidak dapat di kembalikan (STAT)";
                return wrap;
            }

            if (res.Status!.Name == "PENGAJUAN")
            {
                res.Status = this.statusService.GetStatusAsuransi("DRAFT");
            }
            else if (res.Status!.Name == "VERIFIKASI")
            {
                res.Status = this.statusService.GetStatusAsuransi("PENGAJUAN");
            }

            this.ctx.Insurance.Update(res);
            this.ctx.SaveChanges();

            var appr = new InsuranceApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("SENT BACK");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.InsuranceId = res.Id;
            this.ctx.InsuranceApproval.Add(appr);
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


            var q = this.ctx.Set<Insurance>().Include(i => i.Status)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Asuransi tidak ditemukan di dalam sistem";
                return wrap;
            }

            var res = q[0];
            var allowedStatus = new string[] { "PENGAJUAN" };

            if (allowedStatus.Contains(res.Status!.Name) == false)
            {
                wrap.Message = "Data Asuransi tidak dapat di verifikasi (STAT)";
                return wrap;
            }

            res.Status = this.statusService.GetStatusAsuransi("VERIFIKASI");

            this.ctx.Insurance.Update(res);
            this.ctx.SaveChanges();

            var appr = new InsuranceApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("APPROVED");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.InsuranceId = res.Id;
            this.ctx.InsuranceApproval.Add(appr);
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


            var q = this.ctx.Set<Insurance>().Include(i => i.Status)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Asuransi tidak ditemukan di dalam sistem";
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
                res.Status = this.statusService.GetStatusAsuransi("VERIFIKASI");
            }
            else if (res.Status!.Name == "VERIFIKASI")
            {
                res.Status = this.statusService.GetStatusAsuransi("DISETUJUI");
            }

            this.ctx.Insurance.Update(res);
            this.ctx.SaveChanges();

            var appr = new InsuranceApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("APPROVED");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.InsuranceId = res.Id;
            this.ctx.InsuranceApproval.Add(appr);
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


            var q = this.ctx.Set<Insurance>().Include(i => i.Status)
                                            .Where(q => q.Id.Equals(bean.Id))
                                            .ToList();
            if (q == null || q.Count() < 1)
            {
                wrap.Message = "Data Asuransi tidak ditemukan di dalam sistem";
                return wrap;
            }

            var res = q[0];
            var allowedStatus = new string[] { "DRAFT", "PENGAJUAN", "VERIFIKASI" };

            if (allowedStatus.Contains(res.Status!.Name) == false)
            {
                wrap.Message = "Data Resktruktur tidak dapat di reject (STAT)";
                return wrap;
            }

            res.Status = this.statusService.GetStatusAsuransi("DITOLAK");

            this.ctx.Insurance.Update(res);
            this.ctx.SaveChanges();

            var appr = new InsuranceApproval();
            appr.Execution = this.statusService.GetRecoveryExecution("REJECTED");
            appr.Sender = reqUser;
            appr.Recipient = reqUser;
            appr.CreateDate = DateTime.Now;
            appr.InsuranceId = res.Id;
            this.ctx.InsuranceApproval.Add(appr);
            this.ctx.SaveChanges();

            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<AsuransiDetailResponseBean> Detail(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<AsuransiDetailResponseBean>
            {
                Status = false,
                Message = ""
            };


            var q = this.ctx.Set<Insurance>().Include(i => i.Status).Include(i => i.Loan).ThenInclude(i => i!.Customer)
                                            .Include(i => i.Branch).Include(i => i.BranchPembukuan)
                                            .Include(i => i.BranchProses).Include(i => i.Asuransi)
                                            .Include(i => i.AsuransiSisaKlaim)
                                            .Where(q => q.Id.Equals(filter.Id))
                                            .ToList();

            var ldata = new List<AsuransiDetailResponseBean>();
            foreach (var it in q)
            {
                this.ctx.Entry(it.Loan!).Reference(r => r.ProductSegment!).Load();
                this.ctx.Entry(it.Loan!).Reference(r => r.Product).Load();
                var dto = this.mapper.Map<AsuransiDetailResponseBean>(it);

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }
    }
}
