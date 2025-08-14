namespace Core.Sagas.FindByProperty;

using System.Threading.Tasks;
using NServiceBus;

public class MySaga :
    Saga<MySagaData>,
    IAmStartedByMessages<MyMessage>
{
    #region saga-find-by-property

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.SomeId)
            .ToMessage<MyMessage>(message => message.SomeId);
    }

    #endregion

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}