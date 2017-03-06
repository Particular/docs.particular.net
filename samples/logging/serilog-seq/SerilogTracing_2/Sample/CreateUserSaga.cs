using NServiceBus.Logging;
using NServiceBus.Saga;

public class CreateUserSaga :
    Saga<MySagaData>,
    IAmStartedByMessages<CreateUser>
{
    static ILog log = LogManager.GetLogger(typeof(CreateUserSaga));

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
        mapper.ConfigureMapping<CreateUser>(message => message.UserName)
            .ToSaga(sagaData => sagaData.UserName);
    }

    public void Handle(CreateUser message)
    {
        Data.UserName = message.UserName;
        log.InfoFormat("User Created {@UserName}", message.UserName);
        var userCreated = new UserCreated
        {
            UserName = message.UserName
        };
        Bus.SendLocal(userCreated);
        MarkAsComplete();
    }
}