using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }


        //[Column("create_date")]
        //public DateTime? CreateDate { get; set; }

        //[Column("update_date")]
        //public DateTime? UpdateDate { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        public virtual ICollection<RolePermission>? RolePermission { get; set; }

        public StatusGeneral? Status { get; set; }
    }
}
