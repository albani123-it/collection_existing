using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{

    [Table("restructure")]

    public class Restructure
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

        [Column("principal_pembayaran")]
        public double? PrincipalPembayaran { get; set; }

        [Column("margin_pembayaran")]
        public double? MarginPembayaran { get; set; }

        [Column("principal_pinalty")]
        public double? PrincipalPinalty { get; set; }

        [Column("margin_pinalty")]
        public double? MarginPinalty { get; set; }

        [Column("tgl_jatuh_tempo_baru")]
        public DateTime? TglJatuhTempoBaru { get; set; }

        [Column("keterangan")]
        public string? Keterangan { get; set; }

        [Column("grace_periode")]
        public int? GracePeriode { get; set; }

        [Column("pengurangan_nilai_margin")]
        public int? PenguranganNilaiMargin { get; set; }

        [Column("tgl_awal_periode_diskon")]
        public DateTime? TglAwalPeriodeDiskon { get; set; }

        [Column("tgl_akhir_periode_diskon")]
        public DateTime? TglAkhirPeriodeDiskon { get; set; }

        [Column("periode_diskon")]
        public int? PeriodeDiskon { get; set; }

        [Column("value_date")]
        public DateTime? ValueDate { get; set; }

        [Column("disc_tunggakan_margin")]
        public double? DiskonTunggakanMargin { get; set; }

        [Column("disc_tunggakan_denda")]
        public double? DiskonTunggakanDenda { get; set; }

        [Column("margin")]
        public double? Margin { get; set; }

        [Column("denda")]
        public double? Denda { get; set; }

        [Column("margin_amount")]
        public double? MarginAmount { get; set; }

        [Column("total_diskon_margin")]
        public double? TotalDiskonMargin { get; set; }

        [Column("pola_restruk_id")]
        public int? PolaRestrukId { get; set; }

        [ForeignKey(nameof(PolaRestrukId))]
        public PolaRestruktur? PolaRestruktur { get; set; }

        [Column("pembayaran_gp_id")]
        public int? PembayaranGpId { get; set; }

        [ForeignKey(nameof(PembayaranGpId))]
        public PembayaranGp? PembayaranGp { get; set; }


        [Column("jenis_pengurangan_id")]
        public int? JenisPenguranganId { get; set; }

        [ForeignKey(nameof(JenisPenguranganId))]
        public JenisPengurangan? JenisPengurangan { get; set; }


        [Column("permasalahan")]
        public string? Permasalahan { get; set; }

        [Column("createby_id")]
        public int? CreateById { get; set; }

        [ForeignKey(nameof(CreateById))]
        public User? CreateBy { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("checkby_id")]
        public int? CheckById { get; set; }

        [ForeignKey(nameof(CheckById))]
        public User? CheckBy { get; set; }

        [Column("check_date")]
        public DateTime? CheckDate { get; set; }

        [Column("approveby_id")]
        public int? ApproveById { get; set; }

        [ForeignKey(nameof(ApproveById))]
        public User? ApproveBy { get; set; }

        [Column("approve_date")]
        public DateTime? ApproveDate { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusRestruktur? Status { get; set; }

        public virtual ICollection<RestructureDocument>? Document { get; set; }

        public virtual ICollection<RestructureCashFlow>? CashFlow { get; set; }

        //[Column("no")]
        //public string? No { get; set; }
        //        public double? MarginPembiayaan { get; set; }
        //public string? Akad { get; set; }
        //public string? PaymentMethod { get; set; }
        //public string? PaymentSource { get; set; }
        //public string? ProcInst { get; set; }
        //public string? ProductCode { get; set; }
        //public string? Status { get; set; }
        //public DateTime? DisbursementDate { get; set; }
        //public int? FidStock { get; set; }
        //public int? FidStockItem { get; set; }
        //public string? FidVendor { get; set; }
        //public string? AccountOfficerName { get; set; }
        //public DateTime? ApplicationDate { get; set; }
        //public string? LegalOpinion { get; set; }
        //public string? LegalRecommendation { get; set; }
        //public string? SpkNo { get; set; }
        //public string? BlacklistNote { get; set; }


        //public int? DisposableIncome { get; set; }
        //public int? EducationCost { get; set; }
        //public int? HouseRent { get; set; }
        //public int? HouseholdCost { get; set; }
        //public int? TotalLivingCost { get; set; }
        //public int? TransportationCost { get; set; }
        //public int? UtilitiesCost { get; set; }
        //public string? LoanPurpose { get; set; }
        //public string? OpiniCa { get; set; }
        //public string? IsBacktoback { get; set; }
        //public string? BlacklistStatus { get; set; }
        //public int? OtherCost { get; set; }
        //public string? LegalNotes { get; set; }
        //public int? FicCustomer { get; set; }
        //public string? LoanType { get; set; }
        //public string? MarketingCode { get; set; }
        //public string? AnalisName { get; set; }
        //public string? RekBayarBunga { get; set; }
        //public string? RekBayarPokok { get; set; }

        //public string? JualBeli { get; set; }
        //public string? CaName { get; set; }
        //public string? OlNo { get; set; }
        //public string? FidAccountOfficer { get; set; }
        //public string? FidChannel { get; set; }
        //public string? FidProgram { get; set; }
        //public string? EntryType { get; set; }
        //public string? ProductCname { get; set; }

        //public int? FidInstansi { get; set; }
        //public int? FidKolektor { get; set; }
        //public string? NoSuratPenawaran { get; set; }
        //public int? CustomerAppealed { get; set; }
        //public int? FidDisbInstr { get; set; }
        //public int? FeeCharged { get; set; }
        //public string? ScoringCode { get; set; }
        //public DateTime? OfferingLetterDate { get; set; }
        //public string? OfferingLetterNo { get; set; }
        //public DateTime? ClosingDate { get; set; }
        //public int? PersetujuanPrinsip { get; set; }
        //public string? Ideb { get; set; }
        //public int? TotalOutstanding { get; set; }
        //public int? TotalPlafon { get; set; }
        //public int? FkTasklistPembiayaan { get; set; }
        //public string? Name { get; set; }
        //public int? Tenor { get; set; }
        //public int? TotalMargin { get; set; }
        //public int? Installment { get; set; }
        //public int? BankPrice { get; set; }
        //public int? ResTotal { get; set; }
        //public int? ResPrincipal { get; set; }
        //public int? ResMargin { get; set; }
        //public int? DuePrincipal { get; set; }
        //public int? DueMargin { get; set; }
        //public int? Fine { get; set; }
        //public DateTime? RkPosition { get; set; }
        //public string? BiKol { get; set; }
        //public string? CountResc { get; set; }
        //public string? Arrears { get; set; }
        //public string? BrachCodeArea { get; set; }
        //public string? NoLoan { get; set; }
        //public string? NoFacility { get; set; }
        //public DateTime? StartDate { get; set; }
        //public string? NamaNasabah { get; set; }
        //public string? BrachCode { get; set; }
        //public string? Cif { get; set; }
        //public DateTime? TglJatuhTempo { get; set; }
        //public int? PrincipalPembiayaan { get; set; }

        //public int? PrincipalOutstanding { get; set; }
        //public int? MarginOutstanding { get; set; }
        //public int? PrincipalTunggakan { get; set; }
        //public int? MarginTunggakan { get; set; }

        //public int? RepaymentAmountBaru { get; set; }
        //public int? RepaymentAmountLama { get; set; }
        //public int? JangkaWaktu { get; set; }
        //public int? MarginBagiHasil { get; set; }

        //public DateTime? RepaymentDate { get; set; }
        //public DateTime? RepaymentNextDate { get; set; }
        //public int? Repayment { get; set; }
        //public int? OriginalAmount { get; set; }
        //public int? OustandingAmount { get; set; }
        //public int? OriginalMargin { get; set; }
        //public int? OustandingMargin { get; set; }

        //public string? TipeLoan { get; set; }
        //public int? PrincipalMargin { get; set; }
        //public int? MarginMargin { get; set; }
        //public int? PrincipalDenda { get; set; }
        //public int? MarginDenda { get; set; }

        //public string? BrachCodeAreaPusat { get; set; }
        //public string? NoLoanPusat { get; set; }
        //public string? NoFacilityPusat { get; set; }
        //public DateTime? StartDatePusat { get; set; }
        //public string? NamaNasabahPusat { get; set; }
        //public string? BrachCodePusat { get; set; }
        //public string? CifPusat { get; set; }
        //public DateTime? TglJatuhTempoPusat { get; set; }
        //public int? PrincipalPembiayaanPusat { get; set; }
        //public int? MarginPembiayaanPusat { get; set; }
        //public int? PrincipalPembayaranPusat { get; set; }
        //public int? MarginPembayaranPusat { get; set; }
        //public int? PrincipalOutstandingPusat { get; set; }
        //public int? MarginOutstandingPusat { get; set; }
        //public int? PrincipalTunggakanPusat { get; set; }
        //public int? MarginTunggakanPusat { get; set; }
        //public int? PrincipalPinaltyPusat { get; set; }
        //public int? MarginPinaltyPusat { get; set; }
        //public DateTime? TglJatuhTempoBaruPusat { get; set; }
        //public string? KeteranganPusat { get; set; }
        //public int? RepaymentAmountBaruPusat { get; set; }
        //public int? RepaymentAmountLamaPusat { get; set; }
        //public int? JangkaWaktuPusat { get; set; }
        //public int? MarginBagiHasilPusat { get; set; }
        //public int? GracePeriodePusat { get; set; }
        //public DateTime? RepaymentDatePusat { get; set; }
        //public DateTime? RepaymentNextDatePusat { get; set; }
        //public int? RepaymentPusat { get; set; }
        //public int? OriginalAmountPusat { get; set; }
        //public int? OustandingAmountPusat { get; set; }
        //public int? OriginalMarginPusat { get; set; }
        //public int? OustandingMarginPusat { get; set; }
        //public int? PenguranganNilaiMarginPusat { get; set; }
        //public DateTime? TglAwalPeriodeDiskonPusat { get; set; }
        //public DateTime? TglAkhirPeriodeDiskonPusat { get; set; }
        //public int? PeriodeDiskonPusat { get; set; }
        //public string? TipeLoanPusat { get; set; }
        //public int? PrincipalMarginPusat { get; set; }
        //public int? MarginMarginPusat { get; set; }
        //public int? PrincipalDendaPusat { get; set; }
        //public int? MarginDendaPusat { get; set; }
        //public DateTime? ValueDatePusat { get; set; }
        //public int? DiskonTunggakanMarginPusat { get; set; }
        //public int? DiskonTunggakanDendaPusat { get; set; }
        //public int? MarginPusat { get; set; }
        //public int? DendaPusat { get; set; }
        //public int? MarginAmountPusat { get; set; }
        //public int? TotalDiskonMarginPusat { get; set; }
        //public int? FkCsrCustomer { get; set; }
        //public string? FkMstCsrProduk { get; set; }
        //public int? FkCsrFacility { get; set; }
        //public int? FkCsrOccupation { get; set; }
        //public int? MetodeRestruk { get; set; }
        //public int? PolaRestrukPusat { get; set; }
        //public int? MetodeRestrukPusat { get; set; }
        //public int? PembayaranGpPusat { get; set; }
        //public int? JenisPenguranganPusat { get; set; }
        //public int? ExecutionStatus { get; set; }
    }
}
