using System.Linq;
using System.Threading.Tasks;
using NServiceBus.Testing;
using Xunit;

public class HandlerTests
{
    static HandlerTests()
    {
        #region AddValidators
        TestContextValidator.AddValidatorsFromAssemblyContaining<MyMessage>();
        #endregion
    }

    #region IncorrectlyPasses
    [Fact]
    public async Task IncorrectlyPasses()
    {
        var message = new MyMessage();
        var context = new TestableMessageHandlerContext();
        var handler = new OtherHandler();
        await handler.Handle(message, context);
        Assert.IsType<OtherMessage>(context.SentMessages.Single().Message);
    }
    #endregion

    #region ThrowsForIncoming
    [Fact]
    public async Task ThrowsForIncoming()
    {
        var message = new MyMessage();
        var context = ValidatingContext.Build(message);
        var handler = new OtherHandler();
        await context.Run(handler);
        Assert.IsType<OtherMessage>(context.SentMessages.Single().Message);
    }
    #endregion

    #region ThrowsForOutgoing
    [Fact]
    public async Task ThrowsForOutgoing()
    {
        var message = new MyMessage
        {
            Content = "TheContent"
        };
        var context = ValidatingContext.Build(message);
        var handler = new OtherHandler();
        await context.Run(handler);
        Assert.IsType<OtherMessage>(context.SentMessages.Single().Message);
    }
    #endregion
}