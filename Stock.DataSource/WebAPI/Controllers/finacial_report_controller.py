from datetime import datetime
from Entities.fiancial_report import FinancialReport
from Entities.fiancial_report_detail import FinancialReportDetail
from Entities.financial_report_period import FinancialReportPeriod
from Entities.financial_report_type import FinancialReportType
import akshare as ak
import pandas as pd

def get_finacial_report_for_ak(stock_code:str)->list[FinancialReport]:
    data_zc= ak.stock_financial_report_sina(stock=stock_code, symbol="资产负债表")
    data_lr= ak.stock_financial_report_sina(stock=stock_code, symbol="利润表")
    data_ll= ak.stock_financial_report_sina(stock=stock_code, symbol="现金流量表") 
    list_f=[]
    i=1
    for df in [data_zc,data_lr,data_ll]:
        if i==1:
            t=FinancialReportType.BalanceSheet
        elif i==2:
            t=FinancialReportType.IncomeStatement
        elif i==3:
            t=FinancialReportType.CashFlowStatement
        for idx,row in df.iterrows():
            list_dtl=[]
            for col in row.index:
                if col=='报告日':
                    report_date=datetime.strptime(str(row['报告日']), "%Y%m%d")
                elif col=='公告日期':
                    release_date=datetime.strptime(str(row['公告日期']), "%Y%m%d")
                elif col=='更新日期':
                    update_date=row['更新日期']
                elif col=='币种':
                    currency=row['币种']
                elif '数据源,定期报告,是否审计,类型'.find(col)==-1:
                    list_dtl.append(FinancialReportDetail(
                        ItemName=col,
                        ItemValue=nan_convert_null(row[col])
                    ))
            list_f.append(FinancialReport(
                StockCode=stock_code,
                ReportDate=report_date,
                ReleaseDate=release_date,
                UpdateTime=update_date,
                Currency=currency,
                FinancialReportType=t,
                FinancialReportPeriod=date_convert_financial_report_period(report_date),
                FinancialReportDetails=list_dtl
            ))
        i+=1    
    return list_f

def date_convert_financial_report_period(date:datetime)->FinancialReportPeriod:
    if date.month==3 and date.day==31:
        p=FinancialReportPeriod.FirstQuarterlyReport
    elif date.month==6 and date.day==30:
        p=FinancialReportPeriod.SemiannualReport
    elif date.month==9 and date.day==30:
        p=FinancialReportPeriod.ThirdQuarterlyReport
    elif date.month==12 and date.day==31:
        p=FinancialReportPeriod.AnnualReport
    return p

def nan_convert_null(obj):
    if pd.isna(obj):
        return None
    else:
        return obj