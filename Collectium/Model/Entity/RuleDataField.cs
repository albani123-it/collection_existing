using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("rule_data_field")]
    public class RuleDataField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("code")]
        [StringLength(10)]
        [Unicode(false)]
        public string? Code { get; set; }

        [Column("name")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Name { get; set; }

        [Column("csv_col")]
        public int? CsvCol { get; set; }

        [Column("rule_cond_valtype_id")]
        public int? RuleValId { get; set; }

        [ForeignKey(nameof(RuleValId))]
        public RuleValueType? ValueType { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusGeneral? Status { get; set; }

    }
}
