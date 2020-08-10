namespace Core8.PubSub.Publishing
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region publishFromHandler
    public class CreateUserHandler :
        IHandleMessages<CreateUserCommand>
    {
        public Task Handle(CreateUserCommand message, IMessageHandlerContext context)
        {
            var userCreatedEvent = new UserCreatedEvent
            {
                Name = message.Name
            };
            return context.Publish(userCreatedEvent);
        }
    }
    #endregion
}
