using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("business_exception")]
    public class BusinessException
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("job_rule_id")]
        public int? JobRuleId { get; set; }

        [ForeignKey(nameof(JobRuleId))]
        public JobRule? JobRule { get; set; }

        [Column("loan_id")]
        public int? LoanId { get; set; }

        [ForeignKey(nameof(LoanId))]
        public MasterLoan? Loan { get; set; }

        [Column("message")]
        [Unicode(false)]
        public string? Message { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }


    }
}
