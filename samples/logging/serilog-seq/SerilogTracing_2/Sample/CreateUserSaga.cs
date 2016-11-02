using NServiceBus.Logging;
using NServiceBus.Saga;

public class CreateUserSaga :
    Saga<MySagaData>,
    IAmStartedByMessages<CreateUser>
{
    static ILog logger = LogManager.GetLogger(typeof(CreateUserSaga));

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
        mapper.ConfigureMapping<CreateUser>(message => message.UserName)
            .ToSaga(sagaData => sagaData.UserName);
    }

    public void Handle(CreateUser message)
    {
        Data.UserName = message.UserName;
        logger.Info("User created");
        var userCreated = new UserCreated
        {
            UserName = message.UserName
        };
        Bus.SendLocal(userCreated);
        MarkAsComplete();
    }
}