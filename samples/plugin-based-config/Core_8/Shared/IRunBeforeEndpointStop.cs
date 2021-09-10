using System.Threading.Tasks;
using NServiceBus;

public interface IRunBeforeEndpointStop
{
    Task Run(IEndpointInstance endpoint);
}