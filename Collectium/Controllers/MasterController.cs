using Collectium.Config;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Collectium.Model.Bean.Response.RestructureResponse;

namespace Collectium.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class MasterController : ControllerBase
    {

        private readonly InitialService inital;
        private readonly ActionGroupService action;
        private readonly BranchAreaService baService;
        private readonly ScriptingService scService;
        private readonly StagingService stgService;
        private readonly DistribusiDataService disService;
        private readonly RejigService rejigService;
        private readonly GeneralParameterService genService;
        private readonly RestructureService restructureService;
        private readonly ILogger<MasterController> _logger;

        public MasterController(ILogger<MasterController> _logger, 
            InitialService inital, 
            ActionGroupService action, 
            BranchAreaService baService, 
            ScriptingService scService,
            DistribusiDataService disService,
            RejigService rejigService,
            StagingService stgService,
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
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/init")]
        public IActionResult Initial()
        {
            this.inital.init();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/distribusidataall")]
        public IActionResult DistributeDataAll()
        {
            this._logger.LogInformation("DistributeDataAll >>> started");
            this.stgService.processCopyFromPGToSql();
            this.disService.Distribution();
            this.rejigService.RejigData();
            //this.disService.DistributinData();
            //this.disService.SMSReminder();
            this._logger.LogInformation("DistributeDataAll >>> finished");

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/testdistribusidcfc")]
        public IActionResult TestDist()
        {
            //this.stgService.processCopyFromPGToSql();
            this.disService.Distribution();
            this.disService.DistributinData();
            //this.disService.SMSReminder();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/test/distribusi/copyfromstg")]
        public IActionResult TestDist3()
        {
            this.stgService.processCopyFromPGToSql();
            //this.disService.Distribution();
            //this.disService.DistributinData();
            //this.disService.SMSReminder();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/test/distribusi/distribution")]
        public IActionResult TestDist1()
        {
            //this.stgService.processCopyFromPGToSql();
            this.disService.Distribution();
            //this.disService.DistributinData();
            //this.disService.SMSReminder();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/test/distribusi/distribution/data")]
        public IActionResult TestDist2()
        {
            //this.stgService.processCopyFromPGToSql();
            //this.disService.Distribution();
            this.disService.DistributinData();
            //this.disService.SMSReminder();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/test/distribusi/distribution/data/reset")]
        public IActionResult TestDistReset()
        {
            //this.stgService.processCopyFromPGToSql();
            //this.disService.Distribution();
            //this.disService.DistributinData();
            //this.disService.SMSReminder();
            this.disService.DistributinReset();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/test/distribusi/distribution/data/dc")]
        public IActionResult TestDistDC()
        {
            //this.stgService.processCopyFromPGToSql();
            //this.disService.Distribution();
            //this.disService.DistributinData();
            //this.disService.SMSReminder();
            this.disService.DistributinDataDC();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/test/distribusi/distribution/data/fc")]
        public IActionResult TestDistFC()
        {
            //this.stgService.processCopyFromPGToSql();
            //this.disService.Distribution();
            //this.disService.DistributinData();
            //this.disService.SMSReminder();
            this.disService.DistributinDataFC();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/test/distribusi/distribution/generateletter")]
        public IActionResult TestGenerateLetter()
        {
            //this.stgService.processCopyFromPGToSql();
            //this.disService.Distribution();
            //this.disService.DistributinData();
            //this.disService.SMSReminder();
            //this.disService.DistributinDataFC();

            this.disService.GenerateLetter();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/test/sms")]
        public IActionResult TestSms()
        {
            //this.stgService.processCopyFromPGToSql();
            //this.disService.Distribution();
            //this.disService.DistributinData();
            this.disService.SMSReminder();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/test/loannumber")]
        public IActionResult UpdateLoanNumber()
        {
            //this.stgService.processCopyFromPGToSql();
            //this.disService.Distribution();
            //this.disService.DistributinData();
            this.disService.UpdateLoanNumber();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/rejig")]
        public IActionResult Rejig()
        {
            this.rejigService.RejigMasterLoanBranch();
            this.rejigService.RejigData();
            //this.disService.DistributinData();
            //this.disService.SMSReminder();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/rejig/cabang")]
        public IActionResult RejigCabang()
        {
            this.rejigService.RejigMasterLoanBranch();
            this.rejigService.RejigData();
            //this.disService.DistributinData();
            //this.disService.SMSReminder();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/rejig/cabang/stg")]
        public IActionResult RejigCabangStg()
        {

            this.rejigService.RejigMasterLoanBranchFromStg();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/rejig/test")]
        public IActionResult RejigTest()
        {
            //this.rejigService.RejigDCV2();
            //this.rejigService.RejigFCV2();
            //this.rejigService.ReCount();
            this.rejigService.RejigData();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/rejig/test/redist")]
        public IActionResult RejigTestRedist()
        {
            //this.rejigService.RejigDCV2();
            //this.rejigService.RejigFCV2();
            //this.rejigService.ReCount();
            this.rejigService.RejigDataRedist();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/test/copyloannumber")]
        public IActionResult CopyLoanNumber()
        {
            //this.stgService.processCopyFromPGToSql();
            //this.disService.Distribution();
            //this.disService.DistributinData();
            //this.disService.SMSReminder();
            this.disService.CopyLoanNumber();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/nodb")]
        public IActionResult NoDB()
        {

            return Ok();
        }

        [HttpGet]
        [Route("/api/[controller]/action/list")]
        public GenericResponse<ActionResponseBean> ListAction([FromQuery] ActionListRequestBean bean)
        {
            return this.action.ListAction(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/action/detail")]
        public GenericResponse<ActionResponseBean> DetailAction([FromQuery] UserReqApproveBean bean)
        {
            return this.action.DetailAction(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN2", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/action/request/list")]
        public GenericResponse<ActionReqResponseBean> ListActionRequest([FromQuery] ActionReqListRequestBean bean)
        {
            return this.action.ListActionRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/action/request/detail")]
        public GenericResponse<ActionReqResponseBean> DetailActionRequest([FromQuery] UserReqApproveBean bean)
        {
            return this.action.DetailActionRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/action/create")]
        public GenericResponse<ActionReqCreateBean> CreateAction(ActionReqCreateBean filter)
        {
            return this.action.SaveNewActionRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/action/update")]
        public GenericResponse<ActionReqEditBean> UpdateAction(ActionReqEditBean filter)
        {
            return this.action.SaveEditActionRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/action/request/approve")]
        public GenericResponse<UserReqApproveBean> ApproveRequestAction(UserReqApproveBean filter)
        {
            return this.action.ApproveActionRequest(filter);
        }

        //ACTIONGROUP
        [HttpGet]
        [Route("/api/[controller]/action/group/list")]
        public GenericResponse<ActionGroupResponseBean> ListActionGroup([FromQuery] ActionGroupListRequestBean bean)
        {
            return this.action.ListActionGroup(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/action/group/detail")]
        public GenericResponse<ActionGroupResponseBean> DetailActionGroup([FromQuery] UserReqApproveBean bean)
        {
            return this.action.DetailActionGroup(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/action/group/request/list")]
        public GenericResponse<ActionGroupReqResponseBean> ListActionGroupRequest([FromQuery] ActionGroupReqListRequestBean bean)
        {
            return this.action.ListActionGroupRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/action/group/request/detail")]
        public GenericResponse<ActionGroupReqResponseBean> DetailActionGroupRequest([FromQuery] UserReqApproveBean bean)
        {
            return this.action.DetailActionGroupRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/action/group/create")]
        public GenericResponse<ActionGroupReqCreateBean> CreateActionGroup(ActionGroupReqCreateBean filter)
        {
            return this.action.SaveNewActionGroupRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/action/group/update")]
        public GenericResponse<ActionGroupReqEditBean> UpdateActionGroup(ActionGroupReqEditBean filter)
        {
            return this.action.SaveEditActionGroupRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/action/group/request/approve")]
        public GenericResponse<UserReqApproveBean> ApproveRequestActionGroup(UserReqApproveBean filter)
        {
            return this.action.ApproveActionGroupRequest(filter);
        }

        //ACCOUNTDISTRIBUTION
        [HttpGet]
        [Route("/api/[controller]/accountdistribution/list")]
        public GenericResponse<AccDistResponseBean> ListAccDist([FromQuery] ActionGroupListRequestBean bean)
        {
            return this.action.ListAccDist(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/accountdistribution/detail")]
        public GenericResponse<AccDistResponseBean> DetailAccDist([FromQuery] UserReqApproveBean bean)
        {
            return this.action.DetailAccDist(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/accountdistribution/request/list")]
        public GenericResponse<AccDistReqResponseBean> ListAccDistRequest([FromQuery] AccDistReqListRequestBean bean)
        {
            return this.action.ListAccDistRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/accountdistribution/request/detail")]
        public GenericResponse<AccDistReqResponseBean> DetailAccDistRequest([FromQuery] UserReqApproveBean bean)
        {
            return this.action.DetailAccDistRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/accountdistribution/create")]
        public GenericResponse<AccDistReqCreateBean> SaveNewAccDistRequest(AccDistReqCreateBean filter)
        {
            return this.action.SaveNewAccDistRequest(filter);
        }

        [JWTAuthorize("ADMIN")]
        [HttpPost]
        [Route("/api/[controller]/accountdistribution/update")]
        public GenericResponse<AccDistReqEditBean> SaveEditAccDistRequest(AccDistReqEditBean filter)
        {
            return this.action.SaveEditAccDistRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/accountdistribution/request/approve")]
        public GenericResponse<UserReqApproveBean> ApproveAccDistRequest(UserReqApproveBean filter)
        {
            return this.action.ApproveAccDistRequest(filter);
        }

        //BRANCH
        [HttpGet]
        [Route("/api/[controller]/branch/list")]
        public GenericResponse<BranchResponseBean> ListBranch([FromQuery] BranchListRequestBean bean)
        {
            return this.baService.ListBranch(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/branch/detail")]
        public GenericResponse<BranchResponseBean> DetailBranch([FromQuery] UserReqApproveBean bean)
        {
            return this.baService.DetailBranch(bean);
        }
            
        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/branch/request/list")]
        public GenericResponse<BranchReqResponseBean> ListBranchRequest([FromQuery] BranchReqListRequestBean bean)
        {
            return this.baService.ListBranchRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/branch/request/detail")]
        public GenericResponse<BranchReqResponseBean> DetailBranchRequest([FromQuery] UserReqApproveBean bean)
        {
            return this.baService.DetailBranchRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/branch/create")]
        public GenericResponse<BranchReqCreateBean> SaveNewActionRequest(BranchReqCreateBean filter)
        {
            return this.baService.SaveNewActionRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/branch/update")]
        public GenericResponse<BranchReqEditCreateBean> SaveEditActionRequest(BranchReqEditCreateBean filter)
        {
            return this.baService.SaveEditActionRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/branch/request/approve")]
        public GenericResponse<UserReqApproveBean> ApproveBranchRequest(UserReqApproveBean filter)
        {
            return this.baService.ApproveBranchRequest(filter);
        }

        //Callscript
        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/callscript/list")]
        public GenericResponse<CallScriptResponseBean> ListCallScript([FromQuery] CallScriptListRequestBean bean)
        {
            return this.scService.ListCallScript(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/callscript/detail")]
        public GenericResponse<CallScriptResponseBean> DetailCallScript([FromQuery] UserReqApproveBean bean)
        {
            return this.scService.DetailScript(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/callscript/request/list")]
        public GenericResponse<CallScriptReqResponseBean> ListCallScriptRequest([FromQuery] CallScriptReqListRequestBean bean)
        {
            return this.scService.ListCallScriptRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/callscript/request/detail")]
        public GenericResponse<CallScriptReqResponseBean> DetailCallScriptRequest([FromQuery] UserReqApproveBean bean)
        {
            return this.scService.DetailCallScriptRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/callscript/create")]
        public GenericResponse<CallScriptCreateBean> CreateCallScript(CallScriptCreateBean filter)
        {
            return this.scService.SaveNewCallScriptRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/callscript/update")]
        public GenericResponse<CallScriptReqEditBean> UpdateCallScript(CallScriptReqEditBean filter)
        {
            return this.scService.SaveEditCallScriptRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/callscript/request/approve")]
        public GenericResponse<UserReqApproveBean> ApproveCallScript(UserReqApproveBean filter)
        {
            return this.scService.ApproveCallScriptRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/callscript/request/reject")]
        public GenericResponse<UserReqApproveBean> ReejctCallScript(UserReqApproveBean filter)
        {
            return this.scService.ApproveCallScriptRequest(filter);
        }

        //NotifContent
        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/notifcontent/list")]
        public GenericResponse<NotifResponseBean> ListNotifContent([FromQuery] NotifListRequestBean bean)
        {
            return this.scService.ListNotif(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/notifcontent/detail")]
        public GenericResponse<NotifResponseBean> DetailNotifContent([FromQuery] UserReqApproveBean bean)
        {
            return this.scService.DetailNotifContent(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/notifcontent/request/list")]
        public GenericResponse<NotifReqResponseBean> ListNotifContentRequest([FromQuery] NotifReqListRequestBean bean)
        {
            return this.scService.ListNotifRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/notifcontent/request/detail")]
        public GenericResponse<NotifReqResponseBean> DetailNotifContentRequest([FromQuery] UserReqApproveBean bean)
        {
            return this.scService.DetailNotifRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/notifcontent/create")]
        public GenericResponse<NotifCreateBean> CreateNotifContent(NotifCreateBean filter)
        {
            return this.scService.SaveNewNotifRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/notifcontent/update")]
        public GenericResponse<NotifReqEditBean> UpdateNotifContent(NotifReqEditBean filter)
        {
            return this.scService.SaveEditNotifRequest(filter);
        }

        [HttpPost]
        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [Route("/api/[controller]/notifcontent/request/approve")]
        public GenericResponse<UserReqApproveBean> ApproveNotifContent(UserReqApproveBean filter)
        {
            return this.scService.ApproveNotifRequest(filter);
        }

        [HttpPost]
        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [Route("/api/[controller]/notifcontent/request/reject")]
        public GenericResponse<UserReqApproveBean> RejectNotifContent(UserReqApproveBean filter)
        {
            return this.scService.RejectNotifRequest(filter);
        }


        //Global Parameter
        [HttpGet]
        [Route("/api/[controller]/global/list")]
        public GenericResponse<GlobalResponseBean> ListGlobal([FromQuery] GlobalListRequestBean bean)
        {
            return this.action.ListGlobal(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/global/detail")]
        public GenericResponse<GlobalEditBean> DetailGlobal()
        {
            return this.action.DetailGlobal();
        }

        /*
        [JWTAuthorize("SUPERUSER", "ADMIN")]
        [HttpPost]
        [Route("/api/[controller]/global/create")]
        public GenericResponse<GlobalCreateBean> CreateGlobal(GlobalCreateBean filter)
        {
            return this.action.SaveGlobal(filter);
        }
        */

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/global/update")]
        public GenericResponse<GlobalEditBean> UpdateGlobal(GlobalEditBean filter)
        {
            return this.action.UpdateGlobal(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/global/upload")]
        public GenericResponse<GlobalEditBean> UploadCabang([FromForm] FileUploadModel filter)
        {
            return this.action.UploadCabang(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/global/setting/dc")]
        public GenericResponse<SettingDcBean> SettingDc()
        {
            return this.action.GetSettingDc();
        }

        [HttpPost]
        [Route("/api/[controller]/global/setting/dc/update")]
        public GenericResponse<SettingDcBean> SettingDcUpdate(SettingDcBean filter)
        {
            return this.action.SaveSettingDc(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/global/setting/fc")]
        public GenericResponse<SettingDcBean> SettingFc()
        {
            return this.action.GetSettingFc();
        }

        [HttpPost]
        [Route("/api/[controller]/global/setting/fc/update")]
        public GenericResponse<SettingDcBean> SettingFcUpdate(SettingDcBean filter)
        {
            return this.action.SaveSettingFc(filter);
        }

        //NotifContent
        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/reason/list")]
        public GenericResponse<ReasonResponseBean> ListReason([FromQuery] ReasonListRequestBean bean)
        {
            return this.scService.ListReason(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/reason/detail")]
        public GenericResponse<ReasonResponseBean> DetailReason([FromQuery] UserReqApproveBean bean)
        {
            return this.scService.DetailListReason(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/reason/request/list")]
        public GenericResponse<ReasonReqResponseBean> ListReasonRequest([FromQuery] ReasonListRequestBean bean)
        {
            return this.scService.ListReasonRequest(bean);

        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpGet]
        [Route("/api/[controller]/reason/request/detail")]
        public GenericResponse<ReasonReqResponseBean> DetailReasonRequest([FromQuery] UserReqApproveBean bean)
        {
            return this.scService.DetailReasonRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/reason/create")]
        public GenericResponse<ReasonCreateBean> CreateReason(ReasonCreateBean filter)
        {
            return this.scService.SaveNewReasonRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [HttpPost]
        [Route("/api/[controller]/reason/update")]
        public GenericResponse<ReasonReqEditBean> UpdateReason(ReasonReqEditBean filter)
        {
            return this.scService.SaveEditReasonRequest(filter);
        }

        [HttpPost]
        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [Route("/api/[controller]/reason/request/approve")]
        public GenericResponse<UserReqApproveBean> ApproveReason(UserReqApproveBean filter)
        {
            return this.scService.ApproveReasonRequest(filter);
        }

        [HttpPost]
        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2")]
        [Route("/api/[controller]/reason/request/reject")]
        public GenericResponse<UserReqApproveBean> RejectReason(UserReqApproveBean filter)
        {
            return this.scService.RejectRequestRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/dokumenrestruktur/list")]
        public GenericResponse<DocumentRestruktur> ListDocumentRestruktur()
        {
            return this.genService.ListDocumentRestruktur();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/polarestruktur/list")]
        public GenericResponse<PolaRestruktur> ListPolaRestruktur()
        {
            return this.genService.ListPolaRestruktur();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/jenispengurangan/list")]
        public GenericResponse<JenisPengurangan> ListJenisPengurangan()
        {
            return this.genService.ListJenisPengurangan();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/pembayarangp/list")]
        public GenericResponse<PembayaranGp> ListPembayaranGp()
        {
            return this.genService.ListPembayaranGp();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/alasanlelang/list")]
        public GenericResponse<AlasanLelang> ListAlasanLelang()
        {
            return this.genService.ListAlasanLelang();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/balailelang/list")]
        public GenericResponse<BalaiLelang> ListBalaiLelang()
        {
            return this.genService.ListBalaiLelang();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/jenislelang/list")]
        public GenericResponse<JenisLelang> ListJenisLelang()
        {
            return this.genService.ListJenisLelang();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/dokumenlelang/list")]
        public GenericResponse<DocumentAuction> ListDocumentAuction()
        {
            return this.genService.ListDocumentAuction();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/dokumenhasillelang/list")]
        public GenericResponse<DocumentAuctionResult> ListDocumentAuctionResult()
        {
            return this.genService.ListDocumentAuctionResult();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/asuransi/list")]
        public GenericResponse<Asuransi> ListAsuransi()
        {
            return this.genService.ListAsuransi();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/sisaklaim/list")]
        public GenericResponse<AsuransiSisaKlaim> ListAsuransiSisaKlaim()
        {
            return this.genService.ListAsuransiSisaKlaim();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/dokumenasuransi/list")]
        public GenericResponse<DocumentInsurance> ListDocumentInsurance()
        {
            return this.genService.ListDocumentInsurance();
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "MANAJEMEN")]
        [HttpGet]
        [Route("/api/[controller]/recovery/eksekusi/list")]
        public GenericResponse<RecoveryExecution> ListRecoveryExecution()
        {
            return this.genService.ListRecoveryExecution();
        }

        [HttpGet]
        [Route("/api/[controller]/product/list")]
        public GenericResponse<ProductLoanResponseBean> ListProduct()
        {
            return this.action.ListProduct();
        }

        [HttpGet]
        [Route("/api/[controller]/rule/list")]
        public GenericResponse<DistribusiManualList> DistribusiManualList([FromQuery] DistribusiManualReq bean)
        {
            return this.rejigService.DistribusiManualList(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/rule/create")]
        public GenericResponse<DistribusiBeanSave> SaveDistribusiManual(DistribusiBeanSave filter)
        {
            return this.rejigService.SaveDistribusiManual(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/rule/nasabah/list")]
        public GenericResponse<CollResponseBean> DistribusiNasabahList([FromQuery] DistribusiNasabahReq filter)
        {
            return this.rejigService.DistribusiNasabahList(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/rule/agent/list")]
        public GenericResponse<UserResponseBean> DistribusiAgentList([FromQuery] DistribusiAgentReq filter)
        {
            return this.rejigService.DistribusiAgentList(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/rule/get")]
        public GenericResponse<DistribusiManualGet> DistribusiManualGet([FromQuery] UserReqApproveBean bean)
        {
            return this.rejigService.DistribusiManualGet(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/recovery/recoveryfield")]
        public GenericResponse<RecoveryField> ListRecoveryField()
        {
            return this.restructureService.ListRecoveryField();
        }

    }
}
