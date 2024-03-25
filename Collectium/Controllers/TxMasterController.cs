using Collectium.Config;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Model.Entity.Staging;
using Collectium.Model.Helper;
using Collectium.Service;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Collectium.Controllers
{

    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class TxMasterController
    {

        private readonly ActionGroupService action;
        private readonly BranchAreaService baService;
        private readonly TxMasterService txmService;
        private readonly StagingService stg;
        private readonly ILogger<TxMasterController> _logger;

        public TxMasterController(ILogger<TxMasterController> _logger, 
                                        TxMasterService txmService, 
                                        ActionGroupService action,
                                        StagingService stg,
                                        BranchAreaService baService)
        {
            this._logger = _logger;
            this.txmService = txmService;
            this.action = action;
            this.baService = baService;
            this.stg = stg;
        }

        [HttpPost]
        [Route("/api/[controller]/nationality/create")]
        public GenericResponse<string> SaveNationality(Nationality filter)
        {
            return this.txmService.SaveNationality(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/occupation/create")]
        public GenericResponse<string> SaveOccupation(CustomerOccupation filter)
        {
            return this.txmService.SaveOccupation(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/customer-type/create")]
        public GenericResponse<string> SaveCustomerType(CustomerType filter)
        {
            return this.txmService.SaveCustomerType(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/id-type/create")]
        public GenericResponse<string> SaveIdType(IdType filter)
        {
            return this.txmService.SaveIdType(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/callscript/create")]
        public GenericResponse<string> SaveCallScript(CallScript filter)
        {
            return this.txmService.SaveCallScript(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/reason/create")]
        public GenericResponse<string> SaveReason(Reason filter)
        {
            return this.txmService.SaveReason(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/gender/create")]
        public GenericResponse<string> SaveGender(Gender filter)
        {
            return this.txmService.SaveGender(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/income-type/create")]
        public GenericResponse<string> SaveIncomeType(IncomeType filter)
        {
            return this.txmService.SaveIncomeType(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/marital-status/create")]
        public GenericResponse<string> SaveMaritalStatus(MaritalStatus filter)
        {
            return this.txmService.SaveMaritalStatus(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/provinsi/create")]
        public GenericResponse<string> SaveProvinsi(Provinsi filter)
        {
            return this.txmService.SaveProvinsi(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/city/create")]
        public GenericResponse<string> SaveCity(CityNewCodeRequestBean filter)
        {
            return this.txmService.SaveCity(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/kecamatan/create")]
        public GenericResponse<string> SaveKecamatan(KecamatanNewCodeRequestBean filter)
        {
            return this.txmService.SaveKecamatan(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/kelurahan/create")]
        public GenericResponse<string> SaveKelurahan(KelurahanNewCodeRequestBean filter)
        {
            return this.txmService.SaveKelurahan(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/product/create")]
        public GenericResponse<string> SaveProduct(Product filter)
        {
            return this.txmService.SaveProduct(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/area/create")]
        public GenericResponse<string> SaveArea(Area filter)
        {
            return this.txmService.SaveArea(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/branch/create")]
        public GenericResponse<BranchIntegrationCreateBean> SaveBranchIntegration(BranchIntegrationCreateBean filter)
        {
            return this.baService.SaveBranchIntegration(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/user/create")]
        public GenericResponse<string> SaveUser(UserNewRequestBean filter)
        {
            return this.txmService.SaveUser(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/team/create")]
        public GenericResponse<string> SaveTeam(TeamNewCodeRequestBean filter)
        {
            return this.txmService.SaveTeam(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/loan/create")]
        public GenericResponse<string> SaveLoan(ApiLoanNewRequestBean filter)
        {
            return this.txmService.SaveLoan(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/paymenthistory/create")]
        public GenericResponse<string> SavePaymentHistory(PaymentHistoryBean filter)
        {
            return this.txmService.SavePaymentHistory(filter);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/api/[controller]/terbilang")]
        public string Terbilang(PaymentHistoryBean filter)
        {
            int res = Convert.ToInt32(Math.Floor(filter.TotalBayar!.Value));
            var ter = new Terbilang();
            return ter[res];
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/branch")]
        public GenericResponse<STGBranch> ListBranch([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listStgBranch(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/customer")]
        public GenericResponse<STGCustomer> ListCustomer([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listSTGCustomer(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/jaminan")]
        public GenericResponse<STGDataJaminan> ListDataJaminan([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listSTGDataJaminan(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/kredit")]
        public GenericResponse<STGDataKredit> ListDataKredit([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listSTGDataKredit(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/loan")]
        public GenericResponse<STGLoanDetail> ListSTGLoanDetail([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listSTGLoanDetail(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/branch/pg")]
        public GenericResponse<STGBranchPg> ListBranchPg([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listSTGBranchPg(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/customer/pg")]
        public GenericResponse<STGCustomerPg> ListCustomerPg([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listSTGCustomerPg(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/jaminan/pg")]
        public GenericResponse<STGDataJaminanPg> ListDataJaminanPg([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listSTGDataJaminanPg(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/kredit/pg")]
        public GenericResponse<STGDataKreditPg> ListDataKreditPg([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listSTGDataKreditPg(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/loan/pg")]
        public GenericResponse<STGLoanDetailPg> ListSTGLoanDetailPg([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listSTGLoanDetailPg(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/mastercustomer")]
        public GenericResponse<Customer> ListMasterCustomer([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listMasterCustomerV2(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/collectioncall")]
        public GenericResponse<CollectionCall> ListCollectionCall([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listCollectionCallV2(filter);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/api/[controller]/check/masterloan")]
        public GenericResponse<MasterLoan> ListMasterLoan([FromQuery] SpvMonListBean filter)
        {
            return this.stg.listMasterLoanV2(filter);
        }
    }
}
