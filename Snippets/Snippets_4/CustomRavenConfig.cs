using NServiceBus;


public class CustomRavenConfig
{
    public void Simple()
    {
        #region CustomRavenConfig
        Configure.With().RavenPersistence("http://localhost:8080", "MyDatabase");
        #endregion
    }

}