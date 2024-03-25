using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("user_branch_req")]
    public class UserBranchRequest
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User{ get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        [Column("user_branch_id")]
        public int? UserBranchId { get; set; }

        [ForeignKey(nameof(UserBranchId))]
        public UserBranch? UserBranch { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        //[Column("approve_date")]
        //public DateTime? ApproveDate { get; set; }

        [Column("req_user_id")]
        public int? RequestUserId { get; set; }

        [ForeignKey(nameof(RequestUserId))]
        public User? RequestUser { get; set; }

        [Column("approve_user_id")]
        public int? ApproveUserId { get; set; }

        [ForeignKey(nameof(ApproveUserId))]
        public User? ApprovedUser { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        public StatusRequest? Status { get; set; }

        [Column("user_req_id")]
        public int? UserRequestId { get; set; }
    }
}
