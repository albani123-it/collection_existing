using Collectium.Model.Bean.User;
using Collectium.Model.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Collectium.Model.Bean.Response
{
    public class CameraResponseBean
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }
    }

    public class CameraViewResponseBean
    {
        public string? LiveStreamUrl { get; set; }

        public string? Name { get; set; }

        public string? Location { get; set; }

        public int? CountIn { get; set; }

        public int? CountOut { get; set; }
    }

    public class CameraView2ResponseBean
    {
        public string? LiveStreamUrl { get; set; }

        public int? Id { get; set; }

        public string? CamId { get; set; }

        public string? CamName { get; set; }

        public string? Name { get; set; }

        public AreaResponseBean? Area { get; set; }
    }

    public class SiteDashboardViewResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public int? CountIn { get; set; }

        public int? CountOut { get; set; }

        public int? CountTotal { get; set; }

        public List<int>? History { get; set; }
    }

    public class AreaDashboardViewResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public SiteResponseBean? Site { get; set; }

        public int? CountIn { get; set; }

        public int? CountOut { get; set; }

        public int? CountTotal { get; set; }

        public List<int>? History { get; set; }
    }

    public class CameraDashboardViewResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? CamId { get; set; }

        public string? CamName { get; set; }

        public AreaResponseBean? Area { get; set; }

        public Boolean? Connected { get; set; }

    }

    public class CameraStatusBean
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("apiType")]
        public string ApiType { get; set; }

        [JsonPropertyName("available")]
        public bool Available { get; set; }

        [JsonPropertyName("connectionFailoverLevel")]
        public int ConnectionFailoverLevel { get; set; }

        [JsonPropertyName("connectionState")]
        public string ConnectionState { get; set; }

        [JsonPropertyName("firmwareVersion")]
        public string FirmwareVersion { get; set; }

        [JsonPropertyName("ipAddress")]
        public string IpAddress { get; set; }

        [JsonPropertyName("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("operatingPriority")]
        public int OperatingPriority { get; set; }

        [JsonPropertyName("physicalAddress")]
        public string PhysicalAddress { get; set; }

        [JsonPropertyName("serial")]
        public string Serial { get; set; }

        [JsonPropertyName("serverId")]
        public string ServerId { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("logicalId")]
        public int LogicalId { get; set; }

        [JsonPropertyName("perspective")]
        public string Perspective { get; set; }

        [JsonPropertyName("enableClientDewarping")]
        public bool EnableClientDewarping { get; set; }

        [JsonPropertyName("connected")]
        public bool Connected { get; set; }

    }

    public class ResultCameraStatusResponseBean
    {
        [JsonPropertyName("cameras")]
        public List<CameraStatusBean> Cameras { get; set; }
    }

    public class CameraStatusResponseBean
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("result")]
        public ResultCameraStatusResponseBean Result { get; set; }
    }

    public class MyProfileBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public Branch? Branch { get; set; }

        public ICollection<Menu>? Menu { get; set; }

        public RoleResponseBean? Role { get; set; }

        public StatusGeneralBean? Status { get; set; }

        public string? TelCode { get; set; }

        public string? TelDevice { get; set; }

        public string? TelIp { get; set; }

        public string? TelSecret { get; set; }

        public virtual ICollection<BranchResponseBean>? UserBranch { get; set; }
    }

    public class AreaResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public SiteResponseBean? Site { get; set; }
    }

    public class SiteResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? IpAddress { get; set; }

    }

    public class CameraDTOResponseBean
    {
        public int? Id { get; set; }

        public string? CamId { get; set; }

        public string? CamName { get; set; }

        public string? Name { get; set; }

        public string? LiveStreamUrl { get; set; }

        public AreaResponseBean? Area { get; set; }
    }

    public class MenuResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public int? Mobile { get; set; }

        public MenuResponseBean? Parent { get; set; }
    }

    public class RoleResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public ICollection<RoleMenuResponseBean>? RoleMenu { get; set; }

        public StatusGeneralBean? Status { get; set; }

    }

    public class DistributionResponseBean
    {
        public string? Result { get; set; }

    }

    public class PermissionResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public StatusGeneralBean? Status { get; set; }

    }

    public class RolePermissionResponseBean
    {

        public RoleResponseBean? Role { get; set; }

        public PermissionResponseBean? Permission { get; set; }

        public int? Create { get; set; }

        public int? Read { get; set; }

        public int? Update { get; set; }

        public int? Delete { get; set; }

        public int? Approve { get; set; }

        public int? Assign { get; set; }

    }

    public class RoleMenuResponseBean
    {
        public MenuResponseBean? Menu { get; set; }

    }


    public class UserResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }

        public string? Email { get; set; }

        public RoleResponseBean? Role { get; set; }

        public ICollection<BranchResponseBean>? Branch { get; set; }

        public StatusGeneralBean? Status { get; set; }

        public UserResponseBean? Spv { get; set; }

        public string? TelCode { get; set; }

        public string? TelDevice { get; set; }

        public string? PassDevice { get; set; }
    }

    public class UserReqResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }

        public string? Email { get; set; }

        public RoleResponseBean? Role { get; set; }

        public virtual ICollection<BranchResponseBean>? Branch { get; set; }

        public StatusRequestBean? Status { get; set; }

        public UserResponseBean? User { get; set; }
    }

    public class UserTraceResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public RoleResponseBean? Role { get; set; }

    }

    public class ActionResponseBean
    {
        public int? Id { get; set; }

        public string? ActCode { get; set; }

        public string? ActDesc { get; set; }

        public string? CoreCode { get; set; }

        public DateTime? CreateDate { get; set; }

        public StatusGeneralBean? Status { get; set; }
    }

    public class ActionReqResponseBean
    {
        public int? Id { get; set; }

        public ActionResponseBean? Action { get; set; }

        public string? ActCode { get; set; }

        public string? ActDesc { get; set; }

        public string? CoreCode { get; set; }

        public DateTime? CreateDate { get; set; }

        public StatusRequestBean? Status { get; set; }
    }

    public class ActionGroupResponseBean
    {
        public int? Id { get; set; }

        public ActionResponseBean? Action { get; set; }

        public RoleResponseBean? Role { get; set; }

        public StatusGeneralBean? Status { get; set; }
    }

    public class ActionGroupReqResponseBean
    {
        public int? Id { get; set; }

        public ActionGroupResponseBean? ActionGroup { get; set; }

        public ActionResponseBean? Action { get; set; }

        public RoleResponseBean? Role { get; set; }

        public StatusRequestBean? Status { get; set; }
    }

    public class AccDistResponseBean
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? Dpd { get; set; }

        public int? DpdMin { get; set; }

        public int? DpdMax { get; set; }

        public int? MaxPtp { get; set; }

        public string? CoreCode { get; set; }
    }

    public class AccDistReqResponseBean
    {
        public int? Id { get; set; }

        public AccDistResponseBean? AccAccountDistribution { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? Dpd { get; set; }

        public int? DpdMin { get; set; }

        public int? DpdMax { get; set; }

        public int? MaxPtp { get; set; }

        public string? CoreCode { get; set; }
    }

    public class BranchTypeResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }
    }

    public class BranchResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public BranchTypeResponseBean? BranchType { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

        public string? Addr1 { get; set; }

        public string? Addr2 { get; set; }

        public string? Addr3 { get; set; }

        public string? City { get; set; }

        public string? Zipcode { get; set; }

        public AreaResponseBean? Area { get; set; }

        //public BranchReqResponseBean? BranchCco { get; set; }

        public string? CoreCode { get; set; }

        public string? Pic { get; set; }

        public string? Email { get; set; }

        public string? Norek { get; set; }

        public double? Amount { get; set; }

        public DateTime? CreateDate { get; set; }

        public StatusGeneralBean? Status { get; set; }
    }

    public class BranchReqResponseBean
    {
        public int? Id { get; set; }

        public BranchResponseBean? Branch { get; set; }

        public string? Name { get; set; }

        public BranchTypeResponseBean? BranchType { get; set; }

        public string? Phone { get; set; }

        public string? Fax { get; set; }

        public string? Addr1 { get; set; }

        public string? Addr2 { get; set; }

        public string? Addr3 { get; set; }

        public string? City { get; set; }

        public string? Zipcode { get; set; }

        public AreaResponseBean? Area { get; set; }

        public BranchReqResponseBean? BranchCco { get; set; }

        public string? CoreCode { get; set; }

        public string? Pic { get; set; }

        public string? Email { get; set; }

        public string? Norek { get; set; }

        public double? Amount { get; set; }

        public DateTime? CreateDate { get; set; }

        public StatusRequestBean? Status { get; set; }
    }


    public class CallScriptResponseBean
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? CsDesc { get; set; }

        public int? AccdMin { get; set; }

        public int? AccdMax { get; set; }

        public string? CsScript { get; set; }

        public DateTime? CreateDate { get; set; }

        public StatusGeneralBean? Status { get; set; }
    }

    public class CallScriptReqResponseBean
    {
        public int? Id { get; set; }

        public CallScriptResponseBean? CallScript { get; set; }

        public string? Code { get; set; }

        public string? CsDesc { get; set; }

        public int? AccdMin { get; set; }

        public int? AccdMax { get; set; }

        public string? CsScript { get; set; }

        public DateTime? CreateDate { get; set; }

        public StatusRequestBean? Status { get; set; }
    }

    public class NotifResponseBean
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? Day { get; set; }

        public string? Content { get; set; }

        public DateTime? CreateDate { get; set; }

        public StatusGeneralBean? Status { get; set; }
    }

    public class NotifReqResponseBean
    {
        public int? Id { get; set; }

        public NotifResponseBean? NotifContent { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? Day { get; set; }

        public string? Content { get; set; }

        public DateTime? CreateDate { get; set; }

        public StatusRequestBean? Status { get; set; }
    }

    public class GlobalResponseBean
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Val { get; set; }

        public StatusGeneralBean? Status { get; set; }
    }

    public class GlobalReqResponseBean
    {
        public int? Id { get; set; }

        public GlobalResponseBean? Global { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Val { get; set; }

        public StatusGeneralBean? Status { get; set; }

    }

    public class RoleListResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public RoleResponseBean? Role { get; set; }

        public StatusGeneralBean? Status { get; set; }
    }

    //public class RoleListRequestBean
    //{
    //    public int? Id { get; set; }

    //    public string? Name { get; set; }

    //    public string? Username { get; set; }

    //    public string? Email { get; set; }

    //    public RoleResponseBean? Role { get; set; }

    //    public StatusGeneralBean? Status { get; set; }
    //}

    //public class RoleReqListRequestBean
    //{
    //    public int? Id { get; set; }

    //    public string? Name { get; set; }

    //    public string? Username { get; set; }

    //    public string? Email { get; set; }

    //    public RoleResponseBean? Role { get; set; }

    //    public StatusGeneralBean? Status { get; set; }
    //}

    public class RoleReqResponseBean
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public RoleResponseBean? Role { get; set; }

        public StatusRequestBean? Status { get; set; }

        public UserResponseBean? User { get; set; }
    }

    public class GenerateLetterResponseBean
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? NoSurat { get; set; }


        public DateTime? CreateDate { get; set; }

        public StatusGeneralBean? Status { get; set; }
    }

    public class GenerateLetterHistoryResponseBean
    {
        public int? Id { get; set; }

        public string? AccNo { get; set; }

        public string? Cabang { get; set; }

        public string? Name { get; set; }

        public string? NoSurat { get; set; }

        public string? TglSurat { get; set; }

        public string? JenisSurat { get; set; }

    }

    public class ReasonReqResponseBean
    {
        public int? Id { get; set; }

        public ReasonResponseBean? Reason { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? isDC { get; set; }

        public int? isFC { get; set; }

        public DateTime? CreateDate { get; set; }

        public StatusRequestBean? Status { get; set; }
    }

    public class DashboardBean
    {
        public DashboardBean()
        {
            this.Fuim = 0;
            this.Ll = 0;
            this.Pay = 0;
            this.Ptp = 0;
            this.Vt = 0;
            this.Mess = 0;
            this.Rem = 0;
            this.Mess = 0;
            this.Noas = 0;
            this.Noas1 = 0;
            this.Noas2 = 0;
            this.Noas3 = 0;
        }

        public int? Fuim { get; set; }

        public int? Ll { get; set; }

        public int? Mess { get; set; }

        public int? Noas { get; set; }

        public int? Noas1 { get; set; }

        public int? Noas2 { get; set; }

        public int? Noas3 { get; set; }

        public int? Pay { get; set; }

        public int? Ptp { get; set; }

        public int? Vt { get; set; }

        public int? Rem { get; set; }

        public double? KewajibanAmount { get; set; }

        public double? PayAmount { get; set; }

    }

    public class DashboardWrapper
    {
        public DashboardBean? Data { get; set; }

    }

    public class DashboardTreeSpv
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public List<DashboardTreeSpv>? Agent { get; set; }   

    }

    public class DashboardTreeSpvWrapper
    {
        public List<DashboardTreeSpv>? Spv { get; set; }

    }

    public class CallResultResponse
    {
        public string? Code { get; set; }

        public string? Description { get; set; }

    }

    public class CollTraceResponseBean
    { 

        public string? AccNo { get; set; }

        public string? Name { get; set; }

        public double? Amount { get; set; }

        public DateTime? TraceDate { get; set; }

        public int? Kolek { get; set; }

        public int? DPD { get; set; }

        public UserTraceResponseBean? CallBy { get; set; }

        public CallResultColResponseBean? Result { get; set; }
    }

    public class AddressLatLonResponseBean
    {

        public string? AccNo { get; set; }

        public string? CuCif { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }

    }

    public class PayRecordResponseBean
    {

        public string? AccNo { get; set; }

        public string? Name { get; set; }

        public double? Amount { get; set; }

        public DateTime? RecordDate { get; set; }


        public UserTraceResponseBean? CallBy { get; set; }

        public UserTraceResponseBean? Spv { get; set; }

    }


    public class NewDailyResponse
    {

        public string? AccNo { get; set; }

        public int? Dpd { get; set; }

        public double? TunggakanTotal { get; set; }

        public UserTraceResponseBean? CallBy { get; set; }

        public UserTraceResponseBean? Spv { get; set; }

        public DateTime? STG_DATE { get; set; }


        public CollResponseBean? Loan { get; set; }

        public string? Name { get; set; }

    }
}
