using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{

    [Table("ayda")]

    public class Ayda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("loan_id")]
        public int? LoanId { get; set; }

        [ForeignKey(nameof(LoanId))]
        public MasterLoan? Loan { get; set; }

        [Column("mst_branch_id")]
        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        [Column("last_update_id")]
        public int? LastUpadteById { get; set; }

        [ForeignKey(nameof(LastUpadteById))]
        public User? LastUpadteBy { get; set; }

        [Column("last_update_date")]
        public DateTime? LastUpadteDate { get; set; }

        [Column("mst_branch_pembukuan_id")]
        public int? BranchPembukuanId { get; set; }

        [ForeignKey(nameof(BranchPembukuanId))]
        public Branch? BranchPembukuan { get; set; }

        [Column("mst_branch_proses_id")]
        public int? BranchProsesId { get; set; }

        [ForeignKey(nameof(BranchProsesId))]
        public Branch? BranchProses { get; set; }

        [Column("hubungan_bank_id")]
        public int? HubunganBankId { get; set; }

        [ForeignKey(nameof(HubunganBankId))]
        public AlasanLelang? HubunganBank { get; set; }

        [Column("tgl_ambil_alih")]
        public DateTime? TglAmbilAlih { get; set; }

        [Column("kualitas")]
        public string? Kualitas { get; set; }

        [Column("nilai_pembiayaan_pokok")]
        public double? NilaiPembiayaanPokok { get; set; }

        [Column("nilai_margin")]
        public double? NilaiMargin { get; set; }

        [Column("nilai_perolehan_agunan")]
        public double? NilaiPerolehanAgunan { get; set; }

        [Column("perkiraan_biaya_jual")]
        public double? PerkiraanBiayaPenjualan { get; set; }

        [Column("ppa")]
        public double? Ppa { get; set; }

        [Column("jumlah_ayda")]
        public double? JumlahAyda { get; set; }


        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusAsuransi? Status { get; set; }

        public virtual ICollection<InsuranceDocument>? Document { get; set; }

    }
}
