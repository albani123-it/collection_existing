using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model.Entity
{

    [Table("master_collateral")]
    public class MasterCollateral
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int? Id { get; set; }

        [Column("loan_id")]
        public int? LoanId { get; set; }

        [ForeignKey(nameof(LoanId))]
        public MasterLoan? Loan { get; set; }

        [Column("col_id")]
        [StringLength(100)]
        [Unicode(false)]
        public string? ColId { get; set; }

        [Column("col_type")]
        [StringLength(20)]
        [Unicode(false)]
        public string? ColType { get; set; }

        [Column("veh_bpkb_no")]
        [StringLength(20)]
        [Unicode(false)]
        public string? VehBpkbNo { get; set; }

        [Column("veh_plate_no")]
        [StringLength(20)]
        [Unicode(false)]
        public string? VehPlateNo { get; set; }

        [Column("veh_merek")]
        [StringLength(40)]
        [Unicode(false)]
        public string? VehMerek { get; set; }

        [Column("veh_model")]
        [StringLength(30)]
        [Unicode(false)]
        public string? VehModel { get; set; }

        [Column("veh_bpkb_name")]
        [StringLength(60)]
        [Unicode(false)]
        public string? VehBpkbName { get; set; }

        [Column("veh_engine_no")]
        [StringLength(30)]
        [Unicode(false)]
        public string? VehEngineNo { get; set; }

        [Column("veh_chassis_no")]
        [StringLength(30)]
        [Unicode(false)]
        public string? VehChasisNo { get; set; }

        [Column("veh_stnk_no")]
        [StringLength(30)]
        [Unicode(false)]
        public string? VehStnkNo { get; set; }

        [Column("veh_year")]
        [StringLength(4)]
        [Unicode(false)]
        public string? VehYear { get; set; }

        [Column("veh_build_year")]
        [StringLength(4)]
        [Unicode(false)]
        public string? VehBuildYear { get; set; }

        [Column("veh_cc")]
        [StringLength(20)]
        [Unicode(false)]
        public string? VehCc { get; set; }

        [Column("veh_color")]
        [StringLength(50)]
        [Unicode(false)]
        public string? VehColor { get; set; }
    }
}
