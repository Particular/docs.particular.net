using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
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
        static string connectionString = @"Data Source=.\SqlExpress;Database=Snippets.SqlTransport;Integrated Security=True";
        static string schema = "dbo";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            SqlHelper.EnsureDatabaseExists(connectionString);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DeleteEndpointQueues.DeleteQueuesForEndpoint(connection, schema, endpointName);
                DeleteEndpointQueues.DeleteQueuesForEndpoint(connection, schema, errorQueueName);
            }
        }

        [Test]
        public void ReturnMessageToSourceQueue()
        {
            var state = new State();
            using (var bus = StartBus(state))
            {
                bus.SendLocal(new MessageToSend());
                var msmqMessageId = GetMsmqMessageId();

                state.ShouldHandlerThrow = false;

                ErrorQueue.ReturnMessageToSourceQueue(
                        errorQueueConnection: connectionString,
                        errorQueueName: errorQueueName,
                        retryConnectionString: connectionString,
                        retryQueueName: endpointName,
                        messageId: msmqMessageId);

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

        Guid GetMsmqMessageId()
        {
            var sql = $"SELECT Id FROM [{errorQueueName}]";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sql, connection))
                {
                    while (true)
                    {
                        using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                return reader.GetGuid(0);
                            }
                            Thread.Sleep(100);
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