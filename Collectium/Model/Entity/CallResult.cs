using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("rfresult")]
    public class CallResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("rl_code")]
        [StringLength(10)]
        [Unicode(false)]
        public string? Code { get; set; }

        [Column("rl_desc")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Description { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusGeneral? Status { get; set; }

        [Column("is_dc")]
        public int? isDC { get; set; }

        [Column("is_fc")]
        public int? isFC { get; set; }
    }
}
