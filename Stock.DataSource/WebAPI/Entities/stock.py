from Entities.exchange_type import ExchangeType
from Entities.band_unit import BandUnit
from datetime import datetime
from typing import Optional
class Stock:
    def __init__(
            self,
            StockCode:Optional[str],#股票代码
            StockName:Optional[str]=None,#
            Exchange:Optional[ExchangeType]=None,
            MarketTime:Optional[datetime]=None,
            Plate:Optional[str]=None,
            Company:Optional[str]=None,
            Industry:Optional[str]=None,
            TotalEquity:Optional[float]=None,
            CirculatingEquity:Optional[float]=None,
            StockPrice:Optional[float]=None,
            ChangeRange:Optional[BandUnit]=None,
            ChangeAmount:Optional[float]=None,
            ClosedYesterday:Optional[float]=None,
            TodayOpening:Optional[float]=None,
            Max:Optional[float]=None,
            Min:Optional[float]=None,
            Turnover:Optional[BandUnit]=None,
            TransactionVolume:Optional[float]=None,
            Amplitude:Optional[BandUnit]=None,
            EquivalentRatio:Optional[float]=None,
            TurnoverRate:Optional[BandUnit]=None,
            ForwardPE:Optional[float]=None,
            PB:Optional[float]=None,
            MarketCap:Optional[float]=None,
            CirculationMarketValue:Optional[float]=None,
            SpeedUp:Optional[float]=None,
            FiveMinute:Optional[BandUnit]=None,
            SixtyDays:Optional[BandUnit]=None,
            Year2Date:Optional[BandUnit]=None
            ):
        self.StockCode=StockCode
        self.StockName=StockName
        self.Exchange=Exchange
        self.Plate=Plate
        self.Company=Company
        self.MarketTime=MarketTime
        self.TotalEquity=TotalEquity
        self.CirculatingEquity=CirculatingEquity
        self.Industry=Industry
        self.StockPrice=StockPrice
        self.ChangeRange=ChangeRange
        self.ChangeAmount=ChangeAmount
        self.ClosedYesterday=ClosedYesterday
        self.TodayOpening=TodayOpening
        self.Max=Max
        self.Min=Min
        self.Turnover = Turnover
        self.TransactionVolume = TransactionVolume
        self.Amplitude = Amplitude
        self.EquivalentRatio = EquivalentRatio
        self.TurnoverRate = TurnoverRate
        self.ForwardPE = ForwardPE
        self.PB = PB
        self.MarketCap = MarketCap
        self.CirculationMarketValue = CirculationMarketValue
        self.SpeedUp = SpeedUp
        self.FiveMinute = FiveMinute
        self.SixtyDays = SixtyDays
        self.Year2Date = Year2Date