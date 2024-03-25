using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_data_jaminan")]
    public class STGDataJaminanPg
    {

        [Column("tanggal_pengikatan_jaminan")]
        public DateTime? TANGGAL_PENGIKATAN_JAMINAN { get; set; }
        [Column("no_pengikatan_jaminan")]
        public string? NO_PENGIKATAN_JAMINAN { get; set; }
        [Column("nomor_ht")]
        public string? NOMOR_HT { get; set; }
        [Column("nilai_ht")]
        public string? NILAI_HT { get; set; }
        [Column("nomor_fidusia")]
        public string? NOMOR_FIDUSIA { get; set; }
        [Column("nilai_fidusia")]
        public string? NILAI_FIDUSIA { get; set; }
        [Column("nomor_cessie")]
        public string? NOMOR_CESSIE { get; set; }
        [Column("cu_cif")]
        public string? CU_CIF { get; set; }
        [Column("nilai_cessie")]
        public string? NILAI_CESSIE { get; set; }
        [Column("akta_gadai")]
        public string? AKTA_GADAI { get; set; }
        [Column("nilai_gadai")]
        public string? NILAI_GADAI { get; set; }
        [Column("akta_pg")]
        public string? AKTA_PG { get; set; }
        [Column("nilai_pg")]
        public string? NILAI_PG { get; set; }
        [Column("akta_cg")]
        public string? AKTA_CG { get; set; }
        [Column("nilai_cg")]
        public string? NILAI_CG { get; set; }
        [Column("tanggal_penetapan_aset")]
        public DateTime? TANGGAL_PENETAPAN_ASET { get; set; }
        [Column("direktorat_pengendali_aset")]
        public string? DIREKTORAT_PENGENDALI_ASET { get; set; }
        [Column("jenis_aset")]
        public string? JENIS_ASET { get; set; }
        [Column("tipe_aset")]
        public string? TIPE_ASET { get; set; }
        [Column("peruntukan")]
        public string? PERUNTUKAN { get; set; }
        [Column("nama_aset")]
        public string? NAMA_ASET { get; set; }
        [Column("alamat")]
        public string? ALAMAT { get; set; }
        [Column("latitude")]
        public string? LATITUDE { get; set; }
        [Column("longitude")]
        public string? LONGITUDE { get; set; }
        [Column("rt")]
        public string? RT { get; set; }
        [Column("rw")]
        public string? RW { get; set; }
        [Column("provinsi")]
        public string? PROVINSI { get; set; }
        [Column("kabupaten")]
        public string? KABUPATEN { get; set; }
        [Column("kecamatan")]
        public string? KECAMATAN { get; set; }
        [Column("kelurahan")]
        public string? KELURAHAN { get; set; }
        [Column("kode_pos")]
        public string? KODE_POS { get; set; }
        [Column("total_sertifikat")]
        public string? TOTAL_SERTIFIKAT { get; set; }
        [Column("tipe_sertifikat")]
        public string? TIPE_SERTIFIKAT { get; set; }
        [Column("no_sertifikat")]
        public string? NO_SERTIFIKAT { get; set; }
        [Column("atas_nama_sertifikat")]
        public string? ATAS_NAMA_SERTIFIKAT { get; set; }
        [Column("tgl_exp_sertifikat")]
        public DateTime? TGL_EXP_SERTIFIKAT { get; set; }
        [Column("tanggal_pbb_last_payment")]
        public DateTime? TANGGAL_PBB_LAST_PAYMENT { get; set; }
        [Column("nop_pbb")]
        public string? NOP_PBB { get; set; }
        [Column("nib")]
        public string? NIB { get; set; }
        [Column("panjang_bangunan")]
        public double? PANJANG_BANGUNAN { get; set; }
        [Column("lebar_bangunan")]
        public double? LEBAR_BANGUNAN { get; set; }
        [Column("luas_bangunan")]
        public double? LUAS_BANGUNAN { get; set; }
        [Column("jml_lantai")]
        public int? JML_LANTAI { get; set; }
        [Column("jml_kamar_tidur")]
        public int? JML_KAMAR_TIDUR { get; set; }
        [Column("jml_kamar_mandi")]
        public int? JML_KAMAR_MANDI { get; set; }
        [Column("jml_kapasitas_parkir")]
        public int? JML_KAPASITAS_PARKIR { get; set; }
        [Column("panjang_tanah")]
        public double? PANJANG_TANAH { get; set; }
        [Column("lebar_tanah")]
        public double? LEBAR_TANAH { get; set; }
        [Column("luas_tanah")]
        public double? LUAS_TANAH { get; set; }
        [Column("harga_sewa")]
        public double? HARGA_SEWA { get; set; }
        [Column("harga_jual")]
        public double? HARGA_JUAL { get; set; }
        [Column("harga_njop_tanah")]
        public double? HARGA_NJOP_TANAH { get; set; }
        [Column("harga_njop_bangunan")]
        public double? HARGA_NJOP_BANGUNAN { get; set; }
        [Column("nilai_pasar_wajar_int")]
        public double? NILAI_PASAR_WAJAR_INT { get; set; }
        [Column("nilai_likuidasi")]
        public int? NILAI_LIKUIDASI { get; set; }
        [Column("nama_penilai_jaminan_int")]
        public string? NAMA_PENILAI_JAMINAN_INT { get; set; }
        [Column("tanggal_nilai_pasar_wajar_int")]
        public DateTime? TANGGAL_NILAI_PASAR_WAJAR_INT { get; set; }
        [Column("nilai_pasar_wajar_ext")]
        public double? NILAI_PASAR_WAJAR_EXT { get; set; }
        [Column("nilai_likuidasi_ext")]
        public double? NILAI_LIKUIDASI_EXT { get; set; }
        [Column("nama_kjpp")]
        public string? NAMA_KJPP { get; set; }
        [Column("tanggal_nilai_pasar_wajar_ext")]
        public DateTime? TANGGAL_NILAI_PASAR_WAJAR_EXT { get; set; }
        [Column("arah_mata_angin")]
        public string? ARAH_MATA_ANGIN { get; set; }
        [Column("kondisi_aset")]
        public string? KONDISI_ASET { get; set; }
        [Column("status_serah_terima_kunci")]
        public string? STATUS_SERAH_TERIMA_KUNCI { get; set; }
        [Column("tanggal_serah_terima_kunci")]
        public string? TANGGAL_SERAH_TERIMA_KUNCI { get; set; }
        [Column("cabang_pengelola_aset")]
        public string? CABANG_PENGELOLA_ASET { get; set; }
        [Column("analisa_kondisi_fisik_aset")]
        public string? ANALISA_KONDISI_FISIK_ASET { get; set; }
        [Column("rekomendasi")]
        public string? REKOMENDASI { get; set; }
        [Column("updaya_penyelesaian_aset")]
        public string? UPAYA_PENYELESAIAN_ASET { get; set; }

    }
}
