using System.Threading.Tasks;
using NServiceBus;
#region IRunAfterEndpointStart
public interface IRunAfterEndpointStart
{
    Task Run(IEndpointInstance endpoint);
}
#endregion