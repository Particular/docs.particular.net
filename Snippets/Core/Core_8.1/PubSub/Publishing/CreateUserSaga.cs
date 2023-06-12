namespace Core8.PubSub.Publishing
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region publishFromSaga

    public class CreateUserSaga :
        Saga<CreateUserSaga.SagaData>,
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

        #endregion

        public class SagaData :
            ContainSagaData
        {
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
        }
    }
}