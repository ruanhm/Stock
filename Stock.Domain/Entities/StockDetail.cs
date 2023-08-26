
using Stock.Domain.Entities;

namespace Stock.Domain 
{
     public class StockDetail : Stock,IAggregateRoot
    {
        
        /// <summary>
        /// 公司全称
        /// </summary>
        public string Company { get; private set; }
        /// <summary>
        /// 上市时间
        /// </summary>
        public DateTime MarketTime { get; init; }
        /// <summary>
        /// 总股本
        /// </summary>
        public double TotalEquity { get; private set; }
        /// <summary>
        /// 流通股
        /// </summary>
        public double CirculatingEquity { get; private set; }

      
        private StockDetail() { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="stockCode">股票代码</param>
        /// <param name="stockName">股票名字</param>
        /// <param name="exchange">交易所</param>
        /// <param name="plate">板块</param>
        /// <param name="industry">行业</param>
        /// <param name="company">公司全称</param>
        /// <param name="marketTime">上市时间</param>
        /// <param name="totalEquity">总股本</param>
        /// <param name="circulatingEquity">流通股</param>
        public StockDetail(string stockCode, string stockName, ExchangeType exchange, string? plate, string industry,string company, DateTime marketTime,float totalEquity,float circulatingEquity)         
        {
            this.StockCode = stockCode;
            ChangeStockName(stockName);
            this.Exchange = exchange;
            this.Plate = plate;
            this.Industry = industry;
            this.Company = company;
            this.MarketTime = marketTime;
            this.TotalEquity = totalEquity;
            this.CirculatingEquity = circulatingEquity;

        }


    }
}
