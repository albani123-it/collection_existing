using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Keyless]
    [Table("STG_DATA_KREDIT")]
    public class STGDataKredit
    {

        [Column("KATAGORI_DEBITUR")]
        public string? KATAGORI_DEBITUR { get; set; }
        [Column("OBLIGOR")]
        public string? OBLIGOR { get; set; }
        [Column("CABANG_ASAL_DEBITUR")]
        public string? CABANG_ASAL_DEBITUR { get; set; }
        [Column("KOMITE_PEMUTUS_KREDIT_X")]
        public string? KOMITE_PEMUTUS_KREDIT { get; set; }
        [Column("TANGGAL_PK_X")]
        public DateTime? TANGGAL_PK { get; set; }
        [Column("NO_PK_X")]
        public string? NO_PK { get; set; }
        [Column("NAMA_NOTARIS_X")]
        public string? NAMA_NOTARIS { get; set; }
        [Column("STAFF_LEGAL_X")]
        public string? STAFF_LEGAL { get; set; }
        [Column("KODE_ACCOUNT_OFFICER_X")]
        public string? KODE_ACCOUNT_OFFICER { get; set; }
        [Column("ACCOUNT_OFFICER_X")]
        public string? ACCOUNT_OFFICER { get; set; }
        [Column("TANGGAL_MULAI_MENUNGGAK")]
        public DateTime? TANGGAL_MULAI_MENUNGGAK { get; set; }
        [Column("SEGMENTASI")]
        public string? SEGMENTASI { get; set; }
        [Column("LOAN_NUMBER")]
        public string? LOAN_NUMBER { get; set; }
        [Column("PLAFON")]
        public double? PLAFON { get; set; }
        [Column("LIMIT_ID")]
        public string? LIMIT_ID { get; set; }
        [Column("CU_CIF")]
        public string? CU_CIF { get; set; }
        [Column("ACC_NO")]
        public string? ACC_NO { get; set; }
        [Column("ISO_CURRENCY")]
        public string? ISO_CURRENCY { get; set; }
        [Column("FASILITAS")]
        public string? FASILITAS { get; set; }
        [Column("OUTSTANDING")]
        public double? OUTSTANDING { get; set; }
        [Column("MATURITY_DATE")]
        public DateTime? MATURITY_DATE { get; set; }
        [Column("BOOKING_DATE")]
        public DateTime? BOOKING_DATE { get; set; }
        [Column("SISA_SETORAN")]
        public int? SISA_SETORAN { get; set; }
        [Column("TENOR")]
        public int? TENOR { get; set; }
        [Column("PRINCIPAL_USD")]
        public double? PRINCIPAL_USD { get; set; }
        [Column("PRINCIPAL_IDR")]
        public double? PRINCIPAL_IDR { get; set; }
        [Column("ASURANSI_ID")]
        public string? ASURANSI_ID { get; set; }
        [Column("INSURANCE_TYPE")]
        public string? INSURANCE_TYPE { get; set; }
        [Column("TOTAL_PENARIKAN")]
        public double? TOTAL_PENARIKAN { get; set; }
        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }
    }
}
