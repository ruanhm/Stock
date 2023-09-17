using Stock.Domain;

namespace Stock.WebAPI.Model
{
    public record GetFinancialReportRequest(string StockCode, DateTime? BeginReportDate, DateTime? EndReportDate, 
        FinancialReportType? FinancialReportType, FinancialReportPeriod? FinancialReportPeriod);
}