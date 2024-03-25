using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("agent_loan")]
    public class AgentLoan
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("loan_id")]
        public int? LoanId { get; set; }

        [ForeignKey(nameof(LoanId))]
        public MasterLoan? Loan{ get; set; }

        [Column("dist_id")]
        public int? DistId { get; set; }

        [ForeignKey(nameof(DistId))]
        public DistributionRule? Rule { get; set; }


    }
}
