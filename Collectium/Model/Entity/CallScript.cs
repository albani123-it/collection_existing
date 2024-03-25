using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{

    [Table("call_script")]
    public class CallScript
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("code")]
        [StringLength(10)]
        public string? Code { get; set; }

        [Column("cs_desc")]
        [StringLength(50)]
        public string? CsDesc { get; set; }

        [Column("accd_min")]
        public int? AccdMin { get; set; }

        [Column("accd_max")]
        public int? AccdMax { get; set; }

        [Column("cs_script")]
        [StringLength(1000)]
        public string? CsScript { get; set; }

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
