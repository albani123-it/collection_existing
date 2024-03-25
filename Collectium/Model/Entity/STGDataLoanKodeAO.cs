using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Keyless]
    [Table("STG_DATA_LOAN_KODEAO")]
    public class STGDataLoanKodeAO
    {

        [Column("ACC_NO")]
        public string? ACC_NO { get; set; }
        [Column("KODE_AO")]
        public string? KODE_AO { get; set; }
        [Column("TANGGAL_AO")]
        public DateTime? TANGGAL_AO { get; set; }
        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }

    }
}
