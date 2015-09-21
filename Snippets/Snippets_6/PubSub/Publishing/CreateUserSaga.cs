namespace Snippets6.PubSub.Publishing
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    #region publishFromSaga
    public class CreateUserSaga : Saga<CreateUserSaga.SagaData>,
        IHandleMessages<CreateUserCommand>
    {
        IBus bus;

        public CreateUserSaga(IBus bus)
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
#endregion
        public class SagaData : IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
        }
    }
}