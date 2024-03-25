using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{
    [Table("STG_CUSTOMER_NASABAH")]
    public class STGCustomer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int? ID { get; set; }

        [Column("CU_REF")]
        public string? CU_REF { get; set; }
        [Column("AS_OF_DATE")]
        public DateTime? AS_OF_DATE { get; set; }
        [Column("CU_CIF")]
        public string? CU_CIF { get; set; }
        [Column("NO_REKENING")]
        public string? NO_REKENING { get; set; }
        [Column("CU_FIRSTNAME")]
        public string? CU_FIRSTNAME { get; set; }
        [Column("CU_DOB")]
        public DateTime? CU_DOB { get; set; }
        [Column("CU_POB")]
        public string? CU_POB { get; set; }
        [Column("CU_IDTYPE")]
        public string? CU_IDTYPE { get; set; }
        [Column("CU_IDNUM")]
        public string? CU_IDNUM { get; set; }
        [Column("NO_NPWP")]
        public string? NO_NPWP { get; set; }
        [Column("CU_GENDER")]
        public string? CU_GENDER { get; set; }
        [Column("CU_MARITAL_STATUS")]
        public string? CU_MARITAL_STATUS { get; set; }
        [Column("CU_NATIONALITY")]
        public string? CU_NATIONALITY { get; set; }
        [Column("CU_INCOMETYPE")]
        public string? CU_INCOMETYPE { get; set; }
        [Column("CU_INCOME")]
        public string? CU_INCOME { get; set; }
        [Column("PEKERJAAN")]
        public string? PEKERJAAN { get; set; }
        [Column("CU_TYPE")]
        public string? CU_TYPE { get; set; }
        [Column("CU_OCCUPATION")]
        public string? CU_OCCUPATION { get; set; }
        [Column("CU_COMPANYNAME")]
        public string? CU_COMPANYNAME { get; set; }
        [Column("ALAMAT_PERUSAHAAN_USAHA")]
        public string? ALAMAT_PERUSAHAAN_USAHA { get; set; }
        [Column("BIDANG_USAHA")]
        public string? BIDANG_USAHA { get; set; }
        [Column("CU_EMAIL")]
        public string? CU_EMAIL { get; set; }
        [Column("CU_ADDR1")]
        public string? CU_ADDR1 { get; set; }
        [Column("CU_ADDR2")]
        public string? CU_ADDR2 { get; set; }
        [Column("CU_RT")]
        public string? CU_RT { get; set; }
        [Column("CU_RW")]
        public string? CU_RW { get; set; }
        [Column("CU_KEL")]
        public string? CU_KEL { get; set; }
        [Column("CU_KEC")]
        public string? CU_KEC { get; set; }
        [Column("CU_CITY")]
        public string? CU_CITY { get; set; }
        [Column("CU_PROVINSI")]
        public string? CU_PROVINSI { get; set; }
        [Column("CU_ADDR_STAT")]
        public string? CU_ADDR_STAT { get; set; }
        [Column("CU_ZIP_CODE")]
        public string? CU_ZIP_CODE { get; set; }
        [Column("KELUARGA_TIDAK_SERUMAH")]
        public string? KELUARGA_TIDAK_SERUMAH { get; set; }
        [Column("HUBUNGAN_KELUARGA_TIDAK_SERUMAH")]
        public string? HUBUNGAN_KELUARGA_TIDAK_SERUMAH { get; set; }
        [Column("NO_TLP_KONTAK_DARURAT")]
        public string? NO_TLP_KONTAK_DARURAT { get; set; }
        [Column("ALAMAT_KONTAK_DARURAT")]
        public string? ALAMAT_KONTAK_DARURAT { get; set; }
        [Column("NO_TLP_KANTOR_USAHA")]
        public string? NO_TLP_KANTOR_USAHA { get; set; }
        [Column("CU_PHNNUM")]
        public string? CU_PHNNUM { get; set; }
        [Column("CU_HPNUM")]
        public string? CU_HPNUM { get; set; }
        [Column("BRANCH_CODE")]
        public string? BRANCH_CODE { get; set; }
        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }
    }
}
