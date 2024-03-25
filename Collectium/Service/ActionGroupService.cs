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
using System.Text;
using Action = Collectium.Model.Entity.Action;

namespace Collectium.Service
{
    public class ActionGroupService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<ActionGroupService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ActionGroupService(CollectiumDBContext ctx,
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

        public GenericResponse<ActionResponseBean> ListAction(ActionListRequestBean filter)
        {
            var wrap = new GenericResponse<ActionResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<Action> q = this.ctx.Set<Action>().Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<Action>();
                predicate = predicate.Or(p => p.ActCode!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.ActDesc!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            var pagination = this.pagination.getPagination(filter);
            var data = q.Where(q => q.Status!.Name!.Equals("AKTIF")).Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<ActionResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<ActionResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ActionResponseBean> DetailAction(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<ActionResponseBean>
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
                wrap.Message = "Action tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();

            if (obj.Status!.Name != "AKTIF")
            {
                wrap.Message = "Action tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<ActionResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ActionReqResponseBean> DetailActionRequest(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<ActionReqResponseBean>
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

            var obj = this.ctx.ActionRequest.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Action Request tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();
            this.ctx.Entry(obj).Reference(r => r.Action).Load();

            if (obj.Status!.Name.Equals("REMOVE"))
            {
                wrap.Message = "Action Request tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<ActionReqResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ActionCreateBean> SaveAction(ActionCreateBean filter)
        {
            var wrap = new GenericResponse<ActionCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.ActCode)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.ActDesc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = mapper.Map<Action>(filter);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            this.toolService.EnrichProcessSave(us);

            this.ctx.Action.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<ActionReqResponseBean> ListActionRequest(ActionReqListRequestBean filter)
        {
            var wrap = new GenericResponse<ActionReqResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<ActionRequest> q = this.ctx.Set<ActionRequest>().Include(i => i.Action).Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<ActionRequest>();
                predicatek = predicatek.Or(p => p.ActCode!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.ActDesc!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }

            var predicate = PredicateBuilder.New<ActionRequest>();
            predicate = predicate.Or(o => o.Status!.Name.Equals("DRAFT"));
            predicate = predicate.Or(o => o.Status!.Name.Equals("APPROVE"));
            q = q.Where(predicate);


            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<ActionReqResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<ActionReqResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ActionReqCreateBean> SaveNewActionRequest(ActionReqCreateBean filter)
        {
            var wrap = new GenericResponse<ActionReqCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.ActCode)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.ActDesc)).IsMandatory().AsString().WithMinLen(2).Pack()
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

            var us = mapper.Map<ActionRequest>(filter);
            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;
            
            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.ActionRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<ActionReqEditBean> SaveEditActionRequest(ActionReqEditBean filter)
        {
            var wrap = new GenericResponse<ActionReqEditBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.ActCode)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.ActDesc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.ActionId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
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

            var us = mapper.Map<ActionRequest>(filter);

            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;

            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.ActionRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        public GenericResponse<UserReqApproveBean> ApproveActionRequest(UserReqApproveBean filter)
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

            var us = this.ctx.ActionRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "Action Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();
            this.ctx.Entry(us).Reference(r => r.Action).Load();

            if (us.Action != null)
            {
                this.ctx.Entry(us.Action!).Reference(r => r.Status).Load();

                if (us.Action!.Status!.Name != "AKTIF")
                {
                    wrap.Message = "Status Action tidak AKTIF";
                    return wrap;
                }
            }

            if (us.Status!.Name != "DRAFT")
            {
                wrap.Message = "Status Action Request tidak valid";
                return wrap;
            }



            if (us.Action == null)
            {
                var ucb = mapper.Map<ActionCreateBean>(us);
                this.SaveAction(ucb);
            }
            else
            {
                var pus = us.Action;
                IlKeiCopyObject.Instance.WithSource(us).WithDestination(pus)
                                .Include(nameof(us.ActCode))
                                .Include(nameof(us.ActDesc))
                                .Include(nameof(us.CoreCode))
                                .Execute();
                this.ctx.Action.Update(pus);

                var sr = this.statusService.GetStatusRequest("REMOVE");

                var prevReq = this.ctx.ActionRequest.Where(q => q.Id < filter.Id).Where(q => q.Action!.Id.Equals(pus.Id)).Where(q => q.Status!.Name.Equals("DRAFT")).ToList();
                if (prevReq != null && prevReq.Count > 0)
                {
                    foreach (var item in prevReq)
                    {
                        item.StatusId = sr.Id;
                        this.ctx.ActionRequest.Update(item);
                    }
                }
            }

            var sa = this.statusService.GetStatusRequest("APPROVE");
            us.StatusId = sa.Id;

            this.toolService.EnrichProcessApproveRequest(us);

            this.ctx.ActionRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        //ACTION GROUP
        public GenericResponse<ActionGroupResponseBean> ListActionGroup(ActionGroupListRequestBean filter)
        {
            var wrap = new GenericResponse<ActionGroupResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<ActionGroup> q = this.ctx.Set<ActionGroup>().Include(i => i.Action).Include(i => i.Role).Include(i => i.Role);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<ActionGroup>();
                predicatek = predicatek.Or(p => p.Action!.ActCode!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Action!.ActDesc!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Role!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }
            if (filter.ActionId != null && filter.ActionId > 0)
            {
                q = q.Where(q => q.Action!.Id.Equals(filter.ActionId));
            }
            if (filter.RoleId != null && filter.RoleId > 0)
            {
                q = q.Where(q => q.Role!.Id.Equals(filter.RoleId));
            }

            var pagination = this.pagination.getPagination(filter);
            var data = q.Where(q => q.Status!.Name.Equals("AKTIF")).Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<ActionGroupResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<ActionGroupResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ActionGroupResponseBean> DetailActionGroup(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<ActionGroupResponseBean>
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

            var obj = this.ctx.ActionGroup.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Action Group tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();
            this.ctx.Entry(obj).Reference(r => r.Action).Load();
            this.ctx.Entry(obj).Reference(r => r.Role).Load();

            if (obj.Status!.Name != "AKTIF")
            {
                wrap.Message = "Action Group tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<ActionGroupResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ActionGroupCreateBean> SaveActionGroup(ActionGroupCreateBean filter)
        {
            var wrap = new GenericResponse<ActionGroupCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.ActionId)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.RoleId)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = mapper.Map<ActionGroup>(filter);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;

            this.toolService.EnrichProcessSave(us);

            this.ctx.ActionGroup.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<ActionGroupReqResponseBean> ListActionGroupRequest(ActionGroupReqListRequestBean filter)
        {
            var wrap = new GenericResponse<ActionGroupReqResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<ActionGroupRequest> q = this.ctx.Set<ActionGroupRequest>().Include(i => i.ActionGroup).Include(i => i.Action).Include(i => i.Role).Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<ActionGroupRequest>();
                predicatek = predicatek.Or(p => p.Action!.ActCode!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Action!.ActDesc!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Role!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }

            var predicate = PredicateBuilder.New<ActionGroupRequest>();
            predicate = predicate.Or(o => o.Status!.Name.Equals("DRAFT"));
            predicate = predicate.Or(o => o.Status!.Name.Equals("APPROVE"));
            q = q.Where(predicate);


            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<ActionGroupReqResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<ActionGroupReqResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ActionGroupReqResponseBean> DetailActionGroupRequest(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<ActionGroupReqResponseBean>
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

            var obj = this.ctx.ActionGroupRequest.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Action Group Request tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();
            this.ctx.Entry(obj).Reference(r => r.Action).Load();
            this.ctx.Entry(obj).Reference(r => r.Role).Load();
            this.ctx.Entry(obj).Reference(r => r.ActionGroup).Load();

            if (obj.Status!.Name.Equals("REMOVE"))
            {
                wrap.Message = "Action Group Request tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<ActionGroupReqResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ActionGroupReqCreateBean> SaveNewActionGroupRequest(ActionGroupReqCreateBean filter)
        {
            var wrap = new GenericResponse<ActionGroupReqCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.ActionId)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.RoleId)).IsMandatory().AsInteger().Pack()
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

            var us = mapper.Map<ActionGroupRequest>(filter);
            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;
        
            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.ActionGroupRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<ActionGroupReqEditBean> SaveEditActionGroupRequest(ActionGroupReqEditBean filter)
        {
            var wrap = new GenericResponse<ActionGroupReqEditBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.ActionGroupId)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.ActionId)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.RoleId)).IsMandatory().AsInteger().Pack()
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

            var us = mapper.Map<ActionGroupRequest>(filter);

            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;

            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.ActionGroupRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        public GenericResponse<UserReqApproveBean> ApproveActionGroupRequest(UserReqApproveBean filter)
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

            var us = this.ctx.ActionGroupRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "Action Group Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();
            this.ctx.Entry(us).Reference(r => r.ActionGroup).Load();
            this.ctx.Entry(us).Reference(r => r.Role).Load();
            this.ctx.Entry(us).Reference(r => r.Action).Load();

            if (us.Action != null)
            {
                this.ctx.Entry(us.Action!).Reference(r => r.Status).Load();
                if (us.Action!.Status!.Name != "AKTIF")
                {
                    wrap.Message = "Status Action Group tidak AKTIF";
                    return wrap;
                }
            }

            if (us.Status!.Name != "DRAFT")
            {
                wrap.Message = "Status Action Group Request tidak valid";
                return wrap;
            }



            if (us.ActionGroup == null)
            {
                var ucb = mapper.Map<ActionGroupCreateBean>(us);
                this.SaveActionGroup(ucb);
            }
            else
            {
                var pus = us.ActionGroup;
                IlKeiCopyObject.Instance.WithSource(us).WithDestination(pus).Include(nameof(us.ActionId)).Include(nameof(us.RoleId)).Execute();
                this.ctx.ActionGroup.Update(pus);

                var sr = this.statusService.GetStatusRequest("REMOVE");

                var prevReq = this.ctx.ActionGroupRequest.Where(q => q.Id < filter.Id).Where(q => q.ActionGroup!.Id.Equals(pus.Id)).Where(q => q.Status!.Name.Equals("DRAFT")).ToList();
                if (prevReq != null && prevReq.Count > 0)
                {
                    foreach (var item in prevReq)
                    {
                        item.StatusId = sr.Id;
                        this.ctx.ActionGroupRequest.Update(item);
                    }
                }
            }

            var sa = this.statusService.GetStatusRequest("APPROVE");
            us.StatusId = sa.Id;

            this.toolService.EnrichProcessApproveRequest(us);

            this.ctx.ActionGroupRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        //ACCOUNT DISTRIBUTION
        public GenericResponse<AccDistResponseBean> ListAccDist(ActionGroupListRequestBean filter)
        {
            var wrap = new GenericResponse<AccDistResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<AccountDistribution> q = this.ctx.Set<AccountDistribution>();
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<AccountDistribution>();
                predicatek = predicatek.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.CoreCode!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }

            var pagination = this.pagination.getPagination(filter);
            var data = q.Where(q => q.Status!.Name.Equals("AKTIF")).Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<AccDistResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<AccDistResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<AccDistResponseBean> DetailAccDist(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<AccDistResponseBean>
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

            var obj = this.ctx.AccountDistribution.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Account Distribution tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();

            if (obj.Status!.Name != "AKTIF")
            {
                wrap.Message = "Account Distribution tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<AccDistResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<AccDistCreateBean> SaveAccDist(AccDistCreateBean filter)
        {
            var wrap = new GenericResponse<AccDistCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.CoreCode)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Dpd)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.DpdMax)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.DpdMin)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.MaxPtp)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = mapper.Map<AccountDistribution>(filter);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;

            this.toolService.EnrichProcessSave(us);

            this.ctx.AccountDistribution.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<AccDistReqResponseBean> ListAccDistRequest(AccDistReqListRequestBean filter)
        {
            var wrap = new GenericResponse<AccDistReqResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<AccountDistributionRequest> q = this.ctx.Set<AccountDistributionRequest>().Include(i => i.AccountDistribution).Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<AccountDistributionRequest>();
                predicatek = predicatek.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.CoreCode!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }

            var predicate = PredicateBuilder.New<AccountDistributionRequest>();
            predicate = predicate.Or(o => o.Status!.Name.Equals("DRAFT"));
            predicate = predicate.Or(o => o.Status!.Name.Equals("APPROVE"));
            q = q.Where(predicate);


            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<AccDistReqResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<AccDistReqResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<AccDistReqResponseBean> DetailAccDistRequest(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<AccDistReqResponseBean>
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

            var obj = this.ctx.AccountDistributionRequest.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Account Distribution Request tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();
            this.ctx.Entry(obj).Reference(r => r.AccountDistribution).Load();

            if (obj.Status!.Name.Equals("REMOVE"))
            {
                wrap.Message = "Account Distribution Request tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<AccDistReqResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<AccDistReqCreateBean> SaveNewAccDistRequest(AccDistReqCreateBean filter)
        {
            var wrap = new GenericResponse<AccDistReqCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.CoreCode)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Dpd)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.DpdMax)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.DpdMin)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.MaxPtp)).IsMandatory().AsInteger().Pack()
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

            var us = mapper.Map<AccountDistributionRequest>(filter);
            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;

            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.AccountDistributionRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<AccDistReqEditBean> SaveEditAccDistRequest(AccDistReqEditBean filter)
        {
            var wrap = new GenericResponse<AccDistReqEditBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.CoreCode)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Dpd)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.DpdMax)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.DpdMin)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.MaxPtp)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.AccountDistributionId)).IsMandatory().AsInteger().Pack()
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

            var us = mapper.Map<AccountDistributionRequest>(filter);

            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;

            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.AccountDistributionRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        public GenericResponse<UserReqApproveBean> ApproveAccDistRequest(UserReqApproveBean filter)
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

            var us = this.ctx.AccountDistributionRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "Account Distribution Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();
            this.ctx.Entry(us).Reference(r => r.AccountDistribution).Load();
            if (us.AccountDistribution != null)
            {
                this.ctx.Entry(us.AccountDistribution!).Reference(r => r.Status).Load();
                if (us.AccountDistribution!.Status!.Name != "AKTIF")
                {
                    wrap.Message = "Status Account Distribution Tidak AKTIF";
                    return wrap;
                }

            }
            if (us.Status!.Name != "DRAFT")
            {
                wrap.Message = "Status Account Distribution Request tidak valid";
                return wrap;
            }



            if (us.AccountDistribution == null)
            {
                var ucb = mapper.Map<AccDistCreateBean>(us);
                this.SaveAccDist(ucb);
            }
            else
            {
                var pus = us.AccountDistribution;
                IlKeiCopyObject.Instance.WithSource(us).WithDestination(pus)
                    .Include(nameof(us.CoreCode))
                    .Include(nameof(us.Code))
                    .Include(nameof(us.Name))
                    .Include(nameof(us.Dpd))
                    .Include(nameof(us.DpdMin))
                    .Include(nameof(us.DpdMax))
                    .Include(nameof(us.MaxPtp))
                    .Execute();
                this.ctx.AccountDistribution.Update(pus);

                var sr = this.statusService.GetStatusRequest("REMOVE");

                var prevReq = this.ctx.AccountDistributionRequest.Where(q => q.Id < filter.Id)
                                    .Where(q => q.AccountDistribution!.Id.Equals(pus.Id))
                                    .Where(q => q.Status!.Name.Equals("DRAFT")).ToList();
                if (prevReq != null && prevReq.Count > 0)
                {
                    foreach (var item in prevReq)
                    {
                        item.StatusId = sr.Id;
                        this.ctx.AccountDistributionRequest.Update(item);
                    }
                }
            }

            var sa = this.statusService.GetStatusRequest("APPROVE");
            us.StatusId = sa.Id;

            this.toolService.EnrichProcessApproveRequest(us);

            this.ctx.AccountDistributionRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        //GLOBAL PARAMETER
        public GenericResponse<GlobalResponseBean> ListGlobal(GlobalListRequestBean filter)
        {
            var wrap = new GenericResponse<GlobalResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<RfGlobal> q = this.ctx.Set<RfGlobal>();
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<RfGlobal>();
                predicatek = predicatek.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Val!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }

            var pagination = this.pagination.getPagination(filter);
            var data = q.Where(q => q.Status!.Name!.Equals("AKTIF")).Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<GlobalResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<GlobalResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<GlobalEditBean> DetailGlobal()
        {
            var wrap = new GenericResponse<GlobalEditBean>
            {
                Status = false,
                Message = ""
            };
            

            return wrap;
        }

        public GenericResponse<GlobalCreateBean> SaveGlobal(GlobalCreateBean filter)
        {
            var wrap = new GenericResponse<GlobalCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Val)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = mapper.Map<RfGlobal>(filter);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;

            this.toolService.EnrichProcessSave(us);

            this.ctx.RfGlobal.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        public GenericResponse<GlobalEditBean> UpdateGlobal(GlobalEditBean filter)
        {
            var wrap = new GenericResponse<GlobalEditBean>
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Id)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.Val)).IsMandatory().AsString().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var max = this.ctx.RfGlobal.Where(q => q.Id!.Equals(filter.Id)).FirstOrDefault();
            if (max == null)
            {
                wrap.Message = "Global tidak ditemukan";
                return wrap;
            }

            max.Val = filter.Val;
            this.ctx.RfGlobal.Update(max);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);
            return wrap;
        }

        public GenericResponse<GlobalEditBean> UploadCabang(FileUploadModel filter)
        {
            var wrap = new GenericResponse<GlobalEditBean>
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

            using (var reader = new StreamReader(filter.FileDetails!.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    var str = reader.ReadLine();
                    var s = str!.Split(";");
                    if (s.Length > 1)
                    {
                        if (s[0] != null && s[0].Length > 1)
                        {
                            var id = s[0];
                            var nm = s[1];
                            var core = s[2];
                            var city = s[3];
                            var addr = s[4];
                            var hp = s[5];
                            var fax = s[6];

                            var cbg = this.ctx.Branch.Where(q => q.Code!.Equals(id)).FirstOrDefault();
                            if (cbg != null)
                            {
                                cbg.Name = nm;
                                cbg.Addr1 = addr;
                                cbg.City = city;
                                cbg.CoreCode = core;
                                cbg.Phone = hp;
                                cbg.Fax = fax;
                                this.ctx.Branch.Update(cbg);
                                this.ctx.SaveChanges();
                            }
                        }


                    }

                }
            }



            return wrap;
        }

        public GenericResponse<SettingDcBean> GetSettingDc()
        {
            var wrap = new GenericResponse<SettingDcBean>
            {
                Status = false,
                Message = ""
            };

            var geb = new SettingDcBean();

            var max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("DCDPD")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                geb.Dpd = Int32.Parse(max.Val.ToString());
            }
            max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("DCDPDG")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                geb.DpdGanti = Int32.Parse(max.Val.ToString());
            }
            max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("DCCABANG")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                geb.Cabang = Int32.Parse(max.Val.ToString());
            }

            wrap.AddData(geb);
            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<SettingDcBean> SaveSettingDc(SettingDcBean src)
        {
            var wrap = new GenericResponse<SettingDcBean>
            {
                Status = false,
                Message = ""
            };


            var max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("DCDPD")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                max.Val = src.Dpd.ToString();
                this.ctx.RfGlobal.Update(max);
            } 
            else
            {
                max = new RfGlobal();
                max.Val = src.Dpd.ToString();
                max.Code = "DCDPD";
                max.Name = "DC DPD";
                max.StatusId = 1;
                this.ctx.RfGlobal.Add(max);

            }

            max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("DCDPDG")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                max.Val = src.DpdGanti.ToString();
                this.ctx.RfGlobal.Update(max);
            }
            else
            {
                max = new RfGlobal();
                max.Val = src.DpdGanti.ToString();
                max.Code = "DCDPDG";
                max.Name = "DC DPD Pindah";
                max.StatusId = 1;
                this.ctx.RfGlobal.Add(max);

            }

            max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("DCCABANG")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                max.Val = src.Cabang.ToString();
                this.ctx.RfGlobal.Update(max);
            }
            else
            {
                max = new RfGlobal();
                max.Val = src.Cabang.ToString();
                max.Code = "DCCABANG";
                max.Name = "DC Filter By Cabang";
                max.StatusId = 1;
                this.ctx.RfGlobal.Add(max);

            }

            this.ctx.SaveChanges();
            wrap.AddData(src);
            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<SettingDcBean> GetSettingFc()
        {
            var wrap = new GenericResponse<SettingDcBean>
            {
                Status = false,
                Message = ""
            };

            var geb = new SettingDcBean();

            var max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("FCDPD")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                geb.Dpd = Int32.Parse(max.Val.ToString());
            }
            max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("FCDPDG")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                geb.DpdGanti = Int32.Parse(max.Val.ToString());
            }
            max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("FCCABANG")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                geb.Cabang = Int32.Parse(max.Val.ToString());
            }

            wrap.AddData(geb);
            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<SettingDcBean> SaveSettingFc(SettingDcBean src)
        {
            var wrap = new GenericResponse<SettingDcBean>
            {
                Status = false,
                Message = ""
            };


            var max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("FCDPD")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                max.Val = src.Dpd.ToString();
                this.ctx.RfGlobal.Update(max);
            }
            else
            {
                max = new RfGlobal();
                max.Val = src.Dpd.ToString();
                max.Code = "FCDPD";
                max.Name = "FC DPD";
                max.StatusId = 1;
                this.ctx.RfGlobal.Add(max);

            }

            max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("FCDPDG")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                max.Val = src.DpdGanti.ToString();
                this.ctx.RfGlobal.Update(max);
            }
            else
            {
                max = new RfGlobal();
                max.Val = src.DpdGanti.ToString();
                max.Code = "FCDPDG";
                max.Name = "FC DPD Pindah";
                max.StatusId = 1;
                this.ctx.RfGlobal.Add(max);

            }

            max = this.ctx.RfGlobal.Where(q => q.Code!.Equals("FCCABANG")).FirstOrDefault();
            if (max != null && max.Val != null)
            {
                max.Val = src.Cabang.ToString();
                this.ctx.RfGlobal.Update(max);
            }
            else
            {
                max = new RfGlobal();
                max.Val = src.Cabang.ToString();
                max.Code = "FCCABANG";
                max.Name = "FC Filter By Cabang";
                max.StatusId = 1;
                this.ctx.RfGlobal.Add(max);

            }

            this.ctx.SaveChanges();
            wrap.AddData(src);
            wrap.Status = true;


            return wrap;
        }

        public GenericResponse<ProductLoanResponseBean> ListProduct()
        {
            var wrap = new GenericResponse<ProductLoanResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<Product> q = this.ctx.Set<Product>().Include(i => i.Status);
            var data = q.Where(q => q.Status!.Name!.Equals("AKTIF")).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<ProductLoanResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<ProductLoanResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public void BackDate(int day)
        {
            for (var i = 0; i < day; i++)
            {
                var lastSevenDay = DateTime.Now.AddDays(-1);
            }
        }
    }

}
