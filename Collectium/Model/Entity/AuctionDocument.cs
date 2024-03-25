using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("auction_document")]
    public class AuctionDocument
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("auction_id")]
        public int? AuctionId { get; set; }

        [ForeignKey("AuctionId")]
        public Auction? Auction { get; set; }

        [Column("doc_type_id")]
        public int? DocumentAuctionId { get; set; }

        [ForeignKey("DocumentAuctionId")]
        public DocumentAuction? DocumentAuction { get; set; }

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
