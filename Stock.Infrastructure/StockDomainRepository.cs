using Stock.Domain;
using Newtonsoft.Json;
using Stock.Common;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.VisualBasic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;

namespace Stock.Infrastructure
{
    public class StockDomainRepository : IStockDomainRepository
    {
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


        public Task SynFinancialReportAsync(string stockCode)
        {
            return Task.Run(async () =>
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

                if (list != null)
                {
                    var rTypeArr = new[] { FinancialReportType.BalanceSheet, FinancialReportType.IncomeStatement, FinancialReportType.CashFlowStatement }; 
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < list[i].Count; j++)
                        {
                            var dic = list[i][j];
                            DateTime reportDate = Tools.DateStr2DateTime(dic["报告日"], "yyyyMMdd");
                            DateTime releaseDate = Tools.DateStr2DateTime(dic["公告日期"], "yyyyMMdd");
                            DateTime updateTime = Tools.DateStr2DateTime(dic["更新日期"]);
                            var rType = rTypeArr[i];
                            var rPeriod = DateConvertFinancialReportPeriod(reportDate);
                            var r = new FinancialReport(stockCode, reportDate, updateTime, releaseDate, dic["币种"], rType, rPeriod);
                            string s = "报告日,数据源,定期报告,是否审计,公告日期,更新日期";
                            foreach (var kvp in list[i][j])
                            {
                                if (s.IndexOf(kvp.Key) < 0)
                                {
                                    r.AddFinancialReportDetail(kvp.Key, Tools.Str2Double(kvp.Value));
                                }
                            }
                            await AddFinancialReportAsync(stockCode, r);
                        }
                    }
                    await stockDbContext.SaveChangesAsync();
                }
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
                await Task.Run(() =>
                {
                    Tools.RunPython(ak =>
                    {
                        string data = "";
                        switch (stock.Exchange)
                        {
                            case ExchangeType.SH:
                                data = ak.stock_sh_a_spot_em().to_json(force_ascii: false, orient: "records");
                                break;
                            case ExchangeType.SZ:
                                data = ak.stock_sz_a_spot_em().to_json(force_ascii: false, orient: "records");
                                break;
                            case ExchangeType.BJ:
                                data = ak.stock_bj_a_spot_em().to_json(force_ascii: false, orient: "records");
                                break;
                        }   
                        var b = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
                        var s = b.FirstOrDefault(e => e["代码"] == stock.StockCode);
                        if (s != null)
                        {
                            UpdateStockInfo(stock, s);
                        }
                    });
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

        public async Task<List<StockList>> GetStockListAsync(string? stockCode, string? stockName, int page = 1, int pagesize = 20)
        {
            var list=await stockDbContext.StockLists.Where(e =>
                (string.IsNullOrEmpty(stockCode) || e.StockCode == stockCode)
                && (string.IsNullOrEmpty(stockName) || e.StockName == stockName)
                 ).Skip(page).Take(pagesize).ToListAsync();
            if (list != null && list.Count > 0)
            {
                await Task.Run(() =>
                {
                    var list1=Tools.RunPython(ak =>
                    {
                        string data =  ak.stock_zh_a_spot_em().to_json(force_ascii: false, orient: "records");
                        var b = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
                        return b;
                    });
                    if (list1 != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            var s = list1.FirstOrDefault(e => e["代码"] == list[i].StockCode);
                            if (s != null)
                            {
                                UpdateStockInfo(list[i], s);
                            }
                        }
                    }
                });
            }
            return list;
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
                Tools.RunPython(async ak =>
                {
                    //深证列表
                    foreach (var a in new [] { "A股列表", "B股列表" })
                    {
                        string data = ak.stock_info_sz_name_code(symbol: a).to_json(force_ascii: false, orient: "records");
                        var b = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
                        var prefix = a == "A股列表" ? "A股" : "B股";
                        if (b != null)
                        {
                            for (int i = 0; i < b.Count; i++)
                            {
                                await AddOrUpdateStockAsync(b[i][prefix + "代码"], b[i][prefix + "简称"], b[i][prefix + "上市日期"], ExchangeType.SZ,
                                    b[i]["板块"], b[i]["所属行业"], b[i][prefix + "总股本"], b[i][prefix + "流通股本"],null);
                            }
                        }
                        await Task.Delay(500);
                    }
                    //上证列表
                    foreach (var a in new[] { "主板A股", "主板B股", "科创板" })
                    {
                        string data = ak.stock_info_sh_name_code(symbol: a).to_json(force_ascii: false, orient: "records", date_format: "iso");
                        var b = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
                        if (b != null)
                        {
                            for (int i = 0; i < b.Count; i++)
                            {
                                data=ak.stock_individual_info_em(symbol : b[i]["证券代码"]).to_json(force_ascii: false, orient: "records", date_format: "iso");
                                var c= JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);
                                string? TotalEquity = null, CirculatingEquity = null, Industry = null;
                                if (c != null)
                                {
                                    for (int j = 0; j < c.Count; j++)
                                    {
                                        if (c[j]["item"]== "总市值")
                                            TotalEquity = c[j]["value"];
                                        else if (c[j]["item"] == "流通市值")
                                            CirculatingEquity = c[j]["value"];
                                        else if (c[j]["item"] == "行业")
                                            Industry = c[j]["value"];
                                    }
                                }
                                await AddOrUpdateStockAsync(b[i]["证券代码"], b[i]["证券简称"], b[i]["上市日期"],
                                    ExchangeType.SH, a, Industry, TotalEquity, CirculatingEquity, b[i]["公司全称"]);
                                await Task.Delay(200);
                            }
                        }
                        await Task.Delay(500);
                    }
                    //北证列表
                    string data1 = ak.stock_info_bj_name_code().to_json(force_ascii: false, orient: "records", date_format: "iso");
                    var b1 = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data1);
                    if (b1 != null)
                    {
                        for (int i = 0; i < b1.Count; i++)
                        {
                            await AddOrUpdateStockAsync(b1[i]["证券代码"], b1[i]["证券简称"], b1[i]["上市日期"], ExchangeType.BJ,
                                    null, b1[i]["所属行业"], b1[i]["总股本"], b1[i]["流通股本"], null);
                        }
                    }
                });
                stockDbContext.SaveChangesAsync();
            });
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


        private async Task AddOrUpdateStockAsync(string StockCode,string StockName,string MarketTime, ExchangeType Exchange,string? Plate,string? Industry, string? TotalEquity,string? CirculatingEquity,string? Company)
        {
            var s = await FindOneStockDetailAsync(StockCode);
            if(s != null)
            {
                s.StockName = StockName;
                s.MarketTime = Tools.DateStr2DateTime(MarketTime);
                s.Exchange = Exchange;
                s.Plate = Plate;
                s.TotalEquity = Tools.Str2Double(TotalEquity);
                s.CirculatingEquity = Tools.Str2Double(CirculatingEquity);
                s.Industry = Industry;
                s.Company = Company;
            }
            else
            {
                stockDbContext.StockDetails.Add(new StockDetail(StockCode, StockName, Exchange, Tools.DateStr2DateTime(MarketTime))
                {
                    Plate = Plate,
                    Industry = Industry,
                    TotalEquity = Tools.Str2Double(TotalEquity),
                    CirculatingEquity = Tools.Str2Double(CirculatingEquity),
                    Company= Company
                });
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