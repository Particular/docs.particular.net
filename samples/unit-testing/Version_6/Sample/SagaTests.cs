using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Testing;
using NUnit.Framework;

namespace Sample
{
    [TestFixture]
    public class SagaTests
    {
        [Test]
        public async Task ShouldProcessRegularOrder()
        {
            var saga = new DiscountPolicy { Data = new DiscountPolicyData() };
            var context = new TestableMessageHandlerContext();

            var regularOrder = new SubmitOrder
            {
                CustomerId = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                TotalAmount = 50
            };

            await saga.Handle(regularOrder, context);

            var processMessage = (ProcessOrder)context.SentMessages[0].Message;
            Assert.AreEqual(50, processMessage.TotalAmount);
        }

        #region SagaTest
        [Test]
        public async Task ShouldProcessDiscountOrder()
        {
            var saga = new DiscountPolicy { Data = new DiscountPolicyData() };
            var context = new TestableMessageHandlerContext();

            var discountOrder = new SubmitOrder
            {
                CustomerId = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                TotalAmount = 1000
            };

            await saga.Handle(discountOrder, context);

            var processMessage = (ProcessOrder)context.SentMessages[0].Message;
            Assert.AreEqual(900, processMessage.TotalAmount);
        }
        #endregion
    }

    #region SampleSaga
    public class DiscountPolicy : NServiceBus.Saga<DiscountPolicyData>,
        IAmStartedByMessages<SubmitOrder>
    {
        public async Task Handle(SubmitOrder message, IMessageHandlerContext context)
        {
            Data.CustomerId = message.CustomerId;
            Data.TotalAmount  += message.TotalAmount;

            if (Data.TotalAmount >= 1000)
            {
                await ProcessWithDiscount(message, context);
            }
            else
            {
                await ProcessOrder(message, context);
            }
        }

        private Task ProcessWithDiscount(SubmitOrder message, IMessageHandlerContext context)
        {
            return context.Send<ProcessOrder>(m =>
            {
                m.CustomerId = Data.CustomerId;
                m.OrderId = message.OrderId;
                // Gives 10% discount
                m.TotalAmount = message.TotalAmount * (decimal)0.9;
            });
        }

        private Task ProcessOrder(SubmitOrder message, IMessageHandlerContext context)
        {
            return context.Send<ProcessOrder>(m =>
            {
                m.CustomerId = Data.CustomerId;
                m.OrderId = message.OrderId;
                m.TotalAmount = message.TotalAmount;
            });
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<DiscountPolicyData> mapper)
        {
        }
    }
    #endregion

    public class ProcessOrder : IMessage
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class SubmitOrder : IMessage
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class DiscountPolicyData : IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}