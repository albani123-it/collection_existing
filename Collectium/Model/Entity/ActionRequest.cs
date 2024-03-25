using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{

    [Table("action_req")]
    public class ActionRequest
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("action_id")]
        public int? ActionId { get; set; }

        [ForeignKey(nameof(ActionId))]
        public Action? Action { get; set; }

        [Column("code")]
        [StringLength(10)]
        public string? ActCode { get; set; }

        [Column("act_desc")]
        [StringLength(50)]
        public string? ActDesc { get; set; }

        [Column("core_code")]
        [StringLength(10)]
        public string? CoreCode { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("approve_date")]
        public DateTime? ApproveDate { get; set; }

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
