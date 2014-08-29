using NServiceBus;

public class SendOnlyConfig
{
    public SendOnlyConfig()
    {
        #region SendOnly-v5
        
        var bus = Bus.CreateSendOnly(new BusConfiguration());

        bus.Send(new TestMessage());
        #endregion
    }


    public class TestMessage
    {
    }

}
