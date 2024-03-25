using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("collection_visit")]
    public class CollectionVisit
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("visit_id")]
        [StringLength(50)]
        [Unicode(false)]
        public string? VisitId { get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        [Column("acc_no")]
        [StringLength(25)]
        [Unicode(false)]
        public string? AccNo { get; set; }

        [Column("visit_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string? VisitName { get; set; }

        //tODO
        [Column("add_id")]
        [StringLength(50)]
        [Unicode(false)]
        public string? AddId { get; set; }

        [Column("visit_reason")]
        [StringLength(10)]
        [Unicode(false)]
        public string? VisitReason { get; set; }

        [Column("visit_result")]
        public int? VisitResultId { get; set; }

        [ForeignKey(nameof(VisitResultId))]
        public CallResult? CallResult { get; set; }

        [Column("visit_result_date", TypeName = "datetime")]
        public DateTime? VisitResultDate { get; set; }

        [Column("visit_amount")]
        public double? VisitAmount { get; set; }

        [Column("visit_note")]
        [StringLength(1000)]
        [Unicode(false)]
        public string? VisitNotes { get; set; }

        [Column("visit_date", TypeName = "datetime")]
        public DateTime? VisitDate { get; set; }

        [Column("visit_by")]
        public int? VisitById { get; set; }

        [ForeignKey(nameof(VisitById))]
        public User? VisitBy { get; set; }

        [Column("longitude")]
        public double? Longitude { get; set; }

        [Column("latitude")]
        public double? Latitude { get; set; }

        [Column("picture")]
        [StringLength(1000)]
        [Unicode(false)]
        public string? Picture { get; set; }

        [Column("ubm_id")]
        [StringLength(50)]
        [Unicode(false)]
        public string? UbmId { get; set; }

        [Column("cbm_id")]
        [StringLength(50)]
        [Unicode(false)]
        public string? CbmId { get; set; }

        [Column("kolek")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Kolek { get; set; }
    }
}
