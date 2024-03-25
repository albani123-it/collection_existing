using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Keyless]
    [Table("STG_BRANCH")]
    public class STGBranch
    {

        [Column("ID")]
        public string? ID { get; set; }

        [Column("COMPANY_CODE")]
        public string? COMPANY_CODE { get; set; }

        [Column("COMPANY_NAME")]
        public string? COMPANY_NAME { get; set; }

        [Column("NAME_ADDRESS")]
        public string? NAME_ADDRESS { get; set; }

        [Column("MNEMONIC")]
        public string? MNEMONIC { get; set; }

        [Column("CUSTOMER_COMPANY")]
        public string? CUSTOMER_COMPANY { get; set; }

        [Column("CUSTOMER_MNEMONIC")]
        public string? CUSTOMER_MNEMONIC { get; set; }

        [Column("COMPANY_GROUP")]
        public int? COMPANY_GROUP { get; set; }

        [Column("CURR_NO")]
        public int? CURR_NO { get; set; }

        [Column("DATE_TIME")]
        public DateTime? DATE_TIME { get; set; }

        [Column("CO_CODE")]
        public string? CO_CODE { get; set; }

        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }

    }
}
