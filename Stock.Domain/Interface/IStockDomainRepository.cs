
namespace Stock.Domain
{
    public interface IStockDomainRepository
    {
        Task<List<StockList>> GetStockListAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20);
        Task<int> GetStockListCountAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20);
        Task<StockDetail?> FindOneStockDetailAsync(string stockCode);
        Task<FinancialReport?> FindOneFinancialReportAsync(string stockCode, long reportID);
        Task<FinancialReport?> FindOneFinancialReportAsync(string stockCode, FinancialReportType? financialReportType, FinancialReportPeriod? financialReportPeriod);
        Task AddStockAsync(StockDetail stockDetail);
        Task SynAllStockAsync();
        Task AddFinancialReportAsync(string stockCode, FinancialReport financialReport);
        Task SynFinancialReportAsync(string stockCode);
       
    }
}
