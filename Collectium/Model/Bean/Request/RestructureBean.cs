using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Bean.Request
{
    public class UploadDocumentRestructure
    {
        public int? DocTypeId { get; set; }

        public IFormFile? File { get; set; }
    }

    public class ApprovalLevelRestructure
    {
        public int? Id { get; set; }

        public string? Comment { get; set; }
    }

    public class RestructureCashFlowBean
    {
        public int? PenghasilanNasabah { get; set; }

        public int? PenghasilanPasangan { get; set; }

        public int? PenghasilanLainnya { get; set; }


        public int? TotalPenghasilan { get; set; }

        public int? BiayaPendidikan { get; set; }

        public int? BiayaListrikAirTelp { get; set; }

        public int? BiayaBelanja { get; set; }

        public int? BiayaTransportasi { get; set; }

        public int? BiayaLainnya { get; set; }

        public int? TotalPengeluaran { get; set; }

        public int? HutangDiBank { get; set; }

        public int? CicilanLainnya { get; set; }

        public int? TotalKewajiban { get; set; }

        public int? PenghasilanBersih { get; set; }

        public int? Rpc70Persen { get; set; }
    }

    public class CreateRestructure
    {
        public int? LoanId { get; set; }

        public double? PrincipalPembayaran { get; set; }

        public double? MarginPembayaran { get; set; }

        public double? PrincipalPinalty { get; set; }

        public double? MarginPinalty { get; set; }

        public DateTime? TglJatuhTempoBaru { get; set; }

        public string? Keterangan { get; set; }

        public int? GracePeriode { get; set; }

        public int? PenguranganNilaiMargin { get; set; }

        public DateTime? TglAwalPeriodeDiskon { get; set; }

        public DateTime? TglAkhirPeriodeDiskon { get; set; }

        public int? PeriodeDiskon { get; set; }

        public DateTime? ValueDate { get; set; }

        public double? DiskonTunggakanMargin { get; set; }

        public double? DiskonTunggakanDenda { get; set; }

        public double? Margin { get; set; }

        public double? Denda { get; set; }

        public double? MarginAmount { get; set; }

        public double? TotalDiskonMargin { get; set; }

        public int? PolaRestrukId { get; set; }

        public int? PembayaranGpId { get; set; }

        public int? JenisPenguranganId { get; set; }

        public List<string>? Permasalahan { get; set; }

        public RestructureCashFlowBean? CashFlow { get; set; }

        public List<int>? Document { get; set; }
    }

    public class SubmitRestructure
    {
        public int? Id { get; set; }

        public double? PrincipalPembayaran { get; set; }

        public double? MarginPembayaran { get; set; }

        public double? PrincipalPinalty { get; set; }

        public double? MarginPinalty { get; set; }

        public DateTime? TglJatuhTempoBaru { get; set; }

        public string? Keterangan { get; set; }

        public int? GracePeriode { get; set; }

        public int? PenguranganNilaiMargin { get; set; }

        public DateTime? TglAwalPeriodeDiskon { get; set; }

        public DateTime? TglAkhirPeriodeDiskon { get; set; }

        public int? PeriodeDiskon { get; set; }

        public DateTime? ValueDate { get; set; }

        public double? DiskonTunggakanMargin { get; set; }

        public double? DiskonTunggakanDenda { get; set; }

        public double? Margin { get; set; }

        public double? Denda { get; set; }

        public double? MarginAmount { get; set; }

        public double? TotalDiskonMargin { get; set; }

        public int? PolaRestrukId { get; set; }

        public int? PembayaranGpId { get; set; }

        public int? JenisPenguranganId { get; set; }

        public RestructureCashFlowBean? CashFlow { get; set; }

        public List<int>? Document { get; set; }
    }

    public class RestructureDetailResponseBean
    {
        public int? Id { get; set; }

        public CollDetailResponseBean? Loan { get; set; }

        public double? PrincipalPembayaran { get; set; }

        public double? MarginPembayaran { get; set; }

        public double? PrincipalPinalty { get; set; }

        public double? MarginPinalty { get; set; }

        public DateTime? TglJatuhTempoBaru { get; set; }

        public string? Keterangan { get; set; }

        public int? GracePeriode { get; set; }

        public int? PenguranganNilaiMargin { get; set; }

        public DateTime? TglAwalPeriodeDiskon { get; set; }

        public DateTime? TglAkhirPeriodeDiskon { get; set; }

        public int? PeriodeDiskon { get; set; }

        public DateTime? ValueDate { get; set; }

        public double? DiskonTunggakanMargin { get; set; }

        public double? DiskonTunggakanDenda { get; set; }

        public double? Margin { get; set; }

        public double? Denda { get; set; }

        public double? MarginAmount { get; set; }

        public double? TotalDiskonMargin { get; set; }

        public int? PolaRestrukId { get; set; }

        public int? PembayaranGpId { get; set; }

        public int? JenisPenguranganId { get; set; }

        public RestructureCashFlowBean? CashFlowBean { get; set; }

        public List<int>? Document { get; set; }

        public List<string>? Masalah { get; set; }
    }

    public class CreateLelang
    {
        public int? LoanId { get; set; }

        public int? AlasanLelangId { get; set; }

        public string? NoPK { get; set; }

        public double? NilaiLimitLelang { get; set; }

        public double? UangJaminan { get; set; }

        public string? ObjekLelang { get; set; }

        public string? Keterangan { get; set; }


        public List<int>? Document { get; set; }
    }

    public class SubmitLelang
    {
        public int? Id { get; set; }

        public int? AlasanLelangId { get; set; }

        public string? NoPK { get; set; }

        public double? NilaiLimitLelang { get; set; }

        public double? UangJaminan { get; set; }

        public string? ObjekLelang { get; set; }

        public string? Keterangan { get; set; }


        public List<int>? Document { get; set; }
    }

    public class LelangDetailResponseBean
    {
        public int? Id { get; set; }

        public CollDetailResponseBean? Loan { get; set; }

        public GenericParameterBean? AlasanLelang { get; set; }

        public string? NoPK { get; set; }

        public double? NilaiLimitLelang { get; set; }

        public double? UangJaminan { get; set; }

        public string? ObjekLelang { get; set; }

        public string? Keterangan { get; set; }

        public GenericParameterBean? BalaiLelang { get; set; }

        public GenericParameterBean? JenisLelang { get; set; }

        public string? TataCaraLelang { get; set; }

        public double? BiayaLelang { get; set; }

        public string? CatatanLelang { get; set; }

        public DateTime? TglPenetapanLelang { get; set; }

        public string? NoRekening { get; set; }

        public string? NamaRekening { get; set; }

        public List<int>? Document { get; set; }
    }

    public class CreateAsuransi
    {
        public int? LoanId { get; set; }

        public string? NamaPejabat { get; set; }

        public string? Jabatan { get; set; }

        public string? NoSertifikat { get; set; }

        public DateTime? TglSertifikat { get; set; }


        public int? AsuransiId { get; set; }

        public string? NoPolis { get; set; }

        public DateTime? TglPolis { get; set; }

        public string? NoPk { get; set; }


        public double? TunggakanPokok70Persen { get; set; }

        public double? TunggakanBunga70Persen { get; set; }

        public string? CatatanPolis { get; set; }


        public string? Keterangan { get; set; }


        public List<int>? Document { get; set; }
    }

    public class SubmitAsuransi
    {
        public int? Id { get; set; }

        public string? NamaPejabat { get; set; }

        public string? Jabatan { get; set; }

        public string? NoSertifikat { get; set; }

        public DateTime? TglSertifikat { get; set; }


        public int? AsuransiId { get; set; }

        public string? NoPolis { get; set; }

        public DateTime? TglPolis { get; set; }

        public string? NoPk { get; set; }


        public double? TunggakanPokok70Persen { get; set; }

        public double? TunggakanBunga70Persen { get; set; }

        public string? CatatanPolis { get; set; }


        public string? Keterangan { get; set; }


        public List<int>? Document { get; set; }
    }

    public class AsuransiDetailResponseBean
    {
        public int? Id { get; set; }

        public CollDetailResponseBean? Loan { get; set; }

        public string? NamaPejabat { get; set; }

        public string? Jabatan { get; set; }

        public string? NoSertifikat { get; set; }

        public DateTime? TglSertifikat { get; set; }

        public GenericParameterBean? Asuransi { get; set; }


        public string? NoPolis { get; set; }

        public DateTime? TglPolis { get; set; }

        public string? NoPk { get; set; }


        public double? TunggakanPokok70Persen { get; set; }

        public double? TunggakanBunga70Persen { get; set; }

        public string? CatatanPolis { get; set; }

        public string? Keterangan { get; set; }

        public double? NilaiKlaim { get; set; }

        public double? NilaiKlaimDibayar { get; set; }

        public DateTime? TglKlaimDibayar { get; set; }

        public GenericParameterBean? AsuransiSisaKlaim { get; set; }

        public double? BakiDebitKlaim { get; set; }

        public string? CatatanKlaim { get; set; }


        public string? Permasalahan { get; set; }

        public List<int>? Document { get; set; }
    }

    public class CreateAyda
    {
        public int? LoanId { get; set; }

        public int? HubunganBankIdId { get; set; }

        public DateTime? TglAmbilAlih { get; set; }

        public string? Kualitas { get; set; }

        public double? NilaiPembiayaanPokok { get; set; }

        public double? NilaiMargin { get; set; }

        public double? NilaiPerolehanAgunan { get; set; }

        public double? PerkiraanBiayaPenjualan { get; set; }

        public double? Ppa { get; set; }

        public double? JumlahAyda { get; set; }


        public List<int>? Document { get; set; }
    }

    public class SubmitAyda
    {
        public int? Id { get; set; }

        public int? HubunganBankIdId { get; set; }

        public DateTime? TglAmbilAlih { get; set; }

        public string? Kualitas { get; set; }

        public double? NilaiPembiayaanPokok { get; set; }

        public double? NilaiMargin { get; set; }

        public double? NilaiPerolehanAgunan { get; set; }

        public double? PerkiraanBiayaPenjualan { get; set; }

        public double? Ppa { get; set; }

        public double? JumlahAyda { get; set; }


        public List<int>? Document { get; set; }
    }

    public class AydaDetailResponseBean
    {
        public int? Id { get; set; }

        public CollDetailResponseBean? Loan { get; set; }

        public GenericParameterBean? HubunganBank { get; set; }

        public DateTime? TglAmbilAlih { get; set; }

        public string? Kualitas { get; set; }

        public double? NilaiPembiayaanPokok { get; set; }

        public double? NilaiMargin { get; set; }

        public double? NilaiPerolehanAgunan { get; set; }

        public double? PerkiraanBiayaPenjualan { get; set; }

        public double? Ppa { get; set; }

        public double? JumlahAyda { get; set; }

        public List<int>? Document { get; set; }
    }
}
