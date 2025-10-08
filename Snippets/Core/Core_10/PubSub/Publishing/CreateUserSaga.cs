namespace Core.PubSub.Publishing;

using System.Threading.Tasks;
using NServiceBus;

#region publishFromSaga

public class CreateUserSaga : Saga<CreateUserSaga.SagaData>, IHandleMessages<CreateUserCommand>
{
    public async Task Handle(CreateUserCommand message, IMessageHandlerContext context)
    {
        var userCreatedEvent = new UserCreatedEvent
        {
            Name = message.Name
        };

        await context.Publish(userCreatedEvent);
    }

    #endregion

    public class SagaData : ContainSagaData
    {
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
    }
}