using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Keyless]
    [Table("STG_LOAN_DETAIL")]
    public class STGLoanDetail
    {

        [Column("ACC_NO")]
        public string? ACC_NO { get; set; }
        [Column("SUKU_BUNGA")]
        public double? SUKU_BUNGA { get; set; }
        [Column("INSTALLMENT")]
        public double? INSTALLMENT { get; set; }
        [Column("PRINCIPAL_DUE")]
        public double? PRINCIPAL_DUE { get; set; }
        [Column("INTEREST_DUE")]
        public double? INTEREST_DUE { get; set; }
        [Column("BUNGA_DENDA")]
        public double? BUNGA_DENDA { get; set; }
        [Column("PENALTY_DENDA")]
        public double? PENALTY_DENDA { get; set; }
        [Column("TAGIHAN_LAINNYA_X")]
        public double? TAGIHAN_LAINYA { get; set; }
        [Column("BIAYA_LAINNYA_X")]
        public double? BIAYA_LAINNYA { get; set; }
        [Column("SUB_TOTAL")]
        public double? SUB_TOTAL { get; set; }
        [Column("KSL_X")]
        public double? KSL { get; set; }
        [Column("TOTAL_KEWAJIBAN")]
        public double? TOTAL_KEWAJIBAN { get; set; }
        [Column("LAST_PAYMENT_DATE")]
        public DateTime? LAST_PAYMENT_DATE { get; set; }
        [Column("PRINCIPAL_PAID")]
        public double? PRINCIPAL_PAID { get; set; }
        [Column("INTEREST_PAID")]
        public double? INTEREST_PAID { get; set; }
        [Column("CHARGES_PAID")]
        public double? CHARGES_PAID { get; set; }
        [Column("OUTSTANDING")]
        public double? OUTSTANDING { get; set; }
        [Column("TOT_PAID")]
        public double? TOT_PAID { get; set; }
        [Column("SALDO_AKHIR")]
        public double? SALDO_AKHIR { get; set; }
        [Column("DPD")]
        public int? DPD { get; set; }
        [Column("KOLEKTIBILITY")]
        public int? KOLEKTIBILITY { get; set; }
        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }

        [Column("SEGMENT")]
        public string? Segment { get; set; }

        [Column("PRODUCT_LOAN")]
        public string? ProductLoan { get; set; }

        [Column("START_DATE")]
        public DateTime? StartDate { get; set; }

        [Column("MATURITY_DATE")]
        public DateTime? MaturityDate { get; set; }

        [Column("PLAFOND")]
        public double? Plafond { get; set; }

        [Column("payin_account")]
        public string? PayInAccount { get; set; }

        [Column("file_date")]
        public DateTime? FileDate { get; set; }
    }
}
