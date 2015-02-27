using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace UnitTesting.HeaderManipulation
{

    #region TestingHeaderManipulation

    public class MyTest
    {
        [Test]
        public void Run()
        {
            Test.Initialize();

            Test.Handler<MyMessageHandler>()
                .SetIncomingHeader("Test", "abc")
                .ExpectReply<ResponseMessage>(m => Test.Bus.GetMessageHeader(m, "MyHeaderKey") == "myHeaderValue")
                .OnMessage<RequestMessage>(m => m.String = "hello");
        }
    }

    class MyMessageHandler : IHandleMessages<RequestMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(RequestMessage message)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            Bus.SetMessageHeader(responseMessage, "MyHeaderKey", "myHeaderValue");
            Bus.Reply(responseMessage);
        }
    }

    #endregion

    class RequestMessage : IMessage
    {
        public string String { get; set; }
    }

    class ResponseMessage : IMessage
    {
        public string String { get; set; }
    }
}