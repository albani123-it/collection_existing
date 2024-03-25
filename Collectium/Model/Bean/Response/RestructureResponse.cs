using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Collectium.Model.Bean.Response
{
    public class RestructureResponse
    {

        public class RestructureListResponseBean
        {
            public int? Id { get; set; }

            public string? AccoNo { get; set; }

            public string? Cif { get; set; }

            public string? Name { get; set; }

            public string? BranchName { get; set; }

            public string? BranchProsesName { get; set; }

            public string? BranchPembukuanName { get; set; }

            public string? PolaRestruktur { get; set; }

            public string? AlasanLelang { get; set; }

            public string? HubunganBank { get; set; }

            public string? Asuransi { get; set; }

            public StatusGeneralBean? Status { get; set; }

        }

        public class DistribusiManualReq : PagedRequestBean
        {

            public string? Keyword { get; set; }

            public string? AccountNo { get; set; }

            public string? Name { get; set; }

            public int? Dpd { get; set; }

            public int? OfficerId { get; set; }

            public int? StatusId { get; set; }

            public int? BranchId { get; set; }

        }

        public class DistribusiNasabahReq : PagedRequestBean
        {

            public int? BranchId { get; set; }

            public int? Dpdmin { get; set; }

            public int? Dpdmax { get; set; }

            public int? Kolmin { get; set; }

            public int? Kolmax { get; set; }

            public double? Tunggakanmin { get; set; }

            public double? Tunggakanmax { get; set; }

            public int? ProductId { get; set; }


        }

        public class DistribusiAgentReq : PagedRequestBean
        {

            public int? BranchId { get; set; }

            public string? Group { get; set; }  


        }

        public class DistribusiManualList
        {
            public int? Id { get; set; }

            public BranchResponseBean? Branch { get; set; }

            public ProductLoanResponseBean? Product { get; set; }

            public string? Code { get; set; }

            public string? Name { get; set; }

            public int? DpdMin { get; set; }

            public int? DpdMax { get; set; }

            public int? KolMin { get; set; }

            public int? KolMax { get; set; }

            public double? TunggakanMin { get; set; }

            public double? TunggakanMax { get; set; }

        }

        public class DistribusiManualGet
        {
            public int? Id { get; set; }

            public BranchResponseBean? Branch { get; set; }

            public ProductLoanResponseBean? Product { get; set; }

            public string? Code { get; set; }

            public string? Name { get; set; }

            public int? DpdMin { get; set; }

            public int? DpdMax { get; set; }

            public int? KolMin { get; set; }

            public int? KolMax { get; set; }

            public string? Group { get; set; }

            public double? TunggakanMin { get; set; }

            public double? TunggakanMax { get; set; }

            public List<UserResponseBean>? AgentId { get; set; }

            public List<CollResponseBean>? LoanId { get; set; }

        }
    }
}
