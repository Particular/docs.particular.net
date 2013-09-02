using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace Snippets_3_3
{
    [TestFixture]
    class TestHeaderManipulation
    {
        [Test]
        public void Run()
        {
            Test.Initialize();

            Test.Handler<MyMessageHandler>()
                .SetIncomingHeader("Test", "abc")
                .OnMessage<RequestMessage>(m => m.String = "hello");

            Assert.AreEqual("myHeaderValue", Test.Bus.OutgoingHeaders["MyHeaderKey"]);
        }
        class MyMessageHandler : IHandleMessages<RequestMessage>
        {
            public IBus Bus { get; set; }
            public void Handle(RequestMessage message)
            {
                Bus.OutgoingHeaders["MyHeaderKey"] = "myHeaderValue";
            }
        }

        class RequestMessage : IMessage
        {
            public string String { get; set; }
        }

        class ResponseMessage : IMessage
        {
            public string String { get; set; }
        }
    }
}