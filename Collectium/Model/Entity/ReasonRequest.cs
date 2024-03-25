using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("reason_req")]
    public class ReasonRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("reason_id")]
        public int? ReasonId { get; set; }

        [ForeignKey(nameof(ReasonId))]
        public Reason? Reason{ get; set; }

        [Column("code")]
        [StringLength(10)]
        [Unicode(false)]
        public string? Code { get; set; }

        [Column("name")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Name { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }

        [Column("is_dc")]
        public int? isDC { get; set; }

        [Column("is_fc")]
        public int? isFC { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusRequest? Status { get; set; }

        [Column("req_user_id")]
        public int? RequestUserId { get; set; }

        [ForeignKey(nameof(RequestUserId))]
        public User? RequestUser { get; set; }

        [Column("approve_user_id")]
        public int? ApproveUserId { get; set; }

        [ForeignKey(nameof(ApproveUserId))]
        public User? ApprovedUser { get; set; }

    }
}
