using System.Runtime.ExceptionServices;
using NServiceBus;

class SessionAndConfigurationHolder
{
    public SessionAndConfigurationHolder(EndpointConfiguration endpointConfiguration)
    {
        EndpointConfiguration = endpointConfiguration;
    }

    public EndpointConfiguration EndpointConfiguration { get; }

    public IMessageSession MessageSession { get; internal set; }

    public ExceptionDispatchInfo StartupException { get; internal set; }
}