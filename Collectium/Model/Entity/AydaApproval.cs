using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("ayda_approval")]
    public class AydaApproval
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("ayda_id")]
        public int? AydaId { get; set; }

        [ForeignKey("AydaId")]
        public Ayda? Ayda { get; set; }

        [Column("sender_role_id")]
        public int? SenderRoleId { get; set; }

        [ForeignKey(nameof(SenderRoleId))]
        public Role? SenderRole { get; set; }

        [Column("recipient_role_id")]
        public int? RecipientRoleId { get; set; }

        [ForeignKey(nameof(RecipientRoleId))]
        public Role? RecipientRole { get; set; }

        [Column("sender_id")]
        public int? SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public User? Sender { get; set; }

        [Column("recipient_id")]
        public int? RecipientId { get; set; }

        [ForeignKey(nameof(RecipientId))]
        public User? Recipient { get; set; }

        [Column("execution_id")]
        public int? ExecutionId { get; set; }

        [ForeignKey(nameof(ExecutionId))]
        public RecoveryExecution? Execution { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
    }
}
