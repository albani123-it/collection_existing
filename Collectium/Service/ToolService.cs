using AutoMapper;
using Collectium.Model;
using Collectium.Model.Bean;
using Collectium.Model.Bean.ListRequest;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Model.Helper;
using Collectium.Validation;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace Collectium.Service
{
    public class ToolService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<ToolService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ToolService(CollectiumDBContext ctx,
                                ILogger<ToolService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                IHttpContextAccessor httpContextAccessor)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.pagination = pagination;
            this.statusService = statusService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void EnrichProcessSave(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var cd = obj.GetType().GetProperty("CreateDate");
            if (cd != null)
            {
                cd.SetValue(obj, DateTime.Now);
            }
        }

        public void EnrichProcessSaveRequest(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                return;
            }

            var cd = obj.GetType().GetProperty("CreateDate");
            if (cd != null)
            {
                cd.SetValue(obj, DateTime.Now);
            }

            cd = obj.GetType().GetProperty("RequestUserId");
            if (cd != null)
            {
                cd.SetValue(obj, reqUser.Id);
            }
        }

        public void EnrichProcessApproveRequest(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var reqUser = this.httpContextAccessor.HttpContext!.Items["User"] as User;
            if (reqUser == null)
            {
                return;
            }

            var cd = obj.GetType().GetProperty("ApproveDate");
            if (cd != null)
            {
                cd.SetValue(obj, DateTime.Now);
            }

            cd = obj.GetType().GetProperty("ApproveUserId");
            if (cd != null)
            {
                cd.SetValue(obj, reqUser.Id);
            }
        }
    }
}
