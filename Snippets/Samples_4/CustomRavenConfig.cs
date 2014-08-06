using NServiceBus;


public class CustomRavenConfig
{
    public void Simple()
    {
        #region CustomRavenConfigV4
        Configure.With().RavenPersistence("http://localhost:8080", "MyDatabase");
        #endregion
    }

}