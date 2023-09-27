from fastapi import FastAPI
import akshare as ak

app=FastAPI()

@app.get('/')
async def root():
    return {"msg":"Welcome to my Page"}

@app.get('/api/stock_info_sh_name_code')
async def stock_info_sh_name_code(symbol):
    a=ak.stock_info_sh_name_code(symbol=symbol)
    return a.to_json(force_ascii=False, orient="records")

if __name__ == '__main__':
	uvicorn.run(app='main:app', host="127.0.0.1", port=8000, reload=True) 