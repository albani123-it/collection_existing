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
using Action = Collectium.Model.Entity.Action;

namespace Collectium.Service
{
    public class ScriptingService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<ScriptingService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ScriptingService(CollectiumDBContext ctx,
                                ILogger<ScriptingService> logger,
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

        public GenericResponse<CallScriptResponseBean> ListCallScript(CallScriptListRequestBean filter)
        {
            var wrap = new GenericResponse<CallScriptResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<CallScript> q = this.ctx.Set<CallScript>().Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<CallScript>();
                predicate = predicate.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.CsDesc!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.CsScript!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            q = q.Where(q => q.Status!.Name!.Equals("AKTIF"));

            q = q.OrderByDescending(q => q.Id);
            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<CallScriptResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CallScriptResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CallScriptResponseBean> DetailScript(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<CallScriptResponseBean>
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

            var obj = this.ctx.CallScript.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "CallScript tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();

            if (obj.Status!.Name != "AKTIF")
            {
                wrap.Message = "CallScript tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<CallScriptResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CallScriptReqResponseBean> DetailCallScriptRequest(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<CallScriptReqResponseBean>
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

            var obj = this.ctx.CallScriptRequest.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "CallScript Request tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();
            this.ctx.Entry(obj).Reference(r => r.CallScript).Load();

            if (obj.Status!.Name!.Equals("REMOVE"))
            {
                wrap.Message = "CallScript Request tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<CallScriptReqResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CallScriptCreateBean> SaveCallScript(CallScriptCreateBean filter)
        {
            var wrap = new GenericResponse<CallScriptCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.CsDesc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.CsScript)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.AccdMin)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.AccdMax)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = mapper.Map<CallScript>(filter);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            this.toolService.EnrichProcessSave(us);

            this.ctx.CallScript.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<CallScriptReqResponseBean> ListCallScriptRequest(CallScriptReqListRequestBean filter)
        {
            var wrap = new GenericResponse<CallScriptReqResponseBean>
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

            var role = reqUser.RoleId;

            IQueryable<CallScriptRequest> q = this.ctx.Set<CallScriptRequest>().Include(i => i.CallScript).Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<CallScriptRequest>();
                predicatek = predicatek.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.CsDesc!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.CsScript!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }

            //var predicate = PredicateBuilder.New<CallScriptRequest>();
            //predicate = predicate.Or(o => o.Status!.Name!.Equals("DRAFT"));
            //predicate = predicate.Or(o => o.Status!.Name!.Equals("APPROVE"));
            //q = q.Where(predicate);

            if (role == 9)
            {
                q = q.Where(p => p.Status!.Name!.Equals("DRAFT"));
            }
            else if (role == 1)
            {
                q = q.Where(p => p.Status!.Name!.Equals("CHECK"));
            }

            q = q.OrderByDescending(q => q.Id);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<CallScriptReqResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<CallScriptReqResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CallScriptCreateBean> SaveNewCallScriptRequest(CallScriptCreateBean filter)
        {
            var wrap = new GenericResponse<CallScriptCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.CsDesc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.CsScript)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.AccdMin)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.AccdMax)).IsMandatory().AsInteger().Pack()
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

            var us = mapper.Map<CallScriptRequest>(filter);
            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;
            
            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.CallScriptRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<CallScriptReqEditBean> SaveEditCallScriptRequest(CallScriptReqEditBean filter)
        {
            var wrap = new GenericResponse<CallScriptReqEditBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.CsDesc)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.CsScript)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.AccdMin)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.AccdMax)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.CallScriptId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
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

            var us = mapper.Map<CallScriptRequest>(filter);

            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;

            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.CallScriptRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        public GenericResponse<UserReqApproveBean> ApproveCallScriptRequest(UserReqApproveBean filter)
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

            var us = this.ctx.CallScriptRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "CallScript Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();
            this.ctx.Entry(us).Reference(r => r.CallScript).Load();

            if (us.CallScript != null)
            {
                this.ctx.Entry(us.CallScript!).Reference(r => r.Status).Load();

                if (us.CallScript!.Status!.Name != "AKTIF")
                {
                    wrap.Message = "Status Call Script tidak AKTIF";
                    return wrap;
                }
            }

            if (us.Status!.Name != "DRAFT" && us.Status!.Name != "CHECK")
            {
                wrap.Message = "Status CallScript Request tidak valid";
                return wrap;
            }

            if (us.Status!.Name == "DRAFT")
            {
                us.StatusId = 7;
                this.ctx.CallScriptRequest.Update(us);
                this.ctx.SaveChanges();

                wrap.Status = true;
                wrap.AddData(filter);

                return wrap;
            }


            if (us.CallScript == null)
            {
                var ucb = mapper.Map<CallScriptCreateBean>(us);
                this.SaveCallScript(ucb);
            }
            else
            {
                var pus = us.CallScript;
                IlKeiCopyObject.Instance.WithSource(us).WithDestination(pus)
                                .Include(nameof(us.Code))
                                .Include(nameof(us.CsDesc))
                                .Include(nameof(us.CsScript))
                                .Include(nameof(us.AccdMin))
                                .Include(nameof(us.AccdMax))
                                .Execute();
                this.ctx.CallScript.Update(pus);

                var sr = this.statusService.GetStatusRequest("REMOVE");

                var prevReq = this.ctx.CallScriptRequest.Where(q => q.Id < filter.Id).Where(q => q.CallScript!.Id.Equals(pus.Id)).Where(q => q.Status!.Name!.Equals("DRAFT")).ToList();
                if (prevReq != null && prevReq.Count > 0)
                {
                    foreach (var item in prevReq)
                    {
                        item.StatusId = sr.Id;
                        this.ctx.CallScriptRequest.Update(item);
                    }
                }
            }

            var sa = this.statusService.GetStatusRequest("APPROVE");
            us.StatusId = sa.Id;

            this.toolService.EnrichProcessApproveRequest(us);

            this.ctx.CallScriptRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UserReqApproveBean> RejectCallScriptRequest(UserReqApproveBean filter)
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

            var us = this.ctx.CallScriptRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "CallScript Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();

            if (us.Status!.Name != "DRAFT" && us.Status!.Name != "CHECK")
            {
                wrap.Message = "Status CallScript Request tidak valid";
                return wrap;
            }

            var sa = this.statusService.GetStatusRequest("REJECT");
            us.StatusId = sa.Id;

            this.ctx.CallScriptRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }



        //ACTION GROUP
        public GenericResponse<NotifResponseBean> ListNotif(NotifListRequestBean filter)
        {
            var wrap = new GenericResponse<NotifResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<NotifContent> q = this.ctx.Set<NotifContent>().Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<NotifContent>();
                predicatek = predicatek.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }
            if (filter.Day != null)
            {
                q = q.Where(q => q.Day!.Equals(filter.Day));
            }

            q = q.Where(q => q.Status!.Name!.Equals("AKTIF"));
            q = q.OrderByDescending(q => q.Id);
            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<NotifResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<NotifResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<NotifResponseBean> DetailNotifContent(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<NotifResponseBean>
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

            var obj = this.ctx.NotifContent.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Notification Content tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();

            if (obj.Status!.Name != "AKTIF")
            {
                wrap.Message = "Notification Content tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<NotifResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<NotifCreateBean> SaveNotifContent(NotifCreateBean filter)
        {
            var wrap = new GenericResponse<NotifCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Day)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = mapper.Map<NotifContent>(filter);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;

            this.toolService.EnrichProcessSave(us);

            this.ctx.NotifContent.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<NotifReqResponseBean> ListNotifRequest(NotifReqListRequestBean filter)
        {
            var wrap = new GenericResponse<NotifReqResponseBean>
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

            var role = reqUser.RoleId;

            IQueryable<NotifContentRequest> q = this.ctx.Set<NotifContentRequest>().Include(i => i.NotifContent).Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<NotifContentRequest>();
                predicatek = predicatek.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }

            //var predicate = PredicateBuilder.New<NotifContentRequest>();
            //predicate = predicate.Or(o => o.Status!.Name!.Equals("DRAFT"));
            //predicate = predicate.Or(o => o.Status!.Name!.Equals("APPROVE"));
            //q = q.Where(predicate);

            if (role == 9)
            {
                q = q.Where(p => p.Status!.Name!.Equals("DRAFT"));
            }
            else if (role == 1)
            {
                q = q.Where(p => p.Status!.Name!.Equals("CHECK"));
            }

            if (filter.Day != null)
            {
                q = q.Where(q => q.Day!.Equals(filter.Day));
            }
            q = q.OrderByDescending(o => o.Id);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<NotifReqResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<NotifReqResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<NotifReqResponseBean> DetailNotifRequest(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<NotifReqResponseBean>
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

            var obj = this.ctx.NotifContentRequest.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Notification Content Request tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();
            this.ctx.Entry(obj).Reference(r => r.NotifContent).Load();

            if (obj.Status!.Name!.Equals("REMOVE"))
            {
                wrap.Message = "Notification Content Request tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<NotifReqResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<NotifCreateBean> SaveNewNotifRequest(NotifCreateBean filter)
        {
            var wrap = new GenericResponse<NotifCreateBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Day)).IsMandatory().AsInteger().Pack()
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

            var us = mapper.Map<NotifContentRequest>(filter);
            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;
        
            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.NotifContentRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<NotifReqEditBean> SaveEditNotifRequest(NotifReqEditBean filter)
        {
            var wrap = new GenericResponse<NotifReqEditBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Day)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.NotifContentId)).IsMandatory().AsInteger().Pack()
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

            var us = mapper.Map<NotifContentRequest>(filter);

            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;

            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.NotifContentRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UserReqApproveBean> ApproveNotifRequest(UserReqApproveBean filter)
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

            var us = this.ctx.NotifContentRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "Notification Content Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();
            this.ctx.Entry(us).Reference(r => r.NotifContent).Load();

            if (us.NotifContent != null)
            {
                this.ctx.Entry(us.NotifContent!).Reference(r => r.Status).Load();
                if (us.NotifContent!.Status!.Name != "AKTIF")
                {
                    wrap.Message = "Status Notification Content tidak AKTIF";
                    return wrap;
                }
            }

            if (us.Status!.Name != "DRAFT" && us.Status!.Name != "CHECK")
            {
                wrap.Message = "Status Notification Content Request tidak valid";
                return wrap;
            }

            if (us.Status!.Name == "DRAFT")
            {
                us.StatusId = 7;
                this.ctx.NotifContentRequest.Update(us);
                this.ctx.SaveChanges();

                wrap.Status = true;
                wrap.AddData(filter);

                return wrap;
            }


            if (us.NotifContent == null)
            {
                var ucb = mapper.Map<NotifCreateBean>(us);
                this.SaveNotifContent(ucb);
            }
            else
            {
                var pus = us.NotifContent;
                pus.Code = us.Code;
                pus.Name = us.Name;
                pus.Day = us.Day;
                pus.Content= us.Content;

                this.ctx.NotifContent.Update(pus);

                var sr = this.statusService.GetStatusRequest("REMOVE");

                var prevReq = this.ctx.NotifContentRequest.Where(q => q.Id < filter.Id).Where(q => q.NotifContent!.Id.Equals(pus.Id)).Where(q => q.Status!.Name!.Equals("DRAFT")).ToList();
                if (prevReq != null && prevReq.Count > 0)
                {
                    foreach (var item in prevReq)
                    {
                        item.StatusId = sr.Id;
                        this.ctx.NotifContentRequest.Update(item);
                    }
                }
            }

            var sa = this.statusService.GetStatusRequest("APPROVE");
            us.StatusId = sa.Id;

            this.toolService.EnrichProcessApproveRequest(us);

            this.ctx.NotifContentRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UserReqApproveBean> RejectNotifRequest(UserReqApproveBean filter)
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

            var us = this.ctx.NotifContentRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "Notification Content Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();

            if (us.Status!.Name != "DRAFT" && us.Status!.Name != "CHECK")
            {
                wrap.Message = "Status Notification Content Request tidak valid";
                return wrap;
            }

            var sa = this.statusService.GetStatusRequest("REJECT");
            us.StatusId = sa.Id;

            this.ctx.NotifContentRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }


        public GenericResponse<ReasonResponseBean> ListReason(ReasonListRequestBean filter)
        {
            var wrap = new GenericResponse<ReasonResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<Reason> q = this.ctx.Set<Reason>().Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<Reason>();
                predicatek = predicatek.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }

            q = q.Where(q => q.Status!.Name!.Equals("AKTIF"));
            q = q.OrderByDescending(q => q.Id);
            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<ReasonResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<ReasonResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ReasonResponseBean> DetailListReason(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<ReasonResponseBean>
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

            var obj = this.ctx.Reason.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Reason tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();

            if (obj.Status!.Name != "AKTIF")
            {
                wrap.Message = "Reason tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<ReasonResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ReasonCreateBean> SaveReason(ReasonCreateBean filter)
        {
            var wrap = new GenericResponse<ReasonCreateBean>
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

            var us = mapper.Map<Reason>(filter);
            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;

            this.toolService.EnrichProcessSave(us);

            this.ctx.Reason.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<ReasonReqResponseBean> ListReasonRequest(ReasonListRequestBean filter)
        {
            var wrap = new GenericResponse<ReasonReqResponseBean>
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

            var role = reqUser.RoleId;

            IQueryable<ReasonRequest> q = this.ctx.Set<ReasonRequest>().Include(i => i.Reason).Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicatek = PredicateBuilder.New<ReasonRequest>();
                predicatek = predicatek.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicatek = predicatek.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicatek);
            }

            //var predicate = PredicateBuilder.New<NotifContentRequest>();
            //predicate = predicate.Or(o => o.Status!.Name!.Equals("DRAFT"));
            //predicate = predicate.Or(o => o.Status!.Name!.Equals("APPROVE"));
            //q = q.Where(predicate);

            if (role == 9)
            {
                q = q.Where(p => p.Status!.Name!.Equals("DRAFT"));
            }
            else if (role == 1)
            {
                q = q.Where(p => p.Status!.Name!.Equals("CHECK"));
            }


            q = q.OrderByDescending(o => o.Id);

            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<ReasonReqResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<ReasonReqResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ReasonReqResponseBean> DetailReasonRequest(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<ReasonReqResponseBean>
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

            var obj = this.ctx.ReasonRequest.Find(filter.Id);
            if (obj == null)
            {
                wrap.Message = "Reason Request tidak ditemukan";
                return wrap;
            }

            this.ctx.Entry(obj).Reference(r => r.Status).Load();
            this.ctx.Entry(obj).Reference(r => r.Reason).Load();

            if (obj.Status!.Name!.Equals("REMOVE"))
            {
                wrap.Message = "Reason Request tidak aktif";
                return wrap;
            }

            var res = this.mapper.Map<ReasonReqResponseBean>(obj);

            wrap.AddData(res);
            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<ReasonCreateBean> SaveNewReasonRequest(ReasonCreateBean filter)
        {
            var wrap = new GenericResponse<ReasonCreateBean>
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

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                wrap.Message = "User tidak ditemukan";
                return wrap;
            }

            var us = mapper.Map<ReasonRequest>(filter);
            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;

            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.ReasonRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<ReasonReqEditBean> SaveEditReasonRequest(ReasonReqEditBean filter)
        {
            var wrap = new GenericResponse<ReasonReqEditBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.ReasonId)).IsMandatory().AsInteger().Pack()
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

            var us = mapper.Map<ReasonRequest>(filter);

            var sg = this.statusService.GetStatusRequest("DRAFT");
            us.Status = sg;

            this.toolService.EnrichProcessSaveRequest(us);

            this.ctx.ReasonRequest.Add(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UserReqApproveBean> ApproveReasonRequest(UserReqApproveBean filter)
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

            var us = this.ctx.ReasonRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "Reason Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();
            this.ctx.Entry(us).Reference(r => r.Reason).Load();

            if (us.Reason != null)
            {
                this.ctx.Entry(us.Reason!).Reference(r => r.Status).Load();
                if (us.Reason!.Status!.Name != "AKTIF")
                {
                    wrap.Message = "Status Reason tidak AKTIF";
                    return wrap;
                }
            }

            if (us.Status!.Name != "DRAFT" && us.Status!.Name != "CHECK")
            {
                wrap.Message = "Status Reason Request tidak valid";
                return wrap;
            }

            if (us.Status!.Name == "DRAFT")
            {
                us.StatusId = 7;
                this.ctx.ReasonRequest.Update(us);
                this.ctx.SaveChanges();

                wrap.Status = true;
                wrap.AddData(filter);

                return wrap;
            }


            if (us.Reason == null)
            {
                var ucb = mapper.Map<ReasonCreateBean>(us);
                this.SaveReason(ucb);
            }
            else
            {
                var pus = us.Reason;
                pus.Code = us.Code;
                pus.Name = us.Name;
                pus.isDC = us.isDC;
                pus.isFC = us.isFC;

                this.ctx.Reason.Update(pus);

                var sr = this.statusService.GetStatusRequest("REMOVE");

                var prevReq = this.ctx.ReasonRequest.Where(q => q.Id < filter.Id).Where(q => q.Reason!.Id.Equals(pus.Id)).Where(q => q.Status!.Name!.Equals("DRAFT")).ToList();
                if (prevReq != null && prevReq.Count > 0)
                {
                    foreach (var item in prevReq)
                    {
                        item.StatusId = sr.Id;
                        this.ctx.ReasonRequest.Update(item);
                    }
                }
            }

            var sa = this.statusService.GetStatusRequest("APPROVE");
            us.StatusId = sa.Id;

            this.toolService.EnrichProcessApproveRequest(us);

            this.ctx.ReasonRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UserReqApproveBean> RejectRequestRequest(UserReqApproveBean filter)
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

            var us = this.ctx.ReasonRequest.Find(filter.Id);
            if (us == null)
            {
                wrap.Message = "Reason Request tidak ditemukan dalam sistem";
                return wrap;
            }

            this.ctx.Entry(us).Reference(r => r.Status).Load();

            if (us.Status!.Name != "DRAFT" && us.Status!.Name != "CHECK")
            {
                wrap.Message = "Reason Content Request tidak valid";
                return wrap;
            }

            var sa = this.statusService.GetStatusRequest("REJECT");
            us.StatusId = sa.Id;

            this.ctx.ReasonRequest.Update(us);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }
    }
}
