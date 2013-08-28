using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace Samples_4_0
{
    [TestFixture]
    public class TestHandler
    {
        // start code TestHandler
        [Test]
        public void Run()
        {
            Test.Initialize();

            Test.Handler<MyHandler>()
                .ExpectReply<ResponseMessage>(m => m.String == "hello")
                .OnMessage<RequestMessage>(m => m.String = "hello");
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

        public class ResponseMessage : IMessage
        {
            public string String { get; set; }
        }

        public class RequestMessage : IMessage
        {
            public string String { get; set; }
        }

        // end code TestHandler 
    }


}