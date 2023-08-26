using Microsoft.EntityFrameworkCore;
using Stock.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Stock.Infrastructure
{
    internal class FinancialReportDetailConfig : IEntityTypeConfiguration<FinancialReportDetail>
    {
        public void Configure(EntityTypeBuilder<FinancialReportDetail> builder)
        {
            builder.ToTable("tFinancialReportDetail");
            builder.HasOne(e => e.FinancialReport).WithMany(e => e.FinancialReportDetails).HasForeignKey(e=>e.ReportID).IsRequired();
        }
    }
}
