import mplfinance as mpf
import pandas as pd
import datetime as dt
import pandas_datareader as pdr 

now = dt.datetime.now()
start = now - dt.timedelta(60)

stock = "AMZN"
filename = stock.lower()+'.png'

df = pdr.get_data_yahoo("603927.SS", "yahoo", start, now)
fig, axes = mpf.plot(df,type='candle',style='yahoo',returnfig=True)
