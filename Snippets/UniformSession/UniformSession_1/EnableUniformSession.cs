namespace UniformSession_2
{
    using NServiceBus;

    public class EnableUniformSession
    {
        void Configure(EndpointConfiguration endpointConfiguration)
        {
            #region enable-uniformsession

            endpointConfiguration.EnableUniformSession();

            #endregion
        }
    }
}