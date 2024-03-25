using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("collection_history")]
    public class CollectionHistory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("call_id")]
        public int? CallId { get; set; }

        [ForeignKey(nameof(CallId))]
        public CollectionCall? Call{ get; set; }

        [Column("call_by")]
        public int? CallById { get; set; }

        [ForeignKey(nameof(CallById))]
        public User? CallBy { get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        [Column("acc_no")]
        [StringLength(25)]
        [Unicode(false)]
        public string? AccNo { get; set; }

        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Name { get; set; }

        //tODO
        [Column("add_id")]
        public int? CollectionAddId { get; set; }

        [ForeignKey(nameof(CollectionAddId))]
        public CollectionAddContact? CollectionAdd { get; set; }

        [Column("reason")]
        public int? ReasonId { get; set; }

        [ForeignKey(nameof(ReasonId))]
        public Reason? Reason { get; set; }

        [Column("result")]
        public int? ResultId { get; set; }

        [ForeignKey(nameof(ResultId))]
        public CallResult? Result { get; set; }

        [Column("result_date", TypeName = "datetime")]
        public DateTime? ResultDate { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        [Column("note")]
        [StringLength(1000)]
        [Unicode(false)]
        public string? Notes { get; set; }

        [Column("history_date", TypeName = "datetime")]
        public DateTime? HistoryDate { get; set; }

        [Column("history_by")]
        public int? HistoryById { get; set; }

        [ForeignKey(nameof(HistoryById))]
        public User? HistoryBy { get; set; }

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

        public string? CallResultHh { get; set; }

        [Column("call_result_mm")]
        [StringLength(255)]
        [Unicode(false)]
        public string? CallResultMm { get; set; }

        [Column("call_result_hhmm")]
        [StringLength(255)]
        [Unicode(false)]
        public string? CallResultHhmm { get; set; }

        [Column("dpd")]
        public int? DPD { get; set; }

        public virtual ICollection<CollectionPhoto>? Photo { get; set; }
    }
}
