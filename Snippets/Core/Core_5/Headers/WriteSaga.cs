namespace Core5.Headers
{
    using NServiceBus;
    using NServiceBus.Saga;

    #region header-outgoing-saga

    public class WriteSaga :
        Saga<WriteSagaData>,
        IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            var someOtherMessage = new SomeOtherMessage();
            Bus.SetMessageHeader(
                msg: someOtherMessage,
                key: "MyCustomHeader",
                value: "My custom value");
            Bus.Send(someOtherMessage);
        }

        #endregion

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<WriteSagaData> mapper)
        {
        }
    }

    public class WriteSagaData :
        ContainSagaData
    {
    }
}