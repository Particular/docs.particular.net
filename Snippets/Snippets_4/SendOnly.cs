using NServiceBus;


public class SendOnly
{
    public void Simple()
    {

        #region SendOnly 4

        var bus = Configure.With()
            .DefaultBuilder()
            .UnicastBus()
            .SendOnly();

        #endregion
    }

}