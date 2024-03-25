using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_loan_detail")]
    public class STGLoanDetailPg
    {

        [Column("acc_no")]
        public string? ACC_NO { get; set; }
        [Column("suku bunga")]
        public double? SUKU_BUNGA { get; set; }
        [Column("installment")]
        public double? INSTALLMENT { get; set; }
        [Column("principal_due")]
        public double? PRINCIPAL_DUE { get; set; }
        [Column("interest_due")]
        public double? INTEREST_DUE { get; set; }
        [Column("bunga_denda")]
        public double? BUNGA_DENDA { get; set; }
        [Column("penalty_denda")]
        public double? PENALTY_DENDA { get; set; }
        [Column("tagihan_lainnya_x")]
        public double? TAGIHAN_LAINYA { get; set; }
        [Column("biaya_lainnya_x")]
        public double? BIAYA_LAINNYA { get; set; }
        [Column("sub_total")]
        public double? SUB_TOTAL { get; set; }
        [Column("ksl_x")]
        public double? KSL { get; set; }
        [Column("total_kewajiban")]
        public double? TOTAL_KEWAJIBAN { get; set; }
        [Column("last_payment_date")]
        public DateTime? LAST_PAYMENT_DATE { get; set; }
        [Column("principal_paid")]
        public double? PRINCIPAL_PAID { get; set; }
        [Column("interest_paid")]
        public double? INTEREST_PAID { get; set; }
        [Column("charges_paid")]
        public double? CHARGES_PAID { get; set; }
        [Column("outstanding")]
        public double? OUTSTANDING { get; set; }
        [Column("tot_paid")]
        public double? TOT_PAID { get; set; }
        [Column("saldo_akhir")]
        public double? SALDO_AKHIR { get; set; }
        [Column("dpd")]
        public int? DPD { get; set; }
        [Column("kolektibility")]
        public int? KOLEKTIBILITY { get; set; }


        [Column("segment")]
        public string? Segment { get; set; }

        [Column("product_loan")]
        public string? ProductLoan { get; set; }

        [Column("start_date")]
        public DateTime? StartDate{ get; set; }

        [Column("maturity_date")]
        public DateTime? MaturityDate { get; set; }

        [Column("plafond")]
        public double? Plafond { get; set; }

        [Column("payin_account")]
        public string? PayInAccount { get; set; }

        [Column("file_date")]
        public DateTime? FileDate { get; set; }

        [Column("last_payment_final")]
        public DateTime? LastPaymentFinal { get; set; }
    }
}
