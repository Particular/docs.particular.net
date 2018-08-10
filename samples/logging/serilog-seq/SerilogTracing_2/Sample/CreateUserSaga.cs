using NServiceBus.Saga;
using Serilog;

public class CreateUserSaga :
    Saga<MySagaData>,
    IAmStartedByMessages<CreateUser>
{
    static ILogger log = Log.ForContext<CreateUserSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
    {
        mapper.ConfigureMapping<CreateUser>(message => message.UserName)
            .ToSaga(sagaData => sagaData.UserName);
    }

    public void Handle(CreateUser message)
    {
        Data.UserName = message.UserName;
        log.Information($"User Created {message.UserName}");
        var userCreated = new UserCreated
        {
            UserName = message.UserName
        };
        Bus.SendLocal(userCreated);
        MarkAsComplete();
    }
}