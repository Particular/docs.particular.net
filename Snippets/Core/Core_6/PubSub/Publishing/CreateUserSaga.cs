namespace Core6.PubSub.Publishing
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    #region publishFromSaga

    public class CreateUserSaga :
        Saga<CreateUserSaga.SagaData>,
        IHandleMessages<CreateUserCommand>
    {
        public Task Handle(CreateUserCommand message, IMessageHandlerContext context)
        {
            return context.Publish<UserCreatedEvent>(e =>
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

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
        }
    }
}