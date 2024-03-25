using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_data_kredit")]
    public class STGDataKreditPg
    {

        [Column("kategori_debitur")]
        public string? KATAGORI_DEBITUR { get; set; }
        [Column("obligor")]
        public string? OBLIGOR { get; set; }
        [Column("cabang_asal_debitur")]
        public string? CABANG_ASAL_DEBITUR { get; set; }
        [Column("komite_pemutus_kredit_x")]
        public string? KOMITE_PEMUTUS_KREDIT { get; set; }
        [Column("tanggal_pk_x")]
        public DateTime? TANGGAL_PK { get; set; }
        [Column("no_pk_x")]
        public string? NO_PK { get; set; }
        [Column("nama_notaris_x")]
        public string? NAMA_NOTARIS { get; set; }
        [Column("staff_legal_x")]
        public string? STAFF_LEGAL { get; set; }
        [Column("kode_account_officer_x")]
        public string? KODE_ACCOUNT_OFFICER { get; set; }
        [Column("account_officer_x")]
        public string? ACCOUNT_OFFICER { get; set; }
        [Column("tanggal_mulai_menunggak")]
        public DateTime? TANGGAL_MULAI_MENUNGGAK { get; set; }
        [Column("segmentasi")]
        public string? SEGMENTASI { get; set; }
        [Column("loan_number")]
        public string? LOAN_NUMBER { get; set; }
        [Column("plafon")]
        public double? PLAFON { get; set; }
        [Column("limit_id")]
        public string? LIMIT_ID { get; set; }
        [Column("cu_cif")]
        public string? CU_CIF { get; set; }
        [Column("acc_no")]
        public string? ACC_NO { get; set; }
        [Column("iso_currency")]
        public string? ISO_CURRENCY { get; set; }
        [Column("fasilitas")]
        public string? FASILITAS { get; set; }
        [Column("outstanding")]
        public double? OUTSTANDING { get; set; }
        [Column("maturity_date")]
        public DateTime? MATURITY_DATE { get; set; }
        [Column("booking_date")]
        public DateTime? BOOKING_DATE { get; set; }
        [Column("sisa_setoran")]
        public int? SISA_SETORAN { get; set; }
        [Column("tenor")]
        public int? TENOR { get; set; }
        [Column("principal_usd")]
        public double? PRINCIPAL_USD { get; set; }
        [Column("principal_idr")]
        public double? PRINCIPAL_IDR { get; set; }
        [Column("asuransi_id")]
        public string? ASURANSI_ID { get; set; }
        [Column("insurance_type")]
        public string? INSURANCE_TYPE { get; set; }
        [Column("total_penarikan")]
        public double? TOTAL_PENARIKAN { get; set; }
    }
}
