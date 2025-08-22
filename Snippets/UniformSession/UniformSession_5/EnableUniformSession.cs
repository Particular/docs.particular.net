using NServiceBus;

public class EnableUniformSession
{
    public void Configure(EndpointConfiguration endpointConfiguration)
    {
        #region enable-uniformsession

        endpointConfiguration.EnableUniformSession();

        #endregion
    }
}