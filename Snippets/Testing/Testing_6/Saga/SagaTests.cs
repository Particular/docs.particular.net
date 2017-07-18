using System;
using System.Threading.Tasks;
using NServiceBus.Testing;
using NUnit.Framework;

[TestFixture]
public class SagaTests
{
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

        await saga.Handle(discountOrder, context)
            .ConfigureAwait(false);

        var processMessage = (ProcessOrder)context.SentMessages[0].Message;
        Assert.AreEqual(900, processMessage.TotalAmount);
    }
    #endregion
}