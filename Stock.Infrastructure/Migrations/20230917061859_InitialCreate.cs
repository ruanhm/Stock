using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tFinancialReport",
                columns: table => new
                {
                    ReportID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinancialReportType = table.Column<int>(type: "int", nullable: false),
                    FinancialReportPeriod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tFinancialReport", x => x.ReportID);
                });

            migrationBuilder.CreateTable(
                name: "tStock",
                columns: table => new
                {
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalEquity = table.Column<double>(type: "float", nullable: true),
                    CirculatingEquity = table.Column<double>(type: "float", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Exchange = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    Plate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tStock", x => x.StockCode);
                });

            migrationBuilder.CreateTable(
                name: "tFinancialReportDetail",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportID = table.Column<long>(type: "bigint", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemValue = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tFinancialReportDetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tFinancialReportDetail_tFinancialReport_ReportID",
                        column: x => x.ReportID,
                        principalTable: "tFinancialReport",
                        principalColumn: "ReportID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tFinancialReportDetail_ReportID",
                table: "tFinancialReportDetail",
                column: "ReportID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tFinancialReportDetail");

            migrationBuilder.DropTable(
                name: "tStock");

            migrationBuilder.DropTable(
                name: "tFinancialReport");
        }
    }
}
