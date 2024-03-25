using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{

    [Table("fc_mapping_mikro_collection_req")]
    public class FcMappingMikroCollRequest
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("fc_mapping_mikro_collection_id")]
        public int? FcMappingMikroCollId { get; set; }

        [ForeignKey(nameof(FcMappingMikroCollId))]
        public FcMappingMikroColl? FcMappingMikroColl { get; set; }

        [Column("fc_id")]
        public int? FcId { get; set; }

        public User? Fc { get; set; }

        [Column("type_id")]
        public string? TypeId { get; set; }

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
