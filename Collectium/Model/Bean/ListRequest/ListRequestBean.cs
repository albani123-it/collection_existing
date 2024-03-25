using Microsoft.VisualBasic.FileIO;

namespace Collectium.Model.Bean.ListRequest
{
    public class MenuRequestBean : PagedRequestBean
    {
        public int? Id { get; set; }

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }
    }

    public class RoleRequestBean : PagedRequestBean
    {
        public int? Id { get; set; }

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }
    }

    public class PermissionRequestBean : PagedRequestBean
    {
        public int? Id { get; set; }

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }
    }

    public class RolePermissionRequestBean : PagedRequestBean
    {
        public int? RoleId { get; set; }

    }

    public class SiteListRequestBean : PagedRequestBean
    {
        public int? Id { get; set; }

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }
    }

    public class AreaListRequestBean : PagedRequestBean
    {
        public int? Id { get; set; }

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }

        public int? SiteId { get; set; }
    }

    public class CameraListRequestBean : PagedRequestBean
    {
        public int? SiteId { get; set; }

    }

    public class RoleMenuListRequestBean : PagedRequestBean
    {
        public int? RoleId { get; set; }

        public int? MenuId { get; set; }
    }

    public class UserListRequestBean : PagedRequestBean
    {
        public int? Id { get; set; }

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }

    }

    public class UserReqListRequestBean : PagedRequestBean
    {
        public int? Id { get; set; }

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }

    }

    public class ActionListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }

    }

    public class ActionReqListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }

    }

    public class ActionGroupListRequestBean : PagedRequestBean
    {
        public string? Keyword { get; set; }

        public int? ActionId { get; set; }

        public int? RoleId { get; set; }

    }

    public class ActionGroupReqListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }

        public int? ActionId { get; set; }

        public int? RoleId { get; set; }

        public int? StatusId { get; set; }

    }

    public class AccDistListRequestBean : PagedRequestBean
    {
        public string? Keyword { get; set; }
    }

    public class AccDistReqListRequestBean : PagedRequestBean
    {
        public string? Keyword { get; set; }
    }

    public class BranchListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }

        public int? BranchCCOId { get; set; }

        public int? BranchTypeId { get; set; }

        public int? StatusId { get; set; }

    }

    public class BranchReqListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }

        public int? BranchCCOId { get; set; }

        public int? BranchTypeId { get; set; }

        public int? StatusId { get; set; }

    }

    public class CallScriptListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }


        public int? StatusId { get; set; }

    }

    public class CallScriptReqListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }


        public int? StatusId { get; set; }

    }

    public class NotifListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }

        public int? Day { get; set; }

        public int? StatusId { get; set; }

    }

    public class NotifReqListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }

        public int? Day { get; set; }

        public int? StatusId { get; set; }

    }

    public class GlobalListRequestBean : PagedRequestBean
    {
        public string? Keyword { get; set; }

    }

    public class GlobalReqListRequestBean : PagedRequestBean
    {
        public string? Keyword { get; set; }
    }

    public class RoleListRequestBean : PagedRequestBean
    {
        public int? Id { get; set; }

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }

    }

    public class RoleReqListRequestBean : PagedRequestBean
    {
        public int? Id { get; set; }

        public string? Keyword { get; set; }

        public int? StatusId { get; set; }

    }

    public class GenerateLetterRequestBean : PagedRequestBean
    {

        public int? LoanId { get; set; }

    }

    public class GenerateLetterHistoryBean : PagedRequestBean
    {

        public string? AccNo { get; set; }

        public string? Name { get; set; }

    }

    public class GenerateLetterPdfRequestBean : PagedRequestBean
    {

        public int? Id { get; set; }

        public string? Code { get; set; }

    }

    public class ReportMonitorRequest
    {

        public int? RoleId { get; set; }

        public int? BranchId { get; set; }

        public string? Start { get; set; }

        public string? End { get; set; }

    }

    public class ReportMonitorRequestFE
    {

        public int? BranchId { get; set; }

        public string? Start { get; set; }

        public string? End { get; set; }

    }

    public class FileUploadModel
    {
        public IFormFile? FileDetails { get; set; }

    }

    public class ReasonListRequestBean : PagedRequestBean
    {

        public string? Keyword { get; set; }

        public int? Day { get; set; }

        public int? StatusId { get; set; }

    }

    public class DashboardBeanV2
    {

        public int? BranchId { get; set; }

        public int? SpvId { get; set; }

        public int? AgentId { get; set; }

        public int? PeriodId { get; set; }

    }
}
