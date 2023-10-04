using Stock.Domain;
using Newtonsoft.Json;
using Stock.Common;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Python.Runtime;

namespace Stock.Infrastructure
{
    public class StockDomainRepository : IStockDomainRepository
    {
        private readonly StockDbContext stockDbContext;
        private readonly IConfigService configs;
        private readonly string dataUrl;
        public StockDomainRepository(StockDbContext stockDbContext, IConfigService configs)
        {
            this.stockDbContext = stockDbContext;
            this.configs = configs;
            dataUrl= this.configs.ReadDataDataSourceUrl();
        }

        public async Task AddFinancialReportAsync(string stockCode, FinancialReport financialReport)
        {
            var r = await FindOneFinancialReportAsync(stockCode,financialReport.ReportDate ,financialReport.FinancialReportType, financialReport.FinancialReportPeriod);
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


        public Task SynFinancialReportAsync(string stockCode)
        {
            return Task.Run(async () =>
            {

                try
                {
                    var list = await Tools.SendGetRequestAsync<List<FinancialReport>>(dataUrl + "/get_finacial_report",func: Tools.ResponseHandler);
                    if (list != null && list.Count > 0)
                    {
                        foreach (var r in list)
                        {
                            await AddFinancialReportAsync(stockCode, r);
                        }
                        
                        await stockDbContext.SaveChangesAsync();
                    }
                }
                catch(Exception ex)
                {
                    LogHelper.Error(ex);
                }
            });
            
        }

        public Task<FinancialReport?> FindOneFinancialReportAsync(string stockCode, long reportID)
        {
            return stockDbContext.FinancialReports.SingleOrDefaultAsync(e => e.StockCode == stockCode && e.ReportID == reportID);
        }

        public Task<FinancialReport?> FindOneFinancialReportAsync(string stockCode,DateTime reportDate, FinancialReportType? financialReportType, FinancialReportPeriod? financialReportPeriod)
        {
            return stockDbContext.FinancialReports.SingleOrDefaultAsync(e => e.StockCode == stockCode && e.FinancialReportType == financialReportType && e.FinancialReportPeriod == financialReportPeriod&&e.ReportDate==reportDate);
        }

        public async Task<StockDetail?> FindOneStockDetailAsync(string stockCode)
        {
            var stock = await stockDbContext.StockDetails.SingleOrDefaultAsync(e => e.StockCode == stockCode);
            if (stock != null)
            {
                var s = await Tools.SendGetRequestAsync<StockDetail>(dataUrl + "/get_stock_real_time_quotes",
                    new
                    {
                        stock_code = stockCode,
                        exchange = stock.Exchange,
                    });
      
                if (s != null)
                {
                    UpdateStockInfo(stock, s);
                }
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

        public async Task<List<StockListInfo>> GetStockListAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20)
        {
            var list = await stockDbContext.StockListInfos.Where(e =>
                (string.IsNullOrEmpty(stockCode) || e.StockCode == stockCode)
                && (string.IsNullOrEmpty(stockName) || e.StockName == stockName)
                 ).Skip(page).Take(pagesize).ToListAsync();
            //if (list != null && list.Count > 0)
            //{
            //    var list1=await Tools.SendGetRequestAsync<List<Stock.Domain.Stock>>(dataUrl + "/get_all_stock_real_time_quotes", func: Tools.ResponseHandler);
            //    if (list1 != null)
            //    {
            //        for (int i = 0; i < list.Count; i++)
            //        {
            //            var s = list1.FirstOrDefault(e => e.StockCode == list[i].StockCode);
            //            if (s != null)
            //            {
            //                UpdateStockInfo(list[i], s);
            //            }
            //        }
            //    }
            //}
            return list;
        }

        public Task<int> GetStockListCountAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20)
        {
            return stockDbContext.StockListInfos.Where(e =>
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
        public async Task SynAllStockAsync()
        {
            try
            {
                var dataSH = await Tools.SendGetRequestAsync<List<StockDetail>>(dataUrl + "/get_sh_all_stocks", func: Tools.ResponseHandler);
                var dataSZ = await Tools.SendGetRequestAsync<List<StockDetail>>(dataUrl + "/get_sz_all_stocks", func: Tools.ResponseHandler);
                var dataBJ = await Tools.SendGetRequestAsync<List<StockDetail>>(dataUrl + "/get_bj_all_stocks", func: Tools.ResponseHandler);
                List<StockDetail> stockDetails= await stockDbContext.StockDetails.Where(e => e.StockCode != "").ToListAsync();
                foreach (var list in new[] { dataSH, dataSZ, dataBJ })
                {
                    if (list != null && list.Count > 0)
                    {
                        foreach (var o in list)
                        {
                            AddOrUpdateStockAsync(o, stockDetails);
                        }
                    }
                }
                await stockDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        private void UpdateStockInfo(Stock.Domain.Stock stock, Stock.Domain.Stock s)
        {
            stock.StockPrice = s.StockPrice;
            stock.ChangeRange = s.ChangeRange;
            stock.ChangeAmount = stock.ChangeAmount;
            stock.Turnover = s.Turnover;
            stock.TransactionVolume = s.TransactionVolume;
            stock.Amplitude = s.Amplitude;
            stock.Max = s.Max;
            stock.Min = s.Min;
            stock.TodayOpening = s.TodayOpening;
            stock.ClosedYesterday = s.ClosedYesterday;
            stock.EquivalentRatio = s.EquivalentRatio;
            stock.TurnoverRate = s.TurnoverRate;
            stock.ForwardPE = s.ForwardPE;
            stock.PB = s.PB;
            stock.MarketCap = s.MarketCap;
            stock.CirculationMarketValue = s.CirculationMarketValue;
            stock.SpeedUp = s.SpeedUp;
            stock.FiveMinute = s.FiveMinute;
        }
        private void UpdateStockInfo(Stock.Domain.Stock stock,Dictionary<string,string> s)
        {
            stock.StockPrice = Tools.Str2Double(s["最新价"]);
            stock.ChangeRange = new BandUnit(Tools.Str2Double(s["涨跌幅"]), "手");
            stock.ChangeAmount = Tools.Str2Double(s["涨跌额"]);
            stock.Turnover = new BandUnit(Tools.Str2Double(s["成交量"]), "手");
            stock.TransactionVolume = Tools.Str2Double(s["成交额"]);
            stock.Amplitude = new BandUnit(Tools.Str2Double(s["振幅"]), "%");
            stock.Max = Tools.Str2Double(s["最高"]);
            stock.Min = Tools.Str2Double(s["最低"]);
            stock.TodayOpening = Tools.Str2Double(s["今开"]);
            stock.ClosedYesterday = Tools.Str2Double(s["昨收"]);
            stock.EquivalentRatio = Tools.Str2Double(s["量比"]);
            stock.TurnoverRate = new BandUnit(Tools.Str2Double(s["换手率"]), "%");
            stock.ForwardPE = Tools.Str2Double(s["市盈率-动态"]);
            stock.PB = Tools.Str2Double(s["市净率"]);
            stock.MarketCap = Tools.Str2Double(s["总市值"]);
            stock.CirculationMarketValue = Tools.Str2Double(s["流通市值"]);
            stock.SpeedUp = Tools.Str2Double(s["涨速"]);
            stock.FiveMinute = new BandUnit(Tools.Str2Double(s["5分钟涨跌"]), "%");
            stock.SixtyDays = new BandUnit(Tools.Str2Double(s["60日涨跌幅"]), "%");
            stock.Year2Date = new BandUnit(Tools.Str2Double(s["年初至今涨跌幅"]), "%");
        }

        private void AddOrUpdateStockAsync(StockDetail s,List<StockDetail> stockDetails)
        {
            var s1 = stockDetails.SingleOrDefault(e => e.StockCode == s.StockCode);
            if (s1 != null)
            {
                s1.StockName = s.StockName;
                s1.MarketTime = s.MarketTime;
                s1.Exchange = s.Exchange;
                s1.Plate = s.Plate;
                s1.TotalEquity = s.TotalEquity;
                s1.CirculatingEquity = s.CirculatingEquity;
                s1.Industry = s.Industry;
                s1.Company = s.Company;
            }
            else
            {
                stockDbContext.StockDetails.Add(s);
            }
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