namespace Stock.WebAPI.Model
{
    public record GetStockListRequest(string? StockCode,string? StockName,int Page,int PageSize);
}
