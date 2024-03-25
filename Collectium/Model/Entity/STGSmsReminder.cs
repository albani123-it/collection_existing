using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Keyless]
    [Table("STG_SMSREMINDER")]
    public class STGSmsReminder
    {

        [Column("CU_CIF")]
        public string? CU_CIF { get; set; }

        [Column("ACC_NO")]
        public string? ACC_NO { get; set; }

        [Column("NAMA")]
        public string? NAMA { get; set; }

        [Column("DUE_DATE")]
        public DateTime? DUE_DATE { get; set; }

        [Column("REMINDER_DAY")]
        public int? DAY { get; set; }


        [Column("NO_HP")]
        public string? HP { get; set; }

        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }

    }
}
