using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    public class FinancialReportDetail
    {
        public long ID { get; init; }
        public long ReportID { get; init; }
        public FinancialReport FinancialReport { get; init; }
        public string ItemName { get; init; }
        public double? ItemValue { get; init; }
        private FinancialReportDetail() { }
        public FinancialReportDetail(string itemName, double? itemValue,FinancialReport financialReport)
        {
            this.ReportID = financialReport.ReportID;
            this.ItemName = itemName;
            this.ItemValue = itemValue;
            this.FinancialReport = financialReport;
        }
    }
}
