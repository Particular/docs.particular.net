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
        DeleteCommandData[] deleteCommands = keysToDelete
            .Select(key => new DeleteCommandData
            {
                Key = key
            })
            .ToArray();
        store.DatabaseCommands.Batch(deleteCommands);
    }

    public static IEnumerable<StreamResult<T>> Stream<T>(this IDocumentStore store, string prefix)
    {
        using (IDocumentSession session = store.OpenSession())
        using (IEnumerator<StreamResult<T>> enumerator = session.Advanced.Stream<T>(prefix))
        using (BulkInsertOperation bulkInsert = store.BulkInsert())
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