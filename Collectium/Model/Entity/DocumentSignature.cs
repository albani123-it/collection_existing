using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{

    [Table("doc_signature")]
    public class DocumentSignature
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        [Column("doc_code")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Code { get; set; }

        [Column("doc_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Name { get; set; }


    }
}
