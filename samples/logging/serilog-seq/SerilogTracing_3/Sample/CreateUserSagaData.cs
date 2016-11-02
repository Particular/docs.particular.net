using NServiceBus;

public class CreateUserSagaData :
    ContainSagaData
{
    public string UserName { get; set; }
}