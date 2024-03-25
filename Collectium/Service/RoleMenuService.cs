using AutoMapper;
using Collectium.Model;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Model.Helper;
using Collectium.Validation;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Service
{
    public class RoleMenuService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<RoleMenuService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly IMapper mapper;

        public RoleMenuService(CollectiumDBContext ctx, 
                                ILogger<RoleMenuService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                IMapper mapper)
        {
            this.ctx = ctx; 
            this.logger = logger;
            this.pagination = pagination;
            this.statusService = statusService;
            this.mapper = mapper;
        }


        public GenericResponse<RoleResponseBean> ListRole(RoleRequestBean bean)
        {
            var wrap = new GenericResponse<RoleResponseBean>()
            {
                Status = false,
                Message = ""
            };

            IQueryable<Role> jq = this.ctx.Set<Role>().Include(i => i.Status);

            if (bean.Keyword != null && bean.Keyword.Length > 1)
            {
                jq = jq.Where(q => q.Name!.Contains(bean.Keyword));
            }

            if (bean.StatusId != null)
            {
                jq = jq.Where(q => q.Status!.Id.Equals(bean.StatusId));
            }

            var pagination = this.pagination.getPagination(bean);
            var data = jq.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var ldata = new List<RoleResponseBean>();
            foreach(var it in data)
            {
                var dto = mapper.Map<RoleResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;
            wrap.Status = true;
            return wrap;
        }

        //public GenericResponse<RoleCreateBean> SaveRole(RoleCreateBean menu)
        //{
        //    var wrap = new GenericResponse<RoleCreateBean>()
        //    {
        //        Status = false,
        //        Message = ""
        //    };

        //    var pr = IlKeiValidator.Instance.WithPoCo(menu)
        //        .Pick(nameof(menu.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
        //        .Validate();

        //    if (pr.Result == false)
        //    {
        //        wrap.Message = pr.Message;
        //        return wrap;
        //    }

        //    var cnt = this.ctx.Role.Where(q => q.Name!.Equals(menu.Name)).Count();
        //    if (cnt > 0)
        //    {
        //        wrap.Message = "Role: " + menu.Name + " sudah ada di sistem";
        //        return wrap;
        //    }

        //    var config = new MapperConfiguration(cfg => {
        //        cfg.CreateMap<Role, RoleCreateBean>().ReverseMap();
        //    });
        //    var mapper = new Mapper(config);
        //    var nm = mapper.Map<Role>(menu);

        //    var sg = this.statusService.GetStatusGeneral("AKTIF");
        //    nm.Status = sg;

        //    this.ctx.Role.Add(nm);
        //    this.ctx.SaveChanges();

        //    wrap.Status = true;
        //    wrap.AddData(menu);

        //    return wrap;
        //}

        //public GenericResponse<RoleCreateBean> UpdateRole(RoleCreateBean menu)
        //{
        //    var wrap = new GenericResponse<RoleCreateBean>()
        //    {
        //        Status = false,
        //        Message = ""
        //    };

        //    var pr = IlKeiValidator.Instance.WithPoCo(menu)
        //        .Pick(nameof(menu.Id)).IsMandatory().AsInteger().Pack()
        //        .Pick(nameof(menu.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
        //        .Validate();

        //    if (pr.Result == false)
        //    {
        //        wrap.Message = pr.Message;
        //        return wrap;
        //    }

        //    var cnt = this.ctx.Role.Where(q => q.Name!.Equals(menu.Name)).Where(q => q.Id != menu.Id).Count();
        //    if (cnt > 0)
        //    {
        //        wrap.Message = "Role: " + menu.Name + " sudah ada di sistem";
        //        return wrap;
        //    }

        //    var config = new MapperConfiguration(cfg => {
        //        cfg.CreateMap<Role, RoleCreateBean>().ReverseMap();
        //    });
        //    var mapper = new Mapper(config);
        //    var nm = mapper.Map<Role>(menu);

        //    this.ctx.Role.Update(nm);
        //    this.ctx.SaveChanges();

        //    wrap.Status = true;
        //    wrap.AddData(menu);

        //    return wrap;
        //}

        public GenericResponse<PermissionResponseBean> ListPermission(PermissionRequestBean bean)
        {
            var wrap = new GenericResponse<PermissionResponseBean>()
            {
                Status = false,
                Message = ""
            };

            IQueryable<Permission> jq = this.ctx.Set<Permission>().Include(i => i.Status);

            if (bean.Keyword != null && bean.Keyword.Length > 1)
            {
                jq = jq.Where(q => q.Name!.Contains(bean.Keyword));
            }

            if (bean.StatusId != null)
            {
                jq = jq.Where(q => q.Status!.Id.Equals(bean.StatusId));
            }

            var pagination = this.pagination.getPagination(bean);
            var data = jq.Skip(pagination.Skip).Take(pagination.Limit).OrderByDescending(q => q.Id).ToList();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Permission, PermissionResponseBean>().ReverseMap();
                cfg.CreateMap<Status, StatusGeneralBean>().ReverseMap();
            });
            config.AssertConfigurationIsValid();

            var mapper = new Mapper(config);
            var ldata = new List<PermissionResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<PermissionResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;
            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<PermissionCreateBean> SavePermission(PermissionCreateBean menu)
        {
            var wrap = new GenericResponse<PermissionCreateBean>()
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(menu)
                .Pick(nameof(menu.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Permission.Where(q => q.Name!.Equals(menu.Name)).Count();
            if (cnt > 0)
            {
                wrap.Message = "Permission: " + menu.Name + " sudah ada di sistem";
                return wrap;
            }

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Permission, PermissionCreateBean>().ReverseMap();
            });
            var mapper = new Mapper(config);
            var nm = mapper.Map<Permission>(menu);

            var sg = this.statusService.GetStatusGeneral("AKTIF");
            nm.Status = sg;

            this.ctx.Permission.Add(nm);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(menu);

            return wrap;
        }

        public GenericResponse<PermissionCreateBean> UpdatePermission(PermissionCreateBean menu)
        {
            var wrap = new GenericResponse<PermissionCreateBean>()
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(menu)
                .Pick(nameof(menu.Id)).IsMandatory().AsInteger().Pack()
                .Pick(nameof(menu.Name)).IsMandatory().AsString().WithMinLen(2).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.Permission.Where(q => q.Name!.Equals(menu.Name)).Where(q => q.Id != menu.Id).Count();
            if (cnt > 0)
            {
                wrap.Message = "Permission: " + menu.Name + " sudah ada di sistem";
                return wrap;
            }

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Permission, PermissionCreateBean>().ReverseMap();
            });
            var mapper = new Mapper(config);
            var nm = mapper.Map<Permission>(menu);

            this.ctx.Permission.Update(nm);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(menu);

            return wrap;
        }

        public GenericResponse<RolePermissionResponseBean> ListRolePermission(RolePermissionRequestBean bean)
        {
            var wrap = new GenericResponse<RolePermissionResponseBean>()
            {
                Status = false,
                Message = ""
            };

            IQueryable<RolePermission> jq = this.ctx.Set<RolePermission>().Include(i => i.Role).Include(i => i.Permission);

            if (bean.RoleId != null)
            {
                jq = jq.Where(q => q.RoleId!.Equals(bean.RoleId));
            }

            var data = jq.OrderByDescending(q => q.RoleId).ToList();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RolePermission, RolePermissionResponseBean>().ReverseMap();
                cfg.CreateMap<Role, RoleResponseBean>().ReverseMap();
                cfg.CreateMap<Permission, PermissionResponseBean>().ReverseMap();
                cfg.CreateMap<StatusGeneral, StatusGeneralBean>().ReverseMap();
            });
            config.AssertConfigurationIsValid();

            var mapper = new Mapper(config);
            var ldata = new List<RolePermissionResponseBean>();
            foreach (var it in data)
            {
                var dto = mapper.Map<RolePermissionResponseBean>(it);
                ldata.Add(dto);
            }

            wrap.Data = ldata;
            wrap.Status = true;
            return wrap;
        }

        public GenericResponse<RolePermissionCreateBean> SaveRolePermission(RolePermissionCreateBean menu)
        {
            var wrap = new GenericResponse<RolePermissionCreateBean>()
            {
                Status = false,
                Message = ""
            };

            var pr = IlKeiValidator.Instance.WithPoCo(menu)
                .Pick(nameof(menu.RoleId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Pick(nameof(menu.PermissionId)).IsMandatory().AsInteger().WithMinLen(1).Pack()
                .Validate();

            if (pr.Result == false)
            {
                wrap.Message = pr.Message;
                return wrap;
            }

            var cnt = this.ctx.RolePermission.Where(q => q.RoleId!.Equals(menu.RoleId)).Where(q => q.PermissionId.Equals(menu.PermissionId)).ToList();
            if (cnt != null)
            {
                foreach (var it in cnt)
                {
                    this.ctx.RolePermission.Remove(it);
                }
                this.ctx.SaveChanges();
            }

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RolePermission, RolePermissionCreateBean>().ReverseMap();
            });
            var mapper = new Mapper(config);
            var nm = mapper.Map<RolePermission>(menu);

            this.ctx.RolePermission.Add(nm);
            this.ctx.SaveChanges();

            wrap.Status = true;
            wrap.AddData(menu);

            return wrap;
        }

    }
}
