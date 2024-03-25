using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("branch")]
    public class Branch
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("code")]
        [StringLength(10)]
        public string? Code { get; set; }

        [Column("name")]
        [StringLength(150)]
        public string? Name { get; set; }

        [Column("branch_type_id")]
        public int? BranchTypeId { get; set; }

        [ForeignKey(nameof(BranchTypeId))]
        public BranchType? BranchType { get; set; }

        [Column("prd_segment_id")]
        public int? ProductSegmentId { get; set; }

        [ForeignKey(nameof(ProductSegmentId))]
        public ProductSegment? ProductSegment { get; set; }

        [Column("phone")]
        [StringLength(50)]
        public string? Phone { get; set; }

        [Column("fax")]
        [StringLength(50)]
        public string? Fax { get; set; }

        [Column("addr1")]
        [StringLength(500)]
        public string? Addr1 { get; set; }

        [Column("addr2")]
        [StringLength(500)]
        public string? Addr2 { get; set; }

        [Column("addr3")]
        [StringLength(500)]
        public string? Addr3 { get; set; }

        [Column("city")]
        [StringLength(50)]
        public string? City { get; set; }

        [Column("zipcode")]
        [StringLength(10)]
        public string? Zipcode { get; set; }

        [Column("area_id")]
        public int? AreaId { get; set; }

        [ForeignKey(nameof(AreaId))]
        public Area? Area { get; set; }


        //TODO
        //Kayaknya ke table master
        //[Column("branch_cco_id")]
        //public int? BranchCcoId { get; set; }

        //[ForeignKey(nameof(BranchCcoId))]
        //public Branch? BranchCco { get; set; }

        [Column("core_code")]
        [StringLength(10)]
        public string? CoreCode { get; set; }

        [Column("pic")]
        [StringLength(100)]
        public string? Pic { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Column("norek")]
        [StringLength(20)]
        public string? Norek { get; set; }

        [Column("amount")]
        public double? Amount { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusGeneral? Status { get; set; }

        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }
    }
}
