using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("team_member")]
    public class TeamMember
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("member_id")]
        public int? MemberId { get; set; }

        [ForeignKey(nameof(MemberId))]
        public User? Member { get; set; }

        [Column("team_id")]
        public int? TeamId { get; set; }

        [ForeignKey("TeamId")]
        public Team? Team { get; set; }
    }
}
