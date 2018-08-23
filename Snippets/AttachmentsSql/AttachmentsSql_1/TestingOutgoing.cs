using System.Threading.Tasks;
using System.IO;
using System.Linq;
using NServiceBus;
using NServiceBus.Testing;
using Xunit;

public class TestingOutgoing
{
    #region TestOutgoingHandler

    public class Handler :
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var options = new SendOptions();
            var attachments = options.Attachments();
            attachments.Add("theName", () => File.OpenRead("aFilePath"));
            return context.Send(new OtherMessage(), options);
        }
    }

    #endregion

    #region TestOutgoing

    [Fact]
    public async Task TestOutgoingAttachments()
    {
        //Arrange
        var context = new TestableMessageHandlerContext();
        var handler = new Handler();

        //Act
        await handler.Handle(new MyMessage(), context);

        // Assert
        var sentMessage = context.SentMessages.Single();
        var attachments = sentMessage.Options.Attachments();
        var names = attachments.Names;
        Assert.Single(names);
        Assert.Contains("theName", names);
        Assert.True(attachments.HasPendingAttachments);
    }

    #endregion
}