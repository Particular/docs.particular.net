namespace Core3.PubSub
{
    using NServiceBus;
    #region publishFromHandler
    public class CreateUserHandler : IHandleMessages<CreateUserCommand>
    {
        IBus bus;

        public CreateUserHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(CreateUserCommand message)
        {
            bus.Publish<UserCreatedEvent>(e =>
            {
                e.Name = message.Name;
            });
        }
    }
    #endregion
}
