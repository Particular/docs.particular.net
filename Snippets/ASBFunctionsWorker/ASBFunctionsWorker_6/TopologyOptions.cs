using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace ASBFunctionsWorker_6
{
    class TopologyOptions
    {
        public void SetTopologyOptions()
        {
            #region ASBFunctionsWorker-topology-options
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true))
                .UseNServiceBus()
                .Build();
            #endregion
        }
    }
}
