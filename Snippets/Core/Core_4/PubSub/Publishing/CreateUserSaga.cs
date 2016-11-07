namespace Core4.PubSub.Publishing
{
    using NServiceBus;
    using NServiceBus.Saga;

    #region publishFromSaga
    public class CreateUserSaga :
        Saga<CreateUserSaga.SagaData>,
        IHandleMessages<CreateUserCommand>
    {
        IBus bus;

        public CreateUserSaga(IBus bus)
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
#endregion
        public class SagaData :
            ContainSagaData
        {
        }

    }
}