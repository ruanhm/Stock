using Microsoft.AspNetCore.Mvc;
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
        public async Task<StockList> GetStockList(GetStockListRequest req)
        {
            return await stockDomianService.GetStockListAsync(req.StockCode, req.StockName, req.Page, req.PageSize);
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

    }
}
