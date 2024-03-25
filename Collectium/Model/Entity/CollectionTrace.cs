using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("collection_trace")]
    public class CollectionTrace
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

        [Column("acc_no")]
        [StringLength(25)]
        [Unicode(false)]
        public string? AccNo { get; set; }

        [Column("result")]
        public int? ResultId { get; set; }

        [ForeignKey(nameof(ResultId))]
        public CallResult? Result { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        [Column("trace_date", TypeName = "datetime")]
        public DateTime? TraceDate { get; set; }


        [Column("kolek")]
        [StringLength(50)]
        [Unicode(false)]
        public int? Kolek { get; set; }

        [Column("dpd")]
        public int? DPD { get; set; }


        [Column("job_rule_id")]
        public int? JobRuleId { get; set; }

        [ForeignKey(nameof(JobRuleId))]
        public JobRule? JobRule { get; set; }


    }
}
