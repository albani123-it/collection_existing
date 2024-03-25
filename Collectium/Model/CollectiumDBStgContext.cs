using Collectium.Model.Entity;
using Collectium.Model.Entity.Staging;
using Microsoft.EntityFrameworkCore;

namespace Collectium.Model
{

    public class CollectiumDBStgContext : DbContext
    {

        public CollectiumDBStgContext(DbContextOptions<CollectiumDBStgContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }


        public virtual DbSet<STGBranchPg> STGBranchPg { get; set; }

        public virtual DbSet<STGCustomerPg> STGCustomer { get; set; }
        public virtual DbSet<STGDataJaminanPg> STGDataJaminan { get; set; }
        public virtual DbSet<STGDataKreditPg> STGDataKredit { get; set; }
        public virtual DbSet<STGDataLoanBiayaLainPg> STGDataLoanBiayaLain { get; set; }
        public virtual DbSet<STGDataLoanKodeAOPg> STGDataLoanKodeAO { get; set; }
        public virtual DbSet<STGDataLoanKomiteKreditPg> STGDataLoanKomiteKredit { get; set; }
        public virtual DbSet<STGDataLoanKSLPg> STGDataLoanKSL { get; set; }
        public virtual DbSet<STGDataLoanPKPg> STGDataLoanPK { get; set; }
        public virtual DbSet<STGDataLoanTagihanLainPg> STGDataLoanTagihanLain { get; set; }
        public virtual DbSet<STGLoanDetailPg> STGLoanDetail { get; set; }

        public virtual DbSet<STGCustomerPhonePg> STGCustomerPhone { get; set; }

        public virtual DbSet<STGSmsReminderPg> STGSmsReminder { get; set; }


    }

}
