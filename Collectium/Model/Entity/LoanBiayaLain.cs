using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("LOAN_BIAYA_LAIN")]
    public class LoanBiayaLain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        [Column("loan_id")]
        public int? loan_id { get; set; }
        [Column("ACC_NO")]
        public string? ACC_NO { get; set; }
        [Column("Tanggal_Biaya_Lain")]
        public DateTime? Tanggal_Biaya_Lain { get; set; }
        [Column("NAMA_Biaya_Lain")]
        public string? NAMA_Biaya_Lain { get; set; }
        [Column("Nominal_Biaya_Lain")]
        public double? Nominal_Biaya_Lain { get; set; }
        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }

    }
}
