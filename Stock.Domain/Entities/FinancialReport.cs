using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    public class FinancialReport:IAggregateRoot
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockCode { get; init; }
        /// <summary>
        /// 报告id
        /// </summary>
        public long ReportID { get; init; }
        /// <summary>
        /// 报告期
        /// </summary>
        public DateTime ReportDate { get; init; }
        /// <summary>
        /// 公告日期
        /// </summary>
        public DateTime ReleaseDate { get; init; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; init; }
        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; init; }
        /// <summary>
        /// 财报类型：资产负债表、利润表、现金流量表
        /// </summary>
        public FinancialReportType FinancialReportType { get; init; }
        /// <summary>
        /// 财报时期：一季报、半年报、三季报、年报
        /// </summary>
        public FinancialReportPeriod FinancialReportPeriod { get; init; }
        /// <summary>
        /// 财报明细
        /// </summary>
        public List<FinancialReportDetail> FinancialReportDetails { get; private set; }
        private FinancialReport() { }
        /// <summary>
        /// 财报
        /// </summary>
        /// <param name="stockCode">股票代码</param>
        /// <param name="reportDate">报告期</param>
        /// <param name="updateTime">更新时间</param>
        /// <param name="currency">币种</param>
        /// <param name="financialReportType">财报类型</param>
        /// <param name="financialReportPeriod">财报时期</param>
        public FinancialReport(string stockCode,DateTime reportDate, DateTime updateTime,DateTime releaseDate, string currency, FinancialReportType financialReportType, FinancialReportPeriod financialReportPeriod)
        {
            this.StockCode = stockCode;
            this.ReportDate = reportDate;
            this.UpdateTime = updateTime;
            this.ReleaseDate = releaseDate;
            this.Currency = currency;
            this.FinancialReportType = financialReportType;
            this.FinancialReportPeriod = financialReportPeriod;
            this.FinancialReportDetails = new List<FinancialReportDetail>();
        }
        /// <summary>
        /// 添加财报明细
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemValue"></param>
        public void AddFinancialReportDetail(string itemName,double? itemValue)
        {
            this.FinancialReportDetails.Add(new FinancialReportDetail(itemName, itemValue, this));
        }
    }
}
