using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{

    [Table("token")]
    public class Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("firstname", TypeName = "varchar(100)")]
        public string Firstname { get; set; }

        [Column("user_id")]
        public int? UsersId { get; set; }

        public User Users { get; set; }

        [Column("expire")]
        public DateTime Expire { get; set; }

    }
}
