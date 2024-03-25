using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{

    [Table("rfkabupaten")]
    public class City
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("code")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Code { get; set; }

        [Column("name")]
        [StringLength(200)]
        [Unicode(false)]
        public string? Name { get; set; }

        [Column("kode_wil_sikp")]
        [StringLength(50)]
        [Unicode(false)]
        public string? KodeWilSikp { get; set; }

        [Column("provinsi_id")]
        public int? ProvinsiId { get; set; }

        [ForeignKey(nameof(ProvinsiId))]
        public Provinsi? Provinsi { get; set; }

        [Column("create_date")]
        public DateTime? CreateDate { get; set; }

        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }

        [Column("status_id")]
        public int? StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public StatusGeneral? Status { get; set; }

    }
}
