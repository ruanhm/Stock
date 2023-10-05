using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stock.Common;

namespace Stock.Domain
{
    public class TimingSynFinanciakReport : BackgroundService
    {
        private readonly IServiceScope _scope;
        private readonly IStockDomainRepository _repository;
        public TimingSynFinanciakReport(IServiceScopeFactory serviceScopeFactory)
        {
            _scope = serviceScopeFactory.CreateScope();
            var sp = _scope.ServiceProvider;
            _repository = sp.GetRequiredService<IStockDomainRepository>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var stockLists= await _repository.GetStockListAsync(null, null, 1, 99999);
                    for(int i = 0; i < stockLists.Count; i++)
                    {
                        try
                        {
                            await _repository.SynFinancialReportAsync(stockLists[i].StockCode);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex.Message, ex);
                        }
                    }
                    await Task.Delay(1000 * 60 * 60 * 24);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                    await Task.Delay(1000);
                }
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            _scope.Dispose();
        }
    }
}