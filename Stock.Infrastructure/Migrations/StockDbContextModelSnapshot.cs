﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Stock.Infrastructure;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    [DbContext(typeof(StockDbContext))]
    partial class StockDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Stock.Domain.FinancialReport", b =>
                {
                    b.Property<long>("ReportID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ReportID"));

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FinancialReportPeriod")
                        .HasColumnType("int");

                    b.Property<int>("FinancialReportType")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("StockCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ReportID");

                    b.ToTable("tFinancialReport", (string)null);
                });

            modelBuilder.Entity("Stock.Domain.FinancialReportDetail", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ID"));

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("ItemValue")
                        .HasColumnType("float");

                    b.Property<long>("ReportID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("ReportID");

                    b.ToTable("tFinancialReportDetail", (string)null);
                });

            modelBuilder.Entity("Stock.Domain.StockDetail", b =>
                {
                    b.Property<string>("StockCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("StockCode");

                    b.Property<double?>("CirculatingEquity")
                        .HasColumnType("float");

                    b.Property<string>("Company")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Exchange")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasMaxLength(100)
                        .HasColumnType("int")
                        .HasColumnName("Exchange");

                    b.Property<string>("Industry")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("Industry");

                    b.Property<DateTime>("MarketTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Plate")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Plate");

                    b.Property<string>("StockName")
                        .IsRequired()
                        .ValueGeneratedOnUpdateSometimes()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("StockName");

                    b.Property<double?>("TotalEquity")
                        .HasColumnType("float");

                    b.HasKey("StockCode");

                    b.ToTable("tStock", (string)null);
                });

            modelBuilder.Entity("Stock.Domain.StockListInfo", b =>
                {
                    b.Property<string>("StockCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("StockCode");

                    b.Property<int>("Exchange")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasMaxLength(100)
                        .HasColumnType("int")
                        .HasColumnName("Exchange");

                    b.Property<string>("Industry")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("Industry");

                    b.Property<string>("Plate")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Plate");

                    b.Property<string>("StockName")
                        .IsRequired()
                        .ValueGeneratedOnUpdateSometimes()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("StockName");

                    b.HasKey("StockCode");

                    b.ToTable("tStock", (string)null);
                });

            modelBuilder.Entity("Stock.Domain.FinancialReportDetail", b =>
                {
                    b.HasOne("Stock.Domain.FinancialReport", "FinancialReport")
                        .WithMany("FinancialReportDetails")
                        .HasForeignKey("ReportID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinancialReport");
                });

            modelBuilder.Entity("Stock.Domain.StockDetail", b =>
                {
                    b.HasOne("Stock.Domain.StockListInfo", null)
                        .WithOne()
                        .HasForeignKey("Stock.Domain.StockDetail", "StockCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Stock.Domain.FinancialReport", b =>
                {
                    b.Navigation("FinancialReportDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
