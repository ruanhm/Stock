
namespace Stock.Domain
{
    public interface IStockDomainRepository
    {
        Task<List<StockList>> GetStockListAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20);
        Task<int> GetStockListCountAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20);
        Task<StockDetail?> FindOneStockDetailAsync(string stockCode);
        Task<List<FinancialReport>> GetFinancialReportListAsync(string stockCode, DateTime beginReportDate, DateTime endReportDate, FinancialReportType? financialReportType, FinancialReportPeriod? financialReportPeriod);
        Task<FinancialReport?> FindOneFinancialReportAsync(string stockCode, long reportID);
        Task<FinancialReport?> FindOneFinancialReportAsync(string stockCode, FinancialReportType? financialReportType, FinancialReportPeriod? financialReportPeriod);
        Task AddStockAsync(StockDetail stockDetail);
        Task SynAllStockAsync();
        Task AddFinancialReportAsync(string stockCode, FinancialReport financialReport);
        Task<List<FinancialReport>> CreateFinancialReport(string stockCode);
        Task<FinancialReport> CreateFinancialReport(string stockCode, DateTime reportDate, DateTime updateTime, DateTime releaseDate, string currency, FinancialReportType financialReportType, FinancialReportPeriod financialReportPeriod, List<Dictionary<string, double?>> detailList);
    }
}
