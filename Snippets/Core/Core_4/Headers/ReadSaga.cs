namespace Core4.Headers
{
    using NServiceBus;
    using NServiceBus.Saga;

    #region header-incoming-saga
    public class ReadSaga :
        Saga<ReadSagaData>,
        IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            var headers = Bus.CurrentMessageContext.Headers;
            var nsbVersion = headers[Headers.NServiceBusVersion];
            var customHeader = headers["MyCustomHeader"];
        }
    }

    #endregion
    public class ReadSagaData :
        ContainSagaData
    {
    }

}
