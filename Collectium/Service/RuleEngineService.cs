using AutoMapper;
using Collectium.Model;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Model.Helper;
using Collectium.Validation;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using RestSharp;
using System.Text;
using static Collectium.Model.Bean.ListRequest.RecoveryListRequest;
using Action = Collectium.Model.Entity.Action;

namespace Collectium.Service
{
    public class RuleEngineService

    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<ScriptingService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration conf;

        public RuleEngineService(CollectiumDBContext ctx,
                                ILogger<ScriptingService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                ToolService toolService,
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
            this.toolService = toolService;
            this.conf = conf;
        }

        public GenericResponse<RuleEngineListView> List(RuleEngineListBean filter)
        {
            var wrap = new GenericResponse<RuleEngineListView>
            {
                Status = false,
                Message = ""
            };

            IQueryable<RuleEngine> q = this.ctx.Set<RuleEngine>().Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<RuleEngine>();
                predicate = predicate.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            q = q.Where(q => q.Status!.Name!.Equals("AKTIF"));

            q = q.OrderByDescending(q => q.Id);
            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<RuleEngineListView>();
            foreach (var it in data)
            {
                var dto = mapper.Map<RuleEngineListView>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }



        public GenericResponse<CreateRuleEngineBean> Create(CreateRuleEngineBean filter)
        {
            var wrap = new GenericResponse<CreateRuleEngineBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.IdRuleAction)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var ra = this.ctx.RuleAction.Find(filter.IdRuleAction);
            if (ra == null)
            {
                wrap.Message = "Action adalah mandatory";
                return wrap;
            }

            if (ra.Code!.Equals("ACT"))
            {
                if (filter.IdRuleActionOption == null)
                {
                    wrap.Message = "Action Option adalah mandatory";
                    return wrap;
                }

            }
            else
            {
                if (filter.IdBucket == null || filter.IdBucket.Count() < 1)
                {
                    wrap.Message = "Bucket adalah mandatory";
                    return wrap;
                }
            }

            if (filter.Condition == null || filter.Condition.Count() < 1)
            {
                wrap.Message = "Condition adalah mandatory";
                return wrap;
            }

            var us = new RuleEngine();
            us.Code = filter.Code;
            us.Name = filter.Name;
            us.RuleActionId = filter.IdRuleAction;
            us.RuleOptionId = filter.IdRuleActionOption;


            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            this.toolService.EnrichProcessSave(us);

            this.ctx.RuleEngine.Add(us);
            this.ctx.SaveChanges();

            if (filter.IdBucket != null && filter.IdBucket.Count() > 0)
            {
                foreach (var o in filter.IdBucket)
                {
                    var bu = new RuleBucket();
                    bu.BucketId = o;
                    bu.RuleId = us.Id;

                    this.ctx.RuleBucket.Add(bu);
                    this.ctx.SaveChanges();

                }
            }

            foreach (var o in filter.Condition)
            {
                var bu = new RuleEngineCond();
                bu.RuleEngineId = us.Id;
                bu.RuleFieldId = o.IdRuleDataField;
                bu.RuleOperatorId = o.IdRuleOperator;
                bu.Value = o.Value; 

                this.ctx.RuleEngineCond.Add(bu);
                this.ctx.SaveChanges();

            }

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<UpdateRuleEngineBean> Update(UpdateRuleEngineBean filter)
        {
            var wrap = new GenericResponse<UpdateRuleEngineBean>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.Id)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.Code)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Pick(nameof(filter.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var ra = this.ctx.RuleAction.Find(filter.IdRuleAction);
            if (ra == null)
            {
                wrap.Message = "Action adalah mandatory";
                return wrap;
            }

            if (ra.Code!.Equals("ACT"))
            {
                if (filter.IdRuleActionOption == null)
                {
                    wrap.Message = "Action Option adalah mandatory";
                    return wrap;
                }

            }
            else
            {
                if (filter.IdBucket == null || filter.IdBucket.Count() < 1)
                {
                    wrap.Message = "Bucket adalah mandatory";
                    return wrap;
                }
            }

            if (filter.Condition == null || filter.Condition.Count() > 1)
            {
                wrap.Message = "Condition adalah mandatory";
                return wrap;
            }

            var us = this.ctx.RuleEngine.Find(filter.Id);
            this.ctx.Entry(us!).Reference(r => r.RuleAction!).Load();
            this.ctx.Entry(us!).Reference(r => r.RuleOption!).Load();
            this.ctx.Entry(us!).Collection(r => r.RuleEngineCond!).Load();
            this.ctx.Entry(us!).Collection(r => r.Bucket!).Load();

            us.Code = filter.Code;
            us.Name = filter.Name;

            us.RuleEngineCond!.Clear();
            us.Bucket!.Clear();
            this.ctx.RuleEngine.Update(us);
            this.ctx.SaveChanges();

            if (filter.IdBucket != null && filter.IdBucket.Count() > 0)
            {
                foreach (var o in filter.IdBucket)
                {
                    var bu = new RuleBucket();
                    bu.BucketId = o;
                    bu.RuleId = us.Id;

                    this.ctx.RuleBucket.Add(bu);
                    this.ctx.SaveChanges();

                }
            }

            foreach (var o in filter.Condition)
            {
                var bu = new RuleEngineCond();
                bu.RuleEngineId = us.Id;
                bu.RuleFieldId = o.IdRuleDataField;
                bu.RuleOperatorId = o.IdRuleOperator;
                bu.Value = o.Value;

                this.ctx.RuleEngineCond.Add(bu);
                this.ctx.SaveChanges();

            }
            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<DetailRuleEngineBean> Detail(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<DetailRuleEngineBean>
            {
                Status = false,
                Message = ""
            };


            var q = this.ctx.Set<RuleEngine>().Include(i => i.RuleAction).Include(i => i.RuleEngineCond).Include(i => i!.Bucket).Include(i => i.RuleEngineCond)
                                            .Where(q => q.Id.Equals(filter.Id))
                                            .ToList();

            var ldata = new List<DetailRuleEngineBean>();
            foreach (var it in q)
            {
                var dto = new DetailRuleEngineBean();
                dto.Id = it.Id;
                dto.Code = it.Code;
                dto.Name = it.Name;

                if (it.RuleOption != null)
                {
                    var aa = new RuleActionOptionBean();
                    aa.Id = it.RuleOption.Id;
                    aa.Code = it.RuleOption.Code;
                    aa.Name = it.RuleOption.Name;

                    dto.RuleActionOption = aa;
                }

                if (it.RuleAction != null)
                {
                    var bb = new RuleActionBean();
                    bb.Id = it.RuleAction.Id;
                    bb.Code = it.RuleAction.Code;
                    bb.Name = it.RuleAction.Name;

                    dto.RuleAction = bb;
                }

                if (it.Bucket != null)
                {
                    var lsb = new List<BucketDetailResponseBean>();

                    foreach (var bc in it.Bucket)
                    {
                        this.ctx.Entry(bc).Reference(r => r.Bucket).Load();

                        var cbb = new BucketDetailResponseBean();
                        cbb.Id = bc.BucketId;
                        cbb.Code = bc.Bucket.Code;
                        cbb.Name = bc.Bucket.Name;
                        lsb.Add(cbb); break;
                    }

                    dto.IdBucket = lsb;
                }

                if (it.RuleEngineCond != null)
                {
                    var lsb = new List<CreateRuleEngineConditionBean>();

                    foreach (var bc in it.RuleEngineCond)
                    {

                        var cbb = new CreateRuleEngineConditionBean();
                        cbb.IdRuleDataField = bc.RuleFieldId;
                        cbb.IdRuleOperator = bc.RuleOperatorId;
                        cbb.Value = bc.Value;
                        lsb.Add(cbb);
                    }

                    dto.Condition = lsb;
                }


                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<JobRuleListView> ListJob(RuleEngineListBean filter)
        {
            var wrap = new GenericResponse<JobRuleListView>
            {
                Status = false,
                Message = ""
            };

            IQueryable<JobRule> q = this.ctx.Set<JobRule>().Include(i => i.Status).Include(i => i.RuleEngine).Include(i => i.DataSource);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<JobRule>();
                predicate = predicate.Or(p => p.Code!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                predicate = predicate.Or(p => p.RuleEngine!.Name!.ToUpper().Trim().Contains(filter.Keyword.ToUpper().Trim()));
                q = q.Where(predicate);
            }

            q = q.Where(q => q.Status!.Name!.Equals("AKTIF"));

            q = q.OrderByDescending(q => q.Id);
            var cnt = q.Count();
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<JobRuleListView>();
            foreach (var it in data)
            {
                var dto = new JobRuleListView();
                dto.Id = it.Id;
                dto.Code = it.Code;
                dto.DataSourceName = it.DataSource!.Name;
                dto.NumProcess = it.NumProcess;
                dto.NumData = it.NumData;
                dto.RuleEngineName = it.RuleEngine!.Name;
                dto.StatusId = it.Status!.Id;
                dto.StatusName = it.Status!.Name;
                dto.StartTime = it.StartTime;
                dto.EndTime = it.EndTime;

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<string> CreateJob(CreateJobRuleBean filter)
        {
            var wrap = new GenericResponse<string>
            {
                Status = false,
                Message = ""
            };
            var pr = IlKeiValidator.Instance.WithPoCo(filter)
                .Pick(nameof(filter.IdRule)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(filter.IdDataSource)).IsMandatory().AsInteger().Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var us = new JobRule();

            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();

            us.Code = myuuidAsString;
            us.DataSourceId = filter.IdDataSource;
            us.RuleEngineId = filter.IdRule;

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            this.toolService.EnrichProcessSave(us);

            this.ctx.JobRule.Add(us);
            this.ctx.SaveChanges();


            var path = conf["PhotoPath"];
            path = path + "/upload/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var nm = path + "/" + us.Id.ToString() + ".txt";

            using (FileStream filestream = System.IO.File.Create(nm))
            {
                filter.File.CopyTo(filestream);
                filestream.Flush();
                //  return "\\Upload\\" + objFile.files.FileName;
            }

            us.Url = us.Id.ToString() + ".txt";

            this.ctx.JobRule.Update(us);
            this.ctx.SaveChanges();


            wrap.Status = true;
            wrap.AddData(us.Code);

            var client = new RestClient("http://localhost:9393");
            string vars = "api/admin/test?cnt=" + us.Id;
            this.logger.LogInformation("url: " + vars);

            var request = new RestRequest(vars, Method.Get);
            var res = client.Execute(request);

            return wrap;
        }

        public GenericResponse<JobRuleListView> DetailJob(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<JobRuleListView>
            {
                Status = false,
                Message = ""
            };


            var q = this.ctx.Set<JobRule>().Include(i => i.RuleEngine).Include(i => i.DataSource).Where(q => q.Id.Equals(filter.Id)).Include(i => i.Status)
                                            .ToList();

            var ldata = new List<JobRuleListView>();
            foreach (var it in q)
            {
                var dto = new JobRuleListView();
                dto.Id = it.Id;
                dto.Code = it.Code;
                dto.DataSourceName = it.DataSource!.Name;
                dto.NumProcess = it.NumProcess;
                dto.NumData = it.NumData;
                dto.RuleEngineName = it.RuleEngine!.Name;
                dto.StatusId = it.Status!.Id;
                dto.StatusName = it.Status!.Name;
                dto.StartTime = it.StartTime;
                dto.EndTime = it.EndTime;

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<BusinessExceptionBean> ListBusinessException(RuleEngineListBean filter)
        {
            var wrap = new GenericResponse<BusinessExceptionBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<BusinessException> q = this.ctx.Set<BusinessException>();
            q = q.Where(q => q.JobRuleId.Equals(filter.Id));

            q = q.OrderByDescending(q => q.Id);
            var cnt = q.Count();
            wrap.DataCount = cnt;
            wrap.MaxPage = this.pagination.getMaxPage(filter, cnt);

            var pagination = this.pagination.getPagination(filter);
            var data = q.Skip(pagination.Skip).Take(pagination.Limit).ToList();

            var ldata = new List<BusinessExceptionBean>();
            foreach (var it in data)
            {
                this.ctx.Entry(it).Reference(r => r.Loan).Load();
                var dto = new BusinessExceptionBean();
                dto.Id = it.Id;
                dto.Message = it.Message;
                dto.CreateDate = it.CreateDate;
                dto.AccNo = it.Loan!.AccNo;
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CollTraceResponseBean> ListCallTrace(ListJobRuleSuccess filter)
        {
            var wrap = new GenericResponse<CollTraceResponseBean>
            {
                Status = false,
                Message = ""
            };


            IQueryable<CollectionTrace> q = this.ctx.Set<CollectionTrace>().Include(i => i.CallBy)
                                                .Include(i => i.Result)
                                                .Include(i => i.Call)
                                                    .ThenInclude(i => i!.Loan!)
                                                        .ThenInclude(i => i.Customer!);


            q = q.Where(q => q.JobRuleId!.Equals(filter.Id));

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

        public GenericResponse<UserResponseBean> ListAgentByBranch()
        {
            var wrap = new GenericResponse<UserResponseBean>
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

            var role = reqUser.Role!.Id;
            var urole = new List<int?>();
            urole.Add(3);
            urole.Add(4);



            var ld = new List<UserResponseBean>();
            var ls = this.ctx.User.Where(q => urole.Contains(q.Role!.Id))
                                        .Where(q => q.Status!.Name!.Equals("AKTIF")).ToList();

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
