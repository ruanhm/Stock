using Microsoft.EntityFrameworkCore;
using Stock.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock.Infrastructure
{
    public class FinancialReportConfig : IEntityTypeConfiguration<FinancialReport>
    {
        public void Configure(EntityTypeBuilder<FinancialReport> builder)
        {
            builder.ToTable("tFinancialReport");
            builder.HasKey(e => e.ReportID);
        }
    }
}
