from typing import Callable
from Entities.enum_handler import EnumHandler
from Entities.financial_report_period import FinancialReportPeriod
from Entities.financial_report_type import FinancialReportType
from Entities.exchange_type import ExchangeType
import jsonpickle
from datetime import datetime
import base64           # 导入 base64 模块
import gzip             # 导入 gzip 模块
import json             # 导入 json 模块
from io import BytesIO  # 从 io 模块导入 BytesIO 类

def to_json(obj:object)->str:   
    jsonpickle.set_preferred_backend('json')
    jsonpickle.set_encoder_options('json', ensure_ascii=False)
    jsonpickle.handlers.registry.register(ExchangeType, EnumHandler)
    jsonpickle.handlers.registry.register(FinancialReportPeriod, EnumHandler)
    jsonpickle.handlers.registry.register(FinancialReportType, EnumHandler)
    return jsonpickle.encode(obj,unpicklable=False)


def to_json_callbak(func:Callable,gzip:bool=True,*p):
    if callable(func):
        data=func(*p)
    if data:
        data=to_json(data)
        if gzip:
            data=gzip_str(data)
    return data

def gzip_str(to_gzip: str) -> str:
    out = BytesIO()                         # 创建一个 BytesIO 对象
    with gzip.GzipFile(fileobj=out, mode='w') as f:   # 使用 gzip 对象进行数据压缩
        f.write(to_gzip.encode())            # 将待压缩的字符串写入 gzip 对象中
    return base64.b64encode(out.getvalue()).decode()   # 对压缩后的数据使用 base64 编码并返回结果


# 定义一个函数，输入参数为经过压缩和 base64 编码后的字符串，输出参数为解压缩并解码后的字符串
def ungzip_str(to_ungzip: str) -> str:
    compressed = base64.b64decode(to_ungzip)   # 先对输入字符串进行 base64 解码，再将已压缩的数据转换为 bytes 格式
    with gzip.GzipFile(fileobj=BytesIO(compressed)) as f:  # 使用 GzipFile 对象读取已编码的二进制对象，并解压缩该对象
        return f.read().decode()   # 返回解压缩后的数据，要注意先用 decode 将 bytes 对象转换为字符串格式
