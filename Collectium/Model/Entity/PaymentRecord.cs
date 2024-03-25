using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("payment_record")]
    public class PaymentRecord
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("call_id")]
        public int? CallId { get; set; }

        [ForeignKey(nameof(CallId))]
        public CollectionCall? Call{ get; set; }

        [Column("call_by")]
        public int? CallById { get; set; }

        [ForeignKey(nameof(CallById))]
        public User? CallBy { get; set; }

        [Column("acc_no")]
        [StringLength(25)]
        [Unicode(false)]
        public string? AccNo { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        [Column("record_date", TypeName = "datetime")]
        public DateTime? RecordDate { get; set; }


    }
}
