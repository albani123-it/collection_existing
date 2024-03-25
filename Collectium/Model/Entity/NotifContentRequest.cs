using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{

    [Table("notif_content_req")]
    public class NotifContentRequest
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("notif_content_id")]
        public int? NotifContentId { get; set; }

        [ForeignKey(nameof(NotifContentId))]
        public NotifContent? NotifContent { get; set; }

        [Column("code")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Code { get; set; }

        [Column("name")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Name { get; set; }

        [Column("content")]
        [StringLength(1000)]
        [Unicode(false)]
        public string? Content { get; set; }

        [Column("day")]
        public int? Day { get; set; }

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
