using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    public enum FinancialReportType
    {
        /// <summary>
        /// 资产负债表
        /// </summary>
        BalanceSheet = 1,

        /// <summary>
        /// 利润表
        /// </summary>
        IncomeStatement = 2,

        /// <summary>
        /// 现金流量表
        /// </summary>
        CashFlowStatement = 3
        
    }

    public enum FinancialReportPeriod
    {
        /// <summary>
        /// 一季报
        /// </summary>
        FirstQuarterlyReport = 1,

        /// <summary>
        /// 半年报
        /// </summary>
        SemiannualReport = 2,

        /// <summary>
        /// 三季报
        /// </summary>
        ThirdQuarterlyReport = 3,

        /// <summary>
        /// 年报
        /// </summary>
        AnnualReport = 4
    }
}
