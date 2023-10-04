
using Stock.Domain.Entities;

namespace Stock.Domain 
{
     public class StockDetail : Stock,IAggregateRoot
    {

        /// <summary>
        /// 交易所
        /// </summary>
        public ExchangeType Exchange { get; set; }
        /// <summary>
        /// 公司全称
        /// </summary>
        public string? Company { get; set; }
        /// <summary>
        /// 上市时间
        /// </summary>
        public DateTime MarketTime { get; set; }
        /// <summary>
        /// 总股本
        /// </summary>
        public double? TotalEquity { get; set; }
        /// <summary>
        /// 流通股
        /// </summary>
        public double? CirculatingEquity { get; set; }


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
        public StockDetail(string stockCode, string stockName, ExchangeType exchange, DateTime marketTime)
        {
            this.StockCode = stockCode;
            this.StockName = stockName;
            this.Exchange = exchange;
            this.MarketTime = marketTime;

        }


    }
}
