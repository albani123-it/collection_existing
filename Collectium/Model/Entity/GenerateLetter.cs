using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("generate_letter")]
    public class GenerateLetter
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("loan_id")]
        public int? LoanId { get; set; }

        [ForeignKey(nameof(LoanId))]
        public MasterLoan? Loan { get; set; }

        [Column("cabang")]
        [StringLength(50)]
        public string? Cabang { get; set; }

        [Column("cabang_alamat")]
        [StringLength(200)]
        public string? CabangAlamat { get; set; }

        [Column("cabang_telepon")]
        [StringLength(50)]
        public string? CabangTelp { get; set; }

        [Column("cabang_faks")]
        [StringLength(50)]
        public string? CabangFaks { get; set; }

        [Column("cabang_kota")]
        [StringLength(50)]
        public string? CabangKota { get; set; }

        [Column("type_letter")]
        [StringLength(50)]
        public string? TypeLetter { get; set; }

        [Column("no")]
        [StringLength(50)]
        public string? No { get; set; }

        [Column("tgl")]
        [StringLength(50)]
        public string? Tanggal{ get; set; }

        [Column("nama_nasabah")]
        [StringLength(200)]
        public string? NamaNasabah { get; set; }

        [Column("jumlah")]
        [StringLength(50)]
        public string? Jumlah { get; set; }

        [Column("terbilang")]
        [StringLength(200)]
        public string? Terbilang { get; set; }

        [Column("tgl_bayar")]
        [StringLength(50)]
        public string? TanggalBayar { get; set; }

        [Column("no_kredit")]
        [StringLength(50)]
        public string? NoKredit { get; set; }

        [Column("tgl_kredit")]
        [StringLength(50)]
        public string? TanggalKredit { get; set; }

        [Column("notaris")]
        [StringLength(50)]
        public string? Notaris { get; set; }

        [Column("notaris_di")]
        [StringLength(50)]
        public string? NotarisDi { get; set; }

        [Column("notaris_tgl")]
        [StringLength(50)]
        public string? NotarisTgl { get; set; }

        [Column("no_sp1")]
        [StringLength(50)]
        public string? NoSP1 { get; set; }

        [Column("tgl_sp1")]
        [StringLength(50)]
        public string? TglSp1 { get; set; }

        [Column("no_sp2")]
        [StringLength(50)]
        public string? NoSP2 { get; set; }

        [Column("tgl_sp2")]
        [StringLength(50)]
        public string? TglSp2 { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusGeneral? Status { get; set; }
    }
}
