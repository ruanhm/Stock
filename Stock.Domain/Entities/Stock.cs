using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain 
{
     public class Stock:IAggregateRoot
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockCode { get; init; }
        /// <summary>
        /// 股票名字
        /// </summary>
        public string StockName { get; private set; }
        /// <summary>
        /// 公司全称
        /// </summary>
        public string Company { get; private set; }
        /// <summary>
        /// 上市时间
        /// </summary>
        public string MarketTime { get; init; }
        /// <summary>
        /// 行业
        /// </summary>
        public string Industry { get; init; }
        /// <summary>
        /// 总股本
        /// </summary>
        public string TotalEquity { get; private set; }
        /// <summary>
        /// 流通股
        /// </summary>
        public string CirculatingEquity { get; private set; }


    }
}
