using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_data_loan_kodeao")]
    public class STGDataLoanKodeAOPg
    {

        [Column("acc_no")]
        public string? ACC_NO { get; set; }
        [Column("kode_ao")]
        public string? KODE_AO { get; set; }
        [Column("tanggal_ao")]
        public DateTime? TANGGAL_AO { get; set; }

    }
}
