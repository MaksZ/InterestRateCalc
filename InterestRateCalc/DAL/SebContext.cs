using InterestRateCalc.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace InterestRateCalc.DAL
{
    public class SebContext : DbContext
    {
        public DbSet<SebCustomer> Customers { get; set; }
        public DbSet<SebCustomerAgreement> Agreements { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}