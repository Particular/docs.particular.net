namespace Core9.PubSub.Publishing
{
    using NServiceBus;

    public class CreateUserCommand :
        ICommand
    {
        public string Name { get; set; }
    }
}