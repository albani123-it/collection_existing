using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_data_loan_tagihan_lain")]
    public class STGDataLoanTagihanLainPg
    {

        [Column("acc_no")]
        public string? ACC_NO { get; set; }
        [Column("tanggal_tl")]
        public DateTime? Tanggal_TL { get; set; }
        [Column("nama_tl")]
        public string? NAMA_TL { get; set; }
        [Column("nominal_tl")]
        public double? Nominal_TL { get; set; }
        
    }
}
