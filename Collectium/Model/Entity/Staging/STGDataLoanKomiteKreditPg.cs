using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_data_loan_komitekredit")]
    public class STGDataLoanKomiteKreditPg
    {
         
        [Column("acc_no")]
        public string? ACC_NO { get; set; }
        [Column("nomor_pk")]
        public string? NOMOR_PK { get; set; }
        [Column("tanggal_pk")]
        public DateTime? TANGGAL_PK { get; set; }
        [Column("komite01")]
        public string? KOMITE01 { get; set; }
        [Column("komite02")]
        public string? KOMITE02 { get; set; }
        [Column("komite03")]
        public string? KOMITE03 { get; set; }
        [Column("komite04")]
        public string? KOMITE04 { get; set; }
        [Column("komite05")]
        public string? KOMITE05 { get; set; }
        [Column("komite06")]
        public string? KOMITE06 { get; set; }


    }
}
