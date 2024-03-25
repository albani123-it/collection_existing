using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("rule_engine")]
    public class RuleEngine
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("rule_action_id")]
        public int? RuleActionId { get; set; }

        [ForeignKey(nameof(RuleActionId))]
        public RuleAction? RuleAction { get; set; }

        [Column("rule_option_id")]
        public int? RuleOptionId { get; set; }

        [ForeignKey(nameof(RuleOptionId))]
        public RuleActionOption? RuleOption { get; set; }

        public ICollection<RuleEngineCond> RuleEngineCond { get; set; }

        public virtual ICollection<RuleBucket>? Bucket { get; set; }

        [Column("code")]
        [Unicode(false)]
        public string? Code { get; set; }

        [Column("name")]
        [Unicode(false)]
        public string? Name { get; set; }


        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusGeneral? Status { get; set; }
    }
}
