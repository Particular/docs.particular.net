using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.Testing;
using NUnit.Framework;

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

    [TestCase(typeof(MyRequest))]
    [TestCase(typeof(ProcessOrder))]
    [TestCase(typeof(SubmitOrder))]
    public async Task ShouldNotAddCustomHeaderToOtherMessageTypes(Type messageType)
    {
        var behavior = new CustomBehavior();
        var context = new TestableOutgoingLogicalMessageContext
        {
            Message = new OutgoingLogicalMessage(messageType, Activator.CreateInstance(messageType))
        };

        await behavior.Invoke(context, () => Task.CompletedTask);

        Assert.That(context.Headers.ContainsKey("custom-header"), Is.False);
    }
}