using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
    public const int DefaultBatchSize = 100;

    public static IEnumerable<IEnumerable<T>> BatchWithDefaultSize<T>(this IEnumerable<T> items)
    {
        return Batch(items, DefaultBatchSize);
    }

    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int batchSize)
    {
        return items.Select((item, inx) => new { item, inx })
            .GroupBy(x => x.inx / batchSize)
            .Select(g => g.Select(x => x.item));
    }
}