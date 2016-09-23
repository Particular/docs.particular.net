namespace Testing_4.HeaderManipulation
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
                .ExpectReply<ResponseMessage>(
                    check: message =>
                    {
                        return Test.Bus.GetMessageHeader(message, "MyHeaderKey") == "myHeaderValue";
                    })
                .OnMessage<RequestMessage>(
                    initializeMessage: message =>
                    {
                        message.String = "hello";
                    });
        }
    }

    class MyMessageHandler :
        IHandleMessages<RequestMessage>
    {

        public IBus Bus { get; set; }

        public void Handle(RequestMessage message)
        {
            var header = Bus.GetMessageHeader(message, "MyHeaderKey");

            var responseMessage = new ResponseMessage();
            Bus.SetMessageHeader(responseMessage, "MyHeaderKey", header);
            Bus.Reply(responseMessage);
        }
    }

    #endregion
}