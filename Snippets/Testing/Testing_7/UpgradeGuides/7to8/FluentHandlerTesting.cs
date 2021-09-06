namespace Testing_7.UpgradeGuides._7to8
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    [TestFixture]
    class FluentHandlerTesting
    {
        #region 7to8-testhandler
        [Test]
        public async Task TestHandlerFluent()
        {
            await Test.Handler<RequestMessageHandler>() // Arrange
                .ExpectReply<ResponseMessage>( // Assert
                    check: message =>
                    {
                        return message.String == "hello";
                    })
                .OnMessageAsync<RequestMessage>( // Act 
                    initializeMessage: message =>
                    {
                        message.String = "hello";
                    });
        }
        #endregion
    }

    class RequestMessageHandler : IHandleMessages<RequestMessage>
    {
        public Task Handle(RequestMessage message, IMessageHandlerContext context)
        {
            return context.Reply(new ResponseMessage
            {
                String = message.String
            });
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
