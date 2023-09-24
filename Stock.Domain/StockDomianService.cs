using Stock.Common;
using Stock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    public class StockDomianService: IScopeDenpendency
    {
        private readonly IStockDomainRepository _repository;
        public StockDomianService(IStockDomainRepository stockDomainRepository)
        { 
            _repository = stockDomainRepository;
        }
        public async Task<StockList> GetStockListAsync(string? stockCode, string? stockName, int page, int pagesize)
        {
            var list = await _repository.GetStockListAsync(stockCode, stockName, page, pagesize);
            var count = await _repository.GetStockListCountAsync(stockCode, stockName, page, pagesize);
            var beginIndex = page == 1 ? 1 : page * pagesize + 1;
            var endIndex = page * pagesize;
            return new StockList(list,beginIndex,endIndex,page,pagesize,count);
        }

        public async Task<StockDetail?> GetStockDetailAsync(string stockCode)
        {
            return await _repository.FindOneStockDetailAsync(stockCode);
        }
        public async Task<List<FinancialReport>?> GetFinancialReportListAsync(string stockCode, DateTime? beginReportDate, DateTime? endReportDate, FinancialReportType? financialReportType, FinancialReportPeriod? financialReportPeriod)
        {
            if (beginReportDate == null)
            {
                beginReportDate = DateTime.MinValue;
            }
            if (endReportDate == null)
            {
                endReportDate = DateTime.Now;
            }
            return await _repository.GetFinancialReportListAsync(stockCode, (DateTime)beginReportDate, (DateTime)endReportDate,financialReportType,financialReportPeriod);
        }

        public async Task SynAllStockAsync()
        {
            await _repository.SynAllStockAsync();
        }
    }
}
