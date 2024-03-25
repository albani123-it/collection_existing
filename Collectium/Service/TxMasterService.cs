using AutoMapper;
using Collectium.Model;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Model.Helper;
using Collectium.Validation;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Service
{
    public class TxMasterService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<TxMasterService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly BranchAreaService baService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TxMasterService(CollectiumDBContext ctx,
                                ILogger<TxMasterService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                ToolService toolService,
                                BranchAreaService baService,
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
            this.baService = baService;
        }

        public GenericResponse<string> SaveProductSegment(ProductSegment src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(src.Desc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.ProductSegment.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Product Segment sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.ProductSegment.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveCallResult(CallResult src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(src.Description)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.CallResult.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Call Result sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.CallResult.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveNationality(Nationality src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Desc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Nationality.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Nationality sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.Nationality.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveOccupation(CustomerOccupation src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Desc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.CustomerOccupation.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Customer Occupation sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.CustomerOccupation.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveCustomerType(CustomerType src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Desc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.CustomerType.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Customer Type sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.CustomerType.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveIdType(IdType src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Desc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.IdType.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Id Type sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.IdType.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveCallScript(CallScript src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.CsDesc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(src.CsScript)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(src.AccdMin)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(src.AccdMax)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.CallScript.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Call script sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.CallScript.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveReason(Reason src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Name)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Reason.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Reason sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.Reason.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveGender(Gender src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Desc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Gender.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Gender sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.Gender.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveIncomeType(IncomeType src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Desc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.IncomeType.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Income Type sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.IncomeType.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveMaritalStatus(MaritalStatus src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Desc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.MaritalStatus.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Marital Status sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.MaritalStatus.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveProvinsi(Provinsi src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Provinsi.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Provinsi sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.Provinsi.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveCity(CityNewCodeRequestBean src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(src.ProvinsiCode)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Provinsi.Where(q => q.Code!.Equals(src.ProvinsiCode)).Where(q => q.Status!.Name.Equals("AKTIF")).ToList();
            if (cnt.Count < 1)
            {
                wrap.Message = "Provinsi tidak ada di sistem";
                return wrap;
            }

            var cnt1 = this.ctx.City.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt1 > 0)
            {
                wrap.Message = "Kota tidak ada di sistem";
                return wrap;
            }

            var pv = cnt[0];
            var city = new City();

            IlKeiCopyObject.Instance.WithSource(src).WithDestination(city)
                .Include(nameof(src.Code))
                .Include(nameof(src.Name))
                .Include(nameof(src.KodeWilSikp))
                .Execute();
            city.ProvinsiId = pv.Id;

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            city.Status = sg;
            this.toolService.EnrichProcessSave(city);

            this.ctx.City.Add(city);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveKecamatan(KecamatanNewCodeRequestBean src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(src.CityCode)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.City.Where(q => q.Code!.Equals(src.CityCode)).Where(q => q.Status!.Name.Equals("AKTIF")).ToList();
            if (cnt.Count < 1)
            {
                wrap.Message = "Kota tidak ada di sistem";
                return wrap;
            }

            var cnt1 = this.ctx.Kecamatan.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt1 > 0)
            {
                wrap.Message = "KEcamatan tidak ada di sistem";
                return wrap;
            }

            var pv = cnt[0];
            var kec = new Kecamatan();

            IlKeiCopyObject.Instance.WithSource(src).WithDestination(kec)
                .Include(nameof(src.Code))
                .Include(nameof(src.Name))
                .Execute();
            kec.CityId = pv.Id;

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            kec.Status = sg;
            this.toolService.EnrichProcessSave(kec);

            this.ctx.Kecamatan.Add(kec);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveKelurahan(KelurahanNewCodeRequestBean src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(src.KecamatanCode)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Kecamatan.Where(q => q.Code!.Equals(src.KecamatanCode)).Where(q => q.Status!.Name.Equals("AKTIF")).ToList();
            if (cnt.Count < 1)
            {
                wrap.Message = "Kecamatan tidak ada di sistem";
                return wrap;
            }

            var cnt1 = this.ctx.Kelurahan.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt1 > 0)
            {
                wrap.Message = "Kelurahan tidak ada di sistem";
                return wrap;
            }

            var pv = cnt[0];
            var kel = new Kelurahan();

            IlKeiCopyObject.Instance.WithSource(src).WithDestination(kel)
                .Include(nameof(src.Code))
                .Include(nameof(src.Name))
                .Include(nameof(src.KdDkcplKelurahan))
                .Execute();
            kel.KecamatanId = pv.Id;

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            kel.Status = sg;
            this.toolService.EnrichProcessSave(kel);

            this.ctx.Kelurahan.Add(kel);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveProduct(Product src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Desc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Product.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Produk sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.Product.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveArea(Area src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Area.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Area sudah ada di sistem";
                return wrap;
            }

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            src.Status = sg;
            this.toolService.EnrichProcessSave(src);

            this.ctx.Area.Add(src);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveUser(UserNewRequestBean src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Userid)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.Groupid)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(src.SuBranchCode)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var gs = this.ctx.Role.Where(q => q.Name.Equals(src.Groupid)).ToList();
            if (gs == null || gs.Count() < 1)
            {
                wrap.Message = "Role tidak ada di sistem";
                return wrap;
            }

            var cnt1 = this.ctx.User.Where(q => q.Username!.Equals(src.Userid)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt1 > 0)
            {
                wrap.Message = "User tidak ada di sistem";
                return wrap;
            }

            var user = new User();
            user.Username = src.Userid;
            user.Email = src.SuEmail;
            user.Password = "e10adc3949ba59abbe56e057f20f883e";
            user.RoleId = gs[0].Id;

            this.ctx.User.Add(user);
            this.ctx.SaveChanges();

            char[] sep = { ',' };
            var br = src.SuBranchCode!.Split(sep);
            foreach (var b in br)
            {
                var bt = this.ctx.Branch.Where(q => q.Code!.Equals(b)).FirstOrDefault();
                if (bt != null)
                {
                    var ub = new UserBranch();
                    ub.BranchId = bt.Id;
                    ub.UserId = user.Id;

                    this.ctx.UserBranch.Add(ub);
                }
            }


            var sg = this.statusService.GetStatusGeneral("AKTIF");
            user.Status = sg;
            this.toolService.EnrichProcessSave(user);

            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveTeam(TeamNewCodeRequestBean src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Spv)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Pick(nameof(src.BranchCode)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            if (src.Member == null || src.Member.Count() < 1)
            {
                wrap.Message = "Team member adalah mandatory";
                return wrap;
            }

            var brs = this.ctx.Branch.Where(q => q.Code!.Equals(src.BranchCode)).Where(q => q.Status.Name.Equals("AKTIF")).ToList();
            if (brs == null || brs.Count() < 1)
            {
                wrap.Message = "Branch tidak ada di sistem";
                return wrap;
            }
            var br = brs[0];

            var spvs = this.ctx.User.Where(q => q.Username!.Equals(src.Spv)).Where(q => q.Status!.Name.Equals("AKTIF"))
                .Include(i => i.Role).Include(i => i.Branch).ToList();
            if (spvs == null || spvs.Count() < 1)
            {
                wrap.Message = "Supervisor tidak ada di sistem";
                return wrap;
            }

            var spv = spvs[0];
            if (spv.Role!.Name != "SPVDC" && spv.Role.Name != "SPVFC")
            {
                wrap.Message = "User bukan supervisor tidak ada di sistem";
                return wrap;
            }

            bool found = false;
            foreach(var t in spv.Branch!)
            {
                if (t.BranchId == br.Id)
                {
                    found = true;
                    break;
                }
            }

            if (found  == false)
            {
                wrap.Message = "Branch supervisor tidak sama";
                return wrap;
            }

            var mems = new List<User>();
            foreach(var mm in src.Member)
            {
                var tms = this.ctx.User.Where(q => q.Username!.Equals(mm)).Where(q => q.Status!.Name.Equals("AKTIF")).Include(i => i.Branch).ToList();
                if (tms == null || tms.Count() < 1)
                {
                    wrap.Message = "Member: " + mm + " tidak ada disistem";
                    return wrap;
                }
                var tm = tms[0];
                found = false;
                foreach (var t in tm.Branch!)
                {
                    if (t.BranchId == br.Id)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    wrap.Message = "Member: " + mm + " tidak sama branch";
                    return wrap;
                }

                mems.Add(tms[0]);
            }

            if (mems.Count() < 1)
            {
                wrap.Message = "Team member tidak ada";
                return wrap;
            }

            var cnt1 = this.ctx.Team.Where(q => q.Spv!.Id.Equals(spv.Id)).Count();
            if (cnt1 > 0)
            {
                wrap.Message = "Team dengan supervisor " + spv.Username + " telah ada di sistem";
                return wrap;
            }

            var team = new Team();
            team.SpvId = spv.Id;
            team.AreaId = br.AreaId;
            team.BranchId = br.Id;

            this.ctx.Team.Add(team);
            this.ctx.SaveChanges();

            foreach(var mem in mems)
            {
                var mtem = new TeamMember();
                mtem.TeamId = team.Id;
                mtem.MemberId = mem.Id;
                this.ctx.TeamMember.Add(mtem);
            }

            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<string> SaveLoan(ApiLoanNewRequestBean src)
        {

            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };

            if (src.Loan == null)
            {
                wrap.Message = "Loan mandatory";
                return wrap;
            }

            if (src.Customer == null)
            {
                wrap.Message = "Customer mandatory";
                return wrap;
            }

            var ml = this.BuildMasterLoan(src.Loan);
            var c = this.ctx.MasterLoan.Where(q => q.Cif!.Equals(ml.Cif)).Count();
            if (c > 0)
            {
                wrap.Message = "Cif sudah ada";
                return wrap;
            }

            this.ctx.MasterLoan.Add(ml);
            this.ctx.SaveChanges();


            var cu = this.BuildCustomer(src.Customer);
            this.ctx.Customer.Add(cu);
            this.ctx.SaveChanges();

            var col = this.buildCollateral(src.Collateral!);
            if (col != null && col.Count() > 0)
            {
                foreach (var cl in col)
                {
                    cl.LoanId = ml.Id;
                    this.ctx.MasterCollateral.Add(cl);
                    this.ctx.SaveChanges();
                }
            } 


            var cv = this.buildCollectionVisit(src.Visit!);
            if (cv != null && cv.Count() > 0)
            {
                foreach (var vc in cv)
                {

                }
            }
            var ca = this.buildCollectionAddress(src.Address!);
            if (ca != null && ca.Count() > 0)
            {
                foreach (var a in ca)
                {
                    this.ctx.CollectionAddContact.Add(a);
                    this.ctx.SaveChanges();
                }
            }

            var cc = this.buildCollectionCall(src.Call!);
            if (cc != null && cc.Count() > 0)
            {
                foreach (var a in cc)
                {
                    a.LoanId = ml.Id;
                    this.ctx.CollectionCall.Add(a);
                    this.ctx.SaveChanges();
                }
            }

            var ch = this.buildCollectionHistory(src.History!);
            if (ch != null && ch.Count() > 0)
            {
                foreach (var a in ch)
                {
                    this.ctx.CollectionHistory.Add(a);
                    this.ctx.SaveChanges();
                }
            }

            ml.CustomerId = cu.Id;

            var br = cu.Branch!;
            this.ctx.Entry(br).Reference(r => r.BranchType!).Load();
            var bt = br.BranchType!;
            if (bt.Code!.Equals("UPS") || bt.Code!.Equals("CPS"))
            {
                ml.ProductSegmentId = 2;
            } else
            {
                ml.ProductSegmentId = 1;
            }

            this.ctx.MasterLoan.Update(ml);
            this.ctx.SaveChanges();

            return wrap;
        }

        private MasterLoan BuildMasterLoan(ApiMasterLoan apiLoan)
        {
            if (apiLoan == null)
            {
                return null!;
            }

            var ml = new MasterLoan();

            ml.AccNo = apiLoan.AccNo;
            ml.Ccy = apiLoan.Ccy;
            ml.Cif = apiLoan.CuCif;
            ml.Dpd = apiLoan.Dpd;
            ml.ChannelBranchCode = apiLoan.ChannelBranchCode;
            ml.EconName = apiLoan.EconName;
            ml.EconPhone = apiLoan.EconPhone;
            ml.EconRelation = apiLoan.EconRelation;
            ml.Installment = apiLoan.Installment;
            ml.InstallmentPokok = apiLoan.InstallmentPokok;
            ml.InterestRate = apiLoan.InterestRate;
            ml.KewajibanTotal = apiLoan.KewajibanTotal;
            ml.Kolektibilitas = apiLoan.Kolektibilitas;
            ml.LastPayDate = apiLoan.LastPayDate;
            ml.MarketingCode = apiLoan.MarketingCode;
            ml.MaturityDate = apiLoan.MaturityDate;
            ml.Outstanding = apiLoan.Outstanding;
            ml.PayTotal = apiLoan.PayTotal;
            ml.Plafond = apiLoan.Plafond;
            ml.SisaTenor = apiLoan.SisaTenor;
            ml.StartDate = apiLoan.StartDate;
            ml.Tenor = apiLoan.Tenor;
            ml.TunggakanBunga = apiLoan.TunggakanBunga;
            ml.TunggakanDenda = apiLoan.TunggakanDenda;
            ml.TunggakanPokok = apiLoan.TunggakanPokok;
            ml.TunggakanTotal = apiLoan.TunggakanTotal;

            var pr = this.ctx.Product.Where(q => q.Code!.Equals(apiLoan.Product)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (pr != null)
            {
                ml.Product = pr;
            }

            return ml;
        }

        private Customer BuildCustomer(ApiMasterCustomer apiCustomer)
        {
            if (apiCustomer == null)
            {
                return null!;
            }

            var cu = new Customer();
            cu.Address = apiCustomer.CuAddress;
            cu.Idnumber = apiCustomer.CuIdnumber;
            cu.CuIncome = apiCustomer.CuIncome;
            cu.BornDate = apiCustomer.CuBorndate;
            cu.BornPlace = apiCustomer.CuBornplace;
            cu.Cif = apiCustomer.CuCif;
            cu.Company = apiCustomer.CuCompany;
            cu.Email = apiCustomer.CuName;
            cu.HmPhone = apiCustomer.CuHmphone;
            cu.Idnumber = apiCustomer.CuIdnumber;
            cu.MobilePhone = apiCustomer.CuMobilephone;
            cu.Name = apiCustomer.CuName;
            cu.Rt = apiCustomer.CuRt;
            cu.Rw = apiCustomer.CuRw;
            cu.ZipCode = apiCustomer.CuZipcode;

            var idType = this.ctx.IdType.Where(q => q.Code!.Equals(apiCustomer.CuIdtype)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (idType != null)
            {
                cu.IdTypeId = idType.Id;
            }

            var gender = this.ctx.Gender.Where(q => q.Code!.Equals(apiCustomer.CuGender)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (gender != null)
            {
                cu.GenderId = gender.Id;
            }

            var ms = this.ctx.MaritalStatus.Where(q => q.Code!.Equals(apiCustomer.CuMaritalstatus)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (ms != null)
            {
                cu.MaritalStatusId = ms.Id;
            }

            var nat = this.ctx.Nationality.Where(q => q.Code!.Equals(apiCustomer.CuNationality)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (nat != null)
            {
                cu.NationalityId = nat.Id;
            }

            var inc = this.ctx.IncomeType.Where(q => q.Code!.Equals(apiCustomer.CuIncometype)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (inc != null)
            {
                cu.IncomeTypeId = inc.Id;
            }

            var cust = this.ctx.CustomerType.Where(q => q.Code!.Equals(apiCustomer.CuCusttype)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (cust != null)
            {
                cu.CustomerTypeId = cust.Id;
            }

            var occ = this.ctx.CustomerOccupation.Where(q => q.Code!.Equals(apiCustomer.CuOccupation)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (occ != null)
            {
                cu.CustomerOccupationId = occ.Id;
            }

            var prv = this.ctx.Provinsi.Where(q => q.Code!.Equals(apiCustomer.CuProvinsi)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (prv != null)
            {
                cu.ProvinsiId = prv.Id;
            }

            var ct = this.ctx.City.Where(q => q.Code!.Equals(apiCustomer.CuCity)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (ct != null)
            {
                cu.CityId = ct.Id;
            }

            var kec = this.ctx.Kecamatan.Where(q => q.Code!.Equals(apiCustomer.CuKecamatan)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (kec != null)
            {
                cu.KecamatanId = kec.Id;
            }

            var kel = this.ctx.Kecamatan.Where(q => q.Code!.Equals(apiCustomer.CuKelurahan)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (kel != null)
            {
                cu.KelurahanId = kel.Id;
            }

            var br = this.ctx.Branch.Where(q => q.Code!.Equals(apiCustomer.Branchid)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
            if (br != null)
            {
                cu.BranchId = br.Id;
            }

            return cu;
        }

        public IList<MasterCollateral> buildCollateral(IList<ApiMasterCollateral> src)
        {
            if (src == null || src.Count() < 1)
            {
                return null!;
            }

            var res = new List<MasterCollateral>();

            foreach(var o in src)
            {
                var mc = new MasterCollateral();
                mc.ColId = o.ColId;
                mc.ColType = o.ColType;
                mc.VehBpkbName = o.VehBpkbName;
                mc.VehBuildYear = o.VehBuildYear;
                mc.VehBpkbNo = o.VehBpkbNo;
                mc.VehCc = o.VehCc;
                mc.VehChasisNo = o.VehChasisNo;
                mc.VehColor = o.VehColor;
                mc.VehEngineNo = o.VehEngineNo;
                mc.VehMerek = o.VehMerek;
                mc.VehModel = o.VehModel;
                mc.VehPlateNo = o.VehPlateNo;
                mc.VehStnkNo = o.VehStnkNo;
                mc.VehYear = o.VehYear;
                res.Add(mc);
            }

            return res;
        }

        public IList<CollectionVisit> buildCollectionVisit(IList<ApiCollectionVisit> src)
        {
            if (src == null || src.Count() < 1)
            {
                return null!;
            }

            var res = new List<CollectionVisit>();

            foreach (var o in src)
            {
                var mc = new CollectionVisit();
                IlKeiCopyObject.Instance.WithSource(o).WithDestination(mc)
                    .Exclude(nameof(o.Branchid))
                    .Exclude(nameof(o.VisitBy))
                    .Execute();
                res.Add(mc);
            }

            return res;
        }

        public IList<CollectionAddContact> buildCollectionAddress(IList<ApiCollectionAddContact> src)
        {
            if (src == null || src.Count() < 1)
            {
                return null!;
            }

            var res = new List<CollectionAddContact>();

            foreach (var o in src)
            {
                var mc = new CollectionAddContact();
                IlKeiCopyObject.Instance.WithSource(o).WithDestination(mc)
                    .Exclude(nameof(o.AddBy))
                    .Execute();

                var addBy = this.ctx.User.Where(q => q.Username!.Equals(o.AddBy)).Where(q => q.Status.Name.Equals("AKTIF")).FirstOrDefault();
                if (addBy != null)
                {
                    mc.AddBy = addBy;
                }
                res.Add(mc);
            }

            return res;
        }

        public IList<CollectionCall> buildCollectionCall(IList<ApiCollectionCall> src)
        {
            if (src == null || src.Count() < 1)
            {
                return null!;
            }

            var res = new List<CollectionCall>();

            foreach (var o in src)
            {
                var mc = new CollectionCall();
                IlKeiCopyObject.Instance.WithSource(o).WithDestination(mc)
                    .Exclude(nameof(o.Branchid))
                    .Exclude(nameof(o.CallResult))
                    .Exclude(nameof(o.CallBy))
                    .Exclude(nameof(o.CallReason))
                    .Exclude(nameof(o.AddId))
                    .Execute();

                var br = this.ctx.Branch.Where(q => q.Code!.Equals(o.Branchid)).Where(q => q.Status!.Name.Equals("AKTIF")).FirstOrDefault();
                if (br != null)
                {
                    mc.BranchId = br.Id;
                }

                var add = this.ctx.CollectionAddContact.Where(q => q.AddId!.Equals(o.AddId)).FirstOrDefault();
                if (add != null)
                {
                    mc.CollectionAdd = add;
                }

                var resc = this.ctx.CallResult.Where(q => q.Code!.Equals(o.CallResult)).FirstOrDefault();
                if (resc != null)
                {
                    mc.CallResult = resc;
                }

                var resr = this.ctx.Reason.Where(q => q.Code!.Equals(o.CallReason)).FirstOrDefault();
                if (resr != null)
                {
                    mc.Reason = resr;
                }

                var cb = this.ctx.User.Where(q => q.Username!.Equals(o.CallBy)).FirstOrDefault();
                if (cb != null)
                {
                    mc.CallBy = cb;
                }

                res.Add(mc);
            }

            return res;
        }

        public IList<CollectionHistory> buildCollectionHistory(IList<ApiCollectionHistory> src)
        {
            if (src == null || src.Count() < 1)
            {
                return null!;
            }

            var res = new List<CollectionHistory>();

            foreach (var o in src)
            {
                var mc = new CollectionHistory();
                IlKeiCopyObject.Instance.WithSource(o).WithDestination(mc)
                    .Exclude(nameof(o.BranchId))
                    .Exclude(nameof(o.ReasonId))
                    .Exclude(nameof(o.ResultId))
                    .Exclude(nameof(o.AddId))
                    .Exclude(nameof(o.HistoryById))
                    .Execute();

                var br = this.ctx.Branch.Where(q => q.Code!.Equals(o.BranchId)).Where(q => q.Status!.Name!.Equals("AKTIF")).FirstOrDefault();
                if (br != null)
                {
                    mc.BranchId = br.Id;
                }

                var cc = this.ctx.CollectionCall.Where(q => q.AccNo!.Equals(o.AccNo)).FirstOrDefault();
                if (cc != null)
                {
                    mc.Call = cc; 
                }

                var add = this.ctx.CollectionAddContact.Where(q => q.AddId!.Equals(o.AddId)).FirstOrDefault();
                if (add != null)
                {
                    mc.CollectionAdd = add;
                }

                var resc = this.ctx.CallResult.Where(q => q.Code!.Equals(o.ResultId)).FirstOrDefault();
                if (resc != null)
                {
                    mc.Result = resc;
                }

                var resr = this.ctx.Reason.Where(q => q.Code!.Equals(o.ReasonId)).FirstOrDefault();
                if (resr != null)
                {
                    mc.Reason = resr;
                }

                var cb = this.ctx.User.Where(q => q.Username!.Equals(o.HistoryById)).FirstOrDefault();
                if (cb != null)
                {
                    mc.HistoryBy = cb;
                }

                res.Add(mc);
            }

            return res;
        }

        public GenericResponse<string> SavePaymentHistory(PaymentHistoryBean src)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.AccNo)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.MasterLoan.Where(q => q.AccNo!.Equals(src.AccNo)).FirstOrDefault();
            if (cnt == null)
            {
                wrap.Message = "Loan tidak ada di sistem";
                return wrap;
            }

            var ph = new PaymentHistory();
            IlKeiCopyObject.Instance.WithSource(src).WithDestination(ph)
                    .Include(nameof(src.AccNo))
                    .Include(nameof(src.Bunga))
                    .Include(nameof(src.PokokCicilan))
                    .Include(nameof(src.Denda))
                    .Include(nameof(src.Tgl))
                    .Include(nameof(src.TotalBayar))
                    .Execute();
            ph.LoanId = cnt.Id;

            this.ctx.PaymentHistory.Add(ph);
            this.ctx.SaveChanges();

            wrap.Status = true;


            return wrap;
        }

        public string Terbilang(long a)
        {
            string[] bilangan = new string[] { "", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan", "Sepuluh", "Sebelas" };
            var kalimat = "";
            // 1 - 11
            if (a < 12)
            {
                kalimat = bilangan[a];
            }
            // 12 - 19
            else if (a < 20)
            {
                kalimat = bilangan[a - 10] + " Belas";
            }
            // 20 - 99
            else if (a < 100)
            {
                var utama = a / 10;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 10;
                kalimat = bilangan[depan] + " Puluh " + bilangan[belakang];
            }
            // 100 - 199
            else if (a < 200)
            {
                kalimat = "Seratus " + Terbilang(a - 100);
            }
            // 200 - 999
            else if (a < 1000)
            {
                var utama = a / 100;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 100;
                kalimat = bilangan[depan] + " Ratus " + Terbilang(belakang);
            }
            // 1,000 - 1,999
            else if (a < 2000)
            {
                kalimat = "Seribu " + Terbilang(a - 1000);
            }
            // 2,000 - 9,999
            else if (a < 10000)
            {
                var utama = a / 1000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 1000;
                kalimat = bilangan[depan] + " Ribu " + Terbilang(belakang);
            }
            // 10,000 - 99,999
            else if (a < 100000)
            {
                var utama = a / 100;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                var belakang = a % 1000;
                kalimat = Terbilang(depan) + " Ribu " + Terbilang(belakang);
            }
            // 100,000 - 999,999
            else if (a < 1000000)
            {
                var utama = a / 1000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                var belakang = a % 1000;
                kalimat = Terbilang(depan) + " Ribu " + Terbilang(belakang);
            }
            // 1,000,000 - 	99,999,999
            else if (a < 100000000)
            {
                var utama = a / 1000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));//Substring(0, 4));
                var belakang = a % 1000000;
                kalimat = Terbilang(depan) + " Juta " + Terbilang(belakang);
            }
            else if (a < 1000000000)
            {
                var utama = a / 1000000;
                var sutama = utama.ToString();
                int depan = 0;
                if (sutama.Length > 3)
                {
                    depan = Convert.ToInt32(utama.ToString().Substring(0, 4));
                }
                else
                {
                    depan = Convert.ToInt32(utama.ToString());
                }

                var belakang = a % 1000000;
                kalimat = Terbilang(depan) + " Juta " + Terbilang(belakang);
            }
            else if (a < 10000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 100000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 1000000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 10000000000000)
            {
                var utama = a / 10000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 10000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }
            else if (a < 100000000000000)
            {
                var utama = a / 1000000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 2));
                var belakang = a % 1000000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }

            else if (a < 1000000000000000)
            {
                var utama = a / 1000000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 3));
                var belakang = a % 1000000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }

            else if (a < 10000000000000000)
            {
                var utama = a / 1000000000000000;
                var depan = Convert.ToInt32(utama.ToString().Substring(0, 1));
                var belakang = a % 1000000000000000;
                kalimat = Terbilang(depan) + " Kuadriliun " + Terbilang(belakang);
            }

            var pisah = kalimat.Split(' ');
            List<string> full = new List<string>();// = [];
            for (var i = 0; i < pisah.Length; i++)
            {
                if (pisah[i] != "") { full.Add(pisah[i]); }
            }
            return CombineTerbilang(full.ToArray());// full.Concat(' '); .join(' ');
        }
        private string CombineTerbilang(string[] arr)
        {
            return string.Join(" ", arr);
        }
    }
}
