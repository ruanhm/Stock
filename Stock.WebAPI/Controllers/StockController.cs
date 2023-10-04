using Microsoft.AspNetCore.Mvc;
using Stock.Common;
using Stock.Domain;
using Stock.Domain.Entities;
using Stock.WebAPI.Model;

namespace Stock.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockDomianService stockDomianService;
        public StockController(StockDomianService stockDomianService)
        {
            this.stockDomianService = stockDomianService;
        }
        [HttpGet]
        public async Task<StockList> GetStockList(string? StockCode,string? StockName,int Page,int PageSize)
        {
            return await stockDomianService.GetStockListAsync(StockCode, StockName, Page,PageSize);
        }
        [HttpGet]
        public async Task<StockDetail?> GetStockDetail(string StockCode)
        {
            return await stockDomianService.GetStockDetailAsync(StockCode);
        }
        [HttpGet]
        public async Task<List<FinancialReport>?> GetFinancialReport(GetFinancialReportRequest req)
        {
            return await stockDomianService.GetFinancialReportListAsync(req.StockCode,req.BeginReportDate,req.EndReportDate,req.FinancialReportType,req.FinancialReportPeriod);
        }
        [HttpGet]
        public async Task<IActionResult> SynAllStockAsync()
        {
            var isOk = true;
            try
            {
                await stockDomianService.SynAllStockAsync();
            }
            catch (Exception ex)
            {
                isOk = false;
                LogHelper.Error(ex);
            }
            return isOk?Ok("成功"):BadRequest();
        }
    }
}
