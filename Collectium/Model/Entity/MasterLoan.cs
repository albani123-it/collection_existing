using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{

    [Table("master_loan")]
    public class MasterLoan
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("customer_id")]
        public int? CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

        [Column("prd_segment_id")]
        public int? ProductSegmentId { get; set; }

        [ForeignKey(nameof(ProductSegmentId))]
        public ProductSegment? ProductSegment { get; set; }

        [Column("cu_cif")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Cif { get; set; }
        
        [Column("acc_no")]
        [StringLength(20)]
        [Unicode(false)]
        public string? AccNo { get; set; }

        [Column("ccy")]
        [StringLength(10)]
        [Unicode(false)]
        public string? Ccy { get; set; }

        [Column("product")]
        public int? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [Column("plafond")]
        public double? Plafond { get; set; }

        [Column("maturity_date", TypeName = "datetime")]
        public DateTime? MaturityDate { get; set; }

        [Column("start_date", TypeName = "datetime")]
        public DateTime? StartDate { get; set; }

        [Column("sisa_tenor")]
        public int? SisaTenor { get; set; }

        [Column("tenor")]
        public int? Tenor { get; set; }

        [Column("installment_pokok")]
        public double? InstallmentPokok { get; set; }

        [Column("interest_rate")]
        public double? InterestRate { get; set; }

        [Column("installment")]
        public double? Installment { get; set; }

        [Column("tunggakan_pokok")]
        public double? TunggakanPokok { get; set; }

        [Column("tunggakan_bunga")]
        public double? TunggakanBunga { get; set; }

        [Column("tunggakan_denda")]
        public double? TunggakanDenda { get; set; }

        [Column("tunggakan_total")]
        public double? TunggakanTotal { get; set; }

        [Column("kewajiban_total")]
        public double? KewajibanTotal { get; set; }

        [Column("last_pay_date", TypeName = "datetime")]
        public DateTime? LastPayDate { get; set; }

        [Column("outstanding")]
        public double? Outstanding { get; set; }

        [Column("pay_total")]
        public double? PayTotal { get; set; }

        [Column("dpd")]
        public int? Dpd { get; set; }

        [Column("kolektibilitas")]
        public int? Kolektibilitas { get; set; }

        [Column("econa_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string? EconName { get; set; }

        [Column("econ_phone")]
        [StringLength(20)]
        [Unicode(false)]
        public string? EconPhone { get; set; }

        [Column("econ_relation")]
        [StringLength(10)]
        [Unicode(false)]
        public string? EconRelation { get; set; }

        [Column("marketing_code")]
        [StringLength(20)]
        [Unicode(false)]
        public string? MarketingCode { get; set; }

        [Column("channel_branch_code")]
        [StringLength(50)]
        [Unicode(false)]
        public string? ChannelBranchCode { get; set; }

        public virtual CollectionCall? Call { get; set; }

        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }
        [Column("fasilitas")]
        public string? Fasilitas { get; set; }
        [Column("status")]
        public int? Status { get; set; }

        [Column("payin_account")]
        public string? PayInAccount { get; set; }

        [Column("file_date")]
        public DateTime? FileDate { get; set; }

        [Column("loan_number")]
        public string? LoanNumber { get; set; }
    }
}
