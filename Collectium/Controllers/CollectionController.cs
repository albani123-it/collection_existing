using AutoMapper;
using Collectium.Config;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Service;
using Collectium.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Collectium.Model.Bean.Request.IntegrationRequestBean;

namespace Collectium.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class CollectionController : ControllerBase
    {

        private readonly CollectionService colService;
        private readonly GenerateLetterService letterService;
        private readonly IntegrationService integrationService;
        private readonly CallTraceService traceService;
        private readonly RejigService rejigService;
        private readonly IMapper mapper;
        private readonly ILogger<CollectionController> _logger;

        public CollectionController(ILogger<CollectionController> _logger, 
            CollectionService colService, 
            GenerateLetterService letterService,
            IntegrationService integrationService,
            CallTraceService traceService,
            RejigService jigService,
            IMapper mapper)
        {
            this._logger = _logger;
            this.mapper = mapper;
            this.colService = colService;
            this.letterService = letterService;
            this.integrationService = integrationService;
            this.traceService = traceService;
            this.rejigService = jigService;
        }

        [HttpGet]
        [Route("/api/[controller]/callresult/list")]
        public GenericResponse<CallResultColResponseBean> ListCallResult()
        {
            return this.colService.ListCallResult();
        }

        [HttpGet]
        [Route("/api/[controller]/reason/list")]
        public GenericResponse<ReasonResponseBean> ListReason()
        {
            return this.colService.ListReason();
        }

        [HttpGet]
        [Route("/api/[controller]/callresult/fc/list")]
        public GenericResponse<CallResultColResponseBean> ListCallFCResult()
        {
            return this.colService.ListCallResultFC();
        }

        [HttpGet]
        [Route("/api/[controller]/reason/fc/list")]
        public GenericResponse<ReasonResponseBean> ListReasonFC()
        {
            return this.colService.ListReasonFC();
        }

        [HttpGet]
        [Route("/api/[controller]/team/deskcall")]
        public GenericResponse<UserResponseBean> ListMyTeam()
        {
            return this.colService.ListTeamByBranch();
        }

        [HttpGet]
        [Route("/api/[controller]/team/fieldcoll")]
        public GenericResponse<UserResponseBean> ListFcTeam()
        {
            return this.colService.ListFcByBranch();
        }

        [HttpGet]
        [Route("/api/[controller]/team/branch")]
        public GenericResponse<UserResponseBean> ListAgentByBranch()
        {
            return this.colService.ListAgentByBranch();
        }

        [HttpGet]
        [Route("/api/[controller]/team/branch/my")]
        public GenericResponse<BranchResponseBean> MyBranch()
        {
            return this.colService.ListMyBranch();
        }



        [HttpGet]
        [Route("/api/[controller]/team/list")]
        public GenericResponse<UserResponseBean> ListTeam([FromQuery] ListTeamBean filter)
        {
            return this.colService.ListTeamAll(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/deskcall/list")]
        public GenericResponse<CollResponseBean> ListDeskCall([FromQuery] CollListRequestBean bean)
        {
            return this.colService.ListCollection(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/deskcall/detail")]
        public GenericResponse<CollDetailResponseBean> DetailDeskCall([FromQuery] UserReqApproveBean bean)
        {
            return this.colService.DetailCollection(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/deskcall/save")]
        public GenericResponse<SaveResultBean> SaveDC(SaveResultBean bean)
        {
            var bb = this.mapper.Map<SaveResultBeanToDc>(bean);
            return this.colService.SaveCallResult(bb);
        }

        [HttpPost]
        [Route("/api/[controller]/deskcall/save/tofc")]
        public GenericResponse<SaveResultBean> SaveDCToFC(SaveResultBeanToDc bean)
        {
            return this.colService.SaveCallResult(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/deskcall/spv/list")]
        public GenericResponse<SpvCollResponseBean> ListSpvCollection([FromQuery] SpvListCollectiontBean bean)
        {
            return this.colService.ListMyTeam(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/deskcall/spv/monitor/list")]
        public GenericResponseSum<SpvMonResponseBean, ReportCounter> MonitorSPV([FromQuery] SpvMonListBean bean)
        {
            bean.MyType = "SPV";
            var sum = this.colService.MonitorMyTeamRes(bean);
            var data = this.colService.MonitorMyTeam(bean);

            var res = new GenericResponseSum<SpvMonResponseBean, ReportCounter>();
            res.Data = data.Data;
            res.Status = data.Status;
            res.Message = data.Message;
            res.Page = data.Page;
            res.MaxPage = data.MaxPage;
            res.Summary = sum;

            return res;

        }

        [HttpGet]
        [Route("/api/[controller]/deskcall/dcfc/monitor/list")]
        public GenericResponseSum<SpvMonResponseBean, ReportCounter> MonitorDCFC([FromQuery] SpvMonListBean bean)
        {
            bean.MyType = "DCFC";

            var sum = this.colService.MonitorMyTeamRes(bean);
            var data = this.colService.MonitorMyTeam(bean);

            var res = new GenericResponseSum<SpvMonResponseBean, ReportCounter>();
            res.Data = data.Data;
            res.Status = data.Status;
            res.Message = data.Message;
            res.Page = data.Page;
            res.MaxPage = data.MaxPage;
            res.Summary = sum;

            return res;
        }

        [HttpGet]
        [Route("/api/[controller]/deskcall/cabang/monitor/list")]
        public GenericResponseSum<SpvMonResponseBean, ReportCounter> MonitorCabang([FromQuery] SpvMonListBean bean)
        {
            bean.MyType = "CABANG";

            var sum = this.colService.MonitorMyTeamRes(bean);
            var data = this.colService.MonitorMyTeam(bean);

            var res = new GenericResponseSum<SpvMonResponseBean, ReportCounter>();
            res.Data = data.Data;
            res.Status = data.Status;
            res.Message = data.Message;
            res.Page = data.Page;
            res.MaxPage = data.MaxPage;
            res.Summary = sum;

            return res;
        }

        [HttpGet]
        [Route("/api/[controller]/deskcall/admin/monitor/list")]
        public GenericResponseSum<SpvMonResponseBean, ReportCounter> MonitorAdmin([FromQuery] SpvMonListBean bean)
        {
            bean.MyType = "ADM";

            var sum = this.colService.MonitorMyTeamRes(bean);
            var data = this.colService.MonitorMyTeam(bean);

            var res = new GenericResponseSum<SpvMonResponseBean, ReportCounter>();
            res.Data = data.Data;
            res.Status = data.Status;
            res.Message = data.Message;
            res.Page = data.Page;
            res.MaxPage = data.MaxPage;
            res.Summary = sum;

            return res;
        }

        [HttpPost]
        [Route("/api/[controller]/deskcall/reassign")]
        public GenericResponse<ReassignBean> ReassignDC(ReassignBean bean)
        {
            var wrapper = new ReassignWrapper();
            wrapper.Loan = bean.Loan;
            wrapper.ToMember = bean.ToMember;
            wrapper.ToRole = "DC";
            return this.colService.Reassign(wrapper);
        }

        [HttpPost]
        [Route("/api/[controller]/deskcall/contact/save")]
        public GenericResponse<AddressResponseBean> SaveContactDc(SaveContactBean bean)
        {
            if (bean == null)
            {
                var wrap = new GenericResponse<AddressResponseBean>();
                wrap.Status = false;
                wrap.Message = "Data tidak valid";
                return wrap;
            }

            var dto = new SaveContactDTOBean();
            dto.AddFrom = "DC WEB";
            IlKeiCopyObject.Instance.WithSource(bean).WithDestination(dto)
                .Include(nameof(bean.AddAddress))
                .Include(nameof(bean.AddCity))
                .Include(nameof(bean.AddPhone))
                .Include(nameof(bean.LoanId))
                .Include(nameof(bean.Lat))
                .Include(nameof(bean.Lon))
                .Execute();
            return this.colService.SaveContact(dto);
        }

        [HttpGet]
        [Route("/api/[controller]/fieldcoll/list")]
        public GenericResponse<CollResponseBean> ListFC([FromQuery] SpvListCollectiontBean bean)
        {
            var xbean = new CollListRequestBean();
            xbean.Keyword = bean.Keyword;
            xbean.Page = bean.Page;
            xbean.PageRow = bean.PageRow;
            xbean.sortasc = bean.sortasc;
            xbean.sortdesc = bean.sortdesc;
            xbean.sorttagihan = bean.sorttagihan;
            xbean.sortoverduedate = bean.sortoverduedate;
            xbean.BranchId = bean.BranchId;
            return this.colService.ListCollectionFC(xbean);
        }

        [HttpGet]
        [Route("/api/[controller]/fieldcoll/list/ptp")]
        public GenericResponse<CollResponseBean> ListFCPTP([FromQuery] SpvListCollectiontBean bean)
        {
            var xbean = new CollListRequestBean();
            xbean.Keyword = bean.Keyword;
            xbean.PtpDate = bean.PtpDate;
            xbean.BranchId = bean.BranchId;
            xbean.Page = bean.Page;
            xbean.PageRow = bean.PageRow;
            xbean.CallResultCode = "PTP";
            xbean.sortasc = bean.sortasc;
            xbean.sortdesc = bean.sortdesc;
            xbean.sorttagihan = bean.sorttagihan;
            xbean.sortoverduedate = bean.sortoverduedate;
            return this.colService.ListCollectionFCPTP(xbean);
        }

        [HttpGet]
        [Route("/api/[controller]/fieldcoll/list/riwayat")]
        public GenericResponse<CollResponseBean> ListFCRiwayat([FromQuery] SpvListCollectiontBean bean)
        {
            var xbean = new CollListRequestBean();
            xbean.Keyword = bean.Keyword;
            xbean.Page = bean.Page;
            xbean.PageRow = bean.PageRow;
            //xbean.CallResultCode = "PTP";
            xbean.sortasc = bean.sortasc;
            xbean.sortdesc = bean.sortdesc;
            xbean.sorttagihan = bean.sorttagihan;
            xbean.sortoverduedate = bean.sortoverduedate;
            return this.colService.ListCollectionFCRiwayat(xbean);
        }

        [HttpGet]
        [Route("/api/[controller]/fieldcoll/detail")]
        public GenericResponse<CollDetailResponseBean> DetailFieldCall([FromQuery] UserReqApproveBean bean)
        {
            return this.colService.DetailCollection(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/fieldcoll/save")]
        public GenericResponse<SaveResultBean> SaveFC(SaveResultBeanFc bean)
        {
            return this.colService.SaveCallResultFC(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/fieldcoll/spv/list")]
        public GenericResponse<SpvCollResponseBean> ListSpvFieldCollection([FromQuery] SpvListCollectiontBean bean)
        {
            return this.colService.ListMyTeam(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/fieldcoll/spv/monitor/list")]
        public GenericResponse<SpvMonResponseBean> MonitorFC([FromQuery] SpvMonListBean bean)
        {
            return this.colService.MonitorMyTeam(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/fieldcoll/reassign")]
        public GenericResponse<ReassignBean> ReassignFC(ReassignBean bean)
        {
            var wrapper = new ReassignWrapper();
            wrapper.Loan = bean.Loan;
            wrapper.ToMember = bean.ToMember;
            wrapper.ToRole = "FC";
            return this.colService.Reassign(wrapper);
        }

        [HttpPost]
        [Route("/api/[controller]/fieldcoll/contact/save")]
        public GenericResponse<AddressResponseBean> SaveContactFc(SaveContactBean bean)
        {
            if (bean == null)
            {
                var wrap = new GenericResponse<AddressResponseBean>();
                wrap.Status = false;
                wrap.Message = "Data tidak valid";
                return wrap;
            }

            var dto = new SaveContactDTOBean();
            dto.AddFrom = "FC APPS";
            IlKeiCopyObject.Instance.WithSource(bean).WithDestination(dto)
                .Include(nameof(bean.AddAddress))
                .Include(nameof(bean.AddCity))
                .Include(nameof(bean.AddPhone))
                .Include(nameof(bean.LoanId))
                .Execute();

            if (bean.PhotoId != null && bean.PhotoId.Count > 0)
            {
                dto.PhotoId = bean.PhotoId;
            }
            return this.colService.SaveContact(dto);
        }

        [HttpPost]
        [Route("/api/[controller]/fieldcoll/contact/setasdefault")]
        public GenericResponse<UserReqApproveBean> SetAsDefault(UserReqApproveBean bean)
        {
            return this.colService.SetAsDefault(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/letter/list")]
        public GenericResponse<CollResponseBean> ListLetter([FromQuery] CollListRequestBean bean)
        {
            return this.colService.ListCollectionLetter(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/letter/history")]
        public GenericResponse<GenerateLetterHistoryResponseBean> HistoryLetter([FromQuery] GenerateLetterHistoryBean bean)
        {
            return this.letterService.HistoryGenerateLetter(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/letter/view")]
        public GenericResponse<GenerateLetterResponseBean> GenerateLetter([FromQuery] GenerateLetterRequestBean filter)
        {
            return this.letterService.ListGenerateLetterV2(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/letter/generate")]
        public IActionResult GenerateLetterPdf([FromQuery] GenerateLetterPdfRequestBean bean)
        {
            return this.letterService.GenerateLetterV2(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/deskcall/spv/monitor/print")]
        public IActionResult SPVDCReportMonitor([FromQuery] ReportMonitorRequestFE bean)
        {
            //var b = new ReportMonitorRequest();
            //b.BranchId = bean.BranchId;
            //b.Start = bean.Start;
            //b.End = bean.End;
            //b.RoleId = 3;

            var payload = new SpvMonListBean();
            var st = DateTime.Parse(bean.Start!);
            var ed = DateTime.Parse(bean.End!);

            payload.StartDate = st;
            payload.EndDate = ed;
            payload.MyType = "SPV";

            return this.colService.GenerateReportMonitor(payload);

        }

        [HttpGet]
        [Route("/api/[controller]/fieldcoll/spv/monitor/print")]
        public IActionResult SPVFCReportMonitor([FromQuery] ReportMonitorRequestFE bean)
        {
            var b = new ReportMonitorRequest();
            b.BranchId = bean.BranchId;
            b.Start = bean.Start;
            b.End = bean.End;
            b.RoleId = 4;

            return this.letterService.GenerateReportMonitor(b);
        }

        [HttpGet]
        [Route("/api/[controller]/fieldcoll/monitor/print")]
        public IActionResult FCReportMonitor([FromQuery] ReportMonitorRequestFE bean)
        {
            var payload = new SpvMonListBean();
            var st = DateTime.Parse(bean.Start!);
            var ed = DateTime.Parse(bean.End!);

            payload.StartDate = st;
            payload.EndDate = ed;
            payload.MyType = "DCFC";

            return this.colService.GenerateReportMonitor(payload);
        }

        [HttpGet]
        [Route("/api/[controller]/deskcall/monitor/print")]
        public IActionResult DCReportMonitor([FromQuery] ReportMonitorRequestFE bean)
        {
            var payload = new SpvMonListBean();
            var st = DateTime.Parse(bean.Start!);
            var ed = DateTime.Parse(bean.End!);

            payload.StartDate = st;
            payload.EndDate = ed;
            payload.MyType = "DCFC";

            return this.colService.GenerateReportMonitor(payload);
        }

        [HttpGet]
        [Route("/api/[controller]/cabang/monitor/print")]
        public IActionResult CabReportMonitor([FromQuery] ReportMonitorRequestFE bean)
        {

            //var b = new ReportMonitorRequest();
            //b.BranchId = bean.BranchId;
            //b.Start = bean.Start;
            //b.End = bean.End;
            //b.RoleId = 99998;

            var payload = new SpvMonListBean();
            var st = DateTime.Parse(bean.Start!);
            var ed = DateTime.Parse(bean.End!);

            payload.StartDate = st;
            payload.EndDate = ed;
            payload.MyType = "CABANG";

            return this.colService.GenerateReportMonitor(payload);
        }

        [HttpGet]
        [Route("/api/[controller]/admin/monitor/print")]
        public IActionResult AdmReportMonitor([FromQuery] ReportMonitorRequestFE bean)
        {
            var payload = new SpvMonListBean();
            var st = DateTime.Parse(bean.Start!);
            var ed = DateTime.Parse(bean.End!);

            payload.StartDate = st;
            payload.EndDate = ed;
            payload.MyType = "ADMIN";

            return this.colService.GenerateReportMonitor(payload);
        }

        [HttpPost]
        [Route("/api/[controller]/uploadphoto")]
        public GenericResponse<CollectionPhoto> UploadPhoto([FromForm] UploadRequestBean bean)
        {
            return this.colService.UploadPhoto(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/deletephoto")]
        public GenericResponse<UserReqApproveBean> DeletePhoto(UserReqApproveBean bean)
        {
            return this.colService.DeletePhoto(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/viewphoto")]
        public IActionResult ViewPhoto([FromQuery] UserReqApproveBean bean)
        {
            return this.colService.ViewPhoto(bean);
        }


        [HttpPost]
        [Route("/api/[controller]/contact/uploadphoto")]
        public GenericResponse<CollectionContactPhoto> UploadContactPhoto([FromForm] UploadRequestBean bean)
        {
            return this.colService.UploadContactPhoto(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/contact/viewphoto")]
        public IActionResult ViewContactPhoto([FromQuery] UserReqApproveBean bean)
        {
            return this.colService.ViewContactPhoto(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/trackingfc")]
        public GenericResponse<TrackingBean> TrackingFc(TrackingBean bean)
        {
            return this.colService.TrackingCol(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/trackingfc/history")]
        public GenericResponse<TrackingHistoryResponseBean> GetTrackingFc(SpvListCollectiontBean bean)
        {
            return this.colService.GetTrackingHistory(bean);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/testgen")]
        public GenericResponse<GenerateLetterResponseBean> GenerateLetterX()
        {
            var p = new GenerateLetterRequestBean();
            p.LoanId = 59;
            return this.letterService.ListGenerateLetter(p);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/testgenpdf")]
        public IActionResult GenerateLetterPdfX([FromQuery] GenerateLetterPdfRequestBean bean)
        {
            return this.letterService.GenerateLetter(bean);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/testdailypayment")]
        public GenericResponse<PaymentResponseBean> CheckDailyPayment()
        {
            return this.integrationService.CheckDailyPaymentDummy3();
        }

        [HttpGet]
        [Route("/api/[controller]/checkdailypayment")]
        public GenericResponse<PaymentResponseBean> CheckDailyPayment([FromQuery] DailyPayment bean)
        {
            var p = new GenericResponse<GenerateLetterResponseBean>();
            p.Status = false;

            if (bean.Date == null)
            {
                bean.Date = DateTime.Now.ToString("yyyyMMdd");
            }
            return this.integrationService.CheckDailyPaymentDummy(bean.AccountNo!, bean.Date!, bean.Date!);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/testsms")]
        public GenericResponse<GenerateLetterResponseBean> SMS([FromQuery] ReportMonitorRequestFE bean)
        {
            var p = new GenericResponse<GenerateLetterResponseBean>();
            p.Status = true;
            var res = this.integrationService.SendSms(bean.Start, bean.End);
            return p;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/testcdp")]
        public GenericResponse<PaymentResponseBean> TestCheckDailyPayment([FromQuery] DailyPayment bean)
        {
            return this.integrationService.CheckDailyPayment(bean.AccountNo!, bean.LoanNumber!, bean.Date!, bean.Date!);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/call")]
        public GenericResponse<GenerateLetterResponseBean> Call([FromQuery] TelephonyRequest bean)
        {
            var p = new GenericResponse<GenerateLetterResponseBean>();
            p.Status = true;
            var res = this.integrationService.CallCustomer(bean!.Id!.Value);
            return p;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/call/mobile")]
        public GenericResponse<GenerateLetterResponseBean> CallMobile([FromQuery] TelephonyRequest bean)
        {
            var p = new GenericResponse<GenerateLetterResponseBean>();
            p.Status = true;
            var res = this.integrationService.CallCustomerMobile(bean!.Id!.Value);
            return p;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("/api/[controller]/call/brikerbox/result")]
        public GenericResponse<string> CallBackBrikerBox([FromForm] CallBackBean bean)
        {
            return this.integrationService.CallBackBrikerBox(bean);
        }


        [HttpGet]
        [Route("/api/[controller]/kacab/performance")]
        public ReportPerfKacab KacabPerformance([FromQuery] ReportPerfKacabReq bean)
        {
            return this.integrationService.ReportPerfKacab(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/spv/performance")]
        public ReportPerfSpv SpvPerformance([FromQuery] ReportPerfSpvReq bean)
        {
            return this.integrationService.ReportPerfSpv(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/spv/performance/detail")]
        public ReportPerfSpvSummary SpvPerformanceDetail([FromQuery] ReportPerfSpvReq bean)
        {
            return this.integrationService.ReportPerfSpvSummary(bean);  
        }

        [HttpGet]
        [Route("/api/[controller]/spv/reassign/team")]
        public GenericResponse<UserResponseBean> ReassignTeam()
        {
            return this.colService.ListTeam();
        }

        [HttpGet]
        [Route("/api/[controller]/trace")]
        public GenericResponse<CollTraceResponseBean> Trace([FromQuery] SpvMonListBean bean)
        {
            return this.traceService.ListCallTrace(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/trace/print")]
        public IActionResult TracePrint([FromQuery] ExportTraceBean bean)
        {
            return this.traceService.ListCallTracePrint(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/trace/payrecord")]
        public GenericResponse<PayRecordResponseBean> PayRecord([FromQuery] SpvMonListBean bean)
        {
            return this.traceService.ListPayRecord(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/trace/payrecord/print")]
        public IActionResult PayPrint([FromQuery] ExportTraceBean bean)
        {
            return this.traceService.ListCallTraceRecordPrint(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/trace/team")]
        public GenericResponse<UserResponseBean> TraceListTeam()
        {
            return this.traceService.ListTeamAll();
        }

        [HttpGet]
        [Route("/api/[controller]/address")]
        public GenericResponse<AddressLatLonResponseBean> Address([FromQuery] SpvMonListBean bean)
        {
            return this.traceService.ListAddress(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/address/print")]
        public IActionResult AddressPrint([FromQuery] ExportTraceBean bean)
        {
            return this.traceService.AddressPrint(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/newdaily")]
        public GenericResponse<NewDailyResponse> NewDaily([FromQuery] SpvMonListBean bean)
        {
            return this.traceService.ListNewDaily(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/newdaily/print")]
        public IActionResult NewDailyPrint([FromQuery] ExportTraceBean bean)
        {
            return this.traceService.NewDailyPrint(bean);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/log/generate")]
        public IActionResult GenerateLog([FromQuery] ReportMonitorRequestFE bean)
        {
            return this.letterService.zipLog(bean.Start);
        }

        [HttpGet]
        [Route("/api/[controller]/callback/brikerbox/list")]
        public GenericResponse<CallBackResponseBean> ListCallBackBrikerBox([FromQuery]  CallBackRequest filter)
        {
            return this.integrationService.ListCallBackBrikerBox(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/callback/brikerbox/view")]
        public IActionResult HearRecording([FromQuery] UserReqApproveBean bean)
        {
            return this.integrationService.HearRecording(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/superset/getconfig")]
        public GenericResponse<SupersetConfigResponse> SuperSetConfig()
        {
            return this.integrationService.SuperSetConfig();
        }

        [HttpGet]
        [Route("/api/[controller]/superset/guesttoken")]
        public string SuperSetToken([FromQuery] SupersetPayloadRequest filter)
        {
            return this.integrationService.SuperSetToken(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/distribusi/list")]
        public GenericResponse<SpvCollResponseBean> ListDistribusi([FromQuery] SpvListCollectiontBean bean)
        {
            return this.colService.ListDistribusi(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/distribusi/save")]
        public GenericResponse<string> SaveDistribusi(DistribusiBean bean)
        {
            return this.rejigService.DistribusiManual(bean);
        }
    }
}
