using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("distr_rule")]
    public class DistributionRule
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("code")]
        [StringLength(10)]
        public string? Code { get; set; }

        [Column("name")]
        [StringLength(50)]
        public string? Name { get; set; }

        [Column("dpd_min")]
        public int? DpdMin { get; set; }

        [Column("dpd_max")]
        public int? DpdMax { get; set; }

        [Column("kol_min")]
        public int? KolMin { get; set; }

        [Column("kol_max")]
        public int? KolMax { get; set; }

        [Column("tungg_min")]
        public double? TunggakanMin { get; set; }

        [Column("tungg_max")]
        public double? TunggakanMax { get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        [Column("product_id")]
        public int? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        public virtual ICollection<AgentDistribution>? Agent { get; set; }

        public virtual ICollection<AgentLoan>? Loan { get; set; }

        [Column("groupx")]
        [StringLength(50)]
        public string? Group { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusGeneral? Status { get; set; }

    }
}
