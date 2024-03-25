using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("payment_history")]
    public class PaymentHistory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("loan_id")]
        public int? LoanId { get; set; }

        [ForeignKey(nameof(LoanId))]
        public MasterLoan? Loan { get; set; }

        [Column("acc_no")]
        [StringLength(20)]
        [Unicode(false)]
        public string? AccNo { get; set; }


        [Column("tgl", TypeName = "datetime")]
        public DateTime? Tgl { get; set; }

        [Column("pokok_cicilan")]
        public double? PokokCicilan { get; set; }

        [Column("bunga")]
        public double? Bunga { get; set; }

        [Column("denda")]
        public double? Denda { get; set; }

        [Column("total_bayar")]
        public double? TotalBayar { get; set; }

        [Column("call_by")]
        public int? CallById { get; set; }

        [ForeignKey(nameof(CallById))]
        public User? CallBy { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
    }
}
