using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{
    [Table("collection_add_contact")]
    public class CollectionAddContact
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("add_id")]
        [StringLength(50)]
        [Unicode(false)]
        public string? AddId { get; set; }

        [Column("cu_cif")]
        [StringLength(20)]
        [Unicode(false)]
        public string? CuCif { get; set; }

        [Column("acc_no")]
        [StringLength(20)]
        [Unicode(false)]
        public string? AccNo { get; set; }

        [Column("add_phone")]
        [StringLength(30)]
        [Unicode(false)]
        public string? AddPhone { get; set; }

        [Column("add_address")]
        [StringLength(200)]
        [Unicode(false)]
        public string? AddAddress { get; set; }

        [Column("add_city")]
        [StringLength(50)]
        [Unicode(false)]
        public string? AddCity { get; set; }

        [Column("add_from")]
        [StringLength(25)]
        [Unicode(false)]
        public string? AddFrom { get; set; }

        [Column("lat")]
        public double? Lat { get; set; }

        [Column("lon")]
        public double? Lon { get; set; }

        [Column("add_date", TypeName = "datetime")]
        public DateTime? AddDate { get; set; }

        [Column("add_by")]
        public int? AddById { get; set; }

        [Column("def")]
        public int? Default { get; set; }

        [ForeignKey(nameof(AddById))]
        public User? AddBy { get; set; }

        public virtual ICollection<CollectionContactPhoto>? Photo { get; set; }
    }
}
