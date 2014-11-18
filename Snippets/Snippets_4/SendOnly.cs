using NServiceBus;


public class SendOnly
{
    public void Simple()
    {

        #region SendOnlyV4

        var bus = Configure.With()
            .DefaultBuilder()
            .UnicastBus()
            .SendOnly();

        #endregion
    }

}