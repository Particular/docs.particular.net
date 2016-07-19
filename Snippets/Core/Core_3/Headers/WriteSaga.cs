namespace Core3.Headers
{
    using System;
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
            someOtherMessage.SetHeader("MyCustomHeader", "My custom value");
            Bus.Send(someOtherMessage);
        }
    }
    #endregion

    public class WriteSagaData :
        IContainSagaData
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
    }

}