using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("LOAN_KSL")]
    public class LoanKSL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        [Column("loan_id")]
        public int? loan_id { get; set; }
        [Column("ACC_NO")]
        public string? ACC_NO { get; set; }
        [Column("Tanggal_KSL")]
        public DateTime? Tanggal_KSL { get; set; }
        [Column("NAMA_KSL")]
        public string? NAMA_KSL { get; set; }
        [Column("Saldo_KSL")]
        public string? Saldo_KSL { get; set; }
        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }

    }
}
