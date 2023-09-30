import datetime
from typing import Optional
from Entities.financial_report_period import FinancialReportPeriod
from Entities.financial_report_type import FinancialReportType
from Entities.fiancial_report_detail import FinancialReportDetail
class FinancialReport:
    def __init__(
        self,
        StockCode:str,
        ReportDate:datetime,
        ReleaseDate:datetime,
        UpdateTime:datetime,
        Currency:Optional[str],
        FinancialReportType:FinancialReportType,
        FinancialReportPeriod:FinancialReportPeriod,
        FinancialReportDetails:list[FinancialReportDetail]
        ):
        self.StockCode = StockCode
        self.ReportDate = ReportDate
        self.ReleaseDate = ReleaseDate
        self.UpdateTime = UpdateTime
        self.Currency = Currency
        self.FinancialReportType = FinancialReportType
        self.FinancialReportPeriod = FinancialReportPeriod
        self.FinancialReportDetails = FinancialReportDetails
        