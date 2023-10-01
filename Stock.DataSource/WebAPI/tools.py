from typing import Callable
from Entities.enum_handler import EnumHandler
from Entities.financial_report_period import FinancialReportPeriod
from Entities.financial_report_type import FinancialReportType
from Entities.exchange_type import ExchangeType
import jsonpickle

def to_json(obj:object)->str:   
    jsonpickle.set_preferred_backend('json')
    jsonpickle.set_encoder_options('json', ensure_ascii=False)
    jsonpickle.handlers.registry.register(ExchangeType, EnumHandler)
    jsonpickle.handlers.registry.register(FinancialReportPeriod, EnumHandler)
    jsonpickle.handlers.registry.register(FinancialReportType, EnumHandler)
    return jsonpickle.encode(obj,unpicklable=False)


def to_json_callbak(func:Callable,*p):
    if callable(func):
        data=func(*p)
    if data:
        data=to_json(data)
    return data
