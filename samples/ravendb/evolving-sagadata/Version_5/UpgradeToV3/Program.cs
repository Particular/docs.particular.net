using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus.RavenDB.Persistence.SagaPersister;
using Raven.Client;
using Raven.Client.Document;
using Raven.Abstractions.Commands;

class Program
{
    static void Main()
    {
        using (DocumentStore store = new DocumentStore
        {
            Url = "http://localhost:8083",
            DefaultDatabase = "RavenSampleData",
        })
        {
            store.Initialize();
            ConvertSagas<OrderSagaData, OrdersSagaData>(store, "OrderId", old =>
            new OrdersSagaData
            {
                Id = old.Id,
                OriginalMessageId = old.OriginalMessageId,
                Originator = old.Originator,
                NumberOfItems = old.ItemCount
            });
        }
    }

    static void ConvertSagas<TOldSaga,TNewSaga>(DocumentStore store, string uniqueProperty, Func<TOldSaga,TNewSaga> convert)
    {
        List<string> keysToDelete = new List<string>();
        using (IDocumentSession session = store.OpenSession())
        {
            string oldSagaPrefix = GetSagaTypePrefix<TOldSaga>(store);
            string newSagaPrefix = GetSagaTypePrefix<TNewSaga>(store);
            using (var enumerator = session.Advanced.Stream<TOldSaga>(oldSagaPrefix))
            using (var bulkInsert = store.BulkInsert())
            {
                while (enumerator.MoveNext())
                {
                    TNewSaga newSaga = convert(enumerator.Current.Document);
                    keysToDelete.Add(enumerator.Current.Key);
                    bulkInsert.Store(newSaga);
                }
            }

            string oldIdentityPrefix = GetIdentityPrefix<TOldSaga>(uniqueProperty);
            string newIdentityPrefix = GetIdentityPrefix<TNewSaga>(uniqueProperty);
            using (var enumerator = session.Advanced.Stream<SagaUniqueIdentity>(oldIdentityPrefix))
            using (var bulkInsert = store.BulkInsert())
            {
                while (enumerator.MoveNext())
                {
                    SagaUniqueIdentity oldIdentity = enumerator.Current.Document;

                    keysToDelete.Add(enumerator.Current.Key);
                    SagaUniqueIdentity newIdentity = new SagaUniqueIdentity
                    {
                        Id = oldIdentity.Id.Replace(oldIdentityPrefix, newIdentityPrefix),
                        SagaDocId = oldIdentity.SagaDocId.Replace(oldSagaPrefix, newSagaPrefix),
                        UniqueValue = oldIdentity.UniqueValue,
                        SagaId = oldIdentity.SagaId
                    };
                    bulkInsert.Store(newIdentity);
                }
            }
        }
        DeleteCommandData[] deleteCommands = keysToDelete
            .Select(key => new DeleteCommandData
            {
                Key = key
            })
            .ToArray();
        store.DatabaseCommands.Batch(deleteCommands);
    }

    static string GetSagaTypePrefix<TSaga>(DocumentStore store)
    {
        return store.Conventions.FindTypeTagName(typeof(TSaga)) + "/";
    }

    static string GetIdentityPrefix<TSaga>(string uniqueProperty)
    {
        return string.Format("{0}/{1}/", typeof(TSaga).FullName.Replace('+', '-'), uniqueProperty);
    }
}