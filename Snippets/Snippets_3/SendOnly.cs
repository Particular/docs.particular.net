using NServiceBus;


public class SendOnly
{
    public void Simple()
    {

        #region SendOnly 3

        var bus = Configure.With()
            .DefaultBuilder()
            .UnicastBus()
            .SendOnly();

        #endregion
    }

}