using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Entity
{

    [Table("restructure_cashflow")]

    public class RestructureCashFlow
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("penghasilan_nasabah")]
        public int? PenghasilanNasabah { get; set; }

        [Column("penghasilan_pasangan")]
        public int? PenghasilanPasangan { get; set; }

        [Column("penghasilan_lainnya")]
        public int? PenghasilanLainnya { get; set; }

        [Column("total_penghasilan")]
        public int? TotalPenghasilan { get; set; }

        [Column("biaya_pendidikan")]
        public int? BiayaPendidikan { get; set; }

        [Column("biaya_listrik_air_telp")]
        public int? BiayaListrikAirTelp { get; set; }

        [Column("biaya_belanja")]
        public int? BiayaBelanja { get; set; }

        [Column("biaya_transportasi")]
        public int? BiayaTransportasi { get; set; }

        [Column("biaya_lainnya")]
        public int? BiayaLainnya { get; set; }

        [Column("total_pengeluaran")]
        public int? TotalPengeluaran { get; set; }

        [Column("hutang_di_bank")]
        public int? HutangDiBank { get; set; }

        [Column("cicilan_lainnya")]
        public int? CicilanLainnya { get; set; }

        [Column("total_kewajiban")]
        public int? TotalKewajiban { get; set; }

        [Column("penghasilan_bersih")]
        public int? PenghasilanBersih { get; set; }

        [Column("rpc_70_persen")]
        public int? Rpc70Persen { get; set; }

        [Column("restructure_id")]
        public int? RestructureId { get; set; }

        [ForeignKey("RestructureId")]
        public Restructure? Restructure { get; set; }
    }
}
