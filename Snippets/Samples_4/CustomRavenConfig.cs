using NServiceBus;


public class CustomRavenConfig
{
    public void Simple()
    {
        // start code CustomRavenConfigV4
        Configure.With().RavenPersistence("http://localhost:8080", "MyDatabase");
        // end code CustomRavenConfigV4
    }

}