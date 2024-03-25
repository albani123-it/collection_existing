using AutoMapper;
using Collectium.Model;
using Collectium.Model.Bean;
using Collectium.Model.Bean.Request;
using Collectium.Model.Bean.Response;
using Collectium.Model.Entity;
using Collectium.Service;
using System.Text.Json;

namespace Collectium.Service
{
    public class InitialService
    {
        private readonly ILogger<InitialService> logger;
        private readonly CollectiumDBContext ctx;
        private readonly UserService user;
        private readonly BranchAreaService baService;
        private readonly TxMasterService txmService;

        public InitialService(ILogger<InitialService> logger, 
                                CollectiumDBContext ctx, 
                                UserService user,
                                BranchAreaService baService,
                                TxMasterService txmService)
        {
            this.logger = logger;
            this.ctx = ctx;
            this.user = user;
            this.baService = baService;
            this.txmService = txmService;
        }

        public GenericResponse<string> init()
        {
            var res = new GenericResponse<string>();
            res.Status = true;

            var d = this.ctx.StatusGeneral.Count();
            if (d < 1)
            {
                var st = new StatusGeneral();
                st.Name = "AKTIF";
                this.ctx.StatusGeneral.Add(st);

                st = new StatusGeneral();
                st.Name = "NOT AKTIF";
                this.ctx.StatusGeneral.Add(st);

                this.ctx.SaveChanges();
            }

            var sr = this.ctx.StatusRequest.Count();
            if (d < 4)
            {
                var st = new StatusRequest();
                st.Name = "DRAFT";
                this.ctx.StatusRequest.Add(st);

                st = new StatusRequest();
                st.Name = "APPROVE";
                this.ctx.StatusRequest.Add(st);

                st = new StatusRequest();
                st.Name = "REJECT";
                this.ctx.StatusRequest.Add(st);

                st = new StatusRequest();
                st.Name = "REMOVE";
                this.ctx.StatusRequest.Add(st);

                this.ctx.SaveChanges();
            }

            var r = this.ctx.Role.Count();
            if (r < 1)
            {
                var m = new Role();
                m.Name = "ADMIN";
                m.StatusId = 1;
                this.ctx.Role.Add(m);

                m = new Role();
                m.Name = "USER";
                m.StatusId = 1;
                this.ctx.Role.Add(m);

                m = new Role();
                m.Name = "DC";
                m.StatusId = 1;
                this.ctx.Role.Add(m);

                m = new Role();
                m.Name = "FC";
                m.StatusId = 1;
                this.ctx.Role.Add(m);

                m = new Role();
                m.Name = "SPVDC";
                m.StatusId = 1;
                this.ctx.Role.Add(m);

                m = new Role();
                m.Name = "SPVFC";
                m.StatusId = 1;
                this.ctx.Role.Add(m);

                this.ctx.SaveChanges();

            }

            var c = this.ctx.User.Count();
            if (c < 1)
            {
                var m = new UserCreateBean();
                //m.Administrator = 1;
                m.Email = "admin@gmail.com";
                m.Username = "admin@gmail.com";
                m.Password = "123456";
                m.Name = "Super Admin";
                m.RoleId = 1;

                var t = this.user.SaveUsers(m);
                if (t.Status == false)
                {
                    Console.WriteLine(t.Message);
                }
            }

            var bt = this.ctx.BranchType.Count();
            if (bt < 2)
            {
                var m = new BranchTypeCreateBean();
                m.Code = "A";
                m.Name = "A";
                var t = this.baService.SaveBranchType(m);

                m = new BranchTypeCreateBean();
                m.Code = "B";
                m.Name = "B";
                t = this.baService.SaveBranchType(m);

                m = new BranchTypeCreateBean();
                m.Code = "UPS";
                m.Name = "UPS";
                t = this.baService.SaveBranchType(m);

                m = new BranchTypeCreateBean();
                m.Code = "UPC";
                m.Name = "UPC";
                t = this.baService.SaveBranchType(m);

                m = new BranchTypeCreateBean();
                m.Code = "CPP";
                m.Name = "CPP";
                t = this.baService.SaveBranchType(m);

                m = new BranchTypeCreateBean();
                m.Code = "CPS";
                m.Name = "CPS";
                t = this.baService.SaveBranchType(m);

            }

            /*
            var a = this.ctx.Area.Count();
            if (a < 2)
            {
                var m = new AreaCreateBean();
                m.Value = "MDN";
                m.Description = "Area Medan";
                var t = this.baService.SaveArea(m);

                m = new AreaCreateBean();
                m.Value = "PDG";
                m.Description = "Area Padang";
                t = this.baService.SaveArea(m);

            }
            */

            var ps = this.ctx.ProductSegment.Count();
            if (ps < 2)
            {
                var m = new ProductSegment();
                m.Code = "KONVENT";
                m.Desc = "Konensional";
                var t = this.txmService.SaveProductSegment(m);

                m = new ProductSegment();
                m.Code = "SYARIAH";
                m.Desc = "Syariah";
                t = this.txmService.SaveProductSegment(m);

            }

            var cr = this.ctx.CallResult.Count();
            if (cr < 2)
            {
                var m = new CallResult();
                m.Code = "NTASK";
                m.Description = "New Task";
                var t = this.txmService.SaveCallResult(m);

                m = new CallResult();
                m.Code = "FUIM";
                m.Description = "Callback";
                t = this.txmService.SaveCallResult(m);

                m = new CallResult();
                m.Code = "LL";
                m.Description = "Lainnya";
                t = this.txmService.SaveCallResult(m);

                m = new CallResult();
                m.Code = "MESS";
                m.Description = "Message";
                t = this.txmService.SaveCallResult(m);

                m = new CallResult();
                m.Code = "NOAS";
                m.Description = "No Answer";
                t = this.txmService.SaveCallResult(m);

                m = new CallResult();
                m.Code = "NOAS1";
                m.Description = "Tidak Diangkat";
                t = this.txmService.SaveCallResult(m);

                m = new CallResult();
                m.Code = "NOAS2";
                m.Description = "No Tidak Aktif";
                t = this.txmService.SaveCallResult(m);

                m = new CallResult();
                m.Code = "NOAS3";
                m.Description = "Wrong Number";
                t = this.txmService.SaveCallResult(m);

                m = new CallResult();
                m.Code = "PAY";
                m.Description = "Payment";
                t = this.txmService.SaveCallResult(m);

                m = new CallResult();
                m.Code = "PTP";
                m.Description = "Promise To Pay";
                t = this.txmService.SaveCallResult(m);

            }

            return res;

        }
    }
}
