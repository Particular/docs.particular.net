namespace Snippets6.Headers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;

    #region header-incoming-saga
    public class ReadSaga : Saga<ReadSagaData>,
        IHandleMessages<MyMessage>
    {
        public async Task Handle(MyMessage message, IMessageHandlerContext context)
        {
            IReadOnlyDictionary<string, string> headers = context.MessageHeaders;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
        #endregion
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ReadSagaData> mapper)
        {
        }
    }

    public class ReadSagaData : ContainSagaData
    {
    }
}
