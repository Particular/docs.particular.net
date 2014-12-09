using NServiceBus;

public class BusExtensionMethodForHandler
{
    #region BusExtensionMethodForHandler
    public class MyHandler : IHandleMessages<MyMessage>
    {
        public void Handle(MyMessage message)
        {
            this.Bus().Reply(new OtherMessage());
        }
    }
    #endregion
    public class MyMessage
    {
    }
    public class OtherMessage
    {
    }
}