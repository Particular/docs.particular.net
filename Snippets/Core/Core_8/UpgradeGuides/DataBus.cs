namespace Core8.UpgradeGuides
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.DataBus;

    class DataBusUpgradeGuide
    {
        void ConfigureDataBus(EndpointConfiguration endpointConfiguration)
        {
            #region DataBusUsage-UpgradeGuide
            endpointConfiguration.UseDataBus<FileShareDataBus, SystemJsonDataBusSerializer>();
            #endregion
        }

        void ConfigureCustomDataBus(EndpointConfiguration endpointConfiguration)
        {
            #region CustomDataBus-UpgradeGuide
            endpointConfiguration.UseDataBus(serviceProvider => new MyDataBus(serviceProvider.GetRequiredService<SomeDependency>()), new SystemJsonDataBusSerializer());
            #endregion
        }

        class SomeDependency
        {
        }

        class MyDataBus : IDataBus
        {
            public MyDataBus(SomeDependency dependency)
            {
            }

            public Task<Stream> Get(string key, CancellationToken cancellationToken = new CancellationToken())
            {
                return Task.FromResult(default(Stream));
            }

            public Task<string> Put(Stream stream, TimeSpan timeToBeReceived, CancellationToken cancellationToken = new CancellationToken())
            {
                return Task.FromResult("");
            }

            public Task Start(CancellationToken cancellationToken = new CancellationToken())
            {
                return Task.CompletedTask;
            }
        }
    }
}