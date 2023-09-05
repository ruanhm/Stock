using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain
{
    public class StockDomianService
    {
        private readonly IStockDomainRepository _repository;
        public StockDomianService(IStockDomainRepository stockDomainRepository)
        { 
            _repository = stockDomainRepository;
        }

        public 
    }
}
