using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collectium.Migrations
{
    public partial class two : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "generic_param",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generic_param", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_BIAYA_LAIN",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    ACC_NO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tanggal_Biaya_Lain = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NAMA_Biaya_Lain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nominal_Biaya_Lain = table.Column<double>(type: "float", nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_BIAYA_LAIN", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_KODEAO",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    ACC_NO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KODE_AO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_AO = table.Column<DateTime>(type: "datetime2", nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_KODEAO", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_KOMITEKREDIT",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    ACC_NO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NOMOR_PK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_PK = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KOMITE01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE06 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_KOMITEKREDIT", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_KSL",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    ACC_NO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tanggal_KSL = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NAMA_KSL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Saldo_KSL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_KSL", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_PK",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    ACC_NO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_PK = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NOMOR_PK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAMA_NOTARIS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAMA_LEGAL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_PK", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LOAN_TAGIHAN_LAIN",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    ACC_NO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tanggal_TL = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NAMA_TL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nominal_TL = table.Column<double>(type: "float", nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOAN_TAGIHAN_LAIN", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "STG_BRANCH",
                columns: table => new
                {
                    COMPANY_CODE = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COMPANY_NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAME_ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MNEMONIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUSTOMER_COMPANY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CUSTOMER_MNEMONIC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    COMPANY_GROUP = table.Column<int>(type: "int", nullable: true),
                    CURR_NO = table.Column<int>(type: "int", nullable: true),
                    DATE_TIME = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CO_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_BRANCH", x => new { x.COMPANY_CODE, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "STG_CUSTOMER_NASABAH",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CU_REF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AS_OF_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CU_CIF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NO_REKENING = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_FIRSTNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CU_POB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_IDTYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_IDNUM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NO_NPWP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_GENDER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_MARITAL_STATUS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_NATIONALITY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_INCOMETYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_INCOME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PEKERJAAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_OCCUPATION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_COMPANYNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ALAMAT_PERUSAHAAN_USAHA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BIDANG_USAHA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_EMAIL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_ADDR1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_ADDR2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_RT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_RW = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_KEL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_KEC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_CITY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_PROVINSI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_ADDR_STAT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_ZIP_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KELUARGA_TIDAK_SERUMAH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HUBUNGAN_KELUARGA_TIDAK_SERUMAH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NO_TLP_KONTAK_DARURAT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ALAMAT_KONTAK_DARURAT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NO_TLP_KANTOR_USAHA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_PHNNUM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_HPNUM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BRANCH_CODE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_CUSTOMER_NASABAH", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "STG_CUSTOMER_PHONE",
                columns: table => new
                {
                    CIF = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PHONE = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_CUSTOMER_PHONE", x => new { x.CIF, x.STG_DATE, x.PHONE });
                });

            migrationBuilder.CreateTable(
                name: "STG_DATA_JAMINAN",
                columns: table => new
                {
                    CU_CIF = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TANGGAL_PENGIKATAN_JAMINAN = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NO_PENGIKATAN_JAMINAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NOMOR_HT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NILAI_HT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NOMOR_FIDUSIA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NILAI_FIDUSIA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NOMOR_CESSIE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NILAI_CESSIE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AKTA_GADAI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NILAI_GADAI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AKTA_PG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NILAI_PG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AKTA_CG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NILAI_CG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_PENETAPAN_ASET = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DIREKTORAT_PENGENDALI_ASET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JENIS_ASET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TIPE_ASET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PERUNTUKAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAMA_ASET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ALAMAT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LATITUDE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LONGITUDE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RW = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PROVINSI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KABUPATEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KECAMATAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KELURAHAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KODE_POS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TOTAL_SERTIFIKAT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TIPE_SERTIFIKAT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NO_SERTIFIKAT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ATAS_NAMA_SERTIFIKAT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TGL_EXP_SERTIFIKAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TANGGAL_PBB_LAST_PAYMENT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NOP_PBB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NIB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PANJANG_BANGUNAN = table.Column<double>(type: "float", nullable: true),
                    LEBAR_BANGUNAN = table.Column<double>(type: "float", nullable: true),
                    LUAS_BANGUNAN = table.Column<double>(type: "float", nullable: true),
                    JML_LANTAI = table.Column<int>(type: "int", nullable: true),
                    JML_KAMAR_TIDUR = table.Column<int>(type: "int", nullable: true),
                    JML_KAMAR_MANDI = table.Column<int>(type: "int", nullable: true),
                    JML_KAPASITAS_PARKIR = table.Column<int>(type: "int", nullable: true),
                    PANJANG_TANAH = table.Column<double>(type: "float", nullable: true),
                    LEBAR_TANAH = table.Column<double>(type: "float", nullable: true),
                    LUAS_TANAH = table.Column<double>(type: "float", nullable: true),
                    HARGA_SEWA = table.Column<double>(type: "float", nullable: true),
                    HARGA_JUAL = table.Column<double>(type: "float", nullable: true),
                    HARGA_NJOP_TANAH = table.Column<double>(type: "float", nullable: true),
                    HARGA_NJOP_BANGUNAN = table.Column<double>(type: "float", nullable: true),
                    NILAI_PASAR_WAJAR_INT = table.Column<double>(type: "float", nullable: true),
                    NILAI_LIKUIDASI = table.Column<int>(type: "int", nullable: true),
                    NAMA_PENILAI_JAMINAN_INT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_NILAI_PASAR_WAJAR_INT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NILAI_PASAR_WAJAR_EXT = table.Column<double>(type: "float", nullable: true),
                    NILAI_LIKUIDASI_EXT = table.Column<double>(type: "float", nullable: true),
                    NAMA_KJPP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_NILAI_PASAR_WAJAR_EXT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ARAH_MATA_ANGIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KONDISI_ASET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STATUS_SERAH_TERIMA_KUNCI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_SERAH_TERIMA_KUNCI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CABANG_PENGELOLA_ASET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ANALISA_KONDISI_FISIK_ASET = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REKOMENDASI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UPAYA_PENYELESAIAN_ASET = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_DATA_JAMINAN", x => new { x.CU_CIF, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "STG_DATA_KREDIT",
                columns: table => new
                {
                    ACC_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KATAGORI_DEBITUR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OBLIGOR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CABANG_ASAL_DEBITUR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE_PEMUTUS_KREDIT_X = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_PK_X = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NO_PK_X = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAMA_NOTARIS_X = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STAFF_LEGAL_X = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KODE_ACCOUNT_OFFICER_X = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACCOUNT_OFFICER_X = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_MULAI_MENUNGGAK = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SEGMENTASI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LOAN_NUMBER = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PLAFON = table.Column<double>(type: "float", nullable: true),
                    LIMIT_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CU_CIF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISO_CURRENCY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FASILITAS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OUTSTANDING = table.Column<double>(type: "float", nullable: true),
                    MATURITY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BOOKING_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SISA_SETORAN = table.Column<int>(type: "int", nullable: true),
                    TENOR = table.Column<int>(type: "int", nullable: true),
                    PRINCIPAL_USD = table.Column<double>(type: "float", nullable: true),
                    PRINCIPAL_IDR = table.Column<double>(type: "float", nullable: true),
                    ASURANSI_ID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    INSURANCE_TYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TOTAL_PENARIKAN = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_DATA_KREDIT", x => new { x.ACC_NO, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "STG_DATA_LOAN_BIAYA_LAIN",
                columns: table => new
                {
                    ACC_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tanggal_Biaya_Lain = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NAMA_Biaya_Lain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nominal_Biaya_Lain = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_DATA_LOAN_BIAYA_LAIN", x => new { x.ACC_NO, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "STG_DATA_LOAN_KODEAO",
                columns: table => new
                {
                    ACC_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KODE_AO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_AO = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_DATA_LOAN_KODEAO", x => new { x.ACC_NO, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "STG_DATA_LOAN_KOMITEKREDIT",
                columns: table => new
                {
                    ACC_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NOMOR_PK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TANGGAL_PK = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KOMITE01 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE02 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE03 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE04 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE05 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KOMITE06 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_DATA_LOAN_KOMITEKREDIT", x => new { x.ACC_NO, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "STG_DATA_LOAN_KSL",
                columns: table => new
                {
                    ACC_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tanggal_KSL = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NAMA_KSL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Saldo_KSL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_DATA_LOAN_KSL", x => new { x.ACC_NO, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "STG_DATA_LOAN_PK",
                columns: table => new
                {
                    ACC_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TANGGAL_PK = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NOMOR_PK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAMA_NOTARIS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NAMA_LEGAL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_DATA_LOAN_PK", x => new { x.ACC_NO, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "STG_DATA_LOAN_TAGIHAN_LAIN",
                columns: table => new
                {
                    ACC_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tanggal_TL = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NAMA_TL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nominal_TL = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_DATA_LOAN_TAGIHAN_LAIN", x => new { x.ACC_NO, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "STG_LOAN_DETAIL",
                columns: table => new
                {
                    ACC_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SUKU_BUNGA = table.Column<double>(type: "float", nullable: true),
                    INSTALLMENT = table.Column<double>(type: "float", nullable: true),
                    PRINCIPAL_DUE = table.Column<double>(type: "float", nullable: true),
                    INTEREST_DUE = table.Column<double>(type: "float", nullable: true),
                    BUNGA_DENDA = table.Column<double>(type: "float", nullable: true),
                    PENALTY_DENDA = table.Column<double>(type: "float", nullable: true),
                    TAGIHAN_LAINNYA_X = table.Column<double>(type: "float", nullable: true),
                    BIAYA_LAINNYA_X = table.Column<double>(type: "float", nullable: true),
                    SUB_TOTAL = table.Column<double>(type: "float", nullable: true),
                    KSL_X = table.Column<double>(type: "float", nullable: true),
                    TOTAL_KEWAJIBAN = table.Column<double>(type: "float", nullable: true),
                    LAST_PAYMENT_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PRINCIPAL_PAID = table.Column<double>(type: "float", nullable: true),
                    INTEREST_PAID = table.Column<double>(type: "float", nullable: true),
                    CHARGES_PAID = table.Column<double>(type: "float", nullable: true),
                    OUTSTANDING = table.Column<double>(type: "float", nullable: true),
                    TOT_PAID = table.Column<double>(type: "float", nullable: true),
                    SALDO_AKHIR = table.Column<double>(type: "float", nullable: true),
                    DPD = table.Column<int>(type: "int", nullable: true),
                    KOLEKTIBILITY = table.Column<int>(type: "int", nullable: true),
                    SEGMENT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PRODUCT_LOAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    START_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MATURITY_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PLAFOND = table.Column<double>(type: "float", nullable: true),
                    payin_account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    file_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_LOAN_DETAIL", x => new { x.ACC_NO, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "STG_SMSREMINDER",
                columns: table => new
                {
                    CU_CIF = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ACC_NO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NO_HP = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NAMA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DUE_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    REMINDER_DAY = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STG_SMSREMINDER", x => new { x.CU_CIF, x.ACC_NO, x.NO_HP, x.STG_DATE });
                });

            migrationBuilder.CreateTable(
                name: "account_distribution",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dpd = table.Column<int>(type: "int", nullable: true),
                    dpd_min = table.Column<int>(type: "int", nullable: true),
                    dpd_max = table.Column<int>(type: "int", nullable: true),
                    max_ptp = table.Column<int>(type: "int", nullable: true),
                    core_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_distribution", x => x.id);
                    table.ForeignKey(
                        name: "FK_account_distribution_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "action",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    act_desc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    code_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action", x => x.id);
                    table.ForeignKey(
                        name: "FK_action_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "area",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    core_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area", x => x.id);
                    table.ForeignKey(
                        name: "FK_area_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "branch_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branch_type", x => x.id);
                    table.ForeignKey(
                        name: "FK_branch_type_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "call_script",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    cs_desc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    accd_min = table.Column<int>(type: "int", nullable: true),
                    accd_max = table.Column<int>(type: "int", nullable: true),
                    cs_script = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_call_script", x => x.id);
                    table.ForeignKey(
                        name: "FK_call_script_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "notif_content",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    content = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    day = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notif_content", x => x.id);
                    table.ForeignKey(
                        name: "FK_notif_content_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.id);
                    table.ForeignKey(
                        name: "FK_permission_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "reason",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    is_dc = table.Column<int>(type: "int", nullable: true),
                    is_fc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reason", x => x.id);
                    table.ForeignKey(
                        name: "FK_reason_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfcounter",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    value = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfcounter", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfcounter_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfcustomertype",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    desc = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    core_code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfcustomertype", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfcustomertype_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfgender",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    desc = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    core_code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfgender", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfgender_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfglobal",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    value = table.Column<string>(type: "varchar(3000)", unicode: false, maxLength: 3000, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfglobal", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfglobal_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfidtype",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idtype_code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    idtype_desc = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    core_code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfidtype", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfidtype_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfincometype",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    desc = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    code_code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfincometype", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfincometype_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfmarital",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    desc = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    core_code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfmarital", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfmarital_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfnationality",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    desc = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    core_code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfnationality", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfnationality_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfoccupation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    desc = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    code_code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfoccupation", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfoccupation_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfproduct_segment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    desc = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    code_code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfproduct_segment", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfproduct_segment_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfprovinsi",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfprovinsi", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfprovinsi_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfresult",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rl_code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    rl_desc = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    is_dc = table.Column<int>(type: "int", nullable: true),
                    is_fc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfresult", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfresult_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "account_distribution_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    account_distribution_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dpd = table.Column<int>(type: "int", nullable: true),
                    dpd_min = table.Column<int>(type: "int", nullable: true),
                    dpd_max = table.Column<int>(type: "int", nullable: true),
                    max_ptp = table.Column<int>(type: "int", nullable: true),
                    core_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_distribution_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_account_distribution_req_account_distribution_account_distribution_id",
                        column: x => x.account_distribution_id,
                        principalTable: "account_distribution",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_account_distribution_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "branch",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    branch_type_id = table.Column<int>(type: "int", nullable: true),
                    prd_segment_id = table.Column<int>(type: "int", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    addr1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    addr2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    addr3 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    zipcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    area_id = table.Column<int>(type: "int", nullable: true),
                    core_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    pic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    norek = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    amount = table.Column<double>(type: "float", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branch", x => x.id);
                    table.ForeignKey(
                        name: "FK_branch_area_area_id",
                        column: x => x.area_id,
                        principalTable: "area",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_branch_branch_type_branch_type_id",
                        column: x => x.branch_type_id,
                        principalTable: "branch_type",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_branch_rfproduct_segment_prd_segment_id",
                        column: x => x.prd_segment_id,
                        principalTable: "rfproduct_segment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_branch_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfproduct",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    desc = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    core_code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    prd_segment_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfproduct", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfproduct_rfproduct_segment_prd_segment_id",
                        column: x => x.prd_segment_id,
                        principalTable: "rfproduct_segment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_rfproduct_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfkabupaten",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    kode_wil_sikp = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    provinsi_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfkabupaten", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfkabupaten_rfprovinsi_provinsi_id",
                        column: x => x.provinsi_id,
                        principalTable: "rfprovinsi",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_rfkabupaten_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "action_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    action_id = table.Column<int>(type: "int", nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action_group", x => x.id);
                    table.ForeignKey(
                        name: "FK_action_group_action_action_id",
                        column: x => x.action_id,
                        principalTable: "action",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_action_group_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_action_group_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "role_permission",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false),
                    permission_id = table.Column<int>(type: "int", nullable: false),
                    save = table.Column<int>(type: "int", nullable: true),
                    read = table.Column<int>(type: "int", nullable: true),
                    update = table.Column<int>(type: "int", nullable: true),
                    delete = table.Column<int>(type: "int", nullable: true),
                    approve = table.Column<int>(type: "int", nullable: true),
                    assign = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permission", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "FK_role_permission_permission_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permission",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permission_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_req_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_role_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    admin = table.Column<int>(type: "int", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fcm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: true),
                    active_branch_id = table.Column<int>(type: "int", nullable: true),
                    tel_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tel_device = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pass_device = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fail = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "area_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    area_id = table.Column<int>(type: "int", nullable: true),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    adesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_area_req_area_area_id",
                        column: x => x.area_id,
                        principalTable: "area",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_area_req_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_area_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "branch_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    branch_type_id = table.Column<int>(type: "int", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    addr1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    addr2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    addr3 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    zipcode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    area_id = table.Column<int>(type: "int", nullable: true),
                    branch_cco_id = table.Column<int>(type: "int", nullable: true),
                    core_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    pic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    norek = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    amount = table.Column<double>(type: "float", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branch_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_branch_req_area_area_id",
                        column: x => x.area_id,
                        principalTable: "area",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_branch_req_branch_branch_cco_id",
                        column: x => x.branch_cco_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_branch_req_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_branch_req_branch_type_branch_type_id",
                        column: x => x.branch_type_id,
                        principalTable: "branch_type",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_branch_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "counter",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    counter_type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    year = table.Column<int>(type: "int", nullable: true),
                    month = table.Column<int>(type: "int", nullable: true),
                    ctr = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_counter", x => x.id);
                    table.ForeignKey(
                        name: "FK_counter_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_counter_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "doc_signature",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    doc_code = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    doc_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doc_signature", x => x.id);
                    table.ForeignKey(
                        name: "FK_doc_signature_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "distr_rule",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dpd_min = table.Column<int>(type: "int", nullable: true),
                    dpd_max = table.Column<int>(type: "int", nullable: true),
                    kol_min = table.Column<int>(type: "int", nullable: true),
                    kol_max = table.Column<int>(type: "int", nullable: true),
                    tungg_min = table.Column<double>(type: "float", nullable: true),
                    tungg_max = table.Column<double>(type: "float", nullable: true),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_distr_rule", x => x.id);
                    table.ForeignKey(
                        name: "FK_distr_rule_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_distr_rule_rfproduct_product_id",
                        column: x => x.product_id,
                        principalTable: "rfproduct",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_distr_rule_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfkecamatan",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    kabupaten_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfkecamatan", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfkecamatan_rfkabupaten_kabupaten_id",
                        column: x => x.kabupaten_id,
                        principalTable: "rfkabupaten",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_rfkecamatan_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "action_group_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    action_group_id = table.Column<int>(type: "int", nullable: true),
                    action_id = table.Column<int>(type: "int", nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    req_user_id = table.Column<int>(type: "int", nullable: true),
                    approve_user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action_group_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_action_group_req_action_action_id",
                        column: x => x.action_id,
                        principalTable: "action",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_action_group_req_action_group_action_group_id",
                        column: x => x.action_group_id,
                        principalTable: "action_group",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_action_group_req_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_action_group_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_action_group_req_users_approve_user_id",
                        column: x => x.approve_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_action_group_req_users_req_user_id",
                        column: x => x.req_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "action_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    action_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    act_desc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    core_code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    approve_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    req_user_id = table.Column<int>(type: "int", nullable: true),
                    approve_user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_action_req_action_action_id",
                        column: x => x.action_id,
                        principalTable: "action",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_action_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_action_req_users_approve_user_id",
                        column: x => x.approve_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_action_req_users_req_user_id",
                        column: x => x.req_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "call_script_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    call_script_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    cs_desc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    accd_min = table.Column<int>(type: "int", nullable: true),
                    accd_max = table.Column<int>(type: "int", nullable: true),
                    cs_script = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    req_user_id = table.Column<int>(type: "int", nullable: true),
                    approve_user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_call_script_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_call_script_req_call_script_call_script_id",
                        column: x => x.call_script_id,
                        principalTable: "call_script",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_call_script_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_call_script_req_users_approve_user_id",
                        column: x => x.approve_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_call_script_req_users_req_user_id",
                        column: x => x.req_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "collection_add_contact",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    add_id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    cu_cif = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    acc_no = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    add_phone = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    add_address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    add_city = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    add_from = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true),
                    add_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    add_by = table.Column<int>(type: "int", nullable: true),
                    def = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_add_contact", x => x.id);
                    table.ForeignKey(
                        name: "FK_collection_add_contact_users_add_by",
                        column: x => x.add_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "collection_visit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    visit_id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    acc_no = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    visit_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    add_id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    visit_reason = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    visit_result = table.Column<int>(type: "int", nullable: true),
                    visit_result_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    visit_amount = table.Column<double>(type: "float", nullable: true),
                    visit_note = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    visit_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    visit_by = table.Column<int>(type: "int", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    picture = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    ubm_id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    cbm_id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    kolek = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_visit", x => x.id);
                    table.ForeignKey(
                        name: "FK_collection_visit_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_visit_rfresult_visit_result",
                        column: x => x.visit_result,
                        principalTable: "rfresult",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_visit_users_visit_by",
                        column: x => x.visit_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "fc_mapping_mikro_collection",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fc_id = table.Column<int>(type: "int", nullable: true),
                    type_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fc_mapping_mikro_collection", x => x.id);
                    table.ForeignKey(
                        name: "FK_fc_mapping_mikro_collection_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_fc_mapping_mikro_collection_users_fc_id",
                        column: x => x.fc_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "notif_content_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    notif_content_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    content = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    day = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    req_user_id = table.Column<int>(type: "int", nullable: true),
                    approve_user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notif_content_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_notif_content_req_notif_content_notif_content_id",
                        column: x => x.notif_content_id,
                        principalTable: "notif_content",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notif_content_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notif_content_req_users_approve_user_id",
                        column: x => x.approve_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notif_content_req_users_req_user_id",
                        column: x => x.req_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "reason_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reason_id = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_dc = table.Column<int>(type: "int", nullable: true),
                    is_fc = table.Column<int>(type: "int", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    req_user_id = table.Column<int>(type: "int", nullable: true),
                    approve_user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reason_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_reason_req_reason_reason_id",
                        column: x => x.reason_id,
                        principalTable: "reason",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_reason_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_reason_req_users_approve_user_id",
                        column: x => x.approve_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_reason_req_users_req_user_id",
                        column: x => x.req_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "team",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    area_id = table.Column<int>(type: "int", nullable: true),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    spv_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team", x => x.id);
                    table.ForeignKey(
                        name: "FK_team_area_area_id",
                        column: x => x.area_id,
                        principalTable: "area",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_team_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_team_users_spv_id",
                        column: x => x.spv_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "token",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstname = table.Column<string>(type: "varchar(100)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    expire = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_token", x => x.id);
                    table.ForeignKey(
                        name: "FK_token_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tracking",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tgl = table.Column<DateTime>(type: "datetime", nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tracking", x => x.id);
                    table.ForeignKey(
                        name: "FK_tracking_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_branch",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_branch", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_branch_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_branch_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_branch_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    admin = table.Column<int>(type: "int", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    approve_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fcm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    role_id = table.Column<int>(type: "int", nullable: true),
                    spv_id = table.Column<int>(type: "int", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    req_user_id = table.Column<int>(type: "int", nullable: true),
                    approve_user_id = table.Column<int>(type: "int", nullable: true),
                    tel_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tel_device = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_req_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_req_users_approve_user_id",
                        column: x => x.approve_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_req_users_req_user_id",
                        column: x => x.req_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_req_users_spv_id",
                        column: x => x.spv_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_req_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "agent_dist",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    dist_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_dist", x => x.id);
                    table.ForeignKey(
                        name: "FK_agent_dist_distr_rule_dist_id",
                        column: x => x.dist_id,
                        principalTable: "distr_rule",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_agent_dist_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rfkelurahan",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    kode_pos = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    kd_dkcpl_kelurahan = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    kecamatan_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfkelurahan", x => x.id);
                    table.ForeignKey(
                        name: "FK_rfkelurahan_rfkecamatan_kecamatan_id",
                        column: x => x.kecamatan_id,
                        principalTable: "rfkecamatan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_rfkelurahan_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "collection_contact_photo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    coll_contact_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    mime = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_contact_photo", x => x.id);
                    table.ForeignKey(
                        name: "FK_collection_contact_photo_collection_add_contact_coll_contact_id",
                        column: x => x.coll_contact_id,
                        principalTable: "collection_add_contact",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_contact_photo_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "fc_mapping_mikro_collection_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fc_mapping_mikro_collection_id = table.Column<int>(type: "int", nullable: true),
                    fc_id = table.Column<int>(type: "int", nullable: true),
                    type_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fc_mapping_mikro_collection_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_fc_mapping_mikro_collection_req_fc_mapping_mikro_collection_fc_mapping_mikro_collection_id",
                        column: x => x.fc_mapping_mikro_collection_id,
                        principalTable: "fc_mapping_mikro_collection",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_fc_mapping_mikro_collection_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_fc_mapping_mikro_collection_req_users_fc_id",
                        column: x => x.fc_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "team_member",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    member_id = table.Column<int>(type: "int", nullable: true),
                    team_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team_member", x => x.id);
                    table.ForeignKey(
                        name: "FK_team_member_team_team_id",
                        column: x => x.team_id,
                        principalTable: "team",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_team_member_users_member_id",
                        column: x => x.member_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_branch_req",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    user_branch_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    req_user_id = table.Column<int>(type: "int", nullable: true),
                    approve_user_id = table.Column<int>(type: "int", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true),
                    user_req_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_branch_req", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_branch_req_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_branch_req_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_branch_req_user_branch_user_branch_id",
                        column: x => x.user_branch_id,
                        principalTable: "user_branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_branch_req_users_approve_user_id",
                        column: x => x.approve_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_branch_req_users_req_user_id",
                        column: x => x.req_user_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_branch_req_users_req_user_req_id",
                        column: x => x.user_req_id,
                        principalTable: "users_req",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_branch_req_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "master_customer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cu_cif = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    cu_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    cu_borndate = table.Column<DateTime>(type: "datetime", nullable: true),
                    cu_bornplace = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    cu_idtype = table.Column<int>(type: "int", nullable: true),
                    cu_idnumber = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    cu_gender = table.Column<int>(type: "int", nullable: true),
                    cu_maritalstatus = table.Column<int>(type: "int", nullable: true),
                    cu_nationality = table.Column<int>(type: "int", nullable: true),
                    cu_incometype = table.Column<int>(type: "int", nullable: true),
                    cu_income = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cu_custtype = table.Column<int>(type: "int", nullable: true),
                    cu_occupation = table.Column<int>(type: "int", nullable: true),
                    cu_company = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    cu_email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    cu_address = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    cu_rt = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    cu_rw = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    cu_kelurahan = table.Column<int>(type: "int", nullable: true),
                    cu_kecamatan = table.Column<int>(type: "int", nullable: true),
                    cu_city = table.Column<int>(type: "int", nullable: true),
                    cu_provinsi = table.Column<int>(type: "int", nullable: true),
                    cu_zipcode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    cu_hmphone = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    cu_mobilephone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    pekerjaan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    jabatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kelurahan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kecamatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    provinsi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_customer", x => x.id);
                    table.ForeignKey(
                        name: "FK_master_customer_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfcustomertype_cu_custtype",
                        column: x => x.cu_custtype,
                        principalTable: "rfcustomertype",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfgender_cu_gender",
                        column: x => x.cu_gender,
                        principalTable: "rfgender",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfidtype_cu_idtype",
                        column: x => x.cu_idtype,
                        principalTable: "rfidtype",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfincometype_cu_incometype",
                        column: x => x.cu_incometype,
                        principalTable: "rfincometype",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfkabupaten_cu_city",
                        column: x => x.cu_city,
                        principalTable: "rfkabupaten",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfkecamatan_cu_kecamatan",
                        column: x => x.cu_kecamatan,
                        principalTable: "rfkecamatan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfkelurahan_cu_kelurahan",
                        column: x => x.cu_kelurahan,
                        principalTable: "rfkelurahan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfmarital_cu_maritalstatus",
                        column: x => x.cu_maritalstatus,
                        principalTable: "rfmarital",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfnationality_cu_nationality",
                        column: x => x.cu_nationality,
                        principalTable: "rfnationality",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfoccupation_cu_occupation",
                        column: x => x.cu_occupation,
                        principalTable: "rfoccupation",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_customer_rfprovinsi_cu_provinsi",
                        column: x => x.cu_provinsi,
                        principalTable: "rfprovinsi",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "master_loan",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: true),
                    prd_segment_id = table.Column<int>(type: "int", nullable: true),
                    cu_cif = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    acc_no = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    ccy = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    product = table.Column<int>(type: "int", nullable: true),
                    plafond = table.Column<double>(type: "float", nullable: true),
                    maturity_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    sisa_tenor = table.Column<int>(type: "int", nullable: true),
                    tenor = table.Column<int>(type: "int", nullable: true),
                    installment_pokok = table.Column<double>(type: "float", nullable: true),
                    interest_rate = table.Column<double>(type: "float", nullable: true),
                    installment = table.Column<double>(type: "float", nullable: true),
                    tunggakan_pokok = table.Column<double>(type: "float", nullable: true),
                    tunggakan_bunga = table.Column<double>(type: "float", nullable: true),
                    tunggakan_denda = table.Column<double>(type: "float", nullable: true),
                    tunggakan_total = table.Column<double>(type: "float", nullable: true),
                    kewajiban_total = table.Column<double>(type: "float", nullable: true),
                    last_pay_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    outstanding = table.Column<double>(type: "float", nullable: true),
                    pay_total = table.Column<double>(type: "float", nullable: true),
                    dpd = table.Column<int>(type: "int", nullable: true),
                    kolektibilitas = table.Column<int>(type: "int", nullable: true),
                    econa_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    econ_phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    econ_relation = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    marketing_code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    channel_branch_code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fasilitas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    payin_account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    file_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    loan_number = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_loan", x => x.id);
                    table.ForeignKey(
                        name: "FK_master_loan_master_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "master_customer",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_loan_rfproduct_product",
                        column: x => x.product,
                        principalTable: "rfproduct",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_loan_rfproduct_segment_prd_segment_id",
                        column: x => x.prd_segment_id,
                        principalTable: "rfproduct_segment",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "auction",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    mst_branch_id = table.Column<int>(type: "int", nullable: true),
                    last_update_id = table.Column<int>(type: "int", nullable: true),
                    last_update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    mst_branch_pembukuan_id = table.Column<int>(type: "int", nullable: true),
                    mst_branch_proses_id = table.Column<int>(type: "int", nullable: true),
                    alasan_lelang_id = table.Column<int>(type: "int", nullable: true),
                    no_pk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nilai_limit_lelang = table.Column<double>(type: "float", nullable: true),
                    uang_jaminan = table.Column<double>(type: "float", nullable: true),
                    objek_lelang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    keterangan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    balai_lelang_id = table.Column<int>(type: "int", nullable: true),
                    jenis_lelang_id = table.Column<int>(type: "int", nullable: true),
                    tata_cara_lelang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    biaya_lelang = table.Column<double>(type: "float", nullable: true),
                    catatan_lelang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tgl_penetapan_lelang = table.Column<DateTime>(type: "datetime2", nullable: true),
                    no_rekening = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nama_rekening = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auction", x => x.id);
                    table.ForeignKey(
                        name: "FK_auction_branch_mst_branch_id",
                        column: x => x.mst_branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_branch_mst_branch_pembukuan_id",
                        column: x => x.mst_branch_pembukuan_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_branch_mst_branch_proses_id",
                        column: x => x.mst_branch_proses_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_generic_param_alasan_lelang_id",
                        column: x => x.alasan_lelang_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_generic_param_balai_lelang_id",
                        column: x => x.balai_lelang_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_generic_param_jenis_lelang_id",
                        column: x => x.jenis_lelang_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_master_loan_loan_id",
                        column: x => x.loan_id,
                        principalTable: "master_loan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_users_last_update_id",
                        column: x => x.last_update_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ayda",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    mst_branch_id = table.Column<int>(type: "int", nullable: true),
                    last_update_id = table.Column<int>(type: "int", nullable: true),
                    last_update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    mst_branch_pembukuan_id = table.Column<int>(type: "int", nullable: true),
                    mst_branch_proses_id = table.Column<int>(type: "int", nullable: true),
                    hubungan_bank_id = table.Column<int>(type: "int", nullable: true),
                    tgl_ambil_alih = table.Column<DateTime>(type: "datetime2", nullable: true),
                    kualitas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nilai_pembiayaan_pokok = table.Column<double>(type: "float", nullable: true),
                    nilai_margin = table.Column<double>(type: "float", nullable: true),
                    nilai_perolehan_agunan = table.Column<double>(type: "float", nullable: true),
                    perkiraan_biaya_jual = table.Column<double>(type: "float", nullable: true),
                    ppa = table.Column<double>(type: "float", nullable: true),
                    jumlah_ayda = table.Column<double>(type: "float", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ayda", x => x.id);
                    table.ForeignKey(
                        name: "FK_ayda_branch_mst_branch_id",
                        column: x => x.mst_branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_branch_mst_branch_pembukuan_id",
                        column: x => x.mst_branch_pembukuan_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_branch_mst_branch_proses_id",
                        column: x => x.mst_branch_proses_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_generic_param_hubungan_bank_id",
                        column: x => x.hubungan_bank_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_master_loan_loan_id",
                        column: x => x.loan_id,
                        principalTable: "master_loan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_users_last_update_id",
                        column: x => x.last_update_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "collection_call",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    acc_no = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    call_name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    add_id = table.Column<int>(type: "int", nullable: true),
                    call_reason = table.Column<int>(type: "int", nullable: true),
                    call_result_id = table.Column<int>(type: "int", nullable: true),
                    call_result_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    call_amount = table.Column<double>(type: "float", nullable: true),
                    call_notes = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    call_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    call_by = table.Column<int>(type: "int", nullable: true),
                    call_result_hh = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: true),
                    call_result_mm = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: true),
                    call_result_hhmm = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_call", x => x.id);
                    table.ForeignKey(
                        name: "FK_collection_call_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_call_collection_add_contact_add_id",
                        column: x => x.add_id,
                        principalTable: "collection_add_contact",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_call_master_loan_loan_id",
                        column: x => x.loan_id,
                        principalTable: "master_loan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_call_reason_call_reason",
                        column: x => x.call_reason,
                        principalTable: "reason",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_call_rfresult_call_result_id",
                        column: x => x.call_result_id,
                        principalTable: "rfresult",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_call_users_call_by",
                        column: x => x.call_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "generate_letter",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    cabang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cabang_alamat = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    cabang_telepon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cabang_faks = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    cabang_kota = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    type_letter = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    no = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tgl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    nama_nasabah = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    jumlah = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    terbilang = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    tgl_bayar = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    no_kredit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tgl_kredit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    notaris = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    notaris_di = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    notaris_tgl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    no_sp1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tgl_sp1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    no_sp2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tgl_sp2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generate_letter", x => x.id);
                    table.ForeignKey(
                        name: "FK_generate_letter_master_loan_loan_id",
                        column: x => x.loan_id,
                        principalTable: "master_loan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_generate_letter_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "insurance",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    mst_branch_id = table.Column<int>(type: "int", nullable: true),
                    last_update_id = table.Column<int>(type: "int", nullable: true),
                    last_update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    mst_branch_pembukuan_id = table.Column<int>(type: "int", nullable: true),
                    mst_branch_proses_id = table.Column<int>(type: "int", nullable: true),
                    nama_pejabat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    jabatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    no_sertifikat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tgl_sertifikat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    asuransi_id = table.Column<int>(type: "int", nullable: true),
                    no_polis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tgl_polis = table.Column<DateTime>(type: "datetime2", nullable: true),
                    no_pk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nilai_tunggakan_pokok = table.Column<double>(type: "float", nullable: true),
                    nilai_tunggakan_bunga = table.Column<double>(type: "float", nullable: true),
                    catatan_polis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    keterangan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nilai_klaim = table.Column<double>(type: "float", nullable: true),
                    nilai_klaim_dibayar = table.Column<double>(type: "float", nullable: true),
                    tgl_klaim_dibayar = table.Column<DateTime>(type: "datetime2", nullable: true),
                    asuransi_sisa_klaim_id = table.Column<int>(type: "int", nullable: true),
                    baki_debit_klaim = table.Column<double>(type: "float", nullable: true),
                    catatan_klaim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    permasalahan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance", x => x.id);
                    table.ForeignKey(
                        name: "FK_insurance_branch_mst_branch_id",
                        column: x => x.mst_branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_branch_mst_branch_pembukuan_id",
                        column: x => x.mst_branch_pembukuan_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_branch_mst_branch_proses_id",
                        column: x => x.mst_branch_proses_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_generic_param_asuransi_id",
                        column: x => x.asuransi_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_generic_param_asuransi_sisa_klaim_id",
                        column: x => x.asuransi_sisa_klaim_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_master_loan_loan_id",
                        column: x => x.loan_id,
                        principalTable: "master_loan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_users_last_update_id",
                        column: x => x.last_update_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "master_collateral",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    col_id = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    col_type = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    veh_bpkb_no = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    veh_plate_no = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    veh_merek = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    veh_model = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    veh_bpkb_name = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: true),
                    veh_engine_no = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    veh_chassis_no = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    veh_stnk_no = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    veh_year = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    veh_build_year = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
                    veh_cc = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    veh_color = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_collateral", x => x.id);
                    table.ForeignKey(
                        name: "FK_master_collateral_master_loan_loan_id",
                        column: x => x.loan_id,
                        principalTable: "master_loan",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "master_loan_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    call_by = table.Column<int>(type: "int", nullable: true),
                    STG_DATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                    customer_id = table.Column<int>(type: "int", nullable: true),
                    prd_segment_id = table.Column<int>(type: "int", nullable: true),
                    cu_cif = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    acc_no = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    ccy = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    product = table.Column<int>(type: "int", nullable: true),
                    plafond = table.Column<double>(type: "float", nullable: true),
                    maturity_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    sisa_tenor = table.Column<int>(type: "int", nullable: true),
                    tenor = table.Column<int>(type: "int", nullable: true),
                    installment_pokok = table.Column<double>(type: "float", nullable: true),
                    interest_rate = table.Column<double>(type: "float", nullable: true),
                    installment = table.Column<double>(type: "float", nullable: true),
                    tunggakan_pokok = table.Column<double>(type: "float", nullable: true),
                    tunggakan_bunga = table.Column<double>(type: "float", nullable: true),
                    tunggakan_denda = table.Column<double>(type: "float", nullable: true),
                    tunggakan_total = table.Column<double>(type: "float", nullable: true),
                    kewajiban_total = table.Column<double>(type: "float", nullable: true),
                    last_pay_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    outstanding = table.Column<double>(type: "float", nullable: true),
                    pay_total = table.Column<double>(type: "float", nullable: true),
                    dpd = table.Column<int>(type: "int", nullable: true),
                    kolektibilitas = table.Column<int>(type: "int", nullable: true),
                    econa_name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    econ_phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    econ_relation = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    marketing_code = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    channel_branch_code = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fasilitas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payin_account = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_loan_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_master_loan_history_master_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "master_customer",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_loan_history_master_loan_loan_id",
                        column: x => x.loan_id,
                        principalTable: "master_loan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_loan_history_rfproduct_product",
                        column: x => x.product,
                        principalTable: "rfproduct",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_loan_history_rfproduct_segment_prd_segment_id",
                        column: x => x.prd_segment_id,
                        principalTable: "rfproduct_segment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_master_loan_history_users_call_by",
                        column: x => x.call_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "payment_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    acc_no = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    tgl = table.Column<DateTime>(type: "datetime", nullable: true),
                    pokok_cicilan = table.Column<double>(type: "float", nullable: true),
                    bunga = table.Column<double>(type: "float", nullable: true),
                    denda = table.Column<double>(type: "float", nullable: true),
                    total_bayar = table.Column<double>(type: "float", nullable: true),
                    call_by = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_history_master_loan_loan_id",
                        column: x => x.loan_id,
                        principalTable: "master_loan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_payment_history_users_call_by",
                        column: x => x.call_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "restructure",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    mst_branch_id = table.Column<int>(type: "int", nullable: true),
                    last_update_id = table.Column<int>(type: "int", nullable: true),
                    last_update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    mst_branch_pembukuan_id = table.Column<int>(type: "int", nullable: true),
                    mst_branch_proses_id = table.Column<int>(type: "int", nullable: true),
                    principal_pembayaran = table.Column<double>(type: "float", nullable: true),
                    margin_pembayaran = table.Column<double>(type: "float", nullable: true),
                    principal_pinalty = table.Column<double>(type: "float", nullable: true),
                    margin_pinalty = table.Column<double>(type: "float", nullable: true),
                    tgl_jatuh_tempo_baru = table.Column<DateTime>(type: "datetime2", nullable: true),
                    keterangan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    grace_periode = table.Column<int>(type: "int", nullable: true),
                    pengurangan_nilai_margin = table.Column<int>(type: "int", nullable: true),
                    tgl_awal_periode_diskon = table.Column<DateTime>(type: "datetime2", nullable: true),
                    tgl_akhir_periode_diskon = table.Column<DateTime>(type: "datetime2", nullable: true),
                    periode_diskon = table.Column<int>(type: "int", nullable: true),
                    value_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    disc_tunggakan_margin = table.Column<double>(type: "float", nullable: true),
                    disc_tunggakan_denda = table.Column<double>(type: "float", nullable: true),
                    margin = table.Column<double>(type: "float", nullable: true),
                    denda = table.Column<double>(type: "float", nullable: true),
                    margin_amount = table.Column<double>(type: "float", nullable: true),
                    total_diskon_margin = table.Column<double>(type: "float", nullable: true),
                    pola_restruk_id = table.Column<int>(type: "int", nullable: true),
                    pembayaran_gp_id = table.Column<int>(type: "int", nullable: true),
                    jenis_pengurangan_id = table.Column<int>(type: "int", nullable: true),
                    permasalahan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createby_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    checkby_id = table.Column<int>(type: "int", nullable: true),
                    check_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    approveby_id = table.Column<int>(type: "int", nullable: true),
                    approve_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restructure", x => x.id);
                    table.ForeignKey(
                        name: "FK_restructure_branch_mst_branch_id",
                        column: x => x.mst_branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_branch_mst_branch_pembukuan_id",
                        column: x => x.mst_branch_pembukuan_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_branch_mst_branch_proses_id",
                        column: x => x.mst_branch_proses_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_generic_param_jenis_pengurangan_id",
                        column: x => x.jenis_pengurangan_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_generic_param_pembayaran_gp_id",
                        column: x => x.pembayaran_gp_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_generic_param_pola_restruk_id",
                        column: x => x.pola_restruk_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_master_loan_loan_id",
                        column: x => x.loan_id,
                        principalTable: "master_loan",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_users_approveby_id",
                        column: x => x.approveby_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_users_checkby_id",
                        column: x => x.checkby_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_users_createby_id",
                        column: x => x.createby_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_users_last_update_id",
                        column: x => x.last_update_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "auction_approval",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    auction_id = table.Column<int>(type: "int", nullable: true),
                    sender_role_id = table.Column<int>(type: "int", nullable: true),
                    recipient_role_id = table.Column<int>(type: "int", nullable: true),
                    sender_id = table.Column<int>(type: "int", nullable: true),
                    recipient_id = table.Column<int>(type: "int", nullable: true),
                    execution_id = table.Column<int>(type: "int", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auction_approval", x => x.id);
                    table.ForeignKey(
                        name: "FK_auction_approval_auction_auction_id",
                        column: x => x.auction_id,
                        principalTable: "auction",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_approval_generic_param_execution_id",
                        column: x => x.execution_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_approval_role_recipient_role_id",
                        column: x => x.recipient_role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_approval_role_sender_role_id",
                        column: x => x.sender_role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_approval_users_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_approval_users_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "auction_document",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    auction_id = table.Column<int>(type: "int", nullable: true),
                    doc_type_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    mime = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auction_document", x => x.id);
                    table.ForeignKey(
                        name: "FK_auction_document_auction_auction_id",
                        column: x => x.auction_id,
                        principalTable: "auction",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_document_generic_param_doc_type_id",
                        column: x => x.doc_type_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_document_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "auction_result_document",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    auction_id = table.Column<int>(type: "int", nullable: true),
                    doc_type_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    mime = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auction_result_document", x => x.id);
                    table.ForeignKey(
                        name: "FK_auction_result_document_auction_auction_id",
                        column: x => x.auction_id,
                        principalTable: "auction",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_result_document_generic_param_doc_type_id",
                        column: x => x.doc_type_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auction_result_document_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ayda_approval",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ayda_id = table.Column<int>(type: "int", nullable: true),
                    sender_role_id = table.Column<int>(type: "int", nullable: true),
                    recipient_role_id = table.Column<int>(type: "int", nullable: true),
                    sender_id = table.Column<int>(type: "int", nullable: true),
                    recipient_id = table.Column<int>(type: "int", nullable: true),
                    execution_id = table.Column<int>(type: "int", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ayda_approval", x => x.id);
                    table.ForeignKey(
                        name: "FK_ayda_approval_ayda_ayda_id",
                        column: x => x.ayda_id,
                        principalTable: "ayda",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_approval_generic_param_execution_id",
                        column: x => x.execution_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_approval_role_recipient_role_id",
                        column: x => x.recipient_role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_approval_role_sender_role_id",
                        column: x => x.sender_role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_approval_users_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_approval_users_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ayda_document",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ayda_id = table.Column<int>(type: "int", nullable: true),
                    doc_type_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    mime = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ayda_document", x => x.id);
                    table.ForeignKey(
                        name: "FK_ayda_document_ayda_ayda_id",
                        column: x => x.ayda_id,
                        principalTable: "ayda",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_document_generic_param_doc_type_id",
                        column: x => x.doc_type_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ayda_document_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "call_request",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    call_id = table.Column<int>(type: "int", nullable: true),
                    phone_no = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    mime = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_call_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_call_request_collection_call_call_id",
                        column: x => x.call_id,
                        principalTable: "collection_call",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_call_request_status_status_id",
                        column: x => x.status_id,
                        principalTable: "status",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_call_request_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "collection_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    call_id = table.Column<int>(type: "int", nullable: true),
                    call_by = table.Column<int>(type: "int", nullable: true),
                    branch_id = table.Column<int>(type: "int", nullable: true),
                    acc_no = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    add_id = table.Column<int>(type: "int", nullable: true),
                    reason = table.Column<int>(type: "int", nullable: true),
                    result = table.Column<int>(type: "int", nullable: true),
                    result_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    amount = table.Column<double>(type: "float", nullable: true),
                    note = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    history_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    history_by = table.Column<int>(type: "int", nullable: true),
                    longitude = table.Column<double>(type: "float", nullable: true),
                    latitude = table.Column<double>(type: "float", nullable: true),
                    picture = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    ubm_id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    cbm_id = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    kolek = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    CallResultHh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    call_result_mm = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    call_result_hhmm = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    dpd = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_collection_history_branch_branch_id",
                        column: x => x.branch_id,
                        principalTable: "branch",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_history_collection_add_contact_add_id",
                        column: x => x.add_id,
                        principalTable: "collection_add_contact",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_history_collection_call_call_id",
                        column: x => x.call_id,
                        principalTable: "collection_call",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_history_reason_reason",
                        column: x => x.reason,
                        principalTable: "reason",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_history_rfresult_result",
                        column: x => x.result,
                        principalTable: "rfresult",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_history_users_call_by",
                        column: x => x.call_by,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_history_users_history_by",
                        column: x => x.history_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "collection_trace",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    call_id = table.Column<int>(type: "int", nullable: true),
                    call_by = table.Column<int>(type: "int", nullable: true),
                    acc_no = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    result = table.Column<int>(type: "int", nullable: true),
                    amount = table.Column<double>(type: "float", nullable: true),
                    trace_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    kolek = table.Column<int>(type: "int", unicode: false, maxLength: 50, nullable: true),
                    dpd = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_trace", x => x.id);
                    table.ForeignKey(
                        name: "FK_collection_trace_collection_call_call_id",
                        column: x => x.call_id,
                        principalTable: "collection_call",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_trace_rfresult_result",
                        column: x => x.result,
                        principalTable: "rfresult",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_trace_users_call_by",
                        column: x => x.call_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "payment_record",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    call_id = table.Column<int>(type: "int", nullable: true),
                    call_by = table.Column<int>(type: "int", nullable: true),
                    acc_no = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true),
                    amount = table.Column<double>(type: "float", nullable: true),
                    record_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_record", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_record_collection_call_call_id",
                        column: x => x.call_id,
                        principalTable: "collection_call",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_payment_record_users_call_by",
                        column: x => x.call_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "insurance_approval",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    insurance_id = table.Column<int>(type: "int", nullable: true),
                    sender_role_id = table.Column<int>(type: "int", nullable: true),
                    recipient_role_id = table.Column<int>(type: "int", nullable: true),
                    sender_id = table.Column<int>(type: "int", nullable: true),
                    recipient_id = table.Column<int>(type: "int", nullable: true),
                    execution_id = table.Column<int>(type: "int", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance_approval", x => x.id);
                    table.ForeignKey(
                        name: "FK_insurance_approval_generic_param_execution_id",
                        column: x => x.execution_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_approval_insurance_insurance_id",
                        column: x => x.insurance_id,
                        principalTable: "insurance",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_approval_role_recipient_role_id",
                        column: x => x.recipient_role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_approval_role_sender_role_id",
                        column: x => x.sender_role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_approval_users_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_approval_users_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "insurance_document",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    insurance_id = table.Column<int>(type: "int", nullable: true),
                    doc_type_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    mime = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AydaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance_document", x => x.id);
                    table.ForeignKey(
                        name: "FK_insurance_document_ayda_AydaId",
                        column: x => x.AydaId,
                        principalTable: "ayda",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_document_generic_param_doc_type_id",
                        column: x => x.doc_type_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_document_insurance_insurance_id",
                        column: x => x.insurance_id,
                        principalTable: "insurance",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_insurance_document_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "restructure_approval",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    restructure_id = table.Column<int>(type: "int", nullable: true),
                    sender_role_id = table.Column<int>(type: "int", nullable: true),
                    recipient_role_id = table.Column<int>(type: "int", nullable: true),
                    sender_id = table.Column<int>(type: "int", nullable: true),
                    recipient_id = table.Column<int>(type: "int", nullable: true),
                    execution_id = table.Column<int>(type: "int", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restructure_approval", x => x.id);
                    table.ForeignKey(
                        name: "FK_restructure_approval_generic_param_execution_id",
                        column: x => x.execution_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_approval_restructure_restructure_id",
                        column: x => x.restructure_id,
                        principalTable: "restructure",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_approval_role_recipient_role_id",
                        column: x => x.recipient_role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_approval_role_sender_role_id",
                        column: x => x.sender_role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_approval_users_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_approval_users_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "restructure_cashflow",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    penghasilan_nasabah = table.Column<int>(type: "int", nullable: true),
                    penghasilan_pasangan = table.Column<int>(type: "int", nullable: true),
                    penghasilan_lainnya = table.Column<int>(type: "int", nullable: true),
                    total_penghasilan = table.Column<int>(type: "int", nullable: true),
                    biaya_pendidikan = table.Column<int>(type: "int", nullable: true),
                    biaya_listrik_air_telp = table.Column<int>(type: "int", nullable: true),
                    biaya_belanja = table.Column<int>(type: "int", nullable: true),
                    biaya_transportasi = table.Column<int>(type: "int", nullable: true),
                    biaya_lainnya = table.Column<int>(type: "int", nullable: true),
                    total_pengeluaran = table.Column<int>(type: "int", nullable: true),
                    hutang_di_bank = table.Column<int>(type: "int", nullable: true),
                    cicilan_lainnya = table.Column<int>(type: "int", nullable: true),
                    total_kewajiban = table.Column<int>(type: "int", nullable: true),
                    penghasilan_bersih = table.Column<int>(type: "int", nullable: true),
                    rpc_70_persen = table.Column<int>(type: "int", nullable: true),
                    restructure_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restructure_cashflow", x => x.id);
                    table.ForeignKey(
                        name: "FK_restructure_cashflow_restructure_restructure_id",
                        column: x => x.restructure_id,
                        principalTable: "restructure",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "restructure_document",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    restructure_id = table.Column<int>(type: "int", nullable: true),
                    doc_type_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    mime = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restructure_document", x => x.id);
                    table.ForeignKey(
                        name: "FK_restructure_document_generic_param_doc_type_id",
                        column: x => x.doc_type_id,
                        principalTable: "generic_param",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_document_restructure_restructure_id",
                        column: x => x.restructure_id,
                        principalTable: "restructure",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_restructure_document_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "collection_photo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    collhistory_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    mime = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    lat = table.Column<double>(type: "float", nullable: true),
                    lon = table.Column<double>(type: "float", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    create_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_photo", x => x.id);
                    table.ForeignKey(
                        name: "FK_collection_photo_collection_history_collhistory_id",
                        column: x => x.collhistory_id,
                        principalTable: "collection_history",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_collection_photo_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_distribution_status_id",
                table: "account_distribution",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_account_distribution_req_account_distribution_id",
                table: "account_distribution_req",
                column: "account_distribution_id");

            migrationBuilder.CreateIndex(
                name: "IX_account_distribution_req_status_id",
                table: "account_distribution_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_status_id",
                table: "action",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_group_action_id",
                table: "action_group",
                column: "action_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_group_role_id",
                table: "action_group",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_group_status_id",
                table: "action_group",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_group_req_action_group_id",
                table: "action_group_req",
                column: "action_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_group_req_action_id",
                table: "action_group_req",
                column: "action_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_group_req_approve_user_id",
                table: "action_group_req",
                column: "approve_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_group_req_req_user_id",
                table: "action_group_req",
                column: "req_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_group_req_role_id",
                table: "action_group_req",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_group_req_status_id",
                table: "action_group_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_req_action_id",
                table: "action_req",
                column: "action_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_req_approve_user_id",
                table: "action_req",
                column: "approve_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_req_req_user_id",
                table: "action_req",
                column: "req_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_action_req_status_id",
                table: "action_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_agent_dist_dist_id",
                table: "agent_dist",
                column: "dist_id");

            migrationBuilder.CreateIndex(
                name: "IX_agent_dist_user_id",
                table: "agent_dist",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_status_id",
                table: "area",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_req_area_id",
                table: "area_req",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_req_branch_id",
                table: "area_req",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_area_req_status_id",
                table: "area_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_alasan_lelang_id",
                table: "auction",
                column: "alasan_lelang_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_balai_lelang_id",
                table: "auction",
                column: "balai_lelang_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_jenis_lelang_id",
                table: "auction",
                column: "jenis_lelang_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_last_update_id",
                table: "auction",
                column: "last_update_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_loan_id",
                table: "auction",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_mst_branch_id",
                table: "auction",
                column: "mst_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_mst_branch_pembukuan_id",
                table: "auction",
                column: "mst_branch_pembukuan_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_mst_branch_proses_id",
                table: "auction",
                column: "mst_branch_proses_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_status_id",
                table: "auction",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_approval_auction_id",
                table: "auction_approval",
                column: "auction_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_approval_execution_id",
                table: "auction_approval",
                column: "execution_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_approval_recipient_id",
                table: "auction_approval",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_approval_recipient_role_id",
                table: "auction_approval",
                column: "recipient_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_approval_sender_id",
                table: "auction_approval",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_approval_sender_role_id",
                table: "auction_approval",
                column: "sender_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_document_auction_id",
                table: "auction_document",
                column: "auction_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_document_doc_type_id",
                table: "auction_document",
                column: "doc_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_document_user_id",
                table: "auction_document",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_result_document_auction_id",
                table: "auction_result_document",
                column: "auction_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_result_document_doc_type_id",
                table: "auction_result_document",
                column: "doc_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_auction_result_document_user_id",
                table: "auction_result_document",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_hubungan_bank_id",
                table: "ayda",
                column: "hubungan_bank_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_last_update_id",
                table: "ayda",
                column: "last_update_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_loan_id",
                table: "ayda",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_mst_branch_id",
                table: "ayda",
                column: "mst_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_mst_branch_pembukuan_id",
                table: "ayda",
                column: "mst_branch_pembukuan_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_mst_branch_proses_id",
                table: "ayda",
                column: "mst_branch_proses_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_status_id",
                table: "ayda",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_approval_ayda_id",
                table: "ayda_approval",
                column: "ayda_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_approval_execution_id",
                table: "ayda_approval",
                column: "execution_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_approval_recipient_id",
                table: "ayda_approval",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_approval_recipient_role_id",
                table: "ayda_approval",
                column: "recipient_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_approval_sender_id",
                table: "ayda_approval",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_approval_sender_role_id",
                table: "ayda_approval",
                column: "sender_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_document_ayda_id",
                table: "ayda_document",
                column: "ayda_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_document_doc_type_id",
                table: "ayda_document",
                column: "doc_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_ayda_document_user_id",
                table: "ayda_document",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_area_id",
                table: "branch",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_branch_type_id",
                table: "branch",
                column: "branch_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_prd_segment_id",
                table: "branch",
                column: "prd_segment_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_status_id",
                table: "branch",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_req_area_id",
                table: "branch_req",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_req_branch_cco_id",
                table: "branch_req",
                column: "branch_cco_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_req_branch_id",
                table: "branch_req",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_req_branch_type_id",
                table: "branch_req",
                column: "branch_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_req_status_id",
                table: "branch_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_branch_type_status_id",
                table: "branch_type",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_call_request_call_id",
                table: "call_request",
                column: "call_id");

            migrationBuilder.CreateIndex(
                name: "IX_call_request_status_id",
                table: "call_request",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_call_request_user_id",
                table: "call_request",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_call_script_status_id",
                table: "call_script",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_call_script_req_approve_user_id",
                table: "call_script_req",
                column: "approve_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_call_script_req_call_script_id",
                table: "call_script_req",
                column: "call_script_id");

            migrationBuilder.CreateIndex(
                name: "IX_call_script_req_req_user_id",
                table: "call_script_req",
                column: "req_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_call_script_req_status_id",
                table: "call_script_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_add_contact_add_by",
                table: "collection_add_contact",
                column: "add_by");

            migrationBuilder.CreateIndex(
                name: "IX_collection_call_add_id",
                table: "collection_call",
                column: "add_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_call_branch_id",
                table: "collection_call",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_call_call_by",
                table: "collection_call",
                column: "call_by");

            migrationBuilder.CreateIndex(
                name: "IX_collection_call_call_reason",
                table: "collection_call",
                column: "call_reason");

            migrationBuilder.CreateIndex(
                name: "IX_collection_call_call_result_id",
                table: "collection_call",
                column: "call_result_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_call_loan_id",
                table: "collection_call",
                column: "loan_id",
                unique: true,
                filter: "[loan_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_collection_contact_photo_coll_contact_id",
                table: "collection_contact_photo",
                column: "coll_contact_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_contact_photo_user_id",
                table: "collection_contact_photo",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_history_add_id",
                table: "collection_history",
                column: "add_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_history_branch_id",
                table: "collection_history",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_history_call_by",
                table: "collection_history",
                column: "call_by");

            migrationBuilder.CreateIndex(
                name: "IX_collection_history_call_id",
                table: "collection_history",
                column: "call_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_history_history_by",
                table: "collection_history",
                column: "history_by");

            migrationBuilder.CreateIndex(
                name: "IX_collection_history_reason",
                table: "collection_history",
                column: "reason");

            migrationBuilder.CreateIndex(
                name: "IX_collection_history_result",
                table: "collection_history",
                column: "result");

            migrationBuilder.CreateIndex(
                name: "IX_collection_photo_collhistory_id",
                table: "collection_photo",
                column: "collhistory_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_photo_user_id",
                table: "collection_photo",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_trace_call_by",
                table: "collection_trace",
                column: "call_by");

            migrationBuilder.CreateIndex(
                name: "IX_collection_trace_call_id",
                table: "collection_trace",
                column: "call_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_trace_result",
                table: "collection_trace",
                column: "result");

            migrationBuilder.CreateIndex(
                name: "IX_collection_visit_branch_id",
                table: "collection_visit",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_collection_visit_visit_by",
                table: "collection_visit",
                column: "visit_by");

            migrationBuilder.CreateIndex(
                name: "IX_collection_visit_visit_result",
                table: "collection_visit",
                column: "visit_result");

            migrationBuilder.CreateIndex(
                name: "IX_counter_branch_id",
                table: "counter",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_counter_status_id",
                table: "counter",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_distr_rule_branch_id",
                table: "distr_rule",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_distr_rule_product_id",
                table: "distr_rule",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_distr_rule_status_id",
                table: "distr_rule",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_doc_signature_branch_id",
                table: "doc_signature",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_fc_mapping_mikro_collection_fc_id",
                table: "fc_mapping_mikro_collection",
                column: "fc_id");

            migrationBuilder.CreateIndex(
                name: "IX_fc_mapping_mikro_collection_status_id",
                table: "fc_mapping_mikro_collection",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_fc_mapping_mikro_collection_req_fc_id",
                table: "fc_mapping_mikro_collection_req",
                column: "fc_id");

            migrationBuilder.CreateIndex(
                name: "IX_fc_mapping_mikro_collection_req_fc_mapping_mikro_collection_id",
                table: "fc_mapping_mikro_collection_req",
                column: "fc_mapping_mikro_collection_id");

            migrationBuilder.CreateIndex(
                name: "IX_fc_mapping_mikro_collection_req_status_id",
                table: "fc_mapping_mikro_collection_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_generate_letter_loan_id",
                table: "generate_letter",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "IX_generate_letter_status_id",
                table: "generate_letter",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_asuransi_id",
                table: "insurance",
                column: "asuransi_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_asuransi_sisa_klaim_id",
                table: "insurance",
                column: "asuransi_sisa_klaim_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_last_update_id",
                table: "insurance",
                column: "last_update_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_loan_id",
                table: "insurance",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_mst_branch_id",
                table: "insurance",
                column: "mst_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_mst_branch_pembukuan_id",
                table: "insurance",
                column: "mst_branch_pembukuan_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_mst_branch_proses_id",
                table: "insurance",
                column: "mst_branch_proses_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_status_id",
                table: "insurance",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_approval_execution_id",
                table: "insurance_approval",
                column: "execution_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_approval_insurance_id",
                table: "insurance_approval",
                column: "insurance_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_approval_recipient_id",
                table: "insurance_approval",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_approval_recipient_role_id",
                table: "insurance_approval",
                column: "recipient_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_approval_sender_id",
                table: "insurance_approval",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_approval_sender_role_id",
                table: "insurance_approval",
                column: "sender_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_document_AydaId",
                table: "insurance_document",
                column: "AydaId");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_document_doc_type_id",
                table: "insurance_document",
                column: "doc_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_document_insurance_id",
                table: "insurance_document",
                column: "insurance_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_document_user_id",
                table: "insurance_document",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_master_collateral_loan_id",
                table: "master_collateral",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_branch_id",
                table: "master_customer",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_city",
                table: "master_customer",
                column: "cu_city");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_custtype",
                table: "master_customer",
                column: "cu_custtype");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_gender",
                table: "master_customer",
                column: "cu_gender");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_idtype",
                table: "master_customer",
                column: "cu_idtype");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_incometype",
                table: "master_customer",
                column: "cu_incometype");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_kecamatan",
                table: "master_customer",
                column: "cu_kecamatan");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_kelurahan",
                table: "master_customer",
                column: "cu_kelurahan");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_maritalstatus",
                table: "master_customer",
                column: "cu_maritalstatus");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_nationality",
                table: "master_customer",
                column: "cu_nationality");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_occupation",
                table: "master_customer",
                column: "cu_occupation");

            migrationBuilder.CreateIndex(
                name: "IX_master_customer_cu_provinsi",
                table: "master_customer",
                column: "cu_provinsi");

            migrationBuilder.CreateIndex(
                name: "IX_master_loan_customer_id",
                table: "master_loan",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_master_loan_prd_segment_id",
                table: "master_loan",
                column: "prd_segment_id");

            migrationBuilder.CreateIndex(
                name: "IX_master_loan_product",
                table: "master_loan",
                column: "product");

            migrationBuilder.CreateIndex(
                name: "IX_master_loan_history_call_by",
                table: "master_loan_history",
                column: "call_by");

            migrationBuilder.CreateIndex(
                name: "IX_master_loan_history_customer_id",
                table: "master_loan_history",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_master_loan_history_loan_id",
                table: "master_loan_history",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "IX_master_loan_history_prd_segment_id",
                table: "master_loan_history",
                column: "prd_segment_id");

            migrationBuilder.CreateIndex(
                name: "IX_master_loan_history_product",
                table: "master_loan_history",
                column: "product");

            migrationBuilder.CreateIndex(
                name: "IX_notif_content_status_id",
                table: "notif_content",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_notif_content_req_approve_user_id",
                table: "notif_content_req",
                column: "approve_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notif_content_req_notif_content_id",
                table: "notif_content_req",
                column: "notif_content_id");

            migrationBuilder.CreateIndex(
                name: "IX_notif_content_req_req_user_id",
                table: "notif_content_req",
                column: "req_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notif_content_req_status_id",
                table: "notif_content_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_history_call_by",
                table: "payment_history",
                column: "call_by");

            migrationBuilder.CreateIndex(
                name: "IX_payment_history_loan_id",
                table: "payment_history",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_record_call_by",
                table: "payment_record",
                column: "call_by");

            migrationBuilder.CreateIndex(
                name: "IX_payment_record_call_id",
                table: "payment_record",
                column: "call_id");

            migrationBuilder.CreateIndex(
                name: "IX_permission_status_id",
                table: "permission",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_reason_status_id",
                table: "reason",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_reason_req_approve_user_id",
                table: "reason_req",
                column: "approve_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reason_req_reason_id",
                table: "reason_req",
                column: "reason_id");

            migrationBuilder.CreateIndex(
                name: "IX_reason_req_req_user_id",
                table: "reason_req",
                column: "req_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reason_req_status_id",
                table: "reason_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_approveby_id",
                table: "restructure",
                column: "approveby_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_checkby_id",
                table: "restructure",
                column: "checkby_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_createby_id",
                table: "restructure",
                column: "createby_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_jenis_pengurangan_id",
                table: "restructure",
                column: "jenis_pengurangan_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_last_update_id",
                table: "restructure",
                column: "last_update_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_loan_id",
                table: "restructure",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_mst_branch_id",
                table: "restructure",
                column: "mst_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_mst_branch_pembukuan_id",
                table: "restructure",
                column: "mst_branch_pembukuan_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_mst_branch_proses_id",
                table: "restructure",
                column: "mst_branch_proses_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_pembayaran_gp_id",
                table: "restructure",
                column: "pembayaran_gp_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_pola_restruk_id",
                table: "restructure",
                column: "pola_restruk_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_status_id",
                table: "restructure",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_approval_execution_id",
                table: "restructure_approval",
                column: "execution_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_approval_recipient_id",
                table: "restructure_approval",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_approval_recipient_role_id",
                table: "restructure_approval",
                column: "recipient_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_approval_restructure_id",
                table: "restructure_approval",
                column: "restructure_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_approval_sender_id",
                table: "restructure_approval",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_approval_sender_role_id",
                table: "restructure_approval",
                column: "sender_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_cashflow_restructure_id",
                table: "restructure_cashflow",
                column: "restructure_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_document_doc_type_id",
                table: "restructure_document",
                column: "doc_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_document_restructure_id",
                table: "restructure_document",
                column: "restructure_id");

            migrationBuilder.CreateIndex(
                name: "IX_restructure_document_user_id",
                table: "restructure_document",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfcounter_status_id",
                table: "rfcounter",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfcustomertype_status_id",
                table: "rfcustomertype",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfgender_status_id",
                table: "rfgender",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfglobal_status_id",
                table: "rfglobal",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfidtype_status_id",
                table: "rfidtype",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfincometype_status_id",
                table: "rfincometype",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfkabupaten_provinsi_id",
                table: "rfkabupaten",
                column: "provinsi_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfkabupaten_status_id",
                table: "rfkabupaten",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfkecamatan_kabupaten_id",
                table: "rfkecamatan",
                column: "kabupaten_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfkecamatan_status_id",
                table: "rfkecamatan",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfkelurahan_kecamatan_id",
                table: "rfkelurahan",
                column: "kecamatan_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfkelurahan_status_id",
                table: "rfkelurahan",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfmarital_status_id",
                table: "rfmarital",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfnationality_status_id",
                table: "rfnationality",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfoccupation_status_id",
                table: "rfoccupation",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfproduct_prd_segment_id",
                table: "rfproduct",
                column: "prd_segment_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfproduct_status_id",
                table: "rfproduct",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfproduct_segment_status_id",
                table: "rfproduct_segment",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfprovinsi_status_id",
                table: "rfprovinsi",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_rfresult_status_id",
                table: "rfresult",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_status_id",
                table: "role",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_permission_permission_id",
                table: "role_permission",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_req_role_id",
                table: "role_req",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_req_status_id",
                table: "role_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_area_id",
                table: "team",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_branch_id",
                table: "team",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_spv_id",
                table: "team",
                column: "spv_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_member_member_id",
                table: "team_member",
                column: "member_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_member_team_id",
                table: "team_member",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_token_user_id",
                table: "token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tracking_user_id",
                table: "tracking",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_branch_branch_id",
                table: "user_branch",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_branch_status_id",
                table: "user_branch",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_branch_user_id",
                table: "user_branch",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_branch_req_approve_user_id",
                table: "user_branch_req",
                column: "approve_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_branch_req_branch_id",
                table: "user_branch_req",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_branch_req_req_user_id",
                table: "user_branch_req",
                column: "req_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_branch_req_status_id",
                table: "user_branch_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_branch_req_user_branch_id",
                table: "user_branch_req",
                column: "user_branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_branch_req_user_id",
                table: "user_branch_req",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_branch_req_user_req_id",
                table: "user_branch_req",
                column: "user_req_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_status_id",
                table: "users",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_req_approve_user_id",
                table: "users_req",
                column: "approve_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_req_req_user_id",
                table: "users_req",
                column: "req_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_req_role_id",
                table: "users_req",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_req_spv_id",
                table: "users_req",
                column: "spv_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_req_status_id",
                table: "users_req",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_req_user_id",
                table: "users_req",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_distribution_req");

            migrationBuilder.DropTable(
                name: "action_group_req");

            migrationBuilder.DropTable(
                name: "action_req");

            migrationBuilder.DropTable(
                name: "agent_dist");

            migrationBuilder.DropTable(
                name: "area_req");

            migrationBuilder.DropTable(
                name: "auction_approval");

            migrationBuilder.DropTable(
                name: "auction_document");

            migrationBuilder.DropTable(
                name: "auction_result_document");

            migrationBuilder.DropTable(
                name: "ayda_approval");

            migrationBuilder.DropTable(
                name: "ayda_document");

            migrationBuilder.DropTable(
                name: "branch_req");

            migrationBuilder.DropTable(
                name: "call_request");

            migrationBuilder.DropTable(
                name: "call_script_req");

            migrationBuilder.DropTable(
                name: "collection_contact_photo");

            migrationBuilder.DropTable(
                name: "collection_photo");

            migrationBuilder.DropTable(
                name: "collection_trace");

            migrationBuilder.DropTable(
                name: "collection_visit");

            migrationBuilder.DropTable(
                name: "counter");

            migrationBuilder.DropTable(
                name: "doc_signature");

            migrationBuilder.DropTable(
                name: "fc_mapping_mikro_collection_req");

            migrationBuilder.DropTable(
                name: "generate_letter");

            migrationBuilder.DropTable(
                name: "insurance_approval");

            migrationBuilder.DropTable(
                name: "insurance_document");

            migrationBuilder.DropTable(
                name: "LOAN_BIAYA_LAIN");

            migrationBuilder.DropTable(
                name: "LOAN_KODEAO");

            migrationBuilder.DropTable(
                name: "LOAN_KOMITEKREDIT");

            migrationBuilder.DropTable(
                name: "LOAN_KSL");

            migrationBuilder.DropTable(
                name: "LOAN_PK");

            migrationBuilder.DropTable(
                name: "LOAN_TAGIHAN_LAIN");

            migrationBuilder.DropTable(
                name: "master_collateral");

            migrationBuilder.DropTable(
                name: "master_loan_history");

            migrationBuilder.DropTable(
                name: "notif_content_req");

            migrationBuilder.DropTable(
                name: "payment_history");

            migrationBuilder.DropTable(
                name: "payment_record");

            migrationBuilder.DropTable(
                name: "reason_req");

            migrationBuilder.DropTable(
                name: "restructure_approval");

            migrationBuilder.DropTable(
                name: "restructure_cashflow");

            migrationBuilder.DropTable(
                name: "restructure_document");

            migrationBuilder.DropTable(
                name: "rfcounter");

            migrationBuilder.DropTable(
                name: "rfglobal");

            migrationBuilder.DropTable(
                name: "role_permission");

            migrationBuilder.DropTable(
                name: "role_req");

            migrationBuilder.DropTable(
                name: "STG_BRANCH");

            migrationBuilder.DropTable(
                name: "STG_CUSTOMER_NASABAH");

            migrationBuilder.DropTable(
                name: "STG_CUSTOMER_PHONE");

            migrationBuilder.DropTable(
                name: "STG_DATA_JAMINAN");

            migrationBuilder.DropTable(
                name: "STG_DATA_KREDIT");

            migrationBuilder.DropTable(
                name: "STG_DATA_LOAN_BIAYA_LAIN");

            migrationBuilder.DropTable(
                name: "STG_DATA_LOAN_KODEAO");

            migrationBuilder.DropTable(
                name: "STG_DATA_LOAN_KOMITEKREDIT");

            migrationBuilder.DropTable(
                name: "STG_DATA_LOAN_KSL");

            migrationBuilder.DropTable(
                name: "STG_DATA_LOAN_PK");

            migrationBuilder.DropTable(
                name: "STG_DATA_LOAN_TAGIHAN_LAIN");

            migrationBuilder.DropTable(
                name: "STG_LOAN_DETAIL");

            migrationBuilder.DropTable(
                name: "STG_SMSREMINDER");

            migrationBuilder.DropTable(
                name: "team_member");

            migrationBuilder.DropTable(
                name: "token");

            migrationBuilder.DropTable(
                name: "tracking");

            migrationBuilder.DropTable(
                name: "user_branch_req");

            migrationBuilder.DropTable(
                name: "account_distribution");

            migrationBuilder.DropTable(
                name: "action_group");

            migrationBuilder.DropTable(
                name: "distr_rule");

            migrationBuilder.DropTable(
                name: "auction");

            migrationBuilder.DropTable(
                name: "call_script");

            migrationBuilder.DropTable(
                name: "collection_history");

            migrationBuilder.DropTable(
                name: "fc_mapping_mikro_collection");

            migrationBuilder.DropTable(
                name: "ayda");

            migrationBuilder.DropTable(
                name: "insurance");

            migrationBuilder.DropTable(
                name: "notif_content");

            migrationBuilder.DropTable(
                name: "restructure");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "team");

            migrationBuilder.DropTable(
                name: "user_branch");

            migrationBuilder.DropTable(
                name: "users_req");

            migrationBuilder.DropTable(
                name: "action");

            migrationBuilder.DropTable(
                name: "collection_call");

            migrationBuilder.DropTable(
                name: "generic_param");

            migrationBuilder.DropTable(
                name: "collection_add_contact");

            migrationBuilder.DropTable(
                name: "master_loan");

            migrationBuilder.DropTable(
                name: "reason");

            migrationBuilder.DropTable(
                name: "rfresult");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "master_customer");

            migrationBuilder.DropTable(
                name: "rfproduct");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "branch");

            migrationBuilder.DropTable(
                name: "rfcustomertype");

            migrationBuilder.DropTable(
                name: "rfgender");

            migrationBuilder.DropTable(
                name: "rfidtype");

            migrationBuilder.DropTable(
                name: "rfincometype");

            migrationBuilder.DropTable(
                name: "rfkelurahan");

            migrationBuilder.DropTable(
                name: "rfmarital");

            migrationBuilder.DropTable(
                name: "rfnationality");

            migrationBuilder.DropTable(
                name: "rfoccupation");

            migrationBuilder.DropTable(
                name: "area");

            migrationBuilder.DropTable(
                name: "branch_type");

            migrationBuilder.DropTable(
                name: "rfproduct_segment");

            migrationBuilder.DropTable(
                name: "rfkecamatan");

            migrationBuilder.DropTable(
                name: "rfkabupaten");

            migrationBuilder.DropTable(
                name: "rfprovinsi");

            migrationBuilder.DropTable(
                name: "status");
        }
    }
}
