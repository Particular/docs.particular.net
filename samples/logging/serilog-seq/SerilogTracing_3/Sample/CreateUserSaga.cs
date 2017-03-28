using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class CreateUserSaga :
    Saga<CreateUserSagaData>,
    IAmStartedByMessages<CreateUser>
{
    static ILog log = LogManager.GetLogger(typeof(CreateUserSaga));

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<CreateUserSagaData> mapper)
    {
        mapper.ConfigureMapping<CreateUser>(message => message.UserName)
            .ToSaga(sagaData => sagaData.UserName);
    }

    public Task Handle(CreateUser message, IMessageHandlerContext context)
    {
        Data.UserName = message.UserName;
        log.InfoFormat("User Created {@Message}", message);
        var userCreated = new UserCreated
        {
            UserName = message.UserName
        };
        MarkAsComplete();
        return context.SendLocal(userCreated);
    }
}