using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("LOAN_TAGIHAN_LAIN")]
    public class LoanTagihanLain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        [Column("loan_id")]
        public int? loan_id { get; set; }
        [Column("ACC_NO")]
        public string? ACC_NO { get; set; }
        [Column("Tanggal_TL")]
        public DateTime? Tanggal_TL { get; set; }
        [Column("NAMA_TL")]
        public string? NAMA_TL { get; set; }
        [Column("Nominal_TL")]
        public double? Nominal_TL { get; set; }
        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }

    }
}
