using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_data_loan_pk")]
    public class STGDataLoanPKPg
    {

        [Column("acc_no")]
        public string? ACC_NO { get; set; }
        [Column("tanggal_pk")]
        public DateTime? TANGGAL_PK { get; set; }
        [Column("nomor_pk")]
        public string? NOMOR_PK { get; set; }
        [Column("nama_notaris")]
        public string? NAMA_NOTARIS { get; set; }
        [Column("nama_legal")]
        public string? NAMA_LEGAL { get; set; }
        
    }
}
