namespace Core3.PubSub
{
    using NServiceBus;
    #region publishFromHandler
    public class CreateUserHandler :
        IHandleMessages<CreateUserCommand>
    {
        IBus bus;

        public CreateUserHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(CreateUserCommand message)
        {
            var userCreatedEvent = new UserCreatedEvent
            {
                Name = message.Name
            };
            bus.Publish(userCreatedEvent);
        }
    }
    #endregion
}
