using NServiceBus;


public class SendOnly
{
    public void Simple()
    {

        #region SendOnly

        IBus bus = Configure.With()
            .DefaultBuilder()
            //Other config
            .UnicastBus()
            .SendOnly();

        #endregion
    }

}