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
        // arrange
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

        // act
        await saga.Handle(discountOrder, context)
            .ConfigureAwait(false);


        // assert
        var processMessage = (ProcessOrder)context.SentMessages[0].Message;
        Assert.That(processMessage.TotalAmount, Is.EqualTo(900));
        Assert.That(saga.Completed, Is.False);
    }
    #endregion
}