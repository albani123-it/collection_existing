using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("rule_engine_bucket")]
    public class RuleBucket
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("rule_engine_id")]
        public int? RuleId { get; set; }

        [ForeignKey(nameof(RuleId))]
        public RuleEngine? RuleEngine{ get; set; }

        [Column("bucket_id")]
        public int? BucketId { get; set; }

        [ForeignKey(nameof(BucketId))]
        public Bucket? Bucket { get; set; }

    }
}
