using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;

namespace Collectium.Model.Bean.ListRequest
{
    public class RecoveryListRequest
    {

        public class RestructureListBean : PagedRequestBean
        {
            public int? Id { get; set; }

            public string? Keyword { get; set; }

            public string? AccNo { get; set; }

            public string? Cif { get; set; }

            public string? Name { get; set; }

            public int? BranchId { get; set; }

            public int? StatusId { get; set; }
        }

        public class BucketListBean : PagedRequestBean
        {
            public int? Id { get; set; }

            public string? Keyword { get; set; }

            public int? StatusId { get; set; }
        }

        public class BucketListResponseBean
        {
            public int? Id { get; set; }

            public string? Code { get; set; }

            public string? Name { get; set; }

            public StatusGeneral? Status { get; set; }
        }

        public class BucketDetailResponseBean
        {
            public int? Id { get; set; }

            public string? Code { get; set; }

            public string? Name { get; set; }

            public IList<UserTraceResponseBean>? User { get; set; }

            public StatusGeneralBean? Status { get; set; }
        }

        public class BucketCreateBean
        {

            public string? Code { get; set; }

            public string? Name { get; set; }

            public IList<int>? IdUser { get; set; }
        }

        public class BucketUpdateBean
        {

            public int? Id { get; set; }

            public string? Code { get; set; }

            public string? Name { get; set; }

            public IList<int>? IdUser { get; set; }
        }

        public class RuleEngineListView
        {
            public int? Id { get; set; }

            public string? Name { get; set; }

            public string? Code { get; set; }

            public RuleAction? RuleAction { get; set; }

            public RuleActionOption? RuleOption { get; set; }

            public ICollection<RuleEngineCond> RuleEngineCond { get; set; }

            public virtual ICollection<RuleBucket>? Bucket { get; set; }

            public StatusGeneral? Status { get; set; }

        }

        public class RuleEngineListBean : PagedRequestBean
        {
            public int? Id { get; set; }

            public string? Keyword { get; set; }

            public int? StatusId { get; set; }
        }

        public class CreateRuleEngineConditionBean
        {
            public int? IdRuleDataField { get; set; }

            public int? IdRuleOperator { get; set; }

            public string? Value { get; set; }

        }

        public class RuleEngineConditionBean
        {
            public int? Id { get; set; }

            public int? FieldId { get; set; }

            public string? Value { get; set; }

        }

        public class CreateRuleEngineBean
        {

            public string? Code { get; set; }

            public string? Name { get; set; }

            public IList<CreateRuleEngineConditionBean>? Condition { get; set; }

            public int? IdRuleAction { get; set; }

            public int? IdRuleActionOption { get; set; }

            public IList<int>? IdBucket { get; set; }
        }

        public class RuleActionBean
        {
            public int? Id { get; set; }

            public string? Code { get; set; }

            public string? Name { get; set; }
        }

        public class RuleActionOptionBean
        {
            public int? Id { get; set; }

            public string? Code { get; set; }

            public string? Name { get; set; }
        }

        public class DetailRuleEngineBean
        {
            public int? Id { get; set; }

            public string? Code { get; set; }

            public string? Name { get; set; }

            public IList<CreateRuleEngineConditionBean>? Condition { get; set; }

            public RuleActionBean RuleAction { get; set; }

            public RuleActionOptionBean RuleActionOption { get; set; }

            public IList<BucketDetailResponseBean>? IdBucket { get; set; }
        }

        public class UpdateRuleEngineBean
        {

            public int? Id { get; set; }

            public string? Code { get; set; }

            public string? Name { get; set; }

            public IList<CreateRuleEngineConditionBean>? Condition { get; set; }

            public int? IdRuleAction { get; set; }

            public int? IdRuleActionOption { get; set; }

            public IList<int>? IdBucket { get; set; }
        }


        public class JobRuleListView
        {
            public int? Id { get; set; }

            public string? Name { get; set; }

            public string? Code { get; set; }

            public string? RuleEngineName { get; set; }

            public string? DataSourceName { get; set; }

            public int? NumData { get; set; }

            public int? NumProcess { get; set; }

            public DateTime? StartTime { get; set; }

            public DateTime? EndTime { get; set; }


            public int? StatusId { get; set; }

            public string? StatusName { get; set; }

        }

        public class CreateJobRuleBean
        {
            public int? IdRule { get; set; }

            public int? IdDataSource { get; set; }

            public IFormFile? File { get; set; }

        }

        public class BusinessExceptionBean
        {
            public int? Id { get; set; }

            public string? Message { get; set; }

            public string? AccNo { get; set; }

            public DateTime? CreateDate { get; set; }

        }

        public class ListJobRuleSuccess : PagedRequestBean
        {
            public int? Id { get; set; }


        }
    }
}
