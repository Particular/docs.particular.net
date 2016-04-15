namespace Core4.Headers
{
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.Saga;

    #region header-incoming-saga
    public class ReadSaga : Saga<ReadSagaData>,
        IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            IDictionary<string, string> headers = Bus.CurrentMessageContext.Headers;
            string nsbVersion = headers[Headers.NServiceBusVersion];
            string customHeader = headers["MyCustomHeader"];
        }
    }

    #endregion
    public class ReadSagaData : ContainSagaData
    {
    }

}
