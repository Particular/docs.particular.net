using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using Common;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Features;
using NUnit.Framework;
using SqlServer_All.Operations.QueueDeletion;

namespace SqlServer_All.Operations.NativeSend
{
    [TestFixture]
    [Explicit]
    public class Tests
    {
        string endpointName = "NativeSendTests";
        static string errorQueueName = "NativeSendTestsError";
        static string connectionString = @"Data Source=.\SqlExpress;Database=Snippets.SqlTransport;Integrated Security=True";
        static string schema = "dbo";

        [SetUp]
        [TearDown]
        public async Task Setup()
        {
            await SqlHelper.EnsureDatabaseExists(connectionString).ConfigureAwait(false);
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
        public async Task Send()
        {
            var state = new State();
            using (var bus = StartBus(state))
            {
                var message = @"{ Property: 'Value' }";

                var headers = new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", typeof(MessageToSend).FullName}
                };

                await NativeSend.SendMessage(connectionString, endpointName, message, headers)
                    .ConfigureAwait(false);
                state.ResetEvent.WaitOne();
            }
        }

        [Test]
        public void SendPowerShell()
        {
            var state = new State();
            using (var bus = StartBus(state))
            {
                var message = @"{ Property: 'Value' }";

                var headers = new Dictionary<string, string>
                {
                    {"NServiceBus.EnclosedMessageTypes", typeof(MessageToSend).FullName}
                };

                var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Operations\NativeSend\NativeSend.ps1");
                var script = File.ReadAllText(scriptPath);

                using (var powershell = PowerShell.Create())
                {
                    powershell.AddScript(script, false);
                    powershell.Invoke();
                    powershell.Commands.Clear();
                    var command = powershell.AddCommand("SendMessage");
                    command.AddParameter(null, connectionString);
                    command.AddParameter(null, endpointName);
                    command.AddParameter(null, message);
                    command.AddParameter(null, headers);
                    var results = powershell.Invoke();
                }

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
            busConfiguration.UseSerialization<JsonSerializer>();
            var transport = busConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(connectionString);
            var sqlTypes = typeof(SqlServerTransport).Assembly.GetTypes();
            busConfiguration.TypesToScan(TypeScanner.NestedTypes<Tests>(sqlTypes));
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.DisableFeature<SecondLevelRetries>();

            return Bus.Create(busConfiguration).Start();
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
                Assert.AreEqual("Value", message.Property);
                state.ResetEvent.Set();
            }
        }

        class State
        {
            public ManualResetEvent ResetEvent = new ManualResetEvent(false);
        }

        class MessageToSend :
            IMessage
        {
            public string Property { get; set; }
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
    }
}