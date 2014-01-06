using System.Data.Entity;
using Coffee.Entities;

namespace Coffee.Repository
{
    public class CoffeeDb : DbContext
    {
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Credit> Credits {get; set;}

        public DbSet<CreditLine> CreditLines {get; set;}

        public DbSet<Decision> Decisions {get; set;}

        public DbSet<CreditRequest> CreditRequests {get; set;}

        public DbSet<PassportInfo> PassportInfos {get; set;}

        public DbSet<SalaryInfo> SalaryInfos {get; set;}
    }
}
