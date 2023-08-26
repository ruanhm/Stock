using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    /// <summary>
    /// 资产负债表
    /// </summary>
    public class BalanceSheet
    {
        public string StockCode { get; init; }

        public int Id { get; init; }

        /// <summary>
        /// 应收利息
        /// </summary>
        public float? InterestReceivable { get; set; }

        /// <summary>
        /// 报告日
        /// </summary>
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// 货币资金
        /// </summary>
        public float? MonetaryFunds { get; set; }

        /// <summary>
        /// 结算备付金
        /// </summary>
        public float? SettlementProvisions { get; set; }

        /// <summary>
        /// 可供出售金融资产	
        /// </summary>
        public float? AvailableForSaleFinancialAssets { get; set; }

        /// <summary>
        /// 持有至到期投资
        /// </summary>
        public float? HeldToMaturityInvestments { get; set; }

        /// <summary>
        /// 长期股权投资	
        /// </summary>
        public float? LongTermEquityInvestment { get; set; }

        /// <summary>
        /// 固定资产
        /// </summary>
        public float? FixedAssets { get; set; }

        /// <summary>
        /// 无形资产
        /// </summary>
        public float? IntangibleAssets { get; set; }

        /// <summary>
        /// 递延所得税资产
        /// </summary>
        public float? DeferredTaxAssets { get; set; }

        /// <summary>
        /// 资产总计
        /// </summary>
        public float? TotalAssets { get; set; }

        /// <summary>
        /// 交易性金融负债
        /// </summary>
        public float? TradingFinancialLiabilities { get; set; }

        /// <summary>
        /// 应付职工薪酬	
        /// </summary>
        public float? PayrollPayable { get; set; }

        /// <summary>
        /// 应交税费
        /// </summary>
        public float? TaxesPayable { get; set; }

        /// <summary>
        /// 应付利息
        /// </summary>
        public float? InterestPayable { get; set; }

        /// <summary>
        /// 应付债券
        /// </summary>
        public float? BondsPayable { get; set; }

        /// <summary>
        /// 递延所得税负债
        /// </summary>
        public float? DeferredIncomeTaxLiabilities { get; set; }

        /// <summary>
        /// 负债合计	
        /// </summary>
        public float? TotalLiabilities { get; set; }

        /// <summary>
        /// 实收资本(或股本)	
        /// </summary>
        public float? PaidInCapitalOrEquity { get; set; }

        /// <summary>
        /// 资本公积金
        /// </summary>
        public float? CapitalReserveFund { get; set; }

        /// <summary>
        /// 盈余公积金
        /// </summary>
        public float? SurplusReserveFund { get; set; }

        /// <summary>
        /// 未分配利润
        /// </summary>
        public float? UndistributedProfits { get; set; }

        /// <summary>
        /// 归属于母公司股东权益合计
        /// </summary>
        public float? TotalShareholdersEquityAttributableToTheParentCompany { get; set; }

        /// <summary>
        /// 少数股东权益
        /// </summary>
        public float? MinorityEquity { get; set; }

        /// <summary>
        /// 负债和股东权益总计
        /// </summary>
        public float? TotalLiabilitiesAndShareholdersEquity { get; set; }

        /// <summary>
        /// 所有者权益合计
        /// </summary>
        public float? TotalOwnersEquity { get; set; }


        /// <summary>
        /// 应收票据
        /// </summary>
        public float? NotesReceivable { get; set; }

        /// <summary>
        /// 应收账款
        /// </summary>
        public float? AccountsReceivable { get; set; }

        /// <summary>
        /// 预付账款	
        /// </summary>
        public float? Prepayments { get; set; }

        /// <summary>
        /// 其他应收款	
        /// </summary>
        public float? OtherReceivables { get; set; }

        /// <summary>
        /// 其他流动资产	
        /// </summary>
        public float? OtherCurrentAssets { get; set; }

        /// <summary>
        /// 流动资产合计	
        /// </summary>
        public float? TotalCurrentAssets { get; set; }

        /// <summary>
        /// 存货
        /// </summary>
        public float? Inventory { get; set; }

        /// <summary>
        /// 在建工程
        /// </summary>
        public float? ConstructionInProgress { get; set; }

        /// <summary>
        /// 工程物资
        /// </summary>
        public float? EngineeringMaterials { get; set; }

        /// <summary>
        /// 长期待摊费用
        /// </summary>
        public float? LongTermDeferredExpenses { get; set; }

        /// <summary>
        /// 非流动资产合计
        /// </summary>
        public float? TotalNonCurrentAssets { get; set; }

        /// <summary>
        /// 短期借款
        /// </summary>
        public float? ShortTermLoans { get; set; }

        /// <summary>
        /// 应付股利	
        /// </summary>
        public float? DividendsPayable { get; set; }

        /// <summary>
        /// 其他应付款
        /// </summary>
        public float? OtherAccountsPayables { get; set; }

        /// <summary>
        /// 一年内到期的非流动负债
        /// </summary>
        public float? NonCurrentLiabilitiesDueWithinOneYear { get; set; }

        /// <summary>
        /// 其他流动负债
        /// </summary>
        public float? OtherCurrentLiabilities { get; set; }

        /// <summary>
        /// 长期应付款
        /// </summary>
        public float? LongTermAccountsPayable { get; set; }

        /// <summary>
        /// 应付账款
        /// </summary>
        public float? AccountsPayable { get; set; }

        /// <summary>
        /// 预收账款
        /// </summary>
        public float? AdvanceReceipts { get; set; }

        /// <summary>
        /// 流动负债合计
        /// </summary>
        public float? TotalCurrentLiabilities { get; set; }

        /// <summary>
        /// 应付票据	
        /// </summary>
        public float? NotesPayable { get; set; }

        /// <summary>
        /// 长期借款
        /// </summary>
        public float? LongTermLoans { get; set; }

        /// <summary>
        /// 专项应付款
        /// </summary>
        public float? SpecialAccountsPayable { get; set; }

        /// <summary>
        /// 其他非流动负债
        /// </summary>
        public float? OtherNonCurrentLiabilities { get; set; }

        /// <summary>
        /// 非流动负债合计
        /// </summary>
        public float? TotalNonCurrentLiabilities { get; set; }

        /// <summary>
        /// 专项储备
        /// </summary>
        public float? SpecialReserve { get; set; }

        /// <summary>
        /// 商誉
        /// </summary>
        public float? Goodwill { get; set; }

        /// <summary>
        /// 报告截止日
        /// </summary>
        public DateTime ReportDeadline { get; set; }

        /// <summary>
        /// 公告日
        /// </summary>
        public DateTime AnnouncementDate { get; set; }
        







    }
}
