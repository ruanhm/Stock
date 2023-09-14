using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain.Entities
{
    public class StockList
    {
        public List<StockListInfo> StockListData { get; init; }
        public int BeginIndex { get; init; }
        public int EndIndex { get; init; }
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
        public int TotalRows { get; init; }

        public StockList(List<StockListInfo> stockListInfos,int benginIndex,int endIndex,int pageIndex,int pageSize,int totalRows)
        {
            this.StockListData = stockListInfos;
            this.BeginIndex = benginIndex;
            this.EndIndex = endIndex;   
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalRows = totalRows;
        }
    }
}
