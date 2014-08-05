using NServiceBus;

public class PurgeOnStartup
{
    public void Simple()
    {
        // start code PurgeOnStartupV5

        Configure.With(builder => builder.PurgeOnStartup(true));

        // end code PurgeOnStartupV5
    }

}