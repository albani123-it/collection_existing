using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("collection_call")]
    public class CollectionCall
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("loan_id")]
        public int? LoanId { get; set; }

        [ForeignKey(nameof(LoanId))]
        public MasterLoan? Loan { get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        [Column("acc_no")]
        [StringLength(25)]
        [Unicode(false)]
        public string? AccNo { get; set; }

        [Column("call_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string? CallName { get; set; }

        [Column("add_id")]
        public int? CollectionAddId { get; set; }

        [ForeignKey(nameof(CollectionAddId))]
        public CollectionAddContact? CollectionAdd { get; set; }

        [Column("call_reason")]
        public int? ReasonId { get; set; }

        [ForeignKey(nameof(ReasonId))]
        public Reason? Reason { get; set; }

        [Column("call_result_id")]
        public int? CallResultId { get; set; }

        [ForeignKey(nameof(CallResultId))]
        public CallResult? CallResult { get; set; }

        [Column("call_result_date", TypeName = "datetime")]
        public DateTime? CallResultDate { get; set; }


        [Column("call_amount")]
        public double? CallAmount { get; set; }

        [Column("call_notes")]
        [StringLength(1000)]
        [Unicode(false)]
        public string? CallNotes { get; set; }

        [Column("call_date", TypeName = "datetime")]
        public DateTime? CallDate { get; set; }

        [Column("call_by")]
        public int? CallById { get; set; }

        [ForeignKey(nameof(CallById))]
        public User? CallBy { get; set; }

        [Column("call_result_hh")]
        [StringLength(2)]
        [Unicode(false)]
        public string? CallResultHh { get; set; }

        [Column("call_result_mm")]
        [StringLength(2)]
        [Unicode(false)]
        public string? CallResultMm { get; set; }

        [Column("call_result_hhmm")]
        [StringLength(5)]
        [Unicode(false)]
        public string? CallResultHhmm { get; set; }
    }
}
