using Collectium.Model.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Collectium.Model.Bean.Request;

namespace Collectium.Model.Bean.Response
{

    public class ProductLoanResponseBean
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? Desc { get; set; }

    }

    public class CustomerLoanResponseBean
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
    }

    public class CustomerDetailResponseBean
    {
        public int? Id { get; set; }
        public string? Cif { get; set; }

        public string? Name { get; set; }

        public DateTime? BornDate { get; set; }

        public string? BornPlace { get; set; }

        public IdType? IdType { get; set; }

        public string? Idnumber { get; set; }

        public Gender? Gender { get; set; }

        public MaritalStatus? MaritalStatus { get; set; }

        public Nationality? Nationality { get; set; }

        public IncomeType? IncomeType { get; set; }

        public string? CuIncome { get; set; }

        public CustomerType? CustomerType { get; set; }

        public CustomerOccupation? CustomerOccupation { get; set; }

        public string? Company { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Rt { get; set; }

        public string? Rw { get; set; }

        public Kelurahan? Kelurahan { get; set; }

        public Kecamatan? Kecamatan { get; set; }

        public City? City { get; set; }

        public Provinsi? Provinsi { get; set; }

        public string? ZipCode { get; set; }

        public string? HmPhone { get; set; }

        public string? MobilePhone { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }

        public BranchResponseBean? Branch { get; set; }
    }

    public class CollResponseBean
    {
        public int? Id { get; set; }

        public CustomerLoanResponseBean? Customer { get; set; }

        public string? Cif { get; set; }

        public string? AccNo { get; set; }

        public string? Ccy { get; set; }

        public ProductLoanResponseBean? Product { get; set; }

        public double? Plafond { get; set; }

        public DateTime? MaturityDate { get; set; }

        public DateTime? StartDate { get; set; }

        public int? SisaTenor { get; set; }

        public int? Tenor { get; set; }

        public double? InstallmentPokok { get; set; }

        public double? InterestRate { get; set; }

        public double? Installment { get; set; }

        public double? TunggakanPokok { get; set; }

        public double? TunggakanBunga { get; set; }

        public double? TunggakanDenda { get; set; }

        public double? TunggakanTotal { get; set; }

        public double? KewajibanTotal { get; set; }

        public DateTime? LastPayDate { get; set; }

        public double? Outstanding { get; set; }

        public double? PayTotal { get; set; }

        public int? Dpd { get; set; }

        public int? Kolektibilitas { get; set; }

        public string? EconName { get; set; }

        public string? EconPhone { get; set; }

        public string? EconRelation { get; set; }

        public string? MarketingCode { get; set; }

        public string? ChannelBranchCode { get; set; }

        public string? Branch { get; set; }

        public string? Area { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }

        public DateTime? FileDate { get; set; }

        public string? LoanNumber { get; set; }
    }

    public class SpvCollResponseBean
    {
        public int? Id { get; set; }
        public CustomerLoanResponseBean? Customer { get; set; }

        public string? Cif { get; set; }

        public string? AccNo { get; set; }

        public string? Ccy { get; set; }

        public ProductLoanResponseBean? Product { get; set; }

        public double? Plafond { get; set; }

        public DateTime? MaturityDate { get; set; }

        public DateTime? StartDate { get; set; }

        public int? SisaTenor { get; set; }

        public int? Tenor { get; set; }

        public double? InstallmentPokok { get; set; }

        public double? InterestRate { get; set; }

        public double? Installment { get; set; }

        public double? TunggakanPokok { get; set; }

        public double? TunggakanBunga { get; set; }

        public double? TunggakanDenda { get; set; }

        public double? TunggakanTotal { get; set; }

        public double? KewajibanTotal { get; set; }

        public DateTime? LastPayDate { get; set; }

        public double? Outstanding { get; set; }

        public double? PayTotal { get; set; }

        public int? Dpd { get; set; }

        public int? Kolektibilitas { get; set; }

        public string? EconName { get; set; }

        public string? EconPhone { get; set; }

        public string? EconRelation { get; set; }

        public string? MarketingCode { get; set; }

        public string? ChannelBranchCode { get; set; }

        public string? BranchName { get; set; }

        public UserResponseBean? Assigned { get; set; }

        public DateTime? LastFollowUp { get; set; }

        public DateTime? FileDate { get; set; }
    }

    public class CollDetailResponseBean
    {
        public int? Id { get; set; }
        public CustomerDetailResponseBean? Customer { get; set; }

        public string? Cif { get; set; }

        public string? AccNo { get; set; }

        public string? Ccy { get; set; }

        public ProductLoanResponseBean? Product { get; set; }

        public ProductSegmentResponseBean? ProductSegment { get; set; }

        public double? Plafond { get; set; }

        public DateTime? MaturityDate { get; set; }

        public DateTime? StartDate { get; set; }

        public int? SisaTenor { get; set; }

        public int? Tenor { get; set; }

        public double? InstallmentPokok { get; set; }

        public double? InterestRate { get; set; }

        public double? Installment { get; set; }

        public double? TunggakanPokok { get; set; }

        public double? TunggakanBunga { get; set; }

        public double? TunggakanDenda { get; set; }

        public double? TunggakanTotal { get; set; }

        public double? KewajibanTotal { get; set; }

        public DateTime? LastPayDate { get; set; }

        public double? Outstanding { get; set; }

        public double? PayTotal { get; set; }

        public int? Dpd { get; set; }

        public int? Kolektibilitas { get; set; }

        public string? EconName { get; set; }

        public string? EconPhone { get; set; }

        public string? EconRelation { get; set; }

        public string? MarketingCode { get; set; }

        public string? ChannelBranchCode { get; set; }

        public bool? Payment { get; set; }

        public IList<CollCollateralDetailResponseBean>? Collateral { get; set; }

        public IList<CollActivityLogDetailResponseBean>? ActivityLog { get; set; }

        public string? CallScript { get; set; }

        public IList<AddressResponseBean>? Address { get; set; }

        public IList<PaymentHistoryBean>? PaymentHistory { get; set; }

        public DateTime? FileDate { get; set; }

        public IList<CallBackResponseBean>? CallRequest { get; set; }

        public string? LoanNumber { get; set; }

    }

    public class CollCollateralDetailResponseBean
    {

        public string? AccNo { get; set; }

        public string? ColId { get; set; }

        public string? ColType { get; set; }

        public string? VehBpkbNo { get; set; }

        public string? VehPlateNo { get; set; }

        public string? VehMerek { get; set; }

        public string? VehModel { get; set; }

        public string? VehBpkbName { get; set; }

        public string? VehEngineNo { get; set; }

        public string? VehChasisNo { get; set; }

        public string? VehStnkNo { get; set; }

        public string? VehYear { get; set; }

        public string? VehBuildYear { get; set; }

        public string? VehCc { get; set; }

        public string? VehColor { get; set; }
    }

    public class CollActivityLogDetailResponseBean
    {
        public string? AccNo { get; set; }

        public string? Name { get; set; }

        public ReasonResponseBean? Reason { get; set; }

        public CallResultColResponseBean? Result { get; set; }

        public DateTime? ResultDate { get; set; }

        public double? Amount { get; set; }

        public string? Notes { get; set; }

        public DateTime? Date { get; set; }

        public UserResponseBean? HistoryBy { get; set; }

        public string? Kolek { get; set; }

        public string? CallResultHh { get; set; }

        public string? CallResultMm { get; set; }

        public IList<CollectionPhotoResponseBean>? Photo { get; set; }

    }

    public class CollectionCallBean
    {
        public int? Id { get; set; }
        public int? LoanId { get; set; }

        public int? BranchId { get; set; }

        public string? AccNo { get; set; }

        public string? CallName { get; set; }

        public int? CollectionAddId { get; set; }

        public int? ReasonId { get; set; }

        public int? CallResultId { get; set; }

        public DateTime? CallResultDate { get; set; }

        public double? CallAmount { get; set; }

       
        public string? CallNotes { get; set; }

        public DateTime? CallDate { get; set; }

        public int? CallById { get; set; }

        public string? CallResultHh { get; set; }
        public string? CallResultMm { get; set; }
        public string? CallResultHhmm { get; set; }
    }

    public class CollectionPhotoResponseBean
    {
        public int? Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Url { get; set; }

        public string? Mime { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }

    }

    public class CallResultColResponseBean
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? isDC { get; set; }

        public int? isFC { get; set; }

    }

    public class ProductSegmentResponseBean
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? Desc { get; set; }

    }

    public class AddressResponseBean
    {
        public int? Id { get; set; }

        public string? AddPhone { get; set; }

        public string? AddFrom { get; set; }

        public string? AddAddress { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }

        public int? Default { get; set; }

        public IList<CollectionPhotoResponseBean>? Photo { get; set; }

    }

    public class ReasonResponseBean
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public StatusGeneral? Status { get; set; }

        public int? isDC { get; set; }

        public int? isFC { get; set; }

    }

    public class CollHistoryResponseBean
    {
        public string? AccNo { get; set; }

        public string? Name { get; set; }

        public string? Reason { get; set; }

        public string? Result { get; set; }

        public DateTime? ResultDate { get; set; }

        public double? Amount { get; set; }

        public string? Notes { get; set; }

        public DateTime? Date { get; set; }

        public string? CallBy { get; set; }

        public string? Kolek { get; set; }

    }

    public class SpvMonResponseBean
    {

        public string? Cif { get; set; }

        public string? AccNo { get; set; }

        public string? Name { get; set; }
        public string? BranchName { get; set; }

        public string? Product { get; set; }

        public double? TunggakanTotal { get; set; }

        public double? KewajibanTotal { get; set; }

        public DateTime? LastPayDate { get; set; }

        public int? Dpd { get; set; }

        public int? Kolektibilitas { get; set; }

        public string? Reason { get; set; }

        public string? Result { get; set; }

        public string? Janji { get; set; }

        public string? Assigned { get; set; }

        public DateTime? CallDate { get; set; }
    }

    public class StatementResponse
    {
        public string? ResponseCode { get; set; }
        public string? ResponseDescription { get; set; }
        public string? TransactionId { get; set; }
        public string? MerchantId { get; set; }
        public string? AccountNo { get; set; }
        public string? AccountName { get; set; }
        public string? Currency { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? OpeningBalance { get; set; }
        public string? ClosingBalance { get; set; }
        public List<Statement>? Statements { get; set; }
    }

    public class Statement
    {
        public string? Date { get; set; }
        public string? Reference { get; set; }
        public string? Narrative { get; set; }
        public string? Amount { get; set; }
        public string? Type { get; set; }
    }

    public class ReportCounter
    {
        public double? TunggakanTotal { get; set; }
        public double? KewajibanTotal { get; set; }
    }

    public class TrackingHistoryResponseBean
    {

        public double? Lat { get; set; }

        public double? Lon { get; set; }

        public string? Time { get; set; }
    }
}
