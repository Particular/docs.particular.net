namespace Core8.Headers
{
    using System.Threading.Tasks;
    using NServiceBus;

    #region header-incoming-saga
    public class ReadSaga :
        Saga<ReadSagaData>,
        IHandleMessages<MyMessage>
    {
        public Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            var headers = context.MessageHeaders;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
            return Task.CompletedTask;
        }
        #endregion
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ReadSagaData> mapper)
        {
        }
    }

    public class ReadSagaData :
        ContainSagaData
    {
    }
}
