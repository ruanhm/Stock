using Microsoft.EntityFrameworkCore;
using Stock.Domain;

namespace Stock.Infrastructure
{
    public class StockDbContext:DbContext
    {
        public DbSet<StockDetail> StockDetails  { get; private set; }
        public DbSet<StockList> StockLists { get; private set; }
        public DbSet<FinancialReport> FinancialReports { get; private set; }

        //public StockDbContext(DbContextOptions<StockDbContext> options)
        //    : base(options)
        //{
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = "Data Source=127.0.0.1;Initial Catalog=Stock; User Id=sa;Password=123456;TrustServerCertificate=true";
            optionsBuilder.UseSqlServer(connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
