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
using static Collectium.Model.Bean.ListRequest.RecoveryListRequest;
using Action = Collectium.Model.Entity.Action;

namespace Collectium.Service
{
    public class BucketService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<ScriptingService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BucketService(CollectiumDBContext ctx,
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

        public GenericResponse<BucketListResponseBean> List(BucketListBean filter)
        {
            var wrap = new GenericResponse<BucketListResponseBean>
            {
                Status = false,
                Message = ""
            };

            IQueryable<Bucket> q = this.ctx.Set<Bucket>().Include(i => i.Status);
            if (filter.Keyword != null && filter.Keyword.Length > 0)
            {
                var predicate = PredicateBuilder.New<Bucket>();
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

            var ldata = new List<BucketListResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<BucketListResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }



        public GenericResponse<BucketCreateBean> Create(BucketCreateBean filter)
        {
            var wrap = new GenericResponse<BucketCreateBean>
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

            if (filter.IdUser == null || filter.IdUser.Count() < 1)
            {
                wrap.Message = "User adalah mandatory";
                return wrap;
            }

            var us = new Bucket();
            us.Code = filter.Code;
            us.Name = filter.Name;


            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            this.toolService.EnrichProcessSave(us);

            this.ctx.Bucket.Add(us);
            this.ctx.SaveChanges();

            foreach (var o in filter.IdUser)
            {
                var bu = new BucketUser();
                bu.BucketId = us.Id;
                bu.UserId = o;

                this.ctx.BucketUser.Add(bu);
                this.ctx.SaveChanges();

            }

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<BucketUpdateBean> Update(BucketUpdateBean filter)
        {
            var wrap = new GenericResponse<BucketUpdateBean>
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

            if (filter.IdUser == null || filter.IdUser.Count() < 1)
            {
                wrap.Message = "User adalah mandatory";
                return wrap;
            }

            var us = this.ctx.Bucket.Find(filter.Id);
            this.ctx.Entry(us!).Collection(r => r.User!).Load();

            us.Code = filter.Code;
            us.Name = filter.Name;
            us.User!.Clear();


            var sg = this.statusService.GetStatusGeneral("AKTIF");
            us.Status = sg;
            this.toolService.EnrichProcessSave(us);

            this.ctx.Bucket.Update(us);
            this.ctx.SaveChanges();

            foreach (var o in filter.IdUser)
            {
                var bu = new BucketUser();
                bu.BucketId = us.Id;
                bu.UserId = o;

                this.ctx.BucketUser.Add(bu);
                this.ctx.SaveChanges();

            }

            wrap.Status = true;
            wrap.AddData(filter);

            return wrap;
        }

        public GenericResponse<BucketDetailResponseBean> Detail(UserReqApproveBean filter)
        {
            var wrap = new GenericResponse<BucketDetailResponseBean>
            {
                Status = false,
                Message = ""
            };


            var q = this.ctx.Set<Bucket>().Include(i => i.Status).Include(i => i.User).ThenInclude(i => i!.User)
                                            .Where(q => q.Id.Equals(filter.Id))
                                            .ToList();

            var ldata = new List<BucketDetailResponseBean>();
            foreach (var it in q)
            {
                var dto = new BucketDetailResponseBean();
                dto.Id = it.Id;
                dto.Code = it.Code;
                dto.Name = it.Name;
                dto.Status = mapper.Map<StatusGeneralBean>(it.Status);

                var ags = new List<UserTraceResponseBean>();
                foreach (var ux in it.User!)
                {
                    var d = mapper.Map<UserTraceResponseBean>(ux.User);
                    ags.Add(d);
                }

                dto.User = ags;

                ldata.Add(dto);
            }

            wrap.Data = ldata;

            wrap.Status = true;

            return wrap;
        }


        public GenericResponse<RuleAction> ListRuleAction()
        {
            var wrap = new GenericResponse<RuleAction>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<RuleAction>().OrderByDescending(q => q.Id).ToList();
            wrap.Data = q;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<RuleActionOption> ListRuleActionOption()
        {
            var wrap = new GenericResponse<RuleActionOption>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<RuleActionOption>().OrderByDescending(q => q.Id).ToList();
            wrap.Data = q;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<RuleValueType> ListRuleValueType()
        {
            var wrap = new GenericResponse<RuleValueType>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<RuleValueType>().OrderByDescending(q => q.Id).ToList();
            wrap.Data = q;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<RuleOperator> ListRuleOperator()
        {
            var wrap = new GenericResponse<RuleOperator>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<RuleOperator>().OrderByDescending(q => q.Id).ToList();
            wrap.Data = q;

            wrap.Status = true;

            return wrap;
        }


        public GenericResponse<RuleDataField> ListRuleDataField()
        {
            var wrap = new GenericResponse<RuleDataField>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<RuleDataField>().OrderByDescending(q => q.Id).ToList();
            wrap.Data = q;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<DataSource> ListDataSource()
        {
            var wrap = new GenericResponse<DataSource>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<DataSource>().OrderByDescending(q => q.Id).ToList();
            wrap.Data = q;

            wrap.Status = true;

            return wrap;
        }

        public GenericResponse<CustomerTag> ListCustomerTag()
        {
            var wrap = new GenericResponse<CustomerTag>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<CustomerTag>().OrderByDescending(q => q.Id).ToList();
            wrap.Data = q;

            wrap.Status = true;

            return wrap;
        }


    }
}
