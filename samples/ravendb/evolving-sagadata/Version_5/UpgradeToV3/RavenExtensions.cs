using System.Collections.Generic;
using System.Linq;
using Raven.Abstractions.Commands;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;

#region RavenExtensions

public static class RavenExtensions
{
    
    public static void BatchDelete(this DocumentStore store, IEnumerable<string> keysToDelete)
    {
        // Process groups in batches of 1000 to avoid a possible timeout
        foreach (var batch in keysToDelete.Batch(1000))
        {
            DeleteCommandData[] deleteCommands = batch
                .Select(key => new DeleteCommandData
                {
                    Key = key
                })
                .ToArray();
            store.DatabaseCommands.Batch(deleteCommands);
        }
    }

    static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int maxItems)
    {
        return items.Select((item, index) => new
        {
            item,
            index
        })
            .GroupBy(x => x.index/maxItems)
            .Select(g => g.Select(x => x.item));
    }

    public static IEnumerable<StreamResult<T>> Stream<T>(this IDocumentStore store, string prefix)
    {
        using (IDocumentSession session = store.OpenSession())
        using (IEnumerator<StreamResult<T>> enumerator = session.Advanced.Stream<T>(prefix))
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }

    public static string GetDocumentPrefix<T>(this DocumentStore store)
    {
        return store.Conventions.FindTypeTagName(typeof(T)) + "/";
    }
}

#endregion