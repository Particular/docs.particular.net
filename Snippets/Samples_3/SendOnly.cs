using NServiceBus;


public class SendOnly
{
    public void Simple()
    {

        #region SendOnlyV3

        var bus = Configure.With()
            .DefaultBuilder()
            .UnicastBus()
            .SendOnly();

        #endregion
    }

}