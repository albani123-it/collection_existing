using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("signature")]
    public class Signature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("signature_id")]
        public int? Id { get; set; }

        [Column("doc_signature_id")]
        public int? DocSignatureId { get; set; }

        [ForeignKey(nameof(DocSignatureId))]
        public DocumentSignature? DocSignature { get; set; }

        [Column("sign_name")]
        [StringLength(90)]
        [Unicode(false)]
        public string? Name { get; set; }

        [Column("sign_role")]
        [StringLength(90)]
        [Unicode(false)]
        public string? SignRole { get; set; }

        [Column("sequence")]
        public int? Sequence { get; set; }

    }
}
