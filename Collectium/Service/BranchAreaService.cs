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
    public class BranchAreaService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<BranchAreaService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BranchAreaService(CollectiumDBContext ctx,
                                ILogger<BranchAreaService> logger,
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

        public GenericResponse<AreaCreateBean> SaveArea(AreaCreateBean filter)
        {
            var wrap = new GenericResponse<AreaCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = mapper.Map<Area>(filter);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            this.toolService.EnrichProcessSave(us);

            this.ctx.Area.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<BranchTypeCreateBean> SaveBranchType(BranchTypeCreateBean filter)
        {
            var wrap = new GenericResponse<BranchTypeCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = mapper.Map<BranchType>(filter);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            this.toolService.EnrichProcessSave(us);

            this.ctx.BranchType.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        
        public GenericResponse<BranchResponseBean> ListBranch(BranchListRequestBean filter)
        {
            var wrap = new GenericResponse<BranchResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<Branch> q = this.ctx.Set<Branch>().Include(i => i.Status).Include(i => i.BranchType)
                                    .Include(i => i.Area);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<Branch>();
                predicate = predicate.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.City!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.Phone!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            q = q.Where(q => q.Status!.Name!.Equals("AKTIF")).OrderByDescending(q => q.Id);

            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();


            var ldata = new List<BranchResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<BranchResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<BranchResponseBean> DetailBranch(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<BranchResponseBean>
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Id)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var obj = this.ctx.Action.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Branch tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();

            if (obj.Status!.Name != "AKTIF")
            {
                wrap.Message = "Branch tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<BranchResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<BranchReqResponseBean> DetailBranchRequest(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<BranchReqResponseBean>
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Id)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var obj = this.ctx.BranchRequest.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Branch Request tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();
            this.ctx.Entry(obj).Reference(r => r.Branch).Load();

            if (obj.Status!.Name.Equals("REMOVE"))
            {
                wrap.Message = "Branch Request tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<BranchReqResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<BranchCreateBean> SaveBranch(BranchCreateBean filter)
        {
            var wrap = new GenericResponse<BranchCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Pic)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.AreaId)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.BranchTypeId)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = mapper.Map<Branch>(filter);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            this.toolService.EnrichProcessSave(us);

            this.ctx.Branch.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<BranchIntegrationCreateBean> SaveBranchIntegration(BranchIntegrationCreateBean src)
        {
            var wrap = new GenericResponse<BranchIntegrationCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(src)
                .Pick(nameof(src.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(src.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(src.BrType)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Branch.Where(q => q.Code!.Equals(src.Code)).Where(q => q.Status!.Name.Equals("AKTIF")).Count();
            if (cnt > 0)
            {
                wrap.Message = "Area sudah ada di sistem";
                return wrap;
            }
            var us = mapper.Map<Branch>(src);

            if (src.Areaid != null)
            {
                var area = this.ctx.Area.Where(q => q.Code!.Equals(src.Areaid)).Where(q => q.Status!.Name.Equals("AKTIF")).ToList();
                if (area == null || area.Count() < 1)
                {
                    wrap.Message = "Area tidak ada di sistem";
                    return wrap;
                }
                var aarea = area[0];
                us.AreaId = aarea.Id;
            }


            var bt = this.ctx.BranchType.Where(q => q.Code!.Equals(src.BrType)).Where(q => q.Status!.Name.Equals("AKTIF")).ToList();
            if (bt == null || bt.Count() < 1)
            {
                wrap.Message = "Branch Type tidak ada di sistem";
                return wrap;
            }
            var bbt = bt[0];

            var cco = this.ctx.Branch.Where(q => q.Code!.Equals(src.Ccobranch)).Where(q => q.Status!.Name.Equals("AKTIF")).ToList();

            us.BranchTypeId = bbt.Id;
            //if (cco != null && cco.Count() >0)
            //{
            //    var ccco = cco[0];
            //    us.BranchCcoId = ccco.Id;
            //}

            if (src.BrType!.Equals("CPP") || src.BrType!.Equals("UPC"))
            {
                us.ProductSegmentId = 1;
            } else if (src.BrType.Equals("CPS") || src.BrType.Equals("UPS"))
            {
                us.ProductSegmentId = 1;
            }


            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            this.toolService.EnrichProcessSave(us);

            this.ctx.Branch.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(src);

            return wrap;
        }

        public GenericResponse<BranchReqResponseBean> ListBranchRequest(BranchReqListRequestBean filter)
        {
            var wrap = new GenericResponse<BranchReqResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<BranchRequest> q = this.ctx.Set<BranchRequest>().Include(i => i.Branch).Include(i => i.Status).Include(i => i.BranchType)
                                    .Include(i => i.BranchCco).Include(i => i.Area);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<BranchRequest>();
                predicatek = predicatek.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.City!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Phone!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }

            var predicate = PredicateBuilder.New<BranchRequest>();
            predicate = predicate.Or(o => o.Status!.Name.Equals("DRAFT"));
            predicate = predicate.Or(o => o.Status!.Name.Equals("APPROVE"));
            q = q.Where(predicate);


            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<BranchReqResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<BranchReqResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<BranchReqCreateBean> SaveNewActionRequest(BranchReqCreateBean filter)
        {
            var wrap = new GenericResponse<BranchReqCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Pic)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.AreaId)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.BranchTypeId)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var us = mapper.Map<BranchRequest>(filter);
            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;

            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.BranchRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<BranchReqEditCreateBean> SaveEditActionRequest(BranchReqEditCreateBean filter)
        {
            var wrap = new GenericResponse<BranchReqEditCreateBean>
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Pic)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.AreaId)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.BranchTypeId)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.BranchId)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var us = mapper.Map<BranchRequest>(filter);

            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;

            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.BranchRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        public GenericResponse<UserReqApproveBean> ApproveBranchRequest(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<UserReqApproveBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Id)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var appUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (appUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var us = this.ctx.BranchRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "Branch Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();
            this.ctx.Entry(us).Reference(r => r.Branch).Load();

            if (us.Branch != null)
            {
                this.ctx.Entry(us.Branch!).Reference(r => r.Status).Load();

                if (us.Branch!.Status!.Name != "AKTIF")
                {
                    wrap.Message = "Status Branch tidak AKTIF";
                    return wrap;
                }
            }

            if (us.Status!.Name != "DRAFT")
            {
                wrap.Message = "Status Action Request tidak valid";
                return wrap;
            }



            if (us.Branch == null)
            {
                var ucb = mapper.Map<BranchCreateBean>(us);
                this.SaveBranch(ucb);
            }
            else
            {
                var pus = us.Branch;
                IlKeiCopyObject.Instance.WithSource(us).WithDestination(pus)
                                .Include(nameof(us.Name))
                                .Include(nameof(us.BranchTypeId))
                                .Include(nameof(us.Phone))
                                .Include(nameof(us.Fax))
                                .Include(nameof(us.Addr1))
                                .Include(nameof(us.Addr2))
                                .Include(nameof(us.Addr3))
                                .Include(nameof(us.City))
                                .Include(nameof(us.Zipcode))
                                .Include(nameof(us.AreaId))
                                .Include(nameof(us.BranchCcoId))
                                .Include(nameof(us.CoreCode))
                                .Include(nameof(us.Pic))
                                .Include(nameof(us.Email))
                                .Include(nameof(us.Norek))
                                .Include(nameof(us.Amount))
                                .Execute();
                this.ctx.Branch.Update(pus);

                var sr = this.statusService.GetStatusRequest("REMOVE");

                var prevReq = this.ctx.BranchRequest.Where(q => q.Id < filter.Id).Where(q => q.Branch!.Id.Equals(pus.Id)).Where(q => q.Status!.Name.Equals("DRAFT")).ToList();
                if (prevReq != null && prevReq.Count > 0)
                {
                    foreach (var item in prevReq)
                    {
                        item.StatusId = sr.Id;
                        this.ctx.BranchRequest.Update(item);
                    }
                }
            }

            var sa = this.statusService.GetStatusRequest("APPROVE");
            us.StatusId = sa.Id;

            this.toolService.EnrichProcessApproveRequest(us);

            this.ctx.BranchRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }
    }
}
