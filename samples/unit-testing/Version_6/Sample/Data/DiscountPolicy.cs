using System.Threading.Tasks;
using NServiceBus;

#region SampleSaga
public class DiscountPolicy : Saga<DiscountPolicyData>,
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

    Task ProcessWithDiscount(SubmitOrder message, IMessageHandlerContext context)
    {
        return context.Send(new ProcessOrder
        {
            CustomerId = Data.CustomerId,
            OrderId = message.OrderId,
            TotalAmount = message.TotalAmount * (decimal)0.9
        });
    }

    Task ProcessOrder(SubmitOrder message, IMessageHandlerContext context)
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