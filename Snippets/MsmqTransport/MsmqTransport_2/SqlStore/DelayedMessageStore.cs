using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

public delegate Task<SqlConnection> CreateSqlConnection(CancellationToken cancellationToken = default);

#region dms-without-infrastructure
public class DelayedMessageStore : IDelayedMessageStore
#endregion
{
    private readonly IDelayedMessageStore instance;

    public DelayedMessageStore(string connectionString)
    {
        instance = new DelayedMessageStoreWithInfrastructure(connectionString);
    }

    public Task<DelayedMessage> FetchNextDueTimeout(DateTimeOffset at, CancellationToken cancellationToken = default)
    {
        return instance.FetchNextDueTimeout(at, cancellationToken);
    }

    public Task<bool> IncrementFailureCount(DelayedMessage entity, CancellationToken cancellationToken = default)
    {
        return instance.IncrementFailureCount(entity, cancellationToken);
    }

    public Task Initialize(string endpointName, TransportTransactionMode transactionMode, CancellationToken cancellationToken = default)
    {
        return instance.Initialize(endpointName, transactionMode, cancellationToken);
    }

    public Task<DateTimeOffset?> Next(CancellationToken cancellationToken = default)
    {
        return instance.Next(cancellationToken);
    }

    public Task<bool> Remove(DelayedMessage entity, CancellationToken cancellationToken = default)
    {
        return instance.Remove(entity, cancellationToken);
    }

    public Task Store(DelayedMessage entity, CancellationToken cancellationToken = default)
    {
        return instance.Store(entity, cancellationToken);
    }
}

#region dms-with-infrastructure
public class DelayedMessageStoreWithInfrastructure : IDelayedMessageStoreWithInfrastructure
#endregion
{
    private readonly string schema;
    private readonly CreateSqlConnection createSqlConnection;

    private string tableName;

    private string quotedFullName;
    private string insertCommand;
    private string removeCommand;
    private string bumpFailureCountCommand;
    private string nextCommand;
    private string fetchCommand;

    public DelayedMessageStoreWithInfrastructure(string connectionString, string schema = null, string tableName = null)
        : this(token => Task.FromResult(new SqlConnection(connectionString)), schema, tableName)
    {
    }

    public DelayedMessageStoreWithInfrastructure(CreateSqlConnection connectionFactory, string schema = null, string tableName = null)
    {
        createSqlConnection = connectionFactory;
        this.tableName = tableName;
        this.schema = schema ?? "dbo";
    }

    #region dms-store
    public async Task Store(DelayedMessage timeout, CancellationToken cancellationToken = default)
    {
        using (var cn = await createSqlConnection(cancellationToken))
        using (var cmd = new SqlCommand(insertCommand, cn))
        {
            cmd.Parameters.AddWithValue("@id", timeout.MessageId);
            cmd.Parameters.AddWithValue("@destination", timeout.Destination);
            cmd.Parameters.AddWithValue("@time", timeout.Time);
            cmd.Parameters.AddWithValue("@headers", timeout.Headers);
            cmd.Parameters.AddWithValue("@state", timeout.Body);
            await cn.OpenAsync(cancellationToken);
            _ = await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }
    #endregion


    #region dms-remove
    public async Task<bool> Remove(DelayedMessage timeout, CancellationToken cancellationToken = default)
    {
        using (var cn = await createSqlConnection(cancellationToken))
        using (var cmd = new SqlCommand(removeCommand, cn))
        {
            cmd.Parameters.AddWithValue("@id", timeout.MessageId);
            await cn.OpenAsync(cancellationToken);
            var affected = await cmd.ExecuteNonQueryAsync(cancellationToken);
            return affected == 1;
        }
    }
    #endregion

    #region dms-increment-failure-count
    public async Task<bool> IncrementFailureCount(DelayedMessage timeout, CancellationToken cancellationToken = default)
    {
        using (var cn = await createSqlConnection(cancellationToken))
        using (var cmd = new SqlCommand(bumpFailureCountCommand, cn))
        {
            cmd.Parameters.AddWithValue("@id", timeout.MessageId);
            await cn.OpenAsync(cancellationToken);
            var affected = await cmd.ExecuteNonQueryAsync(cancellationToken);
            return affected == 1;
        }
    }
    #endregion

    #region dms-initialise
    public Task Initialize(string endpointName, TransportTransactionMode transactionMode, CancellationToken cancellationToken = default)
    {
        if (tableName == null)
        {
            tableName = $"{endpointName}.timeouts";
        }

        quotedFullName = $"{SqlNameHelper.Quote(schema)}.{SqlNameHelper.Quote(tableName)}";

        insertCommand = string.Format(SqlConstants.SqlInsert, quotedFullName);
        removeCommand = string.Format(SqlConstants.SqlDelete, quotedFullName);
        bumpFailureCountCommand = string.Format(SqlConstants.SqlUpdate, quotedFullName);
        nextCommand = string.Format(SqlConstants.SqlGetNext, quotedFullName);
        fetchCommand = string.Format(SqlConstants.SqlFetch, quotedFullName);

        return Task.CompletedTask;
    }
    #endregion

    #region dms-setup-infrastructure
    public async Task SetupInfrastructure(CancellationToken cancellationToken = default)
    {
        await TimeoutTableCreator.CreateIfNecessary(createSqlConnection, quotedFullName, cancellationToken);
    }
    #endregion

    #region dms-next
    public async Task<DateTimeOffset?> Next(CancellationToken cancellationToken = default)
    {
        using (var cn = await createSqlConnection(cancellationToken))
        using (var cmd = new SqlCommand(nextCommand, cn))
        {
            await cn.OpenAsync(cancellationToken);
            var result = (DateTime?)await cmd.ExecuteScalarAsync(cancellationToken);
            return result.HasValue ? (DateTimeOffset?)new DateTimeOffset(result.Value, TimeSpan.Zero) : null;
        }
    }
    #endregion

    #region dms-fetch-next-duetimeout
    public async Task<DelayedMessage> FetchNextDueTimeout(DateTimeOffset at, CancellationToken cancellationToken = default)
    {
        DelayedMessage result = null;
        using (var cn = await createSqlConnection(cancellationToken))
        using (var cmd = new SqlCommand(fetchCommand, cn))
        {
            cmd.Parameters.AddWithValue("@time", at.UtcDateTime);

            await cn.OpenAsync(cancellationToken);
            using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken))
            {
                if (await reader.ReadAsync(cancellationToken))
                {
                    result = new DelayedMessage
                    {
                        MessageId = (string)reader[0],
                        Destination = (string)reader[1],
                        Time = (DateTime)reader[2],
                        Headers = (byte[])reader[3],
                        Body = (byte[])reader[4],
                        NumberOfRetries = (int)reader[5]
                    };
                }
            }
        }

        return result;
    }
    #endregion
}