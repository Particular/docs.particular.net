using NServiceBus;

public class PurgeOnStartup
{
    public void Simple()
    {
        #region PurgeOnStartupV4

        Configure.With()
            .PurgeOnStartup(true);

        #endregion
    }

}