using NServiceBus;

public class PurgeOnStartup
{
    public void Simple()
    {
        #region PurgeOnStartup

        Configure.With()
            .PurgeOnStartup(true);

        #endregion
    }

}