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
using Microsoft.EntityFrameworkCore;

namespace Collectium.Service
{
    public class GeneralParameterService
    {

        private readonly CollectiumDBContext ctx;
        private readonly ILogger<GeneralParameterService> logger;
        private readonly PaginationHelper pagination;
        private readonly StatusService statusService;
        private readonly ToolService toolService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GeneralParameterService(CollectiumDBContext ctx,
                                ILogger<GeneralParameterService> logger,
                                PaginationHelper pagination,
                                StatusService statusService,
                                ToolService toolService,
                                IMapper mapper,
                                IHttpContextAccessor httpContextAccessor)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.pagination = pagination;
            this.statusService = statusService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.toolService = toolService;
        }

        
        public GenericResponse<DocumentRestruktur> ListDocumentRestruktur()
        {
            var wrap = new GenericResponse<DocumentRestruktur>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<DocumentRestruktur>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }


        public GenericResponse<PolaRestruktur> ListPolaRestruktur()
        {
            var wrap = new GenericResponse<PolaRestruktur>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<PolaRestruktur>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<JenisPengurangan> ListJenisPengurangan()
        {
            var wrap = new GenericResponse<JenisPengurangan>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<JenisPengurangan>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<PembayaranGp> ListPembayaranGp()
        {
            var wrap = new GenericResponse<PembayaranGp>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<PembayaranGp>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<AlasanLelang> ListAlasanLelang()
        {
            var wrap = new GenericResponse<AlasanLelang>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<AlasanLelang>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<BalaiLelang> ListBalaiLelang()
        {
            var wrap = new GenericResponse<BalaiLelang>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<BalaiLelang>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<JenisLelang> ListJenisLelang()
        {
            var wrap = new GenericResponse<JenisLelang>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<JenisLelang>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<DocumentAuction> ListDocumentAuction()
        {
            var wrap = new GenericResponse<DocumentAuction>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<DocumentAuction>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<DocumentAuctionResult> ListDocumentAuctionResult()
        {
            var wrap = new GenericResponse<DocumentAuctionResult>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<DocumentAuctionResult>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<Asuransi> ListAsuransi()
        {
            var wrap = new GenericResponse<Asuransi>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<Asuransi>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<AsuransiSisaKlaim> ListAsuransiSisaKlaim()
        {
            var wrap = new GenericResponse<AsuransiSisaKlaim>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<AsuransiSisaKlaim>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<DocumentInsurance> ListDocumentInsurance()
        {
            var wrap = new GenericResponse<DocumentInsurance>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<DocumentInsurance>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }

        public GenericResponse<RecoveryExecution> ListRecoveryExecution()
        {
            var wrap = new GenericResponse<RecoveryExecution>
            {
                Status = false,
                Message = ""
            };

            var q = this.ctx.Set<RecoveryExecution>().OrderBy(o => o.Name).ToList();
            wrap.Status = true;
            wrap.Data = q;

            return wrap;
        }
    }
}
