using Collectium.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Action = Collectium.Model.Entity.Action;

namespace Collectium.Model
{

    public class CollectiumDBContext : DbContext
    {

        public CollectiumDBContext(DbContextOptions<CollectiumDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Status>()
                .HasDiscriminator<string>("Type")
                .HasValue<StatusGeneral>("GEN");

            modelBuilder.Entity<Status>()
                .HasDiscriminator<string>("Type")
                .HasValue<StatusRequest>("REQ");

            modelBuilder.Entity<Status>()
                .HasDiscriminator<string>("Type")
                .HasValue<StatusCall>("CALL");

            modelBuilder.Entity<Status>()
                .HasDiscriminator<string>("Type")
                .HasValue<StatusRestruktur>("REST");

            modelBuilder.Entity<Status>()
                .HasDiscriminator<string>("Type")
                .HasValue<StatusLeLang>("AUCT");

            modelBuilder.Entity<Status>()
                .HasDiscriminator<string>("Type")
                .HasValue<StatusAsuransi>("INSR");

            modelBuilder.Entity<RolePermission>()
            .HasKey(s => new { s.RoleId, s.PermissionId });

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<DocumentRestruktur>("DOCR");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<DocumentAuction>("DOCA");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<DocumentAuctionResult>("DOCS");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<PolaRestruktur>("POLR");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<JenisPengurangan>("JENP");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<PembayaranGp>("BYRG");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<Asuransi>("ASRS");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<AsuransiSisaKlaim>("SIKL");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<DocumentInsurance>("DOCE");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<RecoveryExecution>("EXER");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<AlasanLelang>("REAU");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<BalaiLelang>("BLAU");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<JenisLelang>("TYAU");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<DocumentAyda>("DAYDA");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<HubunganBank>("HUBB");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<RecoveryField>("RECF");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<RuleAction>("RULEACT");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<RuleActionOption>("RULEACTOPT");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<RuleValueType>("RULEVT");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<RuleOperator>("RULEQ");


            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<DataSource>("DSOURCE");

            modelBuilder.Entity<GenericParameter>()
                .HasDiscriminator<string>("Type")
                .HasValue<CustomerTag>("CUSTAG");

            modelBuilder.Entity<STGBranch>(entiry =>
            {
                entiry.HasKey(e => new { e.COMPANY_CODE, e.STG_DATE });
            });


            //modelBuilder.Entity<STGCustomer>(entiry =>
            //{
            //    entiry.HasKey(e => new { e.CU_CIF, e.STG_DATE });
            //});

            modelBuilder.Entity<STGDataJaminan>(entiry =>
            {
                entiry.HasKey(e => new { e.CU_CIF, e.STG_DATE });
            });

            modelBuilder.Entity<STGDataKredit>(entiry =>
            {
                entiry.HasKey(e => new { e.ACC_NO, e.STG_DATE });
            });

            modelBuilder.Entity<STGDataLoanBiayaLain>(entiry =>
            {
                entiry.HasKey(e => new { e.ACC_NO, e.STG_DATE });
            });

            modelBuilder.Entity<STGDataLoanKodeAO>(entiry =>
            {
                entiry.HasKey(e => new { e.ACC_NO, e.STG_DATE });
            });

            modelBuilder.Entity<STGDataLoanKomiteKredit>(entiry =>
            {
                entiry.HasKey(e => new { e.ACC_NO, e.STG_DATE });
            });

            modelBuilder.Entity<STGDataLoanKSL>(entiry =>
            {
                entiry.HasKey(e => new { e.ACC_NO, e.STG_DATE });
            });

            modelBuilder.Entity<STGDataLoanPK>(entiry =>
            {
                entiry.HasKey(e => new { e.ACC_NO, e.STG_DATE });
            });

            modelBuilder.Entity<STGDataLoanTagihanLain>(entiry =>
            {
                entiry.HasKey(e => new { e.ACC_NO, e.STG_DATE });
            });

            modelBuilder.Entity<STGLoanDetail>(entiry =>
            {
                entiry.HasKey(e => new { e.ACC_NO, e.STG_DATE });
            });

            modelBuilder.Entity<STGCustomerPhone>(entiry =>
            {
                entiry.HasKey(e => new { e.CIF, e.STG_DATE, e.PHONE });
            });

            modelBuilder.Entity<STGSmsReminder>(entiry =>
            {
                entiry.HasKey(e => new { e.CU_CIF, e.ACC_NO, e.HP, e.STG_DATE });
            });

        }


        //USER
        public virtual DbSet<Role> Role { get; set; }

        public virtual DbSet<RoleRequest> RoleRequest { get; set; }

        public virtual DbSet<StatusGeneral> StatusGeneral { get; set; }

        public virtual DbSet<StatusRequest> StatusRequest { get; set; }

        public virtual DbSet<StatusRestruktur> StatusRestruktur { get; set; }

        public virtual DbSet<StatusLeLang> StatusLeLang { get; set; }

        public virtual DbSet<StatusAsuransi> StatusAsuransi { get; set; }


        public virtual DbSet<User> User { get; set; }

        public virtual DbSet<UserBranch> UserBranch { get; set; }

        public virtual DbSet<UserBranchRequest> UserBranchRequest { get; set; }

        public virtual DbSet<UserRequest> UserRequest { get; set; }

        public virtual DbSet<Token> Token { get; set; }

        public virtual DbSet<Permission> Permission { get; set; }

        public virtual DbSet<RolePermission> RolePermission { get; set; }

        //public virtual DbSet<RolePermissionRequest> RolePermissionRequest { get; set; }


        //MASTER
        public virtual DbSet<AccountDistribution> AccountDistribution { get; set; }

        public virtual DbSet<AccountDistributionRequest> AccountDistributionRequest { get; set; }

        public virtual DbSet<Action> Action { get; set; }

        public virtual DbSet<ActionRequest> ActionRequest { get; set; }

        public virtual DbSet<ActionGroup> ActionGroup { get; set; }

        public virtual DbSet<ActionGroupRequest> ActionGroupRequest { get; set; }

        public virtual DbSet<Area> Area { get; set; }

        public virtual DbSet<AreaRequest> AreaRequest { get; set; }

        public virtual DbSet<Branch> Branch { get; set; }

        public virtual DbSet<BranchType> BranchType { get; set; }

        public virtual DbSet<BranchRequest> BranchRequest { get; set; }

        public virtual DbSet<CallScript> CallScript { get; set; }

        public virtual DbSet<CallScriptRequest> CallScriptRequest { get; set; }

        public virtual DbSet<FcMappingMikroColl> FcMappingMikroColl { get; set; }

        public virtual DbSet<FcMappingMikroCollRequest> FcMappingMikroCollRequest { get; set; }

        public virtual DbSet<Team> Team { get; set; }

        public virtual DbSet<TeamMember> TeamMember { get; set; }


        //MASTER TX
        public virtual DbSet<City> City { get; set; }

        public virtual DbSet<CustomerOccupation> CustomerOccupation { get; set; }

        public virtual DbSet<CustomerType> CustomerType { get; set; }

        public virtual DbSet<Gender> Gender { get; set; }

        public virtual DbSet<IdType> IdType { get; set; }

        public virtual DbSet<IncomeType> IncomeType { get; set; }

        public virtual DbSet<Kecamatan> Kecamatan { get; set; }

        public virtual DbSet<Kelurahan> Kelurahan { get; set; }

        public virtual DbSet<MaritalStatus> MaritalStatus { get; set; }

        public virtual DbSet<Nationality> Nationality { get; set; }

        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<ProductSegment> ProductSegment { get; set; }

        public virtual DbSet<Provinsi> Provinsi { get; set; }

        public virtual DbSet<Customer> Customer { get; set; }

        public virtual DbSet<MasterLoan> MasterLoan { get; set; }

        public virtual DbSet<MasterCollateral> MasterCollateral { get; set; }

        public virtual DbSet<CollectionAddContact> CollectionAddContact { get; set; }

        public virtual DbSet<CollectionCall> CollectionCall { get; set; }

        public virtual DbSet<CallResult> CallResult { get; set; }

        public virtual DbSet<Reason> Reason { get; set; }

        public virtual DbSet<CollectionVisit> CollectionVisit { get; set; }

        public virtual DbSet<CollectionHistory> CollectionHistory { get; set; }

        public virtual DbSet<CollectionTrace> CollectionTrace { get; set; }

        public virtual DbSet<NotifContent> NotifContent { get; set; }

        public virtual DbSet<NotifContentRequest> NotifContentRequest { get; set; }

        public virtual DbSet<PaymentHistory> PaymentHistory { get; set; }

        public virtual DbSet<RfGlobal> RfGlobal { get; set; }

        public virtual DbSet<RfCounter> RfCounter { get; set; }

        public virtual DbSet<GenerateLetter> GenerateLetter { get; set; }

        public virtual DbSet<Counter> Counter { get; set; }

        public virtual DbSet<DocumentSignature> DocumentSignature { get; set; }

        public virtual DbSet<STGBranch> STGBranch { get; set; }
        public virtual DbSet<STGCustomer> STGCustomer { get; set; }
        public virtual DbSet<STGDataJaminan> STGDataJaminan { get; set; }
        public virtual DbSet<STGDataKredit> STGDataKredit { get; set; }
        public virtual DbSet<STGDataLoanBiayaLain> STGDataLoanBiayaLain { get; set; }
        public virtual DbSet<STGDataLoanKodeAO> STGDataLoanKodeAO { get; set; }
        public virtual DbSet<STGDataLoanKomiteKredit> STGDataLoanKomiteKredit { get; set; }
        public virtual DbSet<STGDataLoanKSL> STGDataLoanKSL { get; set; }
        public virtual DbSet<STGDataLoanPK> STGDataLoanPK { get; set; }
        public virtual DbSet<STGDataLoanTagihanLain> STGDataLoanTagihanLain { get; set; }
        public virtual DbSet<STGLoanDetail> STGLoanDetail { get; set; }

        public virtual DbSet<STGCustomerPhone> STGCustomerPhone { get; set; }

        public virtual DbSet<LoanBiayaLain> LoanBiayaLain { get; set; }
        public virtual DbSet<LoanKodeAO> LoanKodeAO { get; set; }
        public virtual DbSet<LoanKomiteKredit> LoanKomiteKredit { get; set; }
        public virtual DbSet<LoanKSL> LoanKSL { get; set; }
        public virtual DbSet<LoanPK> LoanPK { get; set; }
        public virtual DbSet<LoanTagihanLain> LoanTagihanLain { get; set; }
        public virtual DbSet<MasterLoanHistory> MasterLoanHistory { get; set; }

        public virtual DbSet<TrackingFc> TrackingFc { get; set; }

        public virtual DbSet<CollectionPhoto> CollectionPhoto { get; set; }

        public virtual DbSet<CollectionContactPhoto> CollectionContactPhoto { get; set; }

        public virtual DbSet<STGSmsReminder> STGSmsReminder { get; set; }

        public virtual DbSet<ReasonRequest> ReasonRequest { get; set; }

        public virtual DbSet<PaymentRecord> PaymentRecord { get; set; }

        public virtual DbSet<CallRequest> CallRequest { get; set; }


        //REcovery
        public virtual DbSet<Restructure> Restructure { get; set; }

        public virtual DbSet<RestructureDocument> RestructureDocument { get; set; }

        public virtual DbSet<RestructureCashFlow> RestructureCashFlow { get; set; }

        public virtual DbSet<Auction> Auction { get; set; }

        public virtual DbSet<AuctionDocument> AuctionDocument { get; set; }

        public virtual DbSet<AuctionResultDocument> AuctionResultDocument { get; set; }

        public virtual DbSet<DocumentRestruktur> DocumentRestruktur { get; set; }

        public virtual DbSet<DocumentAuction> DocumentAuction { get; set; }

        public virtual DbSet<DocumentAuctionResult> DocumentAuctionResult { get; set; }

        public virtual DbSet<DocumentAyda> DocumentAyda { get; set; }


        public virtual DbSet<PolaRestruktur> PolaRestruktur { get; set; }

        public virtual DbSet<PembayaranGp> PembayaranGp { get; set; }

        public virtual DbSet<JenisPengurangan> JenisPengurangan { get; set; }

        public virtual DbSet<Insurance> Insurance { get; set; }

        public virtual DbSet<InsuranceDocument> InsuranceDocument { get; set; }

        public virtual DbSet<DocumentInsurance> DocumentInsurance { get; set; }

        public virtual DbSet<Asuransi> Asuransi { get; set; }

        public virtual DbSet<AsuransiSisaKlaim> AsuransiSisaKlaim { get; set; }

        public virtual DbSet<RecoveryExecution> RecoveryExecution { get; set; }

        public virtual DbSet<RestructureApproval> RestructureApproval { get; set; }

        public virtual DbSet<AuctionApproval> AuctionApproval { get; set; }

        public virtual DbSet<InsuranceApproval> InsuranceApproval { get; set; }

        public virtual DbSet<AlasanLelang> AlasanLelang { get; set; }

        public virtual DbSet<Ayda> Ayda { get; set; }

        public virtual DbSet<AydaApproval> AydaApproval { get; set; }

        public virtual DbSet<AydaDocument> AydaDocument { get; set; }

        public virtual DbSet<HubunganBank> HubunganBank { get; set; }

        public virtual DbSet<AgentDistribution> AgentDistribution { get; set; }

        public virtual DbSet<AgentLoan> AgentLoan { get; set; }

        public virtual DbSet<DistributionRule> DistributionRule { get; set; }

        public virtual DbSet<RecoveryField> RecoveryField { get; set; }

        public virtual DbSet<RuleAction> RuleAction { get; set; }

        public virtual DbSet<RuleActionOption> RuleActionOption { get; set; }

        public virtual DbSet<Bucket> Bucket { get; set; }

        public virtual DbSet<BucketUser> BucketUser { get; set; }

        public virtual DbSet<RuleDataField> RuleDataField { get; set; }

        public virtual DbSet<RuleEngineCond> RuleEngineCond { get; set; }

        public virtual DbSet<RuleBucket> RuleBucket { get; set; }

        public virtual DbSet<RuleEngine> RuleEngine { get; set; }

        public virtual DbSet<JobRule> JobRule { get; set; }

        public virtual DbSet<BusinessException> BusinessException { get; set; }


    }

}
