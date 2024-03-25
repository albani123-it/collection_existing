using AutoMapper;
using Collectium.Model.Bean.Response;
using Collectium.Model.Bean;
using Collectium.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Collectium.Model.Bean.Response.RestructureResponse;
using static Collectium.Model.Bean.ListRequest.RecoveryListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Entity;
using Collectium.Model.Bean.ListRequest;

namespace Collectium.Controllers
{

    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]

    public class RecoveryController : ControllerBase
    {
        private readonly RestructureService restructureService;
        private readonly AuctionService auctionService;
        private readonly InsuranceService insuranceService;
        private readonly AydaService aydaService;
        private readonly IMapper mapper;
        private readonly ILogger<RecoveryController> _logger;

        public RecoveryController(ILogger<RecoveryController> _logger,
            RestructureService restructureService,
            AuctionService auctionService,
            AydaService aydaService,
            InsuranceService insuranceService,
            IMapper mapper)
        {
            this._logger = _logger;
            this.mapper = mapper;
            this.restructureService = restructureService;
            this.auctionService = auctionService;
            this.insuranceService = insuranceService;
            this.aydaService = aydaService;
        }

        [HttpGet]
        [Route("/api/[controller]/nasabah/list")]
        public GenericResponse<CollResponseBean> ListDeskCall([FromQuery] CollListRequestBean bean)
        {
            return this.restructureService.ListCollection(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/restruktur/monitoring/list")]
        public GenericResponse<RestructureListResponseBean> RestructureListMonitoring([FromQuery]  RestructureListBean filter)
        {
            return this.restructureService.ListMonitoring(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/restruktur/approval/list")]
        public GenericResponse<RestructureListResponseBean> ApprovalListMonitoring([FromQuery] RestructureListBean filter)
        {
            return this.restructureService.ListApprove(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/restruktur/dokumen/upload")]
        public GenericResponse<int> UploadDocument([FromForm] UploadDocumentRestructure bean)
        {
            return this.restructureService.UploadDocument(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/restruktur/dokumen/view")]
        public IActionResult ViewDocument([FromQuery] UserReqApproveBean bean)
        {
            return this.restructureService.ViewDocument(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/restruktur/dokumen/delete")]
        public GenericResponse<UserReqApproveBean> DeleteDocument([FromQuery] UserReqApproveBean bean)
        {
            return this.restructureService.DeleteDocument(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/restruktur/create")]
        public GenericResponse<CreateRestructure> CreateRestructure(CreateRestructure bean)
        {
            return this.restructureService.Create(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/restruktur/submit")]
        public GenericResponse<SubmitRestructure> SubmitRestructure(SubmitRestructure bean)
        {
            return this.restructureService.Submit(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/restruktur/sendback")]
        public GenericResponse<ApprovalLevelRestructure> SubmitRestructure(ApprovalLevelRestructure bean)
        {
            return this.restructureService.SendBack(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/restruktur/verify")]
        public GenericResponse<ApprovalLevelRestructure> VerifyRestructure(ApprovalLevelRestructure bean)
        {
            return this.restructureService.Verify(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/restruktur/approve")]
        public GenericResponse<ApprovalLevelRestructure> ApproveRestructure(ApprovalLevelRestructure bean)
        {
            return this.restructureService.Approve(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/restruktur/reject")]
        public GenericResponse<ApprovalLevelRestructure> RejectRestructure(ApprovalLevelRestructure bean)
        {
            return this.restructureService.Reject(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/restruktur/detail")]
        public GenericResponse<RestructureDetailResponseBean> Detail([FromQuery] UserReqApproveBean bean)
        {
            return this.restructureService.Detail(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/lelang/monitoring/list")]
        public GenericResponse<RestructureListResponseBean> AuctionListMonitoring([FromQuery] RestructureListBean filter)
        {
            return this.auctionService.ListMonitoring(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/lelang/approval/list")]
        public GenericResponse<RestructureListResponseBean> LelangListApproval([FromQuery] RestructureListBean filter)
        {
            return this.auctionService.ListApprove(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/lelang/dokumen/upload")]
        public GenericResponse<int> UploadAuctionDocument([FromForm] UploadDocumentRestructure bean)
        {
            return this.auctionService.UploadDocument(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/lelang/dokumen/view")]
        public IActionResult ViewAuctionDocument([FromQuery] UserReqApproveBean bean)
        {
            return this.auctionService.ViewDocument(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/lelang/dokumen/delete")]
        public GenericResponse<UserReqApproveBean> DeleteAuctionDocument([FromQuery] UserReqApproveBean bean)
        {
            return this.auctionService.DeleteDocument(bean);
        }


        [HttpPost]
        [Route("/api/[controller]/lelang/hasil/dokumen/upload")]
        public GenericResponse<int> UploadAuctionResultDocument([FromForm] UploadDocumentRestructure bean)
        {
            return this.auctionService.UploadDocumentResult(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/lelang/hasil/dokumen/view")]
        public IActionResult ViewAuctionResultDocument([FromQuery] UserReqApproveBean bean)
        {
            return this.auctionService.ViewDocumentResult(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/lelang/hasil/dokumen/delete")]
        public GenericResponse<UserReqApproveBean> DeleteAuctionResultDocument([FromQuery] UserReqApproveBean bean)
        {
            return this.auctionService.DeleteDocumentResult(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/lelang/create")]
        public GenericResponse<CreateLelang> CreateLelang(CreateLelang bean)
        {
            return this.auctionService.Create(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/lelang/submit")]
        public GenericResponse<SubmitLelang> SubmitLelang(SubmitLelang bean)
        {
            return this.auctionService.Submit(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/lelang/sendback")]
        public GenericResponse<ApprovalLevelRestructure> SubmitLelang(ApprovalLevelRestructure bean)
        {
            return this.auctionService.SendBack(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/lelang/verify")]
        public GenericResponse<ApprovalLevelRestructure> VerifyLelang(ApprovalLevelRestructure bean)
        {
            return this.auctionService.Verify(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/lelang/approve")]
        public GenericResponse<ApprovalLevelRestructure> ApproveLelang(ApprovalLevelRestructure bean)
        {
            return this.auctionService.Approve(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/lelang/reject")]
        public GenericResponse<ApprovalLevelRestructure> RejectLelang(ApprovalLevelRestructure bean)
        {
            return this.auctionService.Reject(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/lelang/detail")]
        public GenericResponse<LelangDetailResponseBean> DetailLelang([FromQuery] UserReqApproveBean bean)
        {
            return this.auctionService.Detail(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/asuransi/monitoring/list")]
        public GenericResponse<RestructureListResponseBean> InsuranceListMonitoring([FromQuery] RestructureListBean filter)
        {
            return this.insuranceService.ListMonitoring(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/asuransi/approval/list")]
        public GenericResponse<RestructureListResponseBean> AsuransiListApproval([FromQuery] RestructureListBean filter)
        {
            return this.insuranceService.ListApprove(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/asuransi/dokumen/upload")]
        public GenericResponse<int> UploadInsuranceDocument([FromForm] UploadDocumentRestructure bean)
        {
            return this.insuranceService.UploadDocument(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/asuransi/dokumen/view")]
        public IActionResult ViewInsuranceDocument([FromQuery] UserReqApproveBean bean)
        {
            return this.insuranceService.ViewDocument(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/asuransi/dokumen/delete")]
        public GenericResponse<UserReqApproveBean> DeleteInsuranceDocument([FromQuery] UserReqApproveBean bean)
        {
            return this.insuranceService.DeleteDocument(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/asuransi/create")]
        public GenericResponse<CreateAsuransi> CreateAsuransi(CreateAsuransi bean)
        {
            return this.insuranceService.Create(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/asuransi/submit")]
        public GenericResponse<SubmitAsuransi> SubmitAsuransi(SubmitAsuransi bean)
        {
            return this.insuranceService.Submit(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/asuransi/sendback")]
        public GenericResponse<ApprovalLevelRestructure> SubmitAsuransi(ApprovalLevelRestructure bean)
        {
            return this.insuranceService.SendBack(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/asuransi/verify")]
        public GenericResponse<ApprovalLevelRestructure> VerifyAsuransi(ApprovalLevelRestructure bean)
        {
            return this.insuranceService.Verify(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/asuransi/approve")]
        public GenericResponse<ApprovalLevelRestructure> ApproveAsuransi(ApprovalLevelRestructure bean)
        {
            return this.insuranceService.Approve(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/asuransi/reject")]
        public GenericResponse<ApprovalLevelRestructure> RejectAsuransi(ApprovalLevelRestructure bean)
        {
            return this.insuranceService.Reject(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/asuransi/detail")]
        public GenericResponse<AsuransiDetailResponseBean> DetailASuransi([FromQuery] UserReqApproveBean bean)
        {
            return this.insuranceService.Detail(bean);
        }


        [HttpGet]
        [Route("/api/[controller]/ayda/monitoring/list")]
        public GenericResponse<RestructureListResponseBean> AydaListMonitoring([FromQuery] RestructureListBean filter)
        {
            return this.aydaService.ListMonitoring(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/ayda/approval/list")]
        public GenericResponse<RestructureListResponseBean> AydaListApproval([FromQuery] RestructureListBean filter)
        {
            return this.aydaService.ListApprove(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/ayda/dokumen/upload")]
        public GenericResponse<int> UploadAydaDocument([FromForm] UploadDocumentRestructure bean)
        {
            return this.aydaService.UploadDocument(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/ayda/dokumen/view")]
        public IActionResult ViewAydaDocument([FromQuery] UserReqApproveBean bean)
        {
            return this.aydaService.ViewDocument(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/ayda/dokumen/delete")]
        public GenericResponse<UserReqApproveBean> DeleteAydaDocument([FromQuery] UserReqApproveBean bean)
        {
            return this.aydaService.DeleteDocument(bean);
        }


        [HttpPost]
        [Route("/api/[controller]/ayda/create")]
        public GenericResponse<CreateAyda> CreateAyda(CreateAyda bean)
        {
            return this.aydaService.Create(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/ayda/submit")]
        public GenericResponse<SubmitAyda> AydaLelang(SubmitAyda bean)
        {
            return this.aydaService.Submit(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/ayda/sendback")]
        public GenericResponse<ApprovalLevelRestructure> SubmitAyda(ApprovalLevelRestructure bean)
        {
            return this.aydaService.SendBack(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/ayda/verify")]
        public GenericResponse<ApprovalLevelRestructure> VerifyAyda(ApprovalLevelRestructure bean)
        {
            return this.aydaService.Verify(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/ayda/approve")]
        public GenericResponse<ApprovalLevelRestructure> ApproveAyda(ApprovalLevelRestructure bean)
        {
            return this.aydaService.Approve(bean);
        }

        [HttpPost]
        [Route("/api/[controller]/ayda/reject")]
        public GenericResponse<ApprovalLevelRestructure> RejectAyda(ApprovalLevelRestructure bean)
        {
            return this.aydaService.Reject(bean);
        }

        [HttpGet]
        [Route("/api/[controller]/ayda/detail")]
        public GenericResponse<AydaDetailResponseBean> DetailAyda([FromQuery] UserReqApproveBean bean)
        {
            return this.aydaService.Detail(bean);
        }
    }
}
