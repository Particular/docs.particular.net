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

[TestFixture]
[Explicit]
public class NativeSendTests
{
    string endpointName = "NativeSendTests";
    static string errorQueueName = "NativeSendTestsError";
    static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";
    static string schema = "dbo";

    [SetUp]
    [TearDown]
    public async Task Setup()
    {
        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync()
                .ConfigureAwait(false);
            await QueueDeletion.DeleteQueuesForEndpoint(connection, schema, endpointName)
                .ConfigureAwait(false);
            await QueueDeletion.DeleteQueuesForEndpoint(connection, schema, errorQueueName)
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
                {"NServiceBus.EnclosedMessageTypes", "NativeSendTests+MessageToSend"}
            };

            await NativeSend.SendMessage(connectionString, endpointName, message, headers)
                .ConfigureAwait(false);
            state.ResetEvent.WaitOne();
        }
    }

    [Test]
    public void SendPowershell()
    {
        var state = new State();
        using (var bus = StartBus(state))
        {
            var message = @"{ Property: 'Value' }";

            var headers = new Dictionary<string, string>
            {
                {"NServiceBus.EnclosedMessageTypes", "NativeSendTests+MessageToSend"}
            };

            var scriptPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Operations\NativeSend.ps1");
            var script = File.ReadAllText(scriptPath);

            using (var powershell = PowerShell.Create())
            {
                powershell.AddScript(script, false);
                powershell.Invoke();
                powershell.Commands.Clear();
                powershell.AddCommand("SendMessage")
                    .AddParameter(null, connectionString)
                    .AddParameter(null, endpointName)
                    .AddParameter(null, message)
                    .AddParameter(null, headers);
                var results = powershell.Invoke();
            }

            state.ResetEvent.WaitOne();
        }
    }

    IBus StartBus(State state)
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.RegisterComponents(c => c.ConfigureComponent(x => state, DependencyLifecycle.SingleInstance));
        busConfiguration.EndpointName(endpointName);
        busConfiguration.UseSerialization<JsonSerializer>();
        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);
        var sqlTypes = typeof(SqlServerTransport).Assembly.GetTypes();
        busConfiguration.TypesToScan(TypeScanner.NestedTypes<NativeSendTests>(sqlTypes));
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.DisableFeature<SecondLevelRetries>();

        return Bus.Create(busConfiguration).Start();
    }

    class MessageHandler : IHandleMessages<MessageToSend>
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

    class MessageToSend : IMessage
    {
        public string Property { get; set; }
    }

    class ConfigTransport : IProvideConfiguration<TransportConfig>
    {
        public TransportConfig GetConfiguration()
        {
            return new TransportConfig
            {
                MaxRetries = 0
            };
        }
    }

    class ConfigErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
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