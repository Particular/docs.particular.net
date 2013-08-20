using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace Samples_4_0
{
    [TestFixture]
    class TestHeaderManipulation
    {
        // start code HeaderManipulation
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
                Bus.Reply(new ResponseMessage());
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
        // end code HeaderManipulation
    }
}