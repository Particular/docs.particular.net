namespace Core.PubSub.Publishing;

using NServiceBus;

public class CreateUserCommand :
    ICommand
{
    public string Name { get; set; }
}