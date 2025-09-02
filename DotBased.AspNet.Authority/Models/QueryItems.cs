namespace DotBased.AspNet.Authority.Models;

public class QueryItems<TItem>
{
    private QueryItems(IEnumerable<TItem> items, int totalCount, int limit, int offset)
    {
        Items = items.ToList();
        TotalCount = totalCount;
        Limit = limit;
        Offset = offset;
    }
    
    public readonly IReadOnlyCollection<TItem> Items;

    public int Count => Items.Count;
    public int TotalCount { get; }
    public int Limit { get; }
    public int Offset { get; }

    public static QueryItems<TItem> Create(IEnumerable<TItem> items, int totalCount, int limit, int offset) => new(items, totalCount, limit, offset);
}