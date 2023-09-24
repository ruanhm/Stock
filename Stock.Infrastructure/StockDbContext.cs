using Microsoft.EntityFrameworkCore;
using Stock.Common;
using Stock.Domain;

namespace Stock.Infrastructure
{
    public class StockDbContext:DbContext
    {
        public DbSet<StockDetail> StockDetails  { get; private set; }
        public DbSet<StockListInfo> StockListInfos { get; private set; }
        public DbSet<FinancialReport> FinancialReports { get; private set; }

        //private readonly IConfigService configService;
        //public StockDbContext(DbContextOptions<StockDbContext> options)
        //    : base(options)
        //{
        //}
        //public StockDbContext(IConfigService configService)
        //{
        //    this.configService= configService;
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = "Data Source=.;Initial Catalog=Stock; User Id=sa;Password=123456;TrustServerCertificate=true";
            optionsBuilder.UseSqlServer(connStr);
            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
