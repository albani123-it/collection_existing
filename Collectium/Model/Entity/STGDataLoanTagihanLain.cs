using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Keyless]
    [Table("STG_DATA_LOAN_TAGIHAN_LAIN")]
    public class STGDataLoanTagihanLain
    {

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
