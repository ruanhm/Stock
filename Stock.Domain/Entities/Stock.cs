using Stock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    public class Stock
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockCode { get; init; }

        /// <summary>
        /// 股票名字
        /// </summary>
        public string StockName { get; set; }

        /// <summary>
        /// 交易所
        /// </summary>
        public ExchangeType Exchange { get; set; }

        /// <summary>
        /// 板块
        /// </summary>
        public string? Plate { get; set; }

        /// <summary>
        /// 行业
        /// </summary>
        public string? Industry { get; set; }

        /// <summary>
        /// 股价
        /// </summary>
        public double? StockPrice { get; set; }

        /// <summary>
        /// 涨跌幅
        /// </summary>
        public string? ChangeRange { get; set; }

        /// <summary>
        /// 涨跌额
        /// </summary>
        public double? ChangeAmount { get; set; }

        /// <summary>
        /// 昨收
        /// </summary>
        public double? ClosedYesterday { get; set; }

        /// <summary>
        /// 今开
        /// </summary>
        public double? TodayOpening { get; set; }

        /// <summary>
        /// 最高
        /// </summary>
        public double? Max { get; set; }

        /// <summary>
        /// 最低
        /// </summary>
        public double? Min { get; set; }

        /// <summary>
        /// 成交量
        /// </summary>
        public string? Turnover { get; set; }

        /// <summary>
        /// 成交额
        /// </summary>
        public double? TransactionVolume { get; set; }

        /// <summary>
        /// 振幅
        /// </summary>
        public string? Amplitude { get; set; }

        /// <summary>
        /// 量比
        /// </summary>
        public string? EquivalentRatio { get; set; }

        /// <summary>
        /// 换手率
        /// </summary>
        public string? TurnoverRate { get; set; }

        /// <summary>
        /// 动态市盈率
        /// </summary>
        public string? ForwardPE { get; set; }

        /// <summary>
        /// 市净率
        /// </summary>
        public string? PB { get; set; }

        /// <summary>
        /// 总市值
        /// </summary>
        public double? MarketCap { get; set; }

        /// <summary>
        /// 流通市值
        /// </summary>
        public double? CirculationMarketValue { get; set; }

        /// <summary>
        /// 涨速
        /// </summary>
        public string? SpeedUp { get; set; }

        /// <summary>
        /// 5分钟涨跌
        /// </summary>
        public string? FiveMinute { get; set; }

        /// <summary>
        /// 60日涨跌幅
        /// </summary>
        public string? SixtyDays { get; set; }

        /// <summary>
        /// 年初至今涨跌幅	
        /// </summary>
        public string? Year2Date { get; set; }

        //private Stock() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockCode">股票代码</param>
        /// <param name="stockName">股票名字</param>
        /// <param name="exchange">交易所</param>
        /// <param name="plate">板块</param>
        /// <param name="industry">行业</param>
        //public Stock(string stockCode, string stockName, string exchange, string plate, string industry)
        //{
        //    this.StockCode = stockCode;
        //    this.StockName = stockName;
        //    this.Exchange = exchange;
        //    this.Plate = plate;
        //    this.Industry = industry;
        //}

        //public void ChangeStockName(string stockName)
        //{
        //    this.StockName = stockName;
        //}
    }
}
