using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("rfproduct")]
    public class Product
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("code")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Code { get; set; }

        [Column("desc")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Desc { get; set; }

        [Column("core_code")]
        [StringLength(20)]
        [Unicode(false)]
        public string? CoreCode { get; set; }

        [Column("prd_segment_id")]
        public int? ProductSegmentId { get; set; }

        [ForeignKey(nameof(ProductSegmentId))]
        public ProductSegment? ProductSegment { get; set; }

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
