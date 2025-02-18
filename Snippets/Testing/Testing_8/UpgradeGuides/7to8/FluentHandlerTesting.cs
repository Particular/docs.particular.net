namespace Testing_8.UpgradeGuides._7to8
{
    using System.Linq;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using NUnit.Framework;

    [Explicit]
    #region 7to8-testhandler
    [TestFixture]
    class ArrangeActAssertHandlerTesting
    {
        [Test]
        public async Task TestHandler()
        {
            // Arrange
            var handler = new RequestMessageHandler();
            
            // Act
            var requestMessage = new RequestMessage {String = "hello"};
            var messageContext = new TestableMessageHandlerContext();
            await handler.Handle(requestMessage, messageContext);

            // Assert
            Assert.That(messageContext.RepliedMessages.Any(x =>
                x.Message<ResponseMessage>()?.String == "hello"),
                Is.True,
                "Should send a ResponseMessage reply that echoes the provided string");
        }
    }
    #endregion

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
