using Stock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    public class StockList:Stock
    {
        private StockList() { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="stockCode">股票代码</param>
        /// <param name="stockName">股票名字</param>
        /// <param name="exchange">交易所</param>
        /// <param name="plate">板块</param>
        /// <param name="industry">行业</param>
        public StockList(string stockCode, string stockName, ExchangeType exchange, string plate, string industry)
        {
            
            this.StockCode = stockCode;
            ChangeStockName(stockName);
            this.Exchange = exchange;
            this.Plate = plate;
            this.Industry = industry;

        }
    }
}
