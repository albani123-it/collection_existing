using System.Text.Json.Serialization;

namespace Collectium.Model.Bean.Request
{
    public class IntegrationRequestBean
    {
        public class PaymentBean
        {
            public string? Index { get; set; }

            public string? StartDate { get; set; }

            public string? LineCount { get; set; }

            public string? Account { get; set; }

            public string? EndDate { get; set; }
        }

        public class PaymentResponseBean
        {
            public string? Status { get; set; }

            public IList<StatementsBeanV2>? Account { get; set; }

            public IList<StatementsBeanV2>? Payment { get; set; }

        }

        public class StatementsBeanV2
        {
            public string? Date { get; set; }

            public string? Date2 { get; set; }

            public string? Status { get; set; }

            public string? Amount { get; set; }

            public string? Outstanding { get; set; }

            public string? AmountDue { get; set; }

            public string? EndBalance { get; set; }

            public DateTime? Sorter { get; set; }
        }

        public class StatementsBean
        {
            public string? Date { get; set; }

            public string? ReferenceNumber { get; set; }

            public string? Amount { get; set; }

            public string? Sign { get; set; }

            public string? Time { get; set; }

            public string? Description { get; set; }

            public string? EndBalance { get; set; }

            public string? PageNo { get; set; }

            public string? PageCount { get; set; }
        }

        public class DailyPayment
        {
            public string? AccountNo { get; set; }

            public string? LoanNumber { get; set; }

            public string? Date { get; set; }

        }

        public class TransactionObject
        {
            public string? ClientID { get; set; }
            public string? BranchID { get; set; }
            public DateTime? RequestTimestamp { get; set; }
            public DateTime? ResponseTimestamp { get; set; }
            public string? TransactionID { get; set; }
            public string? T24TransactionID { get; set; }
        }

        public class ServiceList
        {
            public string? BalanceType { get; set; }
            public string? BalanceAmount { get; set; }
        }

        public class ServiceOrderItem
        {
            public string? OverdueStatus { get; set; }
            public string? AccuralStatus { get; set; }
            public List<ServiceList>? ServiceList { get; set; }
        }

        public class PaymentOff
        {
            public string? StatusCode { get; set; }
            public string? StatusDescription { get; set; }
            public string? ErrorCode { get; set; }
            public string? ErrorDescription { get; set; }
            public TransactionObject? TransactionObject { get; set; }
            public ServiceOrderItem? ServiceOrderItem { get; set; }

        }

        public class Account
        {
            public string? referenceNumber { get; set; }
            public string? sign { get; set; }
            public string? time { get; set; }
            public string? amount { get; set; }
            public string? date { get; set; }
            public string? description { get; set; }
            public string? endBalance { get; set; }
            public string? pageNo { get; set; }
            public string? pageCount { get; set; }
        }

        public class Payment
        {
            public string? PaymentDate { get; set; }
            public string? Type { get; set; }
            public string? BillsAmount { get; set; }
            public string? OutsantdingAmount { get; set; }
            public string? Status { get; set; }
            public string? StatusDate { get; set; }
        }

        public class IntegRoot
        {
            public string? status { get; set; }
            public List<Account>? account { get; set; }
            public List<Payment>? payment { get; set; }

            public PaymentOff? payment1 { get; set; }
        }


        public class PaymentOffRequest
        {
            public string? Loan_number { get; set; }

            public string? Accountid { get; set; }

            public string? Payinaccount { get; set; }

            public string? Tanggal { get; set; }

        }


        public class ReportPerfKacabReq
        {
            [JsonPropertyName("formdate")]
            public string? Formdate { get; set; }

            [JsonPropertyName("todate")]
            public string? Todate { get; set; }

            [JsonPropertyName("branch")]
            public string? Branch { get; set; }
        }

        public class ReportPerfKacab
        {
            [JsonPropertyName("status")]
            public string? Status { get; set; }

            [JsonPropertyName("data")]
            public List<ReportPerfKacabData>? Data { get; set; }
        }

        public class ReportPerfKacabData
        {
            [JsonPropertyName("tanggal")]
            public string? Tanggal { get; set; }

            [JsonPropertyName("spv_name")]
            public string? SpvName { get; set; }

            [JsonPropertyName("count_target")]
            public int? CountTarget { get; set; }

            [JsonPropertyName("nominal_target")]
            public double? NominalTarget { get; set; }

            [JsonPropertyName("count_followup")]
            public int? CountFollowup { get; set; }

            [JsonPropertyName("nominal_followup")]
            public double? NominalFollowup { get; set; }

            [JsonPropertyName("count_unfollows")]
            public int? CountUnfollows { get; set; }

            [JsonPropertyName("nominal_unfollows")]
            public double? NominalUnfollows { get; set; }
        }

        public class ReportPerfSpvReq
        {
            [JsonPropertyName("formdate")]
            public string? Formdate { get; set; }

            [JsonPropertyName("todate")]
            public string? Todate { get; set; }

            [JsonPropertyName("agent")]
            public string? Agent { get; set; }
        }

        public class ReportPerfSpv
        {
            [JsonPropertyName("status")]
            public string? Status { get; set; }

            [JsonPropertyName("data")]
            public List<ReportPerfSpvData>? Data { get; set; }
        }

        public class ReportPerfSpvData
        {
            [JsonPropertyName("acc_no")]
            public string? AccNo { get; set; }

            [JsonPropertyName("account")]
            public string? Account { get; set; }

            [JsonPropertyName("blm_tlpn")]
            public string? BlmTlpn { get; set; }

            [JsonPropertyName("tunggakan")]
            public double? Tunggakan { get; set; }

            [JsonPropertyName("lunas")]
            public double? Lunas { get; set; }

            [JsonPropertyName("janji_bayar")]
            public int? JanjiBayar { get; set; }

            [JsonPropertyName("account_berakhir")]
            public DateTime? AccountBerakhir { get; set; }
        }

        public class ReportPerfSpvSummary
        {
            [JsonPropertyName("status")]
            public string? Status { get; set; }

            [JsonPropertyName("data")]
            public List<ReportPerfSpvSummaryData>? Data { get; set; }
        }


        public class ReportPerfSpvSummaryData
        {
            [JsonPropertyName("agent")]
            public int? Agent { get; set; }

            [JsonPropertyName("ttl_account")]
            public int? TtlAccount { get; set; }

            [JsonPropertyName("ttl_tlpn")]
            public int? TtlTlpn { get; set; }

            [JsonPropertyName("tunggakan")]
            public double? Tunggakan { get; set; }

            [JsonPropertyName("ttl_sdh_bayar")]
            public double? TtlSdhBayar { get; set; }

            [JsonPropertyName("ttl_blm_bayar")]
            public double? TtlBlmBayar { get; set; }
        }

        public class SupersetLoginRequest
        {
            [JsonPropertyName("username")]
            public string? Username { get; set; }

            [JsonPropertyName("password")]
            public string? Password { get; set; }

            [JsonPropertyName("provider")]
            public string? Provider { get; set; }

            [JsonPropertyName("refresh")]
            public bool? Refresh { get; set; }
        }

        public class SupersetLoginResponse
        {
            [JsonPropertyName("access_token")]
            public string? AccessToken { get; set; }

            [JsonPropertyName("refresh_token")]
            public string? RefreshToken { get; set; }

        }


        public class ResourceSupersetRequest
        {
            public string? type { get; set; }
            public string? id { get; set; }
        }

        public class RlSupersetRequest
        {
            public string? clause { get; set; }
        }

        public class GuestTokenRequest
        {
            public UserSupersetRequest? user { get; set; }
            public List<ResourceSupersetRequest>? resources { get; set; }
            public List<RlSupersetRequest>? rls { get; set; }
        }

        public class UserSupersetRequest
        {
            public string? username { get; set; }
            public string? first_name { get; set; }
            public string? last_name { get; set; }
        }

        public class SupersetGuestResponse
        {
            [JsonPropertyName("token")]
            public string? Token { get; set; }

        }

        public class SupersetPayloadRequest
        {
            public string? id { get; set; }
            public string? type { get; set; }
        }

        public class SupersetConfigResponse
        {

            public string? Url { get; set; }

            public string? IdMainDCFC { get; set; }

            public string? IdMainSPV { get; set; }

            public string? IdMainMgt { get; set; }

        }
    }
}
