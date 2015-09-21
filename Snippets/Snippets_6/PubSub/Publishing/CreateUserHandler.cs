namespace Snippets6.PubSub.Publishing
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region publishFromHandler
    public class CreateUserHandler : IHandleMessages<CreateUserCommand>
    {
        IBus bus;

        public CreateUserHandler(IBus bus)
        {
            this.bus = bus;
        }

        public async Task Handle(CreateUserCommand message)
        {
            await bus.PublishAsync<UserCreatedEvent>(e =>
            {
                e.Name = message.Name;
            });
        }
    }
    #endregion
}
