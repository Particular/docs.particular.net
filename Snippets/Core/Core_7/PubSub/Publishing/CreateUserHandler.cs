namespace Core7.PubSub.Publishing
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region publishFromHandler
    public class CreateUserHandler : IHandleMessages<CreateUserCommand>
    {
        public async Task Handle(CreateUserCommand message, IMessageHandlerContext context)
        {
            var userCreatedEvent = new UserCreatedEvent
            {
                Name = message.Name
            };

            await context.Publish(userCreatedEvent);
        }
    }
    #endregion
}
