using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.RavenDB.Persistence.SagaPersister;
using Raven.Abstractions.Data;
using Raven.Client.Document;

#region renamer

class SagaRenamer
{
    public static void RenameSaga<TOldSaga, TNewSaga>(DocumentStore store, Func<TOldSaga, TNewSaga> convert)
        where TOldSaga : IContainSagaData
        where TNewSaga : IContainSagaData
    {
        List<string> keysToDelete = new List<string>();
        string oldSagaPrefix = store.GetDocumentPrefix<TOldSaga>();
        string newSagaPrefix = store.GetDocumentPrefix<TNewSaga>();
        using (BulkInsertOperation bulkInsert = store.BulkInsert())
        {
            foreach (StreamResult<TOldSaga> result in store.Stream<TOldSaga>(oldSagaPrefix))
            {
                TOldSaga oldSaga = result.Document;
                TNewSaga newSaga = convert(oldSaga);
                newSaga.Id = oldSaga.Id;
                newSaga.OriginalMessageId = oldSaga.OriginalMessageId;
                newSaga.Originator = oldSaga.Originator;
                keysToDelete.Add(result.Key);
                bulkInsert.Store(newSaga);
            }
        }
        store.BatchDelete(keysToDelete);
    }

    public static void RenameUniqueIdentities<TOldSaga, TNewSaga>(DocumentStore store, string uniqueProperty)
        where TOldSaga : IContainSagaData
        where TNewSaga : IContainSagaData
    {
        List<string> keysToDelete = new List<string>();
        string oldSagaPrefix = store.GetDocumentPrefix<TOldSaga>();
        string newSagaPrefix = store.GetDocumentPrefix<TNewSaga>();
        string oldIdentityPrefix = GetIdentityPrefix<TOldSaga>(uniqueProperty);
        string newIdentityPrefix = GetIdentityPrefix<TNewSaga>(uniqueProperty);
        using (BulkInsertOperation bulkInsert = store.BulkInsert())
        {
            foreach (StreamResult<SagaUniqueIdentity> result in store.Stream<SagaUniqueIdentity>(oldIdentityPrefix))
            {
                SagaUniqueIdentity oldIdentity = result.Document;
                keysToDelete.Add(result.Key);
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
        store.BatchDelete(keysToDelete);
    }

    static string GetIdentityPrefix<TSaga>(string uniqueProperty)
    {
        return string.Format("{0}/{1}/", typeof(TSaga).FullName.Replace('+', '-'), uniqueProperty);
    }
}

#endregion