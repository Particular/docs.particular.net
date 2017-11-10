namespace Testing_6.UpgradeGuides._6to7
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Testing;
    using Testing_6.Saga;

    class WhenHandling
    {
        public void Run()
        {
            #region 6to7-WhenHandling

            var message = new CompleteSagaMessage();

            Test.Saga<MySaga>()
                .WhenHandling<CompleteSagaMessage>(msg => { /* msg created for you */ })
                .AssertSagaCompletionIs(true);

            #endregion

        }

        class MySaga : NServiceBus.Saga<MySagaData>, 
            IAmStartedByMessages<CompleteSagaMessage>
        {
            public Task Handle(CompleteSagaMessage message, IMessageHandlerContext context)
            {
                return Task.CompletedTask;
            }

            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<Testing_6.Saga.MySagaData> mapper)
            {
            }

            class MySagaData : ContainSagaData
            {

            }
        }

        class CompleteSagaMessage
        {
        }
    }
}
