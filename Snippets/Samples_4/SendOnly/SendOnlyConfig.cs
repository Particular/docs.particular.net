using NServiceBus;

public class SendOnlyConfig
{
    public SendOnlyConfig()
    {
        #region SendOnly-v4
        var bus = Configure.With()
                        .DefaultBuilder()
                        .UseTransport<Msmq>()
                        .UnicastBus()
                        .SendOnly();
        bus.Send(new TestMessage());
        #endregion
    }


    public class TestMessage
    {
    }

}
