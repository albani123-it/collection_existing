using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("rule_engine_cond")]
    public class RuleEngineCond
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("rule_engine_id")]
        public int? RuleEngineId { get; set; }

        [ForeignKey(nameof(RuleEngineId))]
        public RuleEngine? RuleEngine { get; set; }

        [Column("rule_field_id")]
        public int? RuleFieldId { get; set; }

        [ForeignKey(nameof(RuleFieldId))]
        public RuleDataField? RuleDataField{ get; set; }

        [Column("rule_op_id")]
        public int? RuleOperatorId { get; set; }

        [ForeignKey(nameof(RuleOperatorId))]
        public RuleOperator? RuleOperator { get; set; }

        [Column("value")]
        [Unicode(false)]
        public string? Value { get; set; }

    }
}
