using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.AcceptanceTesting.Support;

#region TransportTestConfiguration
public class ConfigureFileTransportInfrastructure : IConfigureEndpointTestExecution
{
    public Task Configure(string endpointName, EndpointConfiguration configuration, RunSettings settings, PublisherMetadata publisherMetadata)
    {
        configuration.UseTransport<FileTransport>();
        return Task.CompletedTask;
    }

    public Task Cleanup()
    {
        return Task.CompletedTask;
    }
}

#endregion