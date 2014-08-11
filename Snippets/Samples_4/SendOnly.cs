using NServiceBus;


public class SendOnly
{
    public void Simple()
    {

        #region SendOnlyV4

        var bus = Configure.With()
            .SendOnly();

        #endregion
    }

}