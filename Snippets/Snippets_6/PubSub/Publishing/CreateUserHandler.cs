namespace Snippets6.PubSub.Publishing
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region publishFromHandler
    public class CreateUserHandler : IHandleMessages<CreateUserCommand>
    {
        public async Task Handle(CreateUserCommand message, IMessageHandlerContext context)
        {
            await context.Publish<UserCreatedEvent>(e =>
            {
                e.Name = message.Name;
            });
        }
    }
    #endregion
}
