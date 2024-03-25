using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Keyless]
    [Table("STG_DATA_JAMINAN")]
    public class STGDataJaminan
    {

        [Column("TANGGAL_PENGIKATAN_JAMINAN")]
        public DateTime? TANGGAL_PENGIKATAN_JAMINAN { get; set; }
        [Column("NO_PENGIKATAN_JAMINAN")]
        public string? NO_PENGIKATAN_JAMINAN { get; set; }
        [Column("NOMOR_HT")]
        public string? NOMOR_HT { get; set; }
        [Column("NILAI_HT")]
        public string? NILAI_HT { get; set; }
        [Column("NOMOR_FIDUSIA")]
        public string? NOMOR_FIDUSIA { get; set; }
        [Column("NILAI_FIDUSIA")]
        public string? NILAI_FIDUSIA { get; set; }
        [Column("NOMOR_CESSIE")]
        public string? NOMOR_CESSIE { get; set; }
        [Column("CU_CIF")]
        public string? CU_CIF { get; set; }
        [Column("NILAI_CESSIE")]
        public string? NILAI_CESSIE { get; set; }
        [Column("AKTA_GADAI")]
        public string? AKTA_GADAI { get; set; }
        [Column("NILAI_GADAI")]
        public string? NILAI_GADAI { get; set; }
        [Column("AKTA_PG")]
        public string? AKTA_PG { get; set; }
        [Column("NILAI_PG")]
        public string? NILAI_PG { get; set; }
        [Column("AKTA_CG")]
        public string? AKTA_CG { get; set; }
        [Column("NILAI_CG")]
        public string? NILAI_CG { get; set; }
        [Column("TANGGAL_PENETAPAN_ASET")]
        public DateTime? TANGGAL_PENETAPAN_ASET { get; set; }
        [Column("DIREKTORAT_PENGENDALI_ASET")]
        public string? DIREKTORAT_PENGENDALI_ASET { get; set; }
        [Column("JENIS_ASET")]
        public string? JENIS_ASET { get; set; }
        [Column("TIPE_ASET")]
        public string? TIPE_ASET { get; set; }
        [Column("PERUNTUKAN")]
        public string? PERUNTUKAN { get; set; }
        [Column("NAMA_ASET")]
        public string? NAMA_ASET { get; set; }
        [Column("ALAMAT")]
        public string? ALAMAT { get; set; }
        [Column("LATITUDE")]
        public string? LATITUDE { get; set; }
        [Column("LONGITUDE")]
        public string? LONGITUDE { get; set; }
        [Column("RT")]
        public string? RT { get; set; }
        [Column("RW")]
        public string? RW { get; set; }
        [Column("PROVINSI")]
        public string? PROVINSI { get; set; }
        [Column("KABUPATEN")]
        public string? KABUPATEN { get; set; }
        [Column("KECAMATAN")]
        public string? KECAMATAN { get; set; }
        [Column("KELURAHAN")]
        public string? KELURAHAN { get; set; }
        [Column("KODE_POS")]
        public string? KODE_POS { get; set; }
        [Column("TOTAL_SERTIFIKAT")]
        public string? TOTAL_SERTIFIKAT { get; set; }
        [Column("TIPE_SERTIFIKAT")]
        public string? TIPE_SERTIFIKAT { get; set; }
        [Column("NO_SERTIFIKAT")]
        public string? NO_SERTIFIKAT { get; set; }
        [Column("ATAS_NAMA_SERTIFIKAT")]
        public string? ATAS_NAMA_SERTIFIKAT { get; set; }
        [Column("TGL_EXP_SERTIFIKAT")]
        public DateTime? TGL_EXP_SERTIFIKAT { get; set; }
        [Column("TANGGAL_PBB_LAST_PAYMENT")]
        public DateTime? TANGGAL_PBB_LAST_PAYMENT { get; set; }
        [Column("NOP_PBB")]
        public string? NOP_PBB { get; set; }
        [Column("NIB")]
        public string? NIB { get; set; }
        [Column("PANJANG_BANGUNAN")]
        public double? PANJANG_BANGUNAN { get; set; }
        [Column("LEBAR_BANGUNAN")]
        public double? LEBAR_BANGUNAN { get; set; }
        [Column("LUAS_BANGUNAN")]
        public double? LUAS_BANGUNAN { get; set; }
        [Column("JML_LANTAI")]
        public int? JML_LANTAI { get; set; }
        [Column("JML_KAMAR_TIDUR")]
        public int? JML_KAMAR_TIDUR { get; set; }
        [Column("JML_KAMAR_MANDI")]
        public int? JML_KAMAR_MANDI { get; set; }
        [Column("JML_KAPASITAS_PARKIR")]
        public int? JML_KAPASITAS_PARKIR { get; set; }
        [Column("PANJANG_TANAH")]
        public double? PANJANG_TANAH { get; set; }
        [Column("LEBAR_TANAH")]
        public double? LEBAR_TANAH { get; set; }
        [Column("LUAS_TANAH")]
        public double? LUAS_TANAH { get; set; }
        [Column("HARGA_SEWA")]
        public double? HARGA_SEWA { get; set; }
        [Column("HARGA_JUAL")]
        public double? HARGA_JUAL { get; set; }
        [Column("HARGA_NJOP_TANAH")]
        public double? HARGA_NJOP_TANAH { get; set; }
        [Column("HARGA_NJOP_BANGUNAN")]
        public double? HARGA_NJOP_BANGUNAN { get; set; }
        [Column("NILAI_PASAR_WAJAR_INT")]
        public double? NILAI_PASAR_WAJAR_INT { get; set; }
        [Column("NILAI_LIKUIDASI")]
        public int? NILAI_LIKUIDASI { get; set; }
        [Column("NAMA_PENILAI_JAMINAN_INT")]
        public string? NAMA_PENILAI_JAMINAN_INT { get; set; }
        [Column("TANGGAL_NILAI_PASAR_WAJAR_INT")]
        public DateTime? TANGGAL_NILAI_PASAR_WAJAR_INT { get; set; }
        [Column("NILAI_PASAR_WAJAR_EXT")]
        public double? NILAI_PASAR_WAJAR_EXT { get; set; }
        [Column("NILAI_LIKUIDASI_EXT")]
        public double? NILAI_LIKUIDASI_EXT { get; set; }
        [Column("NAMA_KJPP")]
        public string? NAMA_KJPP { get; set; }
        [Column("TANGGAL_NILAI_PASAR_WAJAR_EXT")]
        public DateTime? TANGGAL_NILAI_PASAR_WAJAR_EXT { get; set; }
        [Column("ARAH_MATA_ANGIN")]
        public string? ARAH_MATA_ANGIN { get; set; }
        [Column("KONDISI_ASET")]
        public string? KONDISI_ASET { get; set; }
        [Column("STATUS_SERAH_TERIMA_KUNCI")]
        public string? STATUS_SERAH_TERIMA_KUNCI { get; set; }
        [Column("TANGGAL_SERAH_TERIMA_KUNCI")]
        public string? TANGGAL_SERAH_TERIMA_KUNCI { get; set; }
        [Column("CABANG_PENGELOLA_ASET")]
        public string? CABANG_PENGELOLA_ASET { get; set; }
        [Column("ANALISA_KONDISI_FISIK_ASET")]
        public string? ANALISA_KONDISI_FISIK_ASET { get; set; }
        [Column("REKOMENDASI")]
        public string? REKOMENDASI { get; set; }
        [Column("UPAYA_PENYELESAIAN_ASET")]
        public string? UPAYA_PENYELESAIAN_ASET { get; set; }

        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }
    }
}
