using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("insurance_document")]
    public class InsuranceDocument
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("insurance_id")]
        public int? InsuranceId { get; set; }

        [ForeignKey("InsuranceId")]
        public Insurance? Insurance { get; set; }

        [Column("doc_type_id")]
        public int? DocumentInsuranceId { get; set; }

        [ForeignKey("DocumentInsuranceId")]
        public DocumentInsurance? DocumentInsurance { get; set; }

        [Column("title")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Title { get; set; }

        [Column("description")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Description { get; set; }

        [Column("url")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Url { get; set; }

        [Column("mime")]
        [StringLength(255)]
        [Unicode(false)]
        public string? Mime { get; set; }

        [Column("lat")]
        public double? Lat { get; set; }

        [Column("lon")]
        public double? Lon { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
    }
}
