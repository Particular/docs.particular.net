using NServiceBus;

public class SendOnlyConfig
{
    public SendOnlyConfig()
    {
        #region SendOnly-v3
        var bus = Configure.With()
                        .DefaultBuilder()
                        .MsmqTransport()
                        .UnicastBus()
                        .SendOnly();
        bus.Send(new TestMessage());
        #endregion
    }


    public class TestMessage
    {
    }

}
