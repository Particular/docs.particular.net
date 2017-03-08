using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Extensibility;
using NServiceBus.Persistence;


#region SqlPersistenceSagaFinder
// HACK: this is a workaround for a missing API in the sql persister.
// Should be fixed in version in a future version
public static class SqlPersistenceSagaFinder
{

    public static async Task<TSagaData> FindBy<TSagaData>(this SynchronizedStorageSession storageSession, ReadOnlyContextBag context, Action<DbCommand> addParameters, string endpointName, string whereClause) 
        where TSagaData : IContainSagaData
    {
        var tableName = $"{endpointName.Replace('.','_')}_{typeof(TSagaData).Name.RemoveDataFromEnd()}";

        var sqlSession = storageSession.SqlPersistenceSession();
        using (var command = sqlSession.Connection.CreateCommand())
        {
            command.Transaction = sqlSession.Transaction;
            addParameters(command);
            command.CommandText = $@"
select
    Id,
    SagaTypeVersion,
    Concurrency,
    Metadata,
    Data
from {tableName}
where {whereClause}";
            return await GetSagaData<TSagaData>(command, context).ConfigureAwait(false);
        }
    }

    static string RemoveDataFromEnd(this string value)
    {
        if (value.EndsWith("Data"))
        {
            return value.Substring(0, value.Length - "Data".Length);
        }
        return value;
    }

    static void SetConcurrency(this ReadOnlyContextBag context, int concurrencyVersion)
    {
        ((ContextBag)context).Set("NServiceBus.Persistence.Sql.Concurrency", concurrencyVersion);
    }

    static async Task<TSagaData> GetSagaData<TSagaData>(DbCommand command, ReadOnlyContextBag context)
        where TSagaData : IContainSagaData
    {
        // to avoid loading into memory SequentialAccess is required which means each fields needs to be accessed
        using (var dataReader = await command.ExecuteReaderAsync(
                behavior: CommandBehavior.SingleRow | CommandBehavior.SequentialAccess)
            .ConfigureAwait(false))
        {
            if (!await dataReader.ReadAsync()
                .ConfigureAwait(false))
            {
                return default(TSagaData);
            }

            var id = await dataReader.GetGuidAsync(0)
                .ConfigureAwait(false);
            var sagaTypeVersionString = await dataReader.GetFieldValueAsync<string>(1)
                .ConfigureAwait(false);
            var sagaTypeVersion = Version.Parse(sagaTypeVersionString);
            var concurrency = await dataReader.GetFieldValueAsync<int>(2)
                .ConfigureAwait(false);
            string originator;
            string originalMessageId;
            using (var textReader = dataReader.GetTextReader(3))
            {
                var metadata = SqlPersistenceSerializer.Deserialize<Dictionary<string, string>>(textReader);
                metadata.TryGetValue("Originator", out originator);
                metadata.TryGetValue("OriginalMessageId", out originalMessageId);
            }
            TSagaData sagaData;
            using (var textReader = dataReader.GetTextReader(4))
            {
                sagaData = SqlPersistenceSerializer.Deserialize<TSagaData>(textReader);
            }
            sagaData.Id = id;
            sagaData.Originator = originator;
            sagaData.OriginalMessageId = originalMessageId;
            context.SetConcurrency(concurrency);
            return sagaData;
        }
    }

    static async Task<Guid> GetGuidAsync(this DbDataReader reader, int position)
    {
        var type = reader.GetFieldType(position);
        // MySql stores Guids as strings
        if (type == typeof(string))
        {
            return new Guid(await reader.GetFieldValueAsync<string>(position).ConfigureAwait(false));
        }
        return await reader.GetFieldValueAsync<Guid>(position).ConfigureAwait(false);
    }
}
#endregion