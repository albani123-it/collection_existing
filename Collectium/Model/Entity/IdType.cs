using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("rfidtype")]
    public class IdType
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("idtype_code")]
        [StringLength(10)]
        [Unicode(false)]
        public string? Code { get; set; }

        [Column("idtype_desc")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Desc { get; set; }

        [Column("core_code")]
        [StringLength(10)]
        [Unicode(false)]
        public string? CoreCode { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusGeneral? Status { get; set; }
    }
}
