using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_branch")]
    public class STGBranchPg
    {

        [Column("id")]
        public string? ID { get; set; }

        [Column("company_code")]
        public string? COMPANY_CODE { get; set; }

        [Column("company_name")]
        public string? COMPANY_NAME { get; set; }

        [Column("name_address")]
        public string? NAME_ADDRESS { get; set; }

        [Column("mnemonic")]
        public string? MNEMONIC { get; set; }

        [Column("customer_company")]
        public string? CUSTOMER_COMPANY { get; set; }

        [Column("customer_mnemonic")]
        public string? CUSTOMER_MNEMONIC { get; set; }

        [Column("company_group")]
        public int? COMPANY_GROUP { get; set; }

        [Column("curr_no")]
        public int? CURR_NO { get; set; }

        [Column("date_time")]
        public DateTime? DATE_TIME { get; set; }

        [Column("co_code")]
        public string? CO_CODE { get; set; }


    }
}
