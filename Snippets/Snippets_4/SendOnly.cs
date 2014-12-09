using NServiceBus;


public class SendOnly
{
    public void Simple()
    {

        #region SendOnly

        var bus = Configure.With()
            .DefaultBuilder()
            .UnicastBus()
            .SendOnly();

        #endregion
    }

}