namespace ParkingApi.Core.Models;

public class PagedResult<T>
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int TotalPage { get; set; }
    public List<T> Data { get; set; }

    public PagedResult(IEnumerable<T> data, int total, int page, int limit)
    {
        Data = data.ToList();
        Total = total;
        Page = page;
        TotalPage = (int)Math.Ceiling((double)total / limit);
    }
}
