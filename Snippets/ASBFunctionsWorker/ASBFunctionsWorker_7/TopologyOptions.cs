namespace ASBFunctionsWorker;

using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using NServiceBus;

class TopologyOptions
{
    public void SetTopologyOptions(string[] args)
    {
        #region ASBFunctionsWorker-topology-options
        var builder = FunctionsApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        builder.AddNServiceBus();

        var host = builder.Build();
        #endregion
    }
}
