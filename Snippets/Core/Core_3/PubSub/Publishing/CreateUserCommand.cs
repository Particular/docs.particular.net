namespace Core3.PubSub
{
    using NServiceBus;

    public class CreateUserCommand:ICommand
    {
        public string Name { get; set; }
    }
}