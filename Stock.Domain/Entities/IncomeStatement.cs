using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    public class IncomeStatement
    {       
        public string StockCode { get; init; }

        public int Id { get; init; }

        /// <summary>
        /// 投资收益
        /// </summary>
        public float? InvestmentIncome { get; set; }

        /// <summary>
        /// 联营企业和合营企业的投资收益	
        /// </summary>
        public float? InvestmentIncomeFromAssociatesAndJointVentures { get; set; }

        /// <summary>
        /// 营业税金及附加
        /// </summary>
        public float? BusinessTaxesAndSurcharges { get; set; }

        /// <summary>
        /// 营业总收入	
        /// </summary>
        public float? TotalOperatingIncome { get; set; }

        /// <summary>
        /// 营业总成本
        /// </summary>
        public float? TotalOperatingCost { get; set; }

        /// <summary>
        /// 营业收入
        /// </summary>
        public float? OperatingIncome { get; set; }

        /// <summary>
        /// 营业成本
        /// </summary>
        public float? OperatingCosts { get; set; }

        /// <summary>
        /// 资产减值损失
        /// </summary>
        public float? AssetImpairmentLoss { get; set; }

        /// <summary>
        /// 营业利润
        /// </summary>
        public float? OperatingProfit { get; set; }

        /// <summary>
        /// 营业外收入	
        /// </summary>
        public float? NonOperatingIncome { get; set; }

        /// <summary>
        /// 营业外支出
        /// </summary>
        public float? NonOperatingExpenses { get; set; }

        /// <summary>
        /// 利润总额
        /// </summary>
        public float? TotalProfit { get; set; }

        /// <summary>
        /// 所得税
        /// </summary>
        public float? IncomeTax { get; set; }

        /// <summary>
        /// 净利润
        /// </summary>
        public float? NetProfit { get; set; }

        /// <summary>
        /// 归属净利润	
        /// </summary>
        public float? AttributableNetProfit { get; set; }

        /// <summary>
        /// 管理费用	
        /// </summary>
        public float? AdministrativeExpenses { get; set; }

        /// <summary>
        /// 销售费用
        /// </summary>
        public float? SellingExpenses { get; set; }

        /// <summary>
        /// 财务费用
        /// </summary>
        public float? FinancialExpenses { get; set; }

        /// <summary>
        /// 综合收益总额
        /// </summary>
        public float? TotalComprehensiveIncome { get; set; }

        /// <summary>
        /// 归属于少数股东的综合收益总额
        /// </summary>
        public float? TotalComprehensiveIncomeAttributableToMinorityShareholders { get; set; }

        /// <summary>
        /// 公允价值变动收益
        /// </summary>
        public float? GainsFromChangesInFairValue { get; set; }

        /// <summary>
        /// 已赚保费
        /// </summary>
        public float? PremiumEarned { get; set; }

        /// <summary>
        /// 报告截止日
        /// </summary>
        public DateTime ReportingDeadline { get; set; }

        /// <summary>
        /// 公告日
        /// </summary>
        public DateTime AnnouncementDate { get; set; }
    }
}
