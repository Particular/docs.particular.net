using NServiceBus;

public class PurgeOnStartup
{
    public void Simple()
    {
        #region PurgeOnStartupV5

        Configure.With(builder => builder.PurgeOnStartup(true));

        #endregion
    }

}