
using Stock.Common;

namespace Stock.Domain
{
    public interface IStockDomainRepository: IScopeDenpendency
    {
        Task<List<StockListInfo>> GetStockListAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20);
        Task<int> GetStockListCountAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20);
        Task<StockDetail?> FindOneStockDetailAsync(string stockCode);
        Task<FinancialReport?> FindOneFinancialReportAsync(string stockCode, long reportID);
        Task<FinancialReport?> FindOneFinancialReportAsync(string stockCode,DateTime reportDate, FinancialReportType? financialReportType, FinancialReportPeriod? financialReportPeriod);
        Task<List<FinancialReport>?> GetFinancialReportListAsync(string stockCode, DateTime beginReportDate, DateTime endReportDate, FinancialReportType? financialReportType, FinancialReportPeriod? financialReportPeriod); 
        Task AddStockAsync(StockDetail stockDetail);
        Task SynAllStockAsync();
        Task AddFinancialReportAsync(string stockCode, FinancialReport financialReport);
        Task SynFinancialReportAsync(string stockCode);
       
    }
}
