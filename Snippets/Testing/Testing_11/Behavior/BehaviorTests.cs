using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Testing;
using NUnit.Framework;

[Explicit]
[TestFixture]
public class BehaviorTests
{
    #region BehaviorTest
    [Test]
    public async Task ShouldAddCustomHeaderToMyResponse()
    {
        var behavior = new CustomBehavior();
        var context = new TestableOutgoingLogicalMessageContext
        {
            Message = new OutgoingLogicalMessage(typeof(MyResponse), new MyResponse())
        };

        await behavior.Invoke(context, () => Task.CompletedTask);

        Assert.That(context.Headers["custom-header"], Is.EqualTo("custom header value"));
    }
    #endregion
}