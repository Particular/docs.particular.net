namespace Core4.PubSub.Publishing
{
    using System;
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
            bus.Publish<UserCreatedEvent>(e =>
            {
                e.Name = message.Name;
            });
        }
#endregion
        public class SagaData :
            IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }
        }

    }
}