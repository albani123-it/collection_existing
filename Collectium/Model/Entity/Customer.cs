using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("master_customer")]
    public class Customer
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("cu_cif")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Cif { get; set; }

        [Column("cu_name")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Name { get; set; }

        [Column("cu_borndate", TypeName = "datetime")]
        public DateTime? BornDate { get; set; }

        [Column("cu_bornplace")]
        [StringLength(100)]
        [Unicode(false)]
        public string? BornPlace { get; set; }

        [Column("cu_idtype")]
        public int? IdTypeId { get; set; }

        [ForeignKey(nameof(IdTypeId))]
        public IdType? IdType { get; set; }

        [Column("cu_idnumber")]
        [StringLength(30)]
        [Unicode(false)]
        public string? Idnumber { get; set; }

        [Column("cu_gender")]
        public int? GenderId { get; set; }

        [ForeignKey(nameof(GenderId))]
        public Gender? Gender { get; set; }

        [Column("cu_maritalstatus")]
        public int? MaritalStatusId { get; set; }

        [ForeignKey(nameof(MaritalStatusId))]
        public MaritalStatus? MaritalStatus { get; set; }

        [Column("cu_nationality")]
        public int? NationalityId { get; set; }

        [ForeignKey(nameof(NationalityId))]
        public Nationality? Nationality { get; set; }

        [Column("cu_incometype")]
        public int? IncomeTypeId { get; set; }

        [ForeignKey(nameof(IncomeTypeId))]
        public IncomeType? IncomeType { get; set; }

        [Column("cu_income")]
        public string? CuIncome { get; set; }

        [Column("cu_custtype")]
        public int? CustomerTypeId { get; set; }

        [ForeignKey(nameof(CustomerTypeId))]
        public CustomerType? CustomerType { get; set; }

        [Column("cu_occupation")]
        public int? CustomerOccupationId { get; set; }

        [ForeignKey(nameof(CustomerOccupationId))]
        public CustomerOccupation? CustomerOccupation { get; set; }


        [Column("cu_company")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Company { get; set; }

        [Column("cu_email")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Email { get; set; }

        [Column("cu_address")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Address { get; set; }

        [Column("cu_rt")]
        [StringLength(10)]
        [Unicode(false)]
        public string? Rt { get; set; }

        [Column("cu_rw")]
        [StringLength(10)]
        [Unicode(false)]
        public string? Rw { get; set; }

        [Column("cu_kelurahan")]
        public int? KelurahanId { get; set; }

        [ForeignKey(nameof(KelurahanId))]
        public Kelurahan? Kelurahan { get; set; }

        [Column("cu_kecamatan")]
        public int? KecamatanId { get; set; }

        [ForeignKey(nameof(KecamatanId))]
        public Kecamatan? Kecamatan { get; set; }


        [Column("cu_city")]
        public int? CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public City? City { get; set; }

        [Column("cu_provinsi")]
        public int? ProvinsiId { get; set; }

        [ForeignKey(nameof(ProvinsiId))]
        public Provinsi? Provinsi { get; set; }

        [Column("cu_zipcode")]
        [StringLength(10)]
        [Unicode(false)]
        public string? ZipCode { get; set; }

        [Column("cu_hmphone")]
        [StringLength(30)]
        [Unicode(false)]
        public string? HmPhone { get; set; }

        [Column("cu_mobilephone")]
        [StringLength(50)]
        [Unicode(false)]
        public string? MobilePhone { get; set; }

        [Column("branch_id")]
        public int? BranchId { get; set; }

        [ForeignKey(nameof(BranchId))]
        public Branch? Branch { get; set; }

        [Column("pekerjaan")]
        public string? Pekerjaan { get; set; }

        [Column("jabatan")]
        public string? Jabatan { get; set; }
        [Column("kelurahan")]
        public string? KelurahanData { get; set; }
        [Column("kecamatan")]
        public string? KecamatanData { get; set; }
        [Column("city")]
        public string? CityData { get; set; }
        [Column("provinsi")]
        public string? ProvinsiData { get; set; }

        [Column("STG_DATE")]
        public DateTime? STG_DATE { get; set; }
    }
}
