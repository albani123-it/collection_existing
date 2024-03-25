using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("job_rule")]
    public class JobRule
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("rule_engine_id")]
        public int? RuleEngineId { get; set; }

        [ForeignKey(nameof(RuleEngineId))]
        public RuleEngine? RuleEngine{ get; set; }

        [Column("data_source_id")]
        public int? DataSourceId { get; set; }

        [ForeignKey(nameof(DataSourceId))]
        public DataSource? DataSource { get; set; }
        
        public string? Code { get; set; }

        [MaxLength]
        public string? Url { get; set; }


        [Column("num_data")]
        public int? NumData { get; set; }

        [Column("num_process")]
        public int? NumProcess { get; set; }

        [Column("start_time")]
        public DateTime? StartTime { get; set; }

        [Column("end_time")]
        public DateTime? EndTime { get; set; }

        public ICollection<CollectionTrace>? Result { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusGeneral? Status { get; set; }


    }
}
