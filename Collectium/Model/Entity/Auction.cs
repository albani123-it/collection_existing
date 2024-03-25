using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{

    [Table("auction")]

    public class Auction
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

        [Column("alasan_lelang_id")]
        public int? AlasanLelangId { get; set; }

        [ForeignKey(nameof(AlasanLelangId))]
        public AlasanLelang? AlasanLelang { get; set; }

        [Column("no_pk")]
        public string? NoPK { get; set; }

        [Column("nilai_limit_lelang")]
        public double? NilaiLimitLelang { get; set; }

        [Column("uang_jaminan")]
        public double? UangJaminan { get; set; }

        [Column("objek_lelang")]
        public string? ObjekLelang { get; set; }

        [Column("keterangan")]
        public string? Keterangan { get; set; }

        [Column("balai_lelang_id")]
        public int? BalaiLelangId { get; set; }

        [ForeignKey(nameof(BalaiLelangId))]
        public BalaiLelang? BalaiLelang { get; set; }

        [Column("jenis_lelang_id")]
        public int? JenisLelangId { get; set; }

        [ForeignKey(nameof(JenisLelangId))]
        public JenisLelang? JenisLelang { get; set; }

        [Column("tata_cara_lelang")]
        public string? TataCaraLelang { get; set; }

        [Column("biaya_lelang")]
        public double? BiayaLelang { get; set; }

        [Column("catatan_lelang")]
        public string? CatatanLelang { get; set; }

        [Column("tgl_penetapan_lelang")]
        public DateTime? TglPenetapanLelang { get; set; }

        [Column("no_rekening")]
        public string? NoRekening { get; set; }

        [Column("nama_rekening")]
        public string? NamaRekening { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusLeLang? Status { get; set; }

        public virtual ICollection<AuctionDocument>? Document { get; set; }

        public virtual ICollection<AuctionResultDocument>? DocumentResult { get; set; }
    }
}
