using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity.Staging
{
    [Keyless]
    [Table("stg_customer_nasabah")]
    public class STGCustomerPg
    {

        [Column("cu_ref")]
        public string? CU_REF { get; set; }
        [Column("as_of_date")]
        public DateTime? AS_OF_DATE { get; set; }
        [Column("cu_cif")]
        public string? CU_CIF { get; set; }
        [Column("no_rekening")]
        public string? NO_REKENING { get; set; }
        [Column("cu_firstname")]
        public string? CU_FIRSTNAME { get; set; }
        [Column("cu_dob")]
        public DateTime? CU_DOB { get; set; }
        [Column("cu_pob")]
        public string? CU_POB { get; set; }
        [Column("cu_idtype")]
        public string? CU_IDTYPE { get; set; }
        [Column("cu_idnum")]
        public string? CU_IDNUM { get; set; }
        [Column("no_npwp")]
        public string? NO_NPWP { get; set; }
        [Column("cu_gender")]
        public string? CU_GENDER { get; set; }
        [Column("cu_marital status")]
        public string? CU_MARITAL_STATUS { get; set; }
        [Column("cu_nationality")]
        public string? CU_NATIONALITY { get; set; }
        [Column("cu_incometype")]
        public string? CU_INCOMETYPE { get; set; }
        [Column("cu_income")]
        public string? CU_INCOME { get; set; }
        [Column("pekerjaan")]
        public string? PEKERJAAN { get; set; }
        [Column("cu_type")]
        public string? CU_TYPE { get; set; }
        [Column("cu_occupation")]
        public string? CU_OCCUPATION { get; set; }
        [Column("cu_companyname")]
        public string? CU_COMPANYNAME { get; set; }
        [Column("alamat_perusahaan_usaha")]
        public string? ALAMAT_PERUSAHAAN_USAHA { get; set; }
        [Column("bidang_usaha")]
        public string? BIDANG_USAHA { get; set; }
        [Column("cu_email")]
        public string? CU_EMAIL { get; set; }
        [Column("cu_addr1")]
        public string? CU_ADDR1 { get; set; }
        [Column("cu_addr2")]
        public string? CU_ADDR2 { get; set; }
        [Column("cu_rt")]
        public string? CU_RT { get; set; }
        [Column("cu_rw")]
        public string? CU_RW { get; set; }
        [Column("cu_kel")]
        public string? CU_KEL { get; set; }
        [Column("cu_kec")]
        public string? CU_KEC { get; set; }
        [Column("cu_city")]
        public string? CU_CITY { get; set; }
        [Column("cu_provinsi")]
        public string? CU_PROVINSI { get; set; }
        [Column("cu_addr_stat")]
        public string? CU_ADDR_STAT { get; set; }
        [Column("cu_zip_code")]
        public string? CU_ZIP_CODE { get; set; }
        [Column("keluarga_tidak_ serumah")]
        public string? KELUARGA_TIDAK_SERUMAH { get; set; }
        [Column("hubungan_keluarga_tidak_serumah")]
        public string? HUBUNGAN_KELUARGA_TIDAK_SERUMAH { get; set; }
        [Column("no_tlp_ kontak _darurat")]
        public string? NO_TLP_KONTAK_DARURAT { get; set; }
        [Column("alamat_kontak_ darurat")]
        public string? ALAMAT_KONTAK_DARURAT { get; set; }
        [Column("no tlp_kantor_usaha")]
        public string? NO_TLP_KANTOR_USAHA { get; set; }
        [Column("cu_phnnum")]
        public string? CU_PHNNUM { get; set; }
        [Column("cu_hpnum")]
        public string? CU_HPNUM { get; set; }
        [Column("branch_code")]
        public string? BRANCH_CODE { get; set; }
    }
}
