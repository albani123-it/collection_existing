using Collectium.Model.Bean;

namespace Collectium.Model.Bean.ListRequest
{
    public class CollListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }

        public string? AccountNo { get; set; }

        public string? Name { get; set; }

        public int? Dpd { get; set; }

        public int? OfficerId { get; set; }

        public int? StatusId { get; set; }

        public string? CallResultCode { get; set; }

        public DateTime? PtpDate { get; set; }

        public int? sortasc { get; set; } // nama a to z
        public int? sortdesc { get; set; } //nama z to a

        public int? sorttagihan { get; set; } //tagihan terbesar
        public int? sortoverduedate { get; set; } //tgl tunggakan desc

        public int? BranchId { get; set; }

    }


    public class SpvListCollectiontBean : PagedRequestBean
    {

        public string? Keyword { get; set; }

        public string? AccNo { get; set; }

        public DateTime? PtpDate { get; set; }

        public int? BranchId { get; set; }

        public int? AgentId { get; set; }

        public int? sortasc { get; set; } // nama a to z
        public int? sortdesc { get; set; } //nama z to a

        public int? sorttagihan { get; set; } //tagihan terbesar
        public int? sortoverduedate { get; set; } //tgl tunggakan desc

    }

    public class SpvMonListBean : PagedRequestBean
    {
        public string? AccNo { get; set; }

        public string? Name { get; set; }

        public int? BranchId { get; set; }

        public int? DPDmin { get; set; }

        public int? DPDmax { get; set; }

        public int? ResultId { get; set; }

        public int? ReasonId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? MyType { get; set; }

        public int? AgentId { get; set; }

        public int? StatusLoan { get; set; }

    }

    public class ExportTraceBean
    {

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }


    }

    public class DistribusiBean
    {

        public int? BranchId { get; set; }

        public int? Dpdmin { get; set; }

        public int? Dpdmax { get; set; }

        public int? Kolmin { get; set; }

        public int? Kolmax { get; set; }

        public double? Tunggakanmin { get; set; }

        public double? Tunggakanmax { get; set; }

        public int? ProductId { get; set; }

        public int? AgentId { get; set; }

    }

    public class DistribusiBeanSave
    {

        public string? Code { get; set; }

        public string? Name { get; set; }


        public int? BranchId { get; set; }

        public int? Dpdmin { get; set; }

        public int? Dpdmax { get; set; }

        public int? Kolmin { get; set; }

        public int? Kolmax { get; set; }

        public double? Tunggakanmin { get; set; }

        public double? Tunggakanmax { get; set; }

        public int? ProductId { get; set; }

        public string? Group { get; set; }

        public List<int?> AgentId { get; set; }

        public List<int?> LoanId { get; set; }

    }
}