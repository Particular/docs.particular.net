using NServiceBus;

public class PurgeOnStartup
{
    public void Simple()
    {
        // start code PurgeOnStartupV4

        Configure.With()
            .PurgeOnStartup(true);

        // end code PurgeOnStartupV4
    }

}