namespace NServiceBus.Snippets
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Factory method for creating SQL Server connections.
    /// </summary>
    public delegate Task<SqlConnection> CreateSqlConnection(CancellationToken cancellationToken = default);

    #region dms-without-infrastructure
    public class DelayedMessageStore : IDelayedMessageStore
    #endregion
    {
        public IDelayedMessageStore _instance;
        public DelayedMessageStore(string connectionString)
        {
            _instance = new DelayedMessageStoreWithInfrastructure(connectionString);
        }

        public Task<DelayedMessage> FetchNextDueTimeout(DateTimeOffset at, CancellationToken cancellationToken = default)
        {
            return _instance.FetchNextDueTimeout(at, cancellationToken);
        }

        public Task<bool> IncrementFailureCount(DelayedMessage entity, CancellationToken cancellationToken = default)
        {
            return _instance.IncrementFailureCount(entity, cancellationToken);
        }

        public Task Initialize(string endpointName, TransportTransactionMode transactionMode, CancellationToken cancellationToken = default)
        {
            return _instance.Initialize(endpointName, transactionMode, cancellationToken);
        }

        public Task<DateTimeOffset?> Next(CancellationToken cancellationToken = default)
        {
            return _instance.Next(cancellationToken);
        }

        public Task<bool> Remove(DelayedMessage entity, CancellationToken cancellationToken = default)
        {
            return _instance.Remove(entity, cancellationToken);
        }

        public Task Store(DelayedMessage entity, CancellationToken cancellationToken = default)
        {
            return _instance.Store(entity, cancellationToken);
        }
    }

    /// <summary>
    /// Implementation of the delayed message store based on the SQL Server.
    /// </summary>
    #region dms-with-infrastructure
    public class DelayedMessageStoreWithInfrastructure : IDelayedMessageStoreWithInfrastructure
    #endregion
    {
        string schema;
        string tableName;
        CreateSqlConnection createSqlConnection;

        string quotedFullName;
        string insertCommand;
        string removeCommand;
        string bumpFailureCountCommand;
        string nextCommand;
        string fetchCommand;

        /// <summary>
        /// Creates a new instance of the SQL Server delayed message store.
        /// </summary>
        /// <param name="connectionString">Connection string to the SQL Server database.</param>
        /// <param name="schema">(optional) schema to use. Defaults to dbo</param>
        /// <param name="tableName">(optional) name of the table where delayed messages are stored. Defaults to name of the endpoint with .Delayed suffix.</param>
        public DelayedMessageStoreWithInfrastructure(string connectionString, string schema = null, string tableName = null)
            : this(token => Task.FromResult(new SqlConnection(connectionString)), schema, tableName)
        {
        }

        /// <summary>
        /// Creates a new instance of the SQL Server delayed message store.
        /// </summary>
        /// <param name="connectionFactory">Factory for database connections.</param>
        /// <param name="schema">(optional) schema to use. Defaults to dbo</param>
        /// <param name="tableName">(optional) name of the table where delayed messages are stored. Defaults to name of the endpoint with .Delayed suffix.</param>
        public DelayedMessageStoreWithInfrastructure(CreateSqlConnection connectionFactory, string schema = null, string tableName = null)
        {
            createSqlConnection = connectionFactory;
            this.tableName = tableName;
            this.schema = schema ?? "dbo";
        }

        /// <inheritdoc />
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


        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        #region dms-setup-infrastructure
        public async Task SetupInfrastructure(CancellationToken cancellationToken = default)
        {
            var creator = new TimeoutTableCreator(createSqlConnection, quotedFullName);
            await creator.CreateIfNecessary(cancellationToken);
        }
        #endregion

        /// <inheritdoc />
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

        /// <inheritdoc />
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
}