using Stock.Domain;
using Newtonsoft.Json;
using Stock.Common;
using Microsoft.EntityFrameworkCore;

namespace Stock.Infrastructure
{
    public class StockDomainRepository : IStockDomainRepository
    {
        public event EventHandler emd;
        private readonly StockDbContext stockDbContext;
        public StockDomainRepository(StockDbContext stockDbContext)
        {
            this.stockDbContext = stockDbContext;
        }

        public async Task AddFinancialReportAsync(string stockCode, FinancialReport financialReport)
        {
            var r = await FindOneFinancialReportAsync(stockCode, financialReport.FinancialReportType, financialReport.FinancialReportPeriod);
            if (r == null)
            {
                stockDbContext.FinancialReports.Add(financialReport);
            }
        }

        public async Task AddStockAsync(StockDetail stockDetail)
        {
            var s = await FindOneStockDetailAsync(stockDetail.StockCode);
            if (s != null)
            {
                stockDbContext.StockDetails.Add(stockDetail);
            }
        }

        public Task<FinancialReport> CreateFinancialReport(string stockCode, DateTime reportDate, DateTime updateTime, string currency, FinancialReportType financialReportType, FinancialReportPeriod financialReportPeriod, List<Dictionary<string, double?>> detailList)
        {
            throw new NotImplementedException();
        }

        public Task<List<FinancialReport>> CreateFinancialReport(string stockCode)
        {
            return Task.Run(() =>
            {
                var list = Tools.RunPython(ak =>
                {
                    var list = new List<List<Dictionary<string, string>>>();
                    string[] arr = new string[] { "资产负债表", "利润表", "现金流量表" };
                    foreach (var a in arr)
                    {
                        string data = ak.stock_financial_report_sina(stock: stockCode, symbol: a).to_json(force_ascii: false, orient: "records");
                        var b = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
                        list.Add(b);
                    }
                    return list;
                });
                var listDtl = new List<FinancialReport>();
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < list[i].Count; j++)
                    {
                        var dic = list[i][j];
                        DateTime reportDate = Tools.DateStr2DateTime(dic["报告日"], "yyyyMMdd");
                        DateTime releaseDate = Tools.DateStr2DateTime(dic["公告日期"], "yyyyMMdd");
                        DateTime updateTime = Tools.DateStr2DateTime(dic["更新日期"]);
                        var r = new FinancialReport(stockCode, reportDate, updateTime, releaseDate, dic["币种"], FinancialReportType.BalanceSheet, DateConvertFinancialReportPeriod(reportDate));
                        string s = "报告日,数据源,定期报告,是否审计,公告日期,更新日期";
                        foreach (var kvp in list[i][j])
                        {
                            if (s.IndexOf(kvp.Key) < 0)
                            {
                                r.AddFinancialReportDetail(kvp.Key, string.IsNullOrEmpty(kvp.Value) ? null : Convert.ToDouble(kvp.Value));
                            }
                        }
                    }
                }
                return listDtl;
            });
        }

        public Task<FinancialReport?> FindOneFinancialReportAsync(string stockCode, long reportID)
        {
            return stockDbContext.FinancialReports.SingleOrDefaultAsync(e => e.StockCode == stockCode && e.ReportID == reportID);
        }

        public Task<FinancialReport?> FindOneFinancialReportAsync(string stockCode, FinancialReportType? financialReportType, FinancialReportPeriod? financialReportPeriod)
        {
            return stockDbContext.FinancialReports.SingleOrDefaultAsync(e => e.StockCode == stockCode && e.FinancialReportType == financialReportType && e.FinancialReportPeriod == financialReportPeriod);
        }

        public async Task<StockDetail?> FindOneStockDetailAsync(string stockCode)
        {
            var stock = await stockDbContext.StockDetails.SingleOrDefaultAsync(e => e.StockCode == stockCode);
            if (stock != null)
            {
                stock =await Task.Run(() =>
                {
                    
                    return stock;
                });
            }
            return stock;
        }

        public Task<List<FinancialReport>> GetFinancialReportListAsync(string stockCode, DateTime beginReportDate, DateTime endReportDate, FinancialReportType? financialReportType, FinancialReportPeriod? financialReportPeriod)
        {
            return stockDbContext.FinancialReports.Where(e =>
                e.StockCode == stockCode && e.ReportDate >= beginReportDate && e.ReportDate <= endReportDate
                    &&(financialReportType != null||e.FinancialReportType == financialReportType)
                    && (financialReportPeriod != null ||e.FinancialReportPeriod == financialReportPeriod)
            ).ToListAsync();
           
        }

        public Task<List<StockList>> GetStockListAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20)
        {
            return stockDbContext.StockLists.Where(e =>
                (string.IsNullOrEmpty(stockCode) || e.StockCode == stockCode)
                && (string.IsNullOrEmpty(stockName) || e.StockName == stockName)
                 ).Skip(page).Take(pagesize).ToListAsync();
        }

        public Task<int> GetStockListCountAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20)
        {
            return stockDbContext.StockLists.Where(e =>
                 (string.IsNullOrEmpty(stockCode) || e.StockCode == stockCode)
                 && (string.IsNullOrEmpty(stockName) || e.StockName == stockName)
                  ).Skip(page).Take(pagesize).CountAsync();
        }

        public Task<FinancialReport> CreateFinancialReport(string stockCode, DateTime reportDate, DateTime updateTime, DateTime releaseDate, string currency, FinancialReportType financialReportType, FinancialReportPeriod financialReportPeriod, List<Dictionary<string, double?>> detailList)
        {
           return Task.Run(() =>
           {
                var r = new FinancialReport(stockCode, reportDate, updateTime, releaseDate, currency, financialReportType, financialReportPeriod);
                for (int i = 0; i < detailList.Count; i++)
                {
                    foreach (var kvp in detailList[i])
                    {
                        r.AddFinancialReportDetail(kvp.Key, kvp.Value);
                    }
                }
                return r;
            });
        }
        public Task SynAllStockAsync()
        {
            return Task.Run(() =>
            {
                var list = Tools.RunPython(ak =>
                {
                    var list = new List<List<Dictionary<string, string>>>();
                    string[] arr = new string[] { "A股列表", "B股列表"};
                    foreach (var a in arr)
                    {
                        string data = ak.stock_info_sz_name_code(symbol: a).to_json(force_ascii: false, orient: "records");
                        var b = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
                        list.Add(b);
                        Task.Delay(500);
                    }
                    return list;
                });
            });
        }


        private FinancialReportPeriod DateConvertFinancialReportPeriod(DateTime Date)
        {
            FinancialReportPeriod p = FinancialReportPeriod.AnnualReport;
            if(Date.Month == 3 && Date.Day == 31)
            {
                p = FinancialReportPeriod.FirstQuarterlyReport;
            }
            else if (Date.Month == 6 && Date.Day == 30)
            {
                p = FinancialReportPeriod.SemiannualReport;
            }
            else if (Date.Month == 9 && Date.Day == 30)
            {
                p = FinancialReportPeriod.ThirdQuarterlyReport;
            }
            else if (Date.Month == 12 && Date.Day == 31)
            {
                p = FinancialReportPeriod.AnnualReport;
            }
            return p;
        }

    }
}