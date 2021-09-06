namespace Testing_8.UpgradeGuides._7to8
{
    using System.Linq;
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
            // Arrange
            var handler = new RequestMessageHandler();
            
            // Act
            var requestMessage = new RequestMessage {String = "hello"};
            var messageContext = new TestableMessageHandlerContext();
            await handler.Handle(requestMessage, messageContext);

            // Assert
            var reply = messageContext.RepliedMessages.SingleOrDefault()?.Message<ResponseMessage>();
            Assert.IsNotNull(reply);
            Assert.AreEqual("hello", reply.String);
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
