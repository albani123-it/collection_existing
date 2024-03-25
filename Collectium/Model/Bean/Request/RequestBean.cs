using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Collectium.Model.Entity;

namespace Collectium.Model.Bean.Request
{
    public class AvigilonRequestBean
    {
    }

    public class CameraViewRequestBean
    {
        public int? Id { get; set; }
    }

    public class AreaDashboardViewRequestBean
    {
        public int? SiteId { get; set; }
    }

    public class CameraDashboardViewRequestBean
    {
        public int? AreaId { get; set; }
    }

    public class LoginRequestBean
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
    }

    public class SiteRequestBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? IpAddress { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? SecretKey { get; set; }

        public string? ClientName { get; set; }

        public string? Nonce { get; set; }
    }

    public class AreaRequestBean
    {
        public string? Name { get; set; }

        public int? SiteId { get; set; }
    }

    public class CameraRequestBean
    {
        public string? Name { get; set; }

        public string? CamId { get; set; }

        public string? CamName { get; set; }

        public int? AreaId { get; set; }
    }

    public class MenuCreateBean
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public int? Mobile { get; set; }
    }


    public class RoleCreateBean
    {

        public string? Name { get; set; }

        public IList<int>? Permission { get; set; }

    }


    public class PermissionCreateBean
    {
        public int? Id { get; set; }

        public string Name { get; set; }

    }

    public class RolePermissionCreateBean
    {
        public int? RoleId { get; set; }

        public int? PermissionId { get; set; }

        public int? Create { get; set; }

        public int? Read { get; set; }

        public int? Update { get; set; }

        public int? Delete { get; set; }

        public int? Approve { get; set; }

        public int? Assign { get; set; }

    }

    public class RoleMenuCreateBean
    {
        public int? RoleId { get; set; }

        public int? MenuId { get; set; }

    }

    public class UserCreateBean
    {
        public int? Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }


        public string? Password { get; set; }

        public int? RoleId { get; set; }
    }

    public class UserReqCreateBean
    {
        public int? Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }


        public string? Password { get; set; }

        public int? RoleId { get; set; }

        public int? UserId { get; set; }

        public int? SpvId { get; set; }

        public virtual ICollection<UserBranchRequest>? Branch { get; set; }
    }

    public class UserReqApproveBean
    {
        public int? Id { get; set; }

    }

    public class UserReqActiveBranch
    {
        public int? UserId { get; set; }
        public int? BranchId { get; set; }

    }

    public class ActionCreateBean
    {

        public string? ActCode { get; set; }

        public string? ActDesc { get; set; }

        public string? CoreCode { get; set; }

    }

    public class ActionReqCreateBean
    {

        public string? ActCode { get; set; }

        public string? ActDesc { get; set; }

        public string? CoreCode { get; set; }

    }

    public class ActionReqEditBean
    {
        public int? ActionId { get; set; }

        public string? ActCode { get; set; }

        public string? ActDesc { get; set; }

        public string? CoreCode { get; set; }

    }

    public class ActionGroupCreateBean
    {

        public string? ActionId { get; set; }

        public string? RoleId { get; set; }

    }

    public class ActionGroupReqCreateBean
    {

        public string? ActionId { get; set; }

        public string? RoleId { get; set; }

    }

    public class ActionGroupReqEditBean
    {
        public int? ActionGroupId { get; set; }

        public string? ActionId { get; set; }

        public string? RoleId { get; set; }

    }

    public class AccDistCreateBean
    {

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? Dpd { get; set; }

        public int? DpdMin { get; set; }

        public int? DpdMax { get; set; }

        public int? MaxPtp { get; set; }

        public string? CoreCode { get; set; }

    }

    public class AccDistReqCreateBean
    {

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? Dpd { get; set; }

        public int? DpdMin { get; set; }

        public int? DpdMax { get; set; }

        public int? MaxPtp { get; set; }

        public string? CoreCode { get; set; }

    }

    public class AccDistReqEditBean
    {

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? AccountDistributionId { get; set; }

        public int? Dpd { get; set; }

        public int? DpdMin { get; set; }

        public int? DpdMax { get; set; }

        public int? MaxPtp { get; set; }

        public string? CoreCode { get; set; }

    }

    public class AreaCreateBean
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? CoreCode { get; set; }
    }

    public class BranchTypeCreateBean
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

    }

    public class BranchCreateBean
    {

        public string? Name { get; set; }

        public int? BranchTypeId { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

        public string? Addr1 { get; set; }

        public string? Addr2 { get; set; }

        public string? Addr3 { get; set; }

        public string? City { get; set; }

        public string? Zipcode { get; set; }

        public int? AreaId { get; set; }

        public int? BranchCcoId { get; set; }

        public string? CoreCode { get; set; }

        public string? Pic { get; set; }

        public string? Email { get; set; }

        public string? Norek { get; set; }

        public double? Amount { get; set; }

    }

    public class BranchIntegrationCreateBean
    {

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? BrType { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

        public string? Addr1 { get; set; }

        public string? Addr2 { get; set; }

        public string? Addr3 { get; set; }

        public string? City { get; set; }

        public string? Zipcode { get; set; }

        public string? Areaid { get; set; }

        public string? Ccobranch { get; set; }

        public string? CoreCode { get; set; }

        public string? Pic { get; set; }

        public string? Email { get; set; }

        public string? Norek { get; set; }

        public double? Amount { get; set; }

    }

    public class BranchReqCreateBean
    {

        public string? Name { get; set; }

        public int? BranchTypeId { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

        public string? Addr1 { get; set; }

        public string? Addr2 { get; set; }

        public string? Addr3 { get; set; }

        public string? City { get; set; }

        public string? Zipcode { get; set; }

        public int? AreaId { get; set; }

        public int? BranchCcoId { get; set; }

        public string? CoreCode { get; set; }

        public string? Pic { get; set; }

        public string? Email { get; set; }

        public string? Norek { get; set; }

        public double? Amount { get; set; }

    }

    public class BranchReqEditCreateBean
    {

        public string? Name { get; set; }

        public int? BranchId { get; set; }

        public int? BranchTypeId { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

        public string? Addr1 { get; set; }

        public string? Addr2 { get; set; }

        public string? Addr3 { get; set; }

        public string? City { get; set; }

        public string? Zipcode { get; set; }

        public int? AreaId { get; set; }

        public int? BranchCcoId { get; set; }

        public string? CoreCode { get; set; }

        public string? Pic { get; set; }

        public string? Email { get; set; }

        public string? Norek { get; set; }

        public double? Amount { get; set; }

    }

    public class CityNewCodeRequestBean
    {
        public string? Code { get; set; }
        public string? Name { get; set; }

        public string? KodeWilSikp { get; set; }

        public string? ProvinsiCode { get; set; }

    }

    public class KecamatanNewCodeRequestBean
    {
        public string? Code { get; set; }
        public string? Name { get; set; }

        public string? CityCode { get; set; }

    }

    public class KelurahanNewCodeRequestBean
    {
        public string? Code { get; set; }
        public string? Name { get; set; }

        public string? KdDkcplKelurahan { get; set; }

        public string? KecamatanCode { get; set; }

    }

    public class UserNewRequestBean
    {
        public string? Userid { get; set; }
        public string? Groupid { get; set; }

        public string? SuFullname { get; set; }

        public string? SuPwd { get; set; }
        public string? SuIdNo { get; set; }

        public string? SuHpnum { get; set; }

        public string? SuEmail { get; set; }

        public string? SuNip { get; set; }


        public string? SuUpliner { get; set; }

        public string? SuBranchCode { get; set; }

        public bool? SuActive { get; set; }

        public string? SuCore2code { get; set; }

        public string? SuCorecode { get; set; }

        public string? SuJabatan { get; set; }

        public string? SuEmasJabcode { get; set; }

        public bool? SuLoadb { get; set; }

        public bool? SuSharia { get; set; }

        public string? RegcolId { get; set; }

        public string? ArcolId { get; set; }

        public int? Amount { get; set; }

        public string? SuPicture { get; set; }

    }

    public class TeamNewCodeRequestBean
    {
        public string? Spv { get; set; }
        public string? BranchCode { get; set; }

        public IList<string>? Member { get; set; }


    }

    public class ApiMasterLoan
    {

        public string? CuCif { get; set; }
        public string? AccNo { get; set; }

        public string? Ccy { get; set; }

        public string? Product { get; set; }

        public double? Plafond { get; set; }

        public DateTime? MaturityDate { get; set; }

        public DateTime? StartDate { get; set; }

        public int? SisaTenor { get; set; }

        public int? Tenor { get; set; }

        public double? InstallmentPokok { get; set; }

        public double? InterestRate { get; set; }

        public double? Installment { get; set; }

        public double? TunggakanPokok { get; set; }

        public double? TunggakanBunga { get; set; }

        public double? TunggakanDenda { get; set; }

        public double? TunggakanTotal { get; set; }

        public double? KewajibanTotal { get; set; }

        public DateTime? LastPayDate { get; set; }

        public double? Outstanding { get; set; }

        public double? PayTotal { get; set; }

        public int? Dpd { get; set; }

        public int? Kolektibilitas { get; set; }

        public string? EconName { get; set; }

        public string? EconPhone { get; set; }

        public string? EconRelation { get; set; }
        public string? MarketingCode { get; set; }

        public string? ChannelBranchCode { get; set; }
    }

    public class ApiMasterCustomer
    {

        public string? CuCif { get; set; }
        public string? CuName { get; set; }

        public DateTime? CuBorndate { get; set; }

        public string? CuBornplace { get; set; }

        public string? CuIdtype { get; set; }

        public string? CuIdnumber { get; set; }

        public string? CuGender { get; set; }

        public string? CuMaritalstatus { get; set; }

        public string? CuNationality { get; set; }

        public string? CuIncometype { get; set; }

        public string CuIncome { get; set; }

        public string? CuCusttype { get; set; }

        public string? CuOccupation { get; set; }

        public string? CuCompany { get; set; }

        public string? CuEmail { get; set; }

        public string? CuAddress { get; set; }

        public string? CuRt { get; set; }

        public string? CuRw { get; set; }

        public string? CuKelurahan { get; set; }

        public string? CuKecamatan { get; set; }

        public string? CuCity { get; set; }

        public string? CuProvinsi { get; set; }

        public string? CuZipcode { get; set; }

        public string? CuHmphone { get; set; }

        public string? CuMobilephone { get; set; }

        public string? Branchid { get; set; }
    }

    public class ApiMasterCollateral
    {

        public string? AccNo { get; set; }
        public string? ColId { get; set; }

        public string? ColType { get; set; }

        public string? VehBpkbNo { get; set; }

        public string? VehPlateNo { get; set; }

        public string? VehMerek { get; set; }

        public string? VehModel { get; set; }

        public string? VehBpkbName { get; set; }

        public string? VehEngineNo { get; set; }

        public string? VehChasisNo { get; set; }

        public string? VehStnkNo { get; set; }

        public string? VehYear { get; set; }

        public string? VehBuildYear { get; set; }

        public string? VehCc { get; set; }

        public string? VehColor { get; set; }
    }

    public class ApiCollectionVisit
    {

        public string? VisitId { get; set; }
        public string? Branchid { get; set; }

        public string? AccNo { get; set; }

        public string? VisitName { get; set; }

        public string? AddId { get; set; }

        public string? VisitReason { get; set; }

        public string? VisitResult { get; set; }

        public DateTime? VisitResultDate { get; set; }

        public double? VisitAmount { get; set; }

        public string? VisitNotes { get; set; }

        public DateTime? VisitDate { get; set; }

        public string? VisitBy { get; set; }

        public string? Longitude { get; set; }

        public string? Latitude { get; set; }

        public string? Picture { get; set; }

        public string? UbmId { get; set; }

        public string? CbmId { get; set; }
        public string? Kolek { get; set; }
    }

    public class ApiCollectionAddContact
    {
        public string? AddId { get; set; }

        public string? CuCif { get; set; }

        public string? AccNo { get; set; }

        public string? AddPhone { get; set; }

        public string? AddAddress { get; set; }

        public string? AddCity { get; set; }

        public string? AddFrom { get; set; }

        public DateTime? AddDate { get; set; }

        public string? AddBy { get; set; }
    }

    public class ApiCollectionCall
    {

        public string? Branchid { get; set; }
        public string? AccNo { get; set; }
        public string? CallName { get; set; }
        public string? AddId { get; set; }
        public string? CallReason { get; set; }
        public string? CallResult { get; set; }
        public DateTime? CallResultDate { get; set; }
        public double? CallAmount { get; set; }
        public string? CallNotes { get; set; }
        public DateTime? CallDate { get; set; }
        public string? CallBy { get; set; }
        public string? CallResultHh { get; set; }

        public string? CallResultMm { get; set; }

        public string? CallResultHhmm { get; set; }
    }

    public class ApiLoanNewRequestBean
    {
        public ApiMasterLoan? Loan { get; set; }

        public ApiMasterCustomer? Customer { get; set; }

        public IList<ApiMasterCollateral>? Collateral { get; set; }

        public IList<ApiCollectionVisit>? Visit { get; set; }

        public IList<ApiCollectionAddContact>? Address { get; set; }

        public IList<ApiCollectionCall>? Call { get; set; }

        public IList<ApiCollectionHistory>? History { get; set; }

        public string? Member { get; set; }

    }

    public class ApiCollectionHistory
    {

        public int? BranchId { get; set; }

        public string? AccNo { get; set; }

        public string? Name { get; set; }

        public string? AddId { get; set; }

        public string? ReasonId { get; set; }

        public string? ResultId { get; set; }

        public DateTime? ResultDate { get; set; }

        public double? Amount { get; set; }

        public string? Notes { get; set; }

        public DateTime? HistoryDate { get; set; }

        public string? HistoryById { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string? Picture { get; set; }

        public string? UbmId { get; set; }

        public string? CbmId { get; set; }

        public string? Kolek { get; set; }

        public string? ResultHh { get; set; }

        public string? ResultMm { get; set; }

        public string? ResultHhmm { get; set; }
    }

    public class CallScriptCreateBean
    {

        public string? Code { get; set; }

        public string? CsDesc { get; set; }

        public int? AccdMin { get; set; }

        public int? AccdMax { get; set; }

        public string? CsScript { get; set; }

    }

    public class CallScriptReqEditBean
    {
        public int? CallScriptId { get; set; }

        public string? Code { get; set; }

        public string? CsDesc { get; set; }

        public int? AccdMin { get; set; }

        public int? AccdMax { get; set; }

        public string? CsScript { get; set; }


    }

    public class NotifCreateBean
    {

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Content { get; set; }

        public int? Day { get; set; }

    }

    public class NotifReqEditBean
    {
        public int? NotifContentId { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Content { get; set; }

        public int? Day { get; set; }

    }

    public class PaymentHistoryBean
    {

        public string? AccNo { get; set; }

        public DateTime? Tgl { get; set; }

        public double? PokokCicilan { get; set; }

        public double? Bunga { get; set; }

        public double? Denda { get; set; }

        public double? TotalBayar { get; set; }


    }


    public class GlobalCreateBean
    {

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Val { get; set; }

    }

    public class GlobalEditBean
    {

        public int? Id {  get; set; }  

        public string? Val { get; set; }


    }

    public class ListTeamBean
    {

        public string? Rolename { get; set; }

    }

    public class ReasonCreateBean
    {

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? isDC { get; set; }

        public int? isFC { get; set; }

    }

    public class ReasonReqEditBean
    {

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? isDC { get; set; }

        public int? isFC { get; set; }

        public int? ReasonId { get; set; }

    }

    public class SettingDcBean
    {

        public int? Dpd { get; set; }

        public int? DpdGanti { get; set; }

        public int? Cabang { get; set; }


    }
}
