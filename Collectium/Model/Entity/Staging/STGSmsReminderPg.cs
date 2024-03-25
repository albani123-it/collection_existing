using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_data_smsreminder")]
    public class STGSmsReminderPg
    {

        [Column("dsr_cu_cif")]
        public string? CU_CIF { get; set; }

        [Column("dsr_acc_no")]
        public string? ACC_NO { get; set; }

        [Column("dsr_nama")]
        public string? NAMA { get; set; }

        [Column("dsr_due_date")]
        public DateTime? DUE_DATE { get; set; }

        [Column("dsr_remainder_day")]
        public int? DAY { get; set; }


        [Column("dsr_nohp")]
        public string? HP { get; set; }
    }
}
