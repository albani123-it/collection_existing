using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{

    [Table("call_script_req")]
    public class CallScriptRequest
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("call_script_id")]
        public int? CallScriptId { get; set; }

        [ForeignKey(nameof(CallScriptId))]
        public CallScript? CallScript { get; set; }

        [Column("code")]
        [StringLength(10)]
        public string? Code { get; set; }

        [Column("cs_desc")]
        [StringLength(50)]
        public string? CsDesc { get; set; }

        [Column("accd_min")]
        public int? AccdMin { get; set; }

        [Column("accd_max")]
        public int? AccdMax { get; set; }

        [Column("cs_script")]
        [StringLength(1000)]
        public string? CsScript { get; set; }

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
