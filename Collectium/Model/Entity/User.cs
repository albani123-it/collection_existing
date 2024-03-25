using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{

    [Table("users")]
    public class User
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

        [ForeignKey(nameof(StatusId))]
        public StatusGeneral? Status { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("fcm")]
        public string? Fcm { get; set; }

        [Column("url")]
        public string? Url { get; set; }

        [Column("role_id")]
        public int? RoleId { get; set; }

        public Role? Role { get; set; }

        public virtual ICollection<UserBranch>? Branch { get; set; }

        [Column("active_branch_id")]
        public int? ActiveBranchId { get; set; }


        [Column("tel_code")]
        public string? TelCode { get; set; }

        [Column("tel_device")]
        public string? TelDevice { get; set; }

        [Column("pass_device")]
        public string? PassDevice { get; set; }

        [Column("fail")]
        public int? Fail { get; set; }
    }
}
