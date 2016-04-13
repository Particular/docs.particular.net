namespace Snippets5.UnitTesting.HeaderManipulation
{
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    #region TestingHeaderManipulation

    [TestFixture]
    public class Tests
    {
        [Test]
        public void Run()
        {
            Test.Initialize();

            Test.Handler<MyMessageHandler>()
                .SetIncomingHeader("MyHeaderKey", "myHeaderValue")
                .ExpectReply<ResponseMessage>(m => Test.Bus.GetMessageHeader(m, "MyHeaderKey") == "myHeaderValue")
                .OnMessage<RequestMessage>(m => m.String = "hello");
        }
    }

    class MyMessageHandler : IHandleMessages<RequestMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(RequestMessage message)
        {
            string header = Bus.GetMessageHeader(message, "MyHeaderKey");

            ResponseMessage responseMessage = new ResponseMessage();
            Bus.SetMessageHeader(responseMessage, "MyHeaderKey", header);
            Bus.Reply(responseMessage);
        }
    }

    #endregion
}
