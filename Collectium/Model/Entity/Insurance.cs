using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{

    [Table("insurance")]

    public class Insurance
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

        [Column("nama_pejabat")]
        public string? NamaPejabat { get; set; }

        [Column("jabatan")]
        public string? Jabatan { get; set; }

        [Column("no_sertifikat")]
        public string? NoSertifikat { get; set; }

        [Column("tgl_sertifikat")]
        public DateTime? TglSertifikat { get; set; }


        [Column("asuransi_id")]
        public int? AsuransiId { get; set; }

        [ForeignKey(nameof(AsuransiId))]
        public Asuransi? Asuransi { get; set; }


        [Column("no_polis")]
        public string? NoPolis { get; set; }

        [Column("tgl_polis")]
        public DateTime? TglPolis { get; set; }

        [Column("no_pk")]
        public string? NoPk { get; set; }


        [Column("nilai_tunggakan_pokok")]
        public double? TunggakanPokok70Persen { get; set; }

        [Column("nilai_tunggakan_bunga")]
        public double? TunggakanBunga70Persen { get; set; }

        [Column("catatan_polis")]
        public string? CatatanPolis { get; set; }

        [Column("keterangan")]
        public string? Keterangan { get; set; }

        [Column("nilai_klaim")]
        public double? NilaiKlaim { get; set; }

        [Column("nilai_klaim_dibayar")]
        public double? NilaiKlaimDibayar { get; set; }

        [Column("tgl_klaim_dibayar")]
        public DateTime? TglKlaimDibayar { get; set; }

        [Column("asuransi_sisa_klaim_id")]
        public int? AsuransiSisaKlaimId { get; set; }

        [ForeignKey(nameof(AsuransiSisaKlaimId))]
        public AsuransiSisaKlaim? AsuransiSisaKlaim { get; set; }

        [Column("baki_debit_klaim")]
        public double? BakiDebitKlaim { get; set; }

        [Column("catatan_klaim")]
        public string? CatatanKlaim { get; set; }


        [Column("permasalahan")]
        public string? Permasalahan { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusAsuransi? Status { get; set; }

        public virtual ICollection<InsuranceDocument>? Document { get; set; }

    }
}
