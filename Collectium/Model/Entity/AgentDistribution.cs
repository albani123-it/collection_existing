using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("agent_dist")]
    public class AgentDistribution
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User{ get; set; }

        [Column("dist_id")]
        public int? DistId { get; set; }

        [ForeignKey(nameof(DistId))]
        public DistributionRule? Rule { get; set; }


    }
}
