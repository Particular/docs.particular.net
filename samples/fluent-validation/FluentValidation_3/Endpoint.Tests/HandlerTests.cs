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

    #region ThrowsForIncoming
    [Fact]
    public Task ThrowsForIncoming()
    {
        var message = new MyMessage();
        var handlerContext = ValidatingContext.Build(message);
        var handler = new OtherHandler();
        return handlerContext.Run(handler);
    }
    #endregion

    #region ThrowsForOutgoing
    [Fact]
    public Task ThrowsForOutgoing()
    {
        var message = new MyMessage
        {
            Content = "TheContent"
        };
        var handlerContext = ValidatingContext.Build(message);
        var handler = new OtherHandler();
        return handlerContext.Run(handler);
    }
    #endregion
}