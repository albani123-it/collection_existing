using AutoMapper;
using Collectium.Config;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Service;
using Collectium.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IseeBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly StatusService status;
        private readonly InitialService inital;
        private readonly UserService service;
        private readonly RoleMenuService menuService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> _logger,
                                UserService service,
                                StatusService status,
                                InitialService inital,
                                RoleMenuService menuService)
        {
            this._logger = _logger;
            this.status = status;
            this.inital = inital;
            this.service = service;
            this.menuService = menuService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/api/[controller]/login")]
        public GenericResponse<string> Login(LoginRequestBean filter)
        {
            return this.service.Login(filter, true);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/api/[controller]/login/mobile")]
        public GenericResponse<string> LoginMobile(LoginRequestBean filter)
        {
            return this.service.Login(filter, true);
        }

        [HttpGet]
        [Route("/api/[controller]/my/profile")]
        public GenericResponse<MyProfileBean> MyProfile()
        {
            return this.service.GetMyProfile();
        }

        [HttpGet]
        [Route("/api/[controller]/my/dashboard")]
        public GenericResponse<DashboardWrapper> Dashboard([FromQuery] DashboardBeanV2 filter)
        {
            return this.service.Dashboard(filter);
        }

        [HttpGet]
        [Route("/api/[controller]/my/dashboard/listspv")]
        public GenericResponse<UserResponseBean> DashboardListSpv()
        {
            return this.service.ListDashboardSpv();
        }

        [HttpGet]
        [Route("/api/[controller]/my/dashboard/listagent")]
        public GenericResponse<UserResponseBean> DashboardListAgent([FromQuery] UserReqApproveBean filter)
        {
            return this.service.ListDashboardAgentBySpv(filter.Id!.Value);
        }

        [HttpGet]
        [Route("/api/[controller]/my/dashboard/listbranch")]
        public GenericResponse<BranchResponseBean> DashboardListBranch()
        {
            return this.service.ListDashboardBranch();
        }

        [HttpGet]
        [Route("/api/[controller]/structure/fc")]
        public GenericResponse<DashboardTreeSpv> StructureFC()
        {
            return this.service.SpvTree();
        }

        [HttpGet]
        [Route("/api/[controller]/structure/branch")]
        public GenericResponse<DashboardTreeSpv> StructureBranch()
        {
            return this.service.BranchTree();
        }


        [HttpPost]
        [Route("/api/[controller]/setactivebranch")]
        public GenericResponse<UserReqActiveBranch> SetActiveBranch(UserReqActiveBranch filter)
        {
            return this.service.SetActiveBranch(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpGet]
        [Route("/api/[controller]/list")]
        public GenericResponse<UserResponseBean> ListUser([FromQuery] UserListRequestBean bean)
        {
            return this.service.ListUser(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpGet]
        [Route("/api/[controller]/request/list")]
        public GenericResponse<UserReqResponseBean> ListUserRequest([FromQuery] UserListRequestBean bean)
        {
            return this.service.ListUserRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpGet]
        [Route("/api/[controller]/detail")]
        public GenericResponse<UserResponseBean> DetailListUser([FromQuery] UserListRequestBean bean)
        {
            return this.service.DetailListUser(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpGet]
        [Route("/api/[controller]/request/detail")]
        public GenericResponse<UserReqResponseBean> DetailListUserRequest([FromQuery] UserListRequestBean bean)
        {
            return this.service.DetailListUserRequest(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpPost]
        [Route("/api/[controller]/create")]
        public GenericResponse<UserReqCreateBean> CreateUser(UserReqCreateBean filter)
        {
            return this.service.SaveNewUserRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpPost]
        [Route("/api/[controller]/update")]
        public GenericResponse<UserReqCreateBean> UpdateUser(UserReqCreateBean filter)
        {
            return this.service.SaveEditUserRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "SISKON", "SISKON2")]
        [HttpPost]
        [Route("/api/[controller]/request/approve")]
        public GenericResponse<UserReqApproveBean> ApproveRequestUser(UserReqApproveBean filter)
        {
            return this.service.ApproveUserRequest(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "SISKON", "SISKON2")]
        [HttpPost]
        [Route("/api/[controller]/request/reject")]
        public GenericResponse<UserReqApproveBean> RejectRequestUser(UserReqApproveBean filter)
        {
            return this.service.RejectUserRequest(filter);
        }


        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpGet]
        [Route("/api/[controller]/role/list")]
        public GenericResponse<RoleResponseBean> ListRole([FromQuery] RoleRequestBean bean)
        {
            return this.menuService.ListRole(bean);
        }

        //[JWTAuthorize("ADMIN", "USER")]
        //[HttpPost]
        //[Route("/api/[controller]/role/create")]
        //public GenericResponse<RoleCreateBean> CreateRole(RoleCreateBean filter)
        //{
        //    return this.menuService.SaveRole(filter);
        //}

        //[JWTAuthorize("ADMIN", "USER")]
        //[HttpPost]
        //[Route("/api/[controller]/role/update")]
        //public GenericResponse<RoleCreateBean> UpdateRole(RoleCreateBean filter)
        //{
        //    return this.menuService.UpdateRole(filter);
        //}

        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpGet]
        [Route("/api/[controller]/role/permission/list")]
        public GenericResponse<RolePermissionResponseBean> ListRolePermission([FromQuery] RolePermissionRequestBean bean)
        {
            return this.menuService.ListRolePermission(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpPost]
        [Route("/api/[controller]/role/permission/create")]
        public GenericResponse<RolePermissionCreateBean> CreateRolePermission(RolePermissionCreateBean filter)
        {
            return this.menuService.SaveRolePermission(filter);
        }


        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2"  )]
        [HttpGet]
        [Route("/api/[controller]/permission/list")]
        public GenericResponse<PermissionResponseBean> ListPermission([FromQuery] PermissionRequestBean bean)
        {
            return this.menuService.ListPermission(bean);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpPost]
        [Route("/api/[controller]/permission/create")]
        public GenericResponse<PermissionCreateBean> CreatePermission(PermissionCreateBean filter)
        {
            return this.menuService.SavePermission(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "USER", "ADMIN2", "SISKON", "SISKON2")]
        [HttpPost]
        [Route("/api/[controller]/permission/update")]
        public GenericResponse<PermissionCreateBean> UpdatePermission(PermissionCreateBean filter)
        {
            return this.menuService.UpdatePermission(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "SISKON", "SISKON2")]
        [HttpPost]
        [Route("/api/[controller]/activate")]
        public GenericResponse<UserActivateRequest> SetActiveBranch(UserActivateRequest filter)
        {
            return this.service.ActivateUser(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "SISKON", "SISKON2")]
        [HttpPost]
        [Route("/api/[controller]/changepassword")]
        public GenericResponse<UserChangePasswordRequest> ChangePassword(UserChangePasswordRequest filter)
        {
            return this.service.ChangeMyPassword(filter);
        }

        [JWTAuthorize("SUPERUSER", "ADMIN", "ADMIN2", "SISKON", "SISKON2")]
        [HttpPost]
        [Route("/api/[controller]/changedevice")]
        public GenericResponse<UserChangeTelRequest> ChangeTelephony(UserChangeTelRequest filter)
        {
            return this.service.ChangeMyTel(filter);
        }

        [HttpPost]
        [Route("/api/[controller]/changemypassword")]
        public GenericResponse<UserSelfPasswordRequest> ChangeMyPassword(UserSelfPasswordRequest filter)
        {
            return this.service.ChangeSelfPassword(filter);
        }
    }
}
