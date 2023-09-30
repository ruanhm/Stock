from datetime import datetime
from Entities.stock import Stock
from Entities.exchange_type import ExchangeType
from Entities.band_unit import BandUnit
from tools import to_json
import akshare as ak
def get_sh_all_stocks_for_ak()->list[Stock]:
    data_sh_zbag= ak.stock_info_sh_name_code(symbol="主板A股")
    data_sh_zbbg= ak.stock_info_sh_name_code(symbol="主板B股")
    data_sh_kcb= ak.stock_info_sh_name_code(symbol="科创板")
    list_stocks=[]
    for df in [data_sh_zbag,data_sh_zbbg,data_sh_kcb]:
        for index, row in df.iterrows():
            list_stocks.append(Stock(
                StockCode=row['证券代码'],
                StockName=row['证券简称'],
                Exchange=ExchangeType.SH,
                Company=row['公司全称'],
                MarketTime=row['上市日期']
                ))
    return list_stocks

def get_sz_all_stocks_for_ak()->list[Stock]:
    data_sz_ag= ak.stock_info_sz_name_code(symbol="A股列表")
    data_sz_bg= ak.stock_info_sz_name_code(symbol="B股列表")
    list_stocks=[]
    i=1
    for df in [data_sz_ag,data_sz_bg]:
        prefix = 'A股' if i == 1 else 'B股'
        for index, row in df.iterrows():
            list_stocks.append(Stock(
                StockCode=row[prefix + '代码'],
                StockName=row[prefix + '简称'],
                Exchange=ExchangeType.SZ,
                MarketTime=row[prefix +'上市日期'],
                Industry=row['所属行业'],
                TotalEquity=row[prefix +'总股本'],
                CirculatingEquity=row[prefix +'流通股本']
                ))
        i=i+1
    return list_stocks

def get_bj_all_stocks_for_ak()->list[Stock]:
    data_bj= ak.stock_info_bj_name_code()
    list_stocks=[]
    for index, row in data_bj.iterrows():
        list_stocks.append(Stock(
            StockCode=row['证券代码'],
            StockName=row['证券简称'],
            Exchange=ExchangeType.BJ,
            MarketTime=row['上市日期'],
            Industry=row['所属行业'],
            TotalEquity=row['总股本'],
            CirculatingEquity=row['流通股本']
            ))
    return list_stocks

def get_stock_info_for_ak(stock_code:str='000001')->Stock:
    data=ak.stock_individual_info_em(symbol=stock_code)
    for index, row in data.iterrows():
        if row['item']=='股票简称':
            stock_name=row['value']
        elif row['item']=='行业':
            industry=row['value']
        elif row['item']=='总股本':
            total_equity=row['value']
        elif row['item']=='流通股':
            circulating_equity=row['value']
        elif row['item']=='总市值':
            market_cap=row['value']
        elif row['item']=='流通市值':
            circulation_market_value=row['value']
        elif row['item']=='上市时间':
            market_time=datetime.strptime(str(row['value']), "%Y%m%d")
    return Stock(
            StockCode=stock_code,
            StockName=stock_name,
            Industry=industry,
            TotalEquity=total_equity,
            CirculatingEquity=circulating_equity,
            MarketCap=market_cap,
            CirculationMarketValue=circulation_market_value,
            MarketTime=market_time
            )

