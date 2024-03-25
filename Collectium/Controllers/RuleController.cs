using Collectium.Config;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Collectium.Model.Bean.ListRequest.RecoveryListRequest;
using static Collectium.Model.Bean.Response.RestructureResponse;

namespace Collectium.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class RuleController : ControllerBase
    {

        private readonly InitialService inital;
        private readonly ActionGroupService action;
        private readonly BranchAreaService baService;
        private readonly ScriptingService scService;
        private readonly StagingService stgService;
        private readonly DistribusiDataService disService;
        private readonly BucketService bucketService;
        private readonly RuleEngineService ruleEngineService;
        private readonly RejigService rejigService;
        private readonly GeneralParameterService genService;
        private readonly RestructureService restructureService;
        private readonly ILogger<MasterController> _logger;

        public RuleController(ILogger<MasterController> _logger, 
            InitialService inital, 
            ActionGroupService action, 
            BranchAreaService baService, 
            ScriptingService scService,
            DistribusiDataService disService,
            RejigService rejigService,
            BucketService bucketService,
            StagingService stgService,
            RuleEngineService ruleEngineService,
            RestructureService restructureService,
            GeneralParameterService genService)
        {
            this._logger = _logger;
            this.inital = inital;
            this.action = action;
            this.baService = baService;
            this.scService = scService;
            this.stgService = stgService;
            this.disService = disService;
            this.rejigService = rejigService;
            this.genService = genService;
            this.restructureService = restructureService;
            this.bucketService = bucketService;
            this.ruleEngineService = ruleEngineService; 
        }

        [HttpGet]
        [Route("/api/[controller]/bucket/list")]
        public GenericResponse<BucketListResponseBean> ListBucket([FromQuery] BucketListBean bean)
        {
            return this.bucketService.List(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/bucket/detail")]
        public GenericResponse<BucketDetailResponseBean> DetailBucket([FromQuery] UserReqApproveBean bean)
        {
            return this.bucketService.Detail(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/bucket/save")]
        public GenericResponse<BucketCreateBean> SaveBucket(BucketCreateBean bean)
        {
            return this.bucketService.Create(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/bucket/update")]
        public GenericResponse<BucketUpdateBean> UpdateBucket(BucketUpdateBean bean)
        {
            return this.bucketService.Update(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/ruleengine/list")]
        public GenericResponse<RuleEngineListView> ListRule([FromQuery] RuleEngineListBean bean)
        {
            return this.ruleEngineService.List(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/ruleengine/detail")]
        public GenericResponse<DetailRuleEngineBean> DetailRule([FromQuery] UserReqApproveBean bean)
        {
            return this.ruleEngineService.Detail(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/ruleengine/save")]
        public GenericResponse<CreateRuleEngineBean> SaveRule(CreateRuleEngineBean bean)
        {
            return this.ruleEngineService.Create(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/ruleengine/update")]
        public GenericResponse<UpdateRuleEngineBean> UpdateRule(UpdateRuleEngineBean bean)
        {
            return this.ruleEngineService.Update(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/rule/job/list")]
        public GenericResponse<JobRuleListView> ListJob([FromQuery] RuleEngineListBean bean)
        {
            return this.ruleEngineService.ListJob(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/rule/job/create")]
        public GenericResponse<string> UpdateRule([FromForm] CreateJobRuleBean bean)
        {
            return this.ruleEngineService.CreateJob(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/rule/job/detail")]
        public GenericResponse<JobRuleListView> JobDetail([FromQuery] UserReqApproveBean bean)
        {
            return this.ruleEngineService.DetailJob(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/rule/job/exception")]
        public GenericResponse<BusinessExceptionBean> ListException([FromQuery] RuleEngineListBean bean)
        {
            return this.ruleEngineService.ListBusinessException(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/rule/job/success")]
        public GenericResponse<CollTraceResponseBean> ListException([FromQuery] ListJobRuleSuccess bean)
        {
            return this.ruleEngineService.ListCallTrace(bean);
        }


        [HttpGet]
        [Route("/api/[controller]/ref/ruleaction")]
        public GenericResponse<RuleAction> ListRuleAction()
        {
            return this.bucketService.ListRuleAction();
        }

        [HttpGet]
        [Route("/api/[controller]/ref/ruleactionoption")]
        public GenericResponse<RuleActionOption> ListRuleActionOption()
        {
            return this.bucketService.ListRuleActionOption();
        }

        [HttpGet]
        [Route("/api/[controller]/ref/rulevaluetype")]
        public GenericResponse<RuleValueType> ListRuleValueType()
        {
            return this.bucketService.ListRuleValueType();
        }

        [HttpGet]
        [Route("/api/[controller]/ref/ruleoperator")]
        public GenericResponse<RuleOperator> ListRuleOperator()
        {
            return this.bucketService.ListRuleOperator();
        }


        [HttpGet]
        [Route("/api/[controller]/ref/ruledatafield")]
        public GenericResponse<RuleDataField> ListRuleDataField()
        {
            return this.bucketService.ListRuleDataField();
        }

        [HttpGet]
        [Route("/api/[controller]/ref/rulecustomertag")]
        public GenericResponse<CustomerTag> ListCustomerTag()
        {
            return this.bucketService.ListCustomerTag();
        }

        [HttpGet]
        [Route("/api/[controller]/ref/datasource")]
        public GenericResponse<DataSource> ListDataSource()
        {
            return this.bucketService.ListDataSource();
        }

        [HttpGet]
        [Route("/api/[controller]/ref/agent")]
        public GenericResponse<UserResponseBean> ListAgentByBranch()
        {
            return this.ruleEngineService.ListAgentByBranch();
        }
    }
}
