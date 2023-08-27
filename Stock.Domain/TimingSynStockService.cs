using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stock.Common;

namespace Stock.Domain
{
    public class TimingSynStockService : BackgroundService
    {
        private readonly IServiceScope _scope;
        private readonly IStockDomainRepository _repository;
        public TimingSynStockService(IServiceScopeFactory serviceScopeFactory) 
        { 
            _scope=serviceScopeFactory.CreateScope();
           var sp=_scope.ServiceProvider;
            _repository = sp.GetRequiredService<IStockDomainRepository>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _repository.SynAllStockAsync();
                    await Task.Delay(1000*60*60);
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
