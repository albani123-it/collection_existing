using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("action_group_req")]
    public class ActionGroupRequest
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("action_group_id")]
        public int? ActionGroupId { get; set; }

        [ForeignKey(nameof(ActionGroupId))]
        public ActionGroup? ActionGroup { get; set; }

        [Column("action_id")]
        public int? ActionId { get; set; }

        [ForeignKey(nameof(ActionId))]
        public Action? Action { get; set; }

        [Column("role_id")]
        public int? RoleId { get; set; }

        public Role? Role { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }

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
