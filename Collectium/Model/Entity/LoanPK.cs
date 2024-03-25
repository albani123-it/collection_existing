using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("LOAN_PK")]
    public class LoanPK
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        [Column("loan_id")]
        public int? loan_id { get; set; }
        [Column("ACC_NO")]
        public string? ACC_NO { get; set; }
        [Column("TANGGAL_PK")]
        public DateTime? TANGGAL_PK { get; set; }
        [Column("NOMOR_PK")]
        public string? NOMOR_PK { get; set; }
        [Column("NAMA_NOTARIS")]
        public string? NAMA_NOTARIS { get; set; }
        [Column("NAMA_LEGAL")]
        public string? NAMA_LEGAL { get; set; }
        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }

    }
}
