from datetime import datetime
from Entities.stock import Stock
from Entities.exchange_type import ExchangeType
from Entities.band_unit import BandUnit
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

def get_all_stock_real_time_quotes_for_ak()->list[Stock]:
    data=ak.stock_zh_a_spot_em()
    data=data.fillna(value=0) 
    list_stocks=[]
    for index, row in data.iterrows():
        list_stocks.append(Stock(
            StockCode=row['代码'],
            StockName=row['名称'],
            Exchange=0,
            StockPrice=row['最新价'],
            ChangeRange=BandUnit(row['涨跌幅'],'手'),
            ChangeAmount=row['涨跌额'],
            TransactionVolume=row['成交额'],
            Turnover=BandUnit(row['成交量'],'手'),
            Amplitude=BandUnit(row['振幅'],'%'),
            Max=row['最高'],
            Min=row['最低'],
            TodayOpening=row['今开'],
            ClosedYesterday=row['昨收'],
            EquivalentRatio=row['量比'],
            TurnoverRate=BandUnit(row['换手率'],'%'),
            ForwardPE=row['市盈率-动态'],
            PB=row['市净率'],
            MarketCap=row['总市值'],
            SpeedUp=row['涨速'],
            CirculationMarketValue=row['流通市值'],
            FiveMinute=BandUnit(row['5分钟涨跌'],'%'),
            SixtyDays=BandUnit(row['60日涨跌幅'],'%'),
            Year2Date=BandUnit(row['年初至今涨跌幅'],'%')
            ))
    return list_stocks

def get_stock_real_time_quotes_for_ak(stock_code:str='000001',exchange:ExchangeType=ExchangeType.SH)->Stock:
    if exchange==ExchangeType.SH:
        data=ak.stock_sh_a_spot_em()
    elif exchange==ExchangeType.SZ:
        data=ak.stock_sz_a_spot_em()
    elif exchange==ExchangeType.BJ:
        data=ak.stock_bj_a_spot_em()
    data=data[data['代码'] == stock_code]
    data=data.fillna(value=0) 
    for index, row in data.iterrows():
        return Stock(
            StockCode=row['代码'],
            StockName=row['名称'],
            Exchange=exchange,
            StockPrice=row['最新价'],
            ChangeRange=BandUnit(row['涨跌幅'],'手'),
            ChangeAmount=row['涨跌额'],
            TransactionVolume=row['成交额'],
            Turnover=BandUnit(row['成交量'],'手'),
            Amplitude=BandUnit(row['振幅'],'%'),
            Max=row['最高'],
            Min=row['最低'],
            TodayOpening=row['今开'],
            ClosedYesterday=row['昨收'],
            EquivalentRatio=row['量比'],
            TurnoverRate=BandUnit(row['换手率'],'%'),
            ForwardPE=row['市盈率-动态'],
            PB=row['市净率'],
            MarketCap=row['总市值'],
            SpeedUp=row['涨速'],
            CirculationMarketValue=row['流通市值'],
            FiveMinute=BandUnit(row['5分钟涨跌'],'%'),
            SixtyDays=BandUnit(row['60日涨跌幅'],'%'),
            Year2Date=BandUnit(row['年初至今涨跌幅'],'%')
        )
    
def get_sh_stock_real_time_quotes_for_ak()->list[Stock]:
    data=ak.stock_sh_a_spot_em()   
    data=data.fillna(value=0) 
    list_stocks=[]
    for index, row in data.iterrows():
        list_stocks.append(Stock(
            StockCode=row['代码'],
            StockName=row['名称'],
            Exchange=ExchangeType.SH,
            StockPrice=row['最新价'],
            ChangeRange=BandUnit(row['涨跌幅'],'手'),
            ChangeAmount=row['涨跌额'],
            TransactionVolume=row['成交额'],
            Turnover=BandUnit(row['成交量'],'手'),
            Amplitude=BandUnit(row['振幅'],'%'),
            Max=row['最高'],
            Min=row['最低'],
            TodayOpening=row['今开'],
            ClosedYesterday=row['昨收'],
            EquivalentRatio=row['量比'],
            TurnoverRate=BandUnit(row['换手率'],'%'),
            ForwardPE=row['市盈率-动态'],
            PB=row['市净率'],
            MarketCap=row['总市值'],
            SpeedUp=row['涨速'],
            CirculationMarketValue=row['流通市值'],
            FiveMinute=BandUnit(row['5分钟涨跌'],'%'),
            SixtyDays=BandUnit(row['60日涨跌幅'],'%'),
            Year2Date=BandUnit(row['年初至今涨跌幅'],'%')
        ))
    return list_stocks
    
def get_sz_stock_real_time_quotes_for_ak()->list[Stock]:
    data=ak.stock_sz_a_spot_em()
    data=data.fillna(value=0) 
    list_stocks=[]
    for index, row in data.iterrows():
        list_stocks.append(Stock(
            StockCode=row['代码'],
            StockName=row['名称'],
            Exchange=ExchangeType.SZ,
            StockPrice=row['最新价'],
            ChangeRange=BandUnit(row['涨跌幅'],'手'),
            ChangeAmount=row['涨跌额'],
            TransactionVolume=row['成交额'],
            Turnover=BandUnit(row['成交量'],'手'),
            Amplitude=BandUnit(row['振幅'],'%'),
            Max=row['最高'],
            Min=row['最低'],
            TodayOpening=row['今开'],
            ClosedYesterday=row['昨收'],
            EquivalentRatio=row['量比'],
            TurnoverRate=BandUnit(row['换手率'],'%'),
            ForwardPE=row['市盈率-动态'],
            PB=row['市净率'],
            MarketCap=row['总市值'],
            SpeedUp=row['涨速'],
            CirculationMarketValue=row['流通市值'],
            FiveMinute=BandUnit(row['5分钟涨跌'],'%'),
            SixtyDays=BandUnit(row['60日涨跌幅'],'%'),
            Year2Date=BandUnit(row['年初至今涨跌幅'],'%')
        ))
    return list_stocks
def get_bj_stock_real_time_quotes_for_ak()->list[Stock]:
    data=ak.stock_bj_a_spot_em()
    data=data.fillna(value=0) 
    list_stocks=[]
    for index, row in data.iterrows():
        list_stocks.append(Stock(
            StockCode=row['代码'],
            StockName=row['名称'],
            Exchange=ExchangeType.BJ,
            StockPrice=row['最新价'],
            ChangeRange=BandUnit(row['涨跌幅'],'手'),
            ChangeAmount=row['涨跌额'],
            TransactionVolume=row['成交额'],
            Turnover=BandUnit(row['成交量'],'手'),
            Amplitude=BandUnit(row['振幅'],'%'),
            Max=row['最高'],
            Min=row['最低'],
            TodayOpening=row['今开'],
            ClosedYesterday=row['昨收'],
            EquivalentRatio=row['量比'],
            TurnoverRate=BandUnit(row['换手率'],'%'),
            ForwardPE=row['市盈率-动态'],
            PB=row['市净率'],
            MarketCap=row['总市值'],
            SpeedUp=row['涨速'],
            CirculationMarketValue=row['流通市值'],
            FiveMinute=BandUnit(row['5分钟涨跌'],'%'),
            SixtyDays=BandUnit(row['60日涨跌幅'],'%'),
            Year2Date=BandUnit(row['年初至今涨跌幅'],'%')
        ))
    return list_stocks
         
    