using System.Threading.Tasks;
using NServiceBus;

public interface ICustomizeConfiguration
{
    Task Run(EndpointConfiguration endpointConfiguration);
}