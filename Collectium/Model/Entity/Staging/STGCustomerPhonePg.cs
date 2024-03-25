using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_customer_nasabah_phone")]
    public class STGCustomerPhonePg
    {

        [Column("djp_id")]
        public int? DJP_ID{ get; set; }
        [Column("djp_cu_cif")]
        public string? CU_CIF{ get; set; }
        [Column("djp_cu_phnnum")]
        public string? PHONE { get; set; }
        
    }
}
