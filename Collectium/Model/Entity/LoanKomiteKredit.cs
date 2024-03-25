using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("LOAN_KOMITEKREDIT")]
    public class LoanKomiteKredit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        [Column("loan_id")]
        public int? loan_id { get; set; }
        [Column("ACC_NO")]
        public string? ACC_NO { get; set; }
        [Column("NOMOR_PK")]
        public string? NOMOR_PK { get; set; }
        [Column("TANGGAL_PK")]
        public DateTime? TANGGAL_PK { get; set; }
        [Column("KOMITE01")]
        public string? KOMITE01 { get; set; }
        [Column("KOMITE02")]
        public string? KOMITE02 { get; set; }
        [Column("KOMITE03")]
        public string? KOMITE03 { get; set; }
        [Column("KOMITE04")]
        public string? KOMITE04 { get; set; }
        [Column("KOMITE05")]
        public string? KOMITE05 { get; set; }
        [Column("KOMITE06")]
        public string? KOMITE06 { get; set; }
        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }

    }
}
