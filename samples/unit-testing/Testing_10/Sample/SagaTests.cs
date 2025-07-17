using System;
using System.Threading.Tasks;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class SagaTests
{
    [Test]
    public async Task ShouldProcessRegularOrder()
    {
        var saga = new DiscountPolicy
        {
            Data = new DiscountPolicyData()
        };
        var context = new TestableMessageHandlerContext();

        var regularOrder = new SubmitOrder
        {
            CustomerId = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            TotalAmount = 50
        };

        await saga.Handle(regularOrder, context);

        var processMessage = (ProcessOrder)context.SentMessages[0].Message;
        Assert.That(processMessage.TotalAmount, Is.EqualTo(50));
    }

    #region SagaTest
    [Test]
    public async Task ShouldProcessDiscountOrder()
    {
        var saga = new DiscountPolicy
        {
            Data = new DiscountPolicyData()
        };
        var context = new TestableMessageHandlerContext();

        var discountOrder = new SubmitOrder
        {
            CustomerId = Guid.NewGuid(),
            OrderId = Guid.NewGuid(),
            TotalAmount = 1000
        };

        await saga.Handle(discountOrder, context);

        var processMessage = (ProcessOrder)context.SentMessages[0].Message;
        Assert.That(processMessage.TotalAmount, Is.EqualTo(900));
    }
    #endregion
}