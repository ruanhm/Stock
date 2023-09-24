using Microsoft.EntityFrameworkCore;
using Stock.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Stock.Infrastructure 
{
    public class StockDetailConfig : IEntityTypeConfiguration<StockDetail>
    {

        public void Configure(EntityTypeBuilder<StockDetail> builder)
        {
            builder.ToTable("tStock");
            builder.HasKey(e => e.StockCode);
            builder.Ignore(e=>e.StockPrice).Ignore(e => e.ChangeRange).Ignore(e => e.ChangeAmount).Ignore(e => e.ClosedYesterday)
                .Ignore(e => e.TodayOpening).Ignore(e => e.Max).Ignore(e => e.Min).Ignore(e => e.Turnover).Ignore(e => e.TransactionVolume)
                .Ignore(e => e.Amplitude).Ignore(e => e.EquivalentRatio).Ignore(e => e.TurnoverRate).Ignore(e => e.ForwardPE)
                .Ignore(e => e.PB).Ignore(e => e.MarketCap).Ignore(e => e.CirculationMarketValue).Ignore(e => e.SpeedUp)
                .Ignore(e => e.FiveMinute).Ignore(e => e.SixtyDays).Ignore(e => e.Year2Date);
            builder.Property(e => e.StockName).HasColumnName("StockName").HasMaxLength(200).IsRequired();
            builder.Property(e => e.Exchange).HasColumnName("Exchange").HasMaxLength(100);
            builder.Property(e => e.Plate).HasColumnName("Plate").HasMaxLength(100);
            builder.Property(e => e.Industry).HasColumnName("Industry").HasMaxLength(200);
            builder.Property(e => e.StockCode).HasColumnName("StockCode").HasMaxLength(50);
            builder.Property(e => e.MarketTime).HasColumnName("MarketTime").HasColumnType("datetime");
            //builder.HasOne<StockListInfo>().WithOne().HasForeignKey<StockDetail>(e => e.StockCode);
        }
        
    }
}


