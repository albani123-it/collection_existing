using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Keyless]
    [Table("STG_DATA_LOAN_KSL")]
    public class STGDataLoanKSL
    {

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
