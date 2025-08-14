namespace Core.Sagas.FindByHeader;

using System.Threading.Tasks;
using NServiceBus;

public class MySaga : Saga<MySagaData>, IAmStartedByMessages<MyMessage>
{
    #region saga-find-by-message-header

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
        mapper.MapSaga(saga => saga.SomeId)
            .ToMessageHeader<MyMessage>("HeaderName");
    }

    #endregion

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}