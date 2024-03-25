using AutoMapper;
using Collectium.Model.Bean;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Model.Entity.Staging;
using static Collectium.Model.Bean.ListRequest.RecoveryListRequest;
using static Collectium.Model.Bean.Response.RestructureResponse;
using Action = Collectium.Model.Entity.Action;

namespace Collectium.Model.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, MyProfileBean>().ReverseMap();
            CreateMap<User, UserCreateBean>().ReverseMap();
            CreateMap<User, UserResponseBean>().ReverseMap();
            CreateMap<User, UserTraceResponseBean>().ReverseMap();

            CreateMap<UserRequest, UserReqCreateBean>().ReverseMap();
            CreateMap<UserRequest, UserReqResponseBean>().ReverseMap();
            CreateMap<UserRequest, UserCreateBean>().ReverseMap();


            CreateMap<Role, RoleResponseBean>().ReverseMap();
            CreateMap<BranchType, BranchTypeCreateBean>().ReverseMap();
            CreateMap<Area, AreaCreateBean>().ReverseMap();

            CreateMap<StatusGeneral, StatusGeneralBean>().ReverseMap();
            CreateMap<StatusRequest, StatusRequestBean>().ReverseMap();

            CreateMap<DocumentRestruktur, GenericParameterBean>().ReverseMap();
            CreateMap<PolaRestruktur, GenericParameterBean>().ReverseMap();
            CreateMap<JenisPengurangan, GenericParameterBean>().ReverseMap();
            CreateMap<PembayaranGp, GenericParameterBean>().ReverseMap();
            CreateMap<AlasanLelang, GenericParameterBean>().ReverseMap();
            CreateMap<BalaiLelang, GenericParameterBean>().ReverseMap();
            CreateMap<JenisLelang, GenericParameterBean>().ReverseMap();
            CreateMap<DocumentAuction, GenericParameterBean>().ReverseMap();
            CreateMap<DocumentAuctionResult, GenericParameterBean>().ReverseMap();
            CreateMap<Asuransi, GenericParameterBean>().ReverseMap();
            CreateMap<AsuransiSisaKlaim, GenericParameterBean>().ReverseMap();
            CreateMap<DocumentInsurance, GenericParameterBean>().ReverseMap();
            CreateMap<RecoveryExecution, GenericParameterBean>().ReverseMap();

            CreateMap<Action, ActionResponseBean>().ReverseMap();
            CreateMap<Action, ActionCreateBean>().ReverseMap();
            CreateMap<ActionRequest, ActionReqResponseBean>().ReverseMap();
            CreateMap<ActionRequest, ActionReqEditBean>().ReverseMap();
            CreateMap<ActionRequest, ActionReqCreateBean>().ReverseMap();
            CreateMap<ActionRequest, ActionCreateBean>().ReverseMap();

            CreateMap<ActionGroup, ActionGroupResponseBean>().ReverseMap();
            CreateMap<ActionGroupRequest, ActionGroupReqResponseBean>().ReverseMap();
            CreateMap<ActionGroupRequest, ActionGroupReqCreateBean>().ReverseMap();
            CreateMap<ActionGroupRequest, ActionGroupCreateBean>().ReverseMap();
            CreateMap<ActionGroup, ActionGroupCreateBean>().ReverseMap();


            CreateMap<AccountDistribution, AccDistResponseBean>().ReverseMap();
            CreateMap<AccountDistribution, AccDistCreateBean>().ReverseMap();
            CreateMap<AccountDistributionRequest, AccDistReqResponseBean>().ReverseMap();
            CreateMap<AccountDistributionRequest, ActionReqEditBean>().ReverseMap();
            CreateMap<AccountDistributionRequest, AccDistReqEditBean>().ReverseMap();
            CreateMap<AccountDistributionRequest, AccDistReqCreateBean>().ReverseMap();
            CreateMap<AccountDistributionRequest, AccDistCreateBean>().ReverseMap();


            CreateMap<Branch, BranchResponseBean>().ReverseMap();
            CreateMap<Branch, BranchIntegrationCreateBean>().ReverseMap();
            CreateMap<Branch, BranchCreateBean>().ReverseMap();
            CreateMap<BranchType, BranchTypeResponseBean>().ReverseMap();

            CreateMap<UserBranch, BranchResponseBean>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Branch!.Name!))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Branch!.Id!))
                .ReverseMap();

            CreateMap<Area, AreaResponseBean>().ReverseMap();
            CreateMap<BranchRequest, BranchReqCreateBean>().ReverseMap();
            CreateMap<AccountDistributionRequest, ActionReqEditBean>().ReverseMap();
            CreateMap<AccountDistributionRequest, AccDistReqEditBean>().ReverseMap();
            CreateMap<AccountDistributionRequest, AccDistReqCreateBean>().ReverseMap();
            CreateMap<AccountDistributionRequest, AccDistCreateBean>().ReverseMap();

            CreateMap<MasterLoan, CollResponseBean>().ReverseMap();
            CreateMap<MasterLoan, CollDetailResponseBean>().ReverseMap();
            CreateMap<MasterLoan, SpvCollResponseBean>().ReverseMap();

            CreateMap<Product, ProductLoanResponseBean>().ReverseMap();
            CreateMap<ProductSegment, ProductSegmentResponseBean>().ReverseMap();

            CreateMap<Customer, CustomerLoanResponseBean>().ReverseMap();
            CreateMap<Customer, CustomerDetailResponseBean>().ReverseMap();

            CreateMap<CollectionCall, CollActivityLogDetailResponseBean>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.CallName))
                .ForMember(d => d.ResultDate, o => o.MapFrom(s => s.CallResultDate))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.CallDate))
                .ReverseMap();
            CreateMap<CollectionCall, CollectionCallBean>().ReverseMap();

            CreateMap<MasterCollateral, CollCollateralDetailResponseBean>().ReverseMap();

            CreateMap<CollectionAddContact, AddressResponseBean>().ReverseMap();

            CreateMap<CallResult, CallResultColResponseBean>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Description))
                .ReverseMap();

            CreateMap<CallScript, CallScriptResponseBean>().ReverseMap();
            CreateMap<CallScript, CallScriptCreateBean>().ReverseMap();
            CreateMap<CallScriptRequest, CallScriptReqResponseBean>().ReverseMap();
            CreateMap<CallScriptRequest, CallScriptCreateBean>().ReverseMap();
            CreateMap<CallScriptRequest, CallScriptReqEditBean>().ReverseMap();

            CreateMap<NotifContent, NotifResponseBean>().ReverseMap();
            CreateMap<NotifContent, NotifCreateBean>().ReverseMap();
            CreateMap<NotifContentRequest, NotifReqResponseBean>().ReverseMap();
            CreateMap<NotifContentRequest, NotifReqEditBean>().ReverseMap();
            CreateMap<NotifContentRequest, NotifCreateBean>().ReverseMap();

            CreateMap<CollectionHistory, ApiCollectionHistory>().ReverseMap();
            CreateMap<CollectionHistory, CollActivityLogDetailResponseBean>().ReverseMap();

            CreateMap<Reason, ReasonResponseBean>().ReverseMap();
            CreateMap<Reason, ReasonCreateBean>().ReverseMap();
            CreateMap<ReasonRequest, ReasonReqResponseBean>().ReverseMap();
            CreateMap<ReasonRequest, ReasonCreateBean>().ReverseMap();
            CreateMap<ReasonRequest, ReasonReqEditBean>().ReverseMap();

            CreateMap<SaveContactBean, SaveContactDTOBean>().ReverseMap();
            CreateMap<AddressResponseBean, SaveContactDTOBean>().ReverseMap();
            CreateMap<SaveResultBean, SaveResultBeanToDc>().ReverseMap();

            CreateMap<PaymentHistory, PaymentHistoryBean>().ReverseMap();

            CreateMap<RfGlobal, GlobalResponseBean>().ReverseMap();
            CreateMap<RfGlobal, GlobalCreateBean>().ReverseMap();


            CreateMap<UserBranchRequest, BranchResponseBean>().ReverseMap();


            CreateMap<STGBranchPg, STGBranch>().ReverseMap();
            CreateMap<STGCustomerPg, STGCustomer>().ReverseMap();
            CreateMap<STGDataJaminanPg, STGDataJaminan>().ReverseMap();
            CreateMap<STGDataKreditPg, STGDataKredit>().ReverseMap();
            CreateMap<STGDataLoanBiayaLainPg, STGDataLoanBiayaLain>().ReverseMap();
            CreateMap<STGDataLoanKodeAOPg, STGDataLoanKodeAO>().ReverseMap();
            CreateMap<STGDataLoanKomiteKreditPg, STGDataLoanKomiteKredit>().ReverseMap();
            CreateMap<STGDataLoanKSLPg, STGDataLoanKSL>().ReverseMap();
            CreateMap<STGDataLoanPKPg, STGDataLoanPK>().ReverseMap();
            CreateMap<STGDataLoanTagihanLainPg, STGDataLoanTagihanLain>().ReverseMap();
            CreateMap<STGLoanDetailPg, STGLoanDetail>().ReverseMap();

            CreateMap<CollectionPhoto, CollectionPhotoResponseBean>().ReverseMap();
            CreateMap<CollectionContactPhoto, CollectionPhotoResponseBean>().ReverseMap();

            CreateMap<CollectionTrace, CollTraceResponseBean>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Call!.Loan!.Customer!.Name!))
                .ReverseMap();

            CreateMap<PaymentRecord, PayRecordResponseBean>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Call!.Loan!.Customer!.Name!))
                .ReverseMap();


            CreateMap<Restructure, RestructureDetailResponseBean>().ReverseMap();
            CreateMap<Auction, LelangDetailResponseBean>().ReverseMap();
            CreateMap<Insurance, AsuransiDetailResponseBean>().ReverseMap();
            CreateMap<RestructureCashFlow, RestructureCashFlowBean>().ReverseMap();
            CreateMap<Auction, CreateLelang>().ReverseMap();
            CreateMap<Ayda, CreateAyda>().ReverseMap();
            CreateMap<Ayda, AydaDetailResponseBean>().ReverseMap();

            CreateMap<GenerateLetter, GenerateLetterHistoryResponseBean>().ReverseMap();

            CreateMap<CollectionAddContact, AddressLatLonResponseBean>().ReverseMap();
            CreateMap<MasterLoanHistory, NewDailyResponse>().ReverseMap();

            CreateMap<DistributionRule, DistribusiManualList>().ReverseMap();
            CreateMap<DistributionRule, DistribusiManualGet>().ReverseMap();

            CreateMap<Bucket, BucketListResponseBean>().ReverseMap();
            CreateMap<Bucket, BucketDetailResponseBean>().ReverseMap();

            CreateMap<RuleEngine, RuleEngineListView>().ReverseMap();

            CreateMap<RuleAction, RuleActionBean>().ReverseMap();
            CreateMap<RuleActionOption, RuleActionOptionBean>().ReverseMap();

        }
    }
}
