using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("bucket_user")]
    public class BucketUser
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User{ get; set; }

        [Column("bucket_id")]
        public int? BucketId { get; set; }

        [ForeignKey(nameof(BucketId))]
        public Bucket? Bucket { get; set; }

    }
}
