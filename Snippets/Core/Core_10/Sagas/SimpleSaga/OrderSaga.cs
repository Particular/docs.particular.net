namespace Core.Sagas.SimpleSaga;

using System.Threading.Tasks;
using NServiceBus;

#region simple-saga

public class OrderSaga :
    Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<StartOrder>(message => message.OrderId)
            .ToMessage<CompleteOrder>(message => message.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        // code to handle order completion
        MarkAsComplete();
        return Task.CompletedTask;
    }
}

#endregion