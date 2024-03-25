using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{

    [Table("users_req")]
    public class UserRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("username")]
        public string? Username { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("password")]
        public string? Password { get; set; }

        [Column("admin")]
        public int? Administrator { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        public StatusRequest? Status { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("approve_date")]
        public DateTime? ApproveDate { get; set; }

        [Column("fcm")]
        public string? Fcm { get; set; }

        [Column("url")]
        public string? Url { get; set; }

        [Column("role_id")]
        public int? RoleId { get; set; }

        public Role? Role { get; set; }

        [Column("spv_id")]
        public int? SpvId { get; set; }

        [ForeignKey(nameof(SpvId))]
        public User? Spv { get; set; }


        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Column("req_user_id")]
        public int? RequestUserId { get; set; }

        [ForeignKey(nameof(RequestUserId))]
        public User? RequestUser { get; set; }

        [Column("approve_user_id")]
        public int? ApproveUserId { get; set; }

        [ForeignKey(nameof(ApproveUserId))]
        public User? ApprovedUser { get; set; }

        [Column("tel_code")]
        public string? TelCode { get; set; }

        [Column("tel_device")]
        public string? TelDevice { get; set; }

        public virtual ICollection<UserBranchRequest>? Branch { get; set; }
    }
}
