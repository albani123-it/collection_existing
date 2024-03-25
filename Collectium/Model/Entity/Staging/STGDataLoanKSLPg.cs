using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_data_loan_ksl")]
    public class STGDataLoanKSLPg
    {

        [Column("acc_no")]
        public string? ACC_NO { get; set; }
        [Column("tanggal_ksl")]
        public DateTime? Tanggal_KSL { get; set; }
        [Column("nama_ksl")]
        public string? NAMA_KSL { get; set; }
        [Column("saldo_ksl")]
        public string? Saldo_KSL { get; set; }
        
    }
}
