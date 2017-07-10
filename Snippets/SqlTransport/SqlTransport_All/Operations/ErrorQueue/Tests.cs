using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Common;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;
using NServiceBus.Logging;
using NUnit.Framework;
using SqlServer_All.Operations.QueueDeletion;

namespace SqlServer_All.Operations.ErrorQueue
{
    [TestFixture]
    [Explicit]
    public class Tests
    {
        static Tests()
        {
            LogManager.Use<DefaultFactory>()
                .Level(LogLevel.Error);
        }

        string endpointName = "ReturnToSourceQueueTests";
        static string errorQueueName = "ReturnToSourceQueueTestsError";
        static string connectionString = @"Data Source=.\SqlExpress;Database=samples;Integrated Security=True";
        static string schema = "dbo";

        [SetUp]
        [TearDown]
        public async Task Setup()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync()
                    .ConfigureAwait(false);
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(connection, schema, endpointName)
                    .ConfigureAwait(false);
                await DeleteEndpointQueues.DeleteQueuesForEndpoint(connection, schema, errorQueueName)
                    .ConfigureAwait(false);
            }
        }

        [Test]
        public async Task ReturnMessageToSourceQueue()
        {
            var state = new State();
            using (var bus = StartBus(state))
            {
                bus.SendLocal(new MessageToSend());
                var msmqMessageId = await GetMsmqMessageId()
                    .ConfigureAwait(false);

                state.ShouldHandlerThrow = false;

                await ErrorQueue.ReturnMessageToSourceQueue(
                        errorQueueConnection: connectionString,
                        errorQueueName: errorQueueName,
                        retryConnectionString: connectionString,
                        retryQueueName: endpointName,
                        messageId: msmqMessageId)
                    .ConfigureAwait(false);

                state.ResetEvent.WaitOne();
            }
        }

        IBus StartBus(State state)
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance);
                });
            busConfiguration.EndpointName(endpointName);
            var sqlTransportTypes = typeof(SqlServerTransport)
                .Assembly
                .GetTypes();
            busConfiguration.TypesToScan(TypeScanner.NestedTypes<Tests>(sqlTransportTypes));
            busConfiguration.EnableInstallers();
            var transport = busConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(connectionString);
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.DisableFeature<SecondLevelRetries>();
            return Bus.Create(busConfiguration).Start();
        }

        class State
        {
            public ManualResetEvent ResetEvent = new ManualResetEvent(false);
            public bool ShouldHandlerThrow = true;
        }

        async Task<Guid> GetMsmqMessageId()
        {
            var sql = $"SELECT Id FROM [{errorQueueName}]";
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync()
                    .ConfigureAwait(false);
                using (var command = new SqlCommand(sql, connection))
                {
                    while (true)
                    {
                        using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow)
                            .ConfigureAwait(false))
                        {
                            if (await reader.ReadAsync()
                                .ConfigureAwait(false))
                            {
                                return reader.GetGuid(0);
                            }
                            await Task.Delay(100)
                                .ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
        {
            State state;

            public MessageHandler(State state)
            {
                this.state = state;
            }

            public void Handle(MessageToSend message)
            {
                if (state.ShouldHandlerThrow)
                {
                    throw new Exception("The exception message from the handler.");
                }
                state.ResetEvent.Set();
            }
        }

        class ConfigTransport :
            IProvideConfiguration<TransportConfig>
        {
            public TransportConfig GetConfiguration()
            {
                return new TransportConfig
                {
                    MaxRetries = 0
                };
            }
        }

        class ConfigErrorQueue :
            IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
        {
            public MessageForwardingInCaseOfFaultConfig GetConfiguration()
            {
                return new MessageForwardingInCaseOfFaultConfig
                {
                    ErrorQueue = errorQueueName
                };
            }
        }

        class MessageToSend :
            IMessage
        {
        }

    }
}