using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_data_loan_biaya_lain")]
    public class STGDataLoanBiayaLainPg
    {

        [Column("acc_no")]
        public string? ACC_NO { get; set; }
        [Column("tanggal_biaya_lain")]
        public DateTime? Tanggal_Biaya_Lain { get; set; }
        [Column("nama_biaya_lain")]
        public string? NAMA_Biaya_Lain { get; set; }
        [Column("nominal_biaya_lain")]
        public double? Nominal_Biaya_Lain { get; set; }

    }
}
