from fastapi import APIRouter
import Controllers as c
from tools import to_json_callbak
router = APIRouter(prefix="/api", tags=["数据接口"])

@router.get('/get_sh_all_stocks')
async def get_sh_all_stocks():
     return to_json_callbak(c.get_sh_all_stocks_for_ak)

@router.get('/get_sz_all_stocks')
async def get_sz_all_stocks():
     return to_json_callbak(c.get_sz_all_stocks_for_ak)

@router.get('/get_bj_all_stocks')
async def get_bj_all_stocks():
     return to_json_callbak(c.get_bj_all_stocks_for_ak)

@router.get('/get_stock_info')
async def get_stock_info(stock_code):
     return to_json_callbak(c.get_stock_info_for_ak,stock_code)

@router.get('/get_all_stock_real_time_quotes')
async def get_all_stock_real_time_quotes():
     return to_json_callbak(c.get_all_stock_real_time_quotes_for_ak)

@router.get('/get_stock_real_time_quotes')
async def get_stock_real_time_quotes(stock_code):
     return to_json_callbak(c.get_stock_real_time_quotes_for_ak,stock_code)

@router.get('/get_finacial_report')
async def get_finacial_report(stock_code):
     return to_json_callbak(c.get_finacial_report_for_ak,stock_code)