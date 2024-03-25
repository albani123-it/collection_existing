using Collectium.Model;
using Collectium.Model.Entity;
using Collectium.Model.Helper;

namespace Collectium.Service
{
    public class StatusService
    {

        private readonly CollectiumDBContext ctx;

        private readonly ILogger<StatusService> logger;

        private readonly PaginationHelper pagination;

        public StatusService(CollectiumDBContext ctx, ILogger<StatusService> logger, PaginationHelper pagination)
        {
            this.ctx = ctx;
            this.logger = logger;
            this.pagination = pagination;
        }

        public StatusGeneral GetStatusGeneral(string name)
        {
            return this.ctx.StatusGeneral.Where(q => q.Name.Equals(name)).FirstOrDefault();
        }

        public StatusRequest GetStatusRequest(string name)
        {
            return this.ctx.StatusRequest.Where(q => q.Name.Equals(name)).FirstOrDefault();
        }

        public StatusRestruktur GetStatusRestruktur(string name)
        {
            return this.ctx.StatusRestruktur.Where(q => q.Name.Equals(name)).FirstOrDefault();
        }

        public StatusLeLang GetStatusLeLang(string name)
        {
            return this.ctx.StatusLeLang.Where(q => q.Name.Equals(name)).FirstOrDefault();
        }

        public StatusAsuransi GetStatusAsuransi(string name)
        {
            return this.ctx.StatusAsuransi.Where(q => q.Name.Equals(name)).FirstOrDefault();
        }

        public RecoveryExecution GetRecoveryExecution(string name)
        {
            return this.ctx.RecoveryExecution.Where(q => q.Name.Equals(name)).FirstOrDefault();
        }
    }
}
