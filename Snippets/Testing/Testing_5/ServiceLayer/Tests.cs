namespace Testing_5.ServiceLayer
{
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]

    #region TestingServiceLayer

    [TestFixture]
    public class Tests
    {
        [Test]
        public void Run()
        {
            Test.Initialize();

            Test.Handler<MyHandler>()
                .ExpectReply<ResponseMessage>(
                    check: message =>
                    {
                        return message.String == "hello";
                    })
                .OnMessage<RequestMessage>(
                    initializeMessage: message =>
                    {
                        message.String = "hello";
                    });
        }
    }

    public class MyHandler :
        IHandleMessages<RequestMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(RequestMessage message)
        {
            var reply = new ResponseMessage
            {
                String = message.String
            };
            Bus.Reply(reply);
        }
    }

    #endregion
}