using System;
using NServiceBus;
using NUnit.Framework;

namespace Samples_4_0
{
    [TestFixture]
    [Explicit]
    class UnobtrusiveMessageMode
    {
        [Test]
        public void Run()
        {
            Configure.With().MsmqTransport() //Configure.With().MsmqTransport() in V 3
                .DefaultBuilder()
                .FileShareDataBus(@"\\MyDataBusShare\")
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
                .DefiningMessagesAs(t => t.Namespace == "Messages")
                .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"))
                .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"))
                .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"))
                .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires")
             ? TimeSpan.FromSeconds(30)
             : TimeSpan.MaxValue);
        }
    }
}