using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("team")]

    public class Team
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("area_id")]
        public int? AreaId { get; set; }

        [ForeignKey(nameof(AreaId))]
        public Area? Area { get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        [Column("spv_id")]
        public int? SpvId { get; set; }

        [ForeignKey(nameof(SpvId))]
        public User? Spv { get; set; }

        public virtual ICollection<TeamMember>? Member { get; set; }

    }
}
