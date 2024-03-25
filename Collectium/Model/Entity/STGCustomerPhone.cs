using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Keyless]
    [Table("STG_CUSTOMER_PHONE")]
    public class STGCustomerPhone
    {

        [Column("CIF")]
        public string? CIF { get; set; }
        [Column("PHONE")]
        public string? PHONE { get; set; }
        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }

    }
}
