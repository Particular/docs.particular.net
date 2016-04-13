namespace Snippets4.UnitTesting.ServiceLayer
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
                .ExpectReply<ResponseMessage>(m => m.String == "hello")
                .OnMessage<RequestMessage>(m => m.String = "hello");
        }
    }

    public class MyHandler :
        IHandleMessages<RequestMessage>
    {
        public IBus Bus { get; set; }

        public void Handle(RequestMessage message)
        {
            ResponseMessage reply = new ResponseMessage
            {
                String = message.String
            };
            Bus.Reply(reply);
        }
    }

    #endregion
}