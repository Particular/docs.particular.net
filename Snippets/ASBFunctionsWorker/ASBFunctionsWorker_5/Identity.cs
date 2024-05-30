using Microsoft.Extensions.Hosting;
using NServiceBus;
using System.Threading.Tasks;

#endregion asb-function-isolated-configuration

#region asb-function-isolated-identity-connection
[assembly: NServiceBusTriggerFunction("WorkerDemoEndpoint", Connection="MyConnectionName")]
#endregion
public class Program_with_connection
{
}