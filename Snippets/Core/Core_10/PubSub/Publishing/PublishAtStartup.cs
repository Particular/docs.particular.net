using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.PubSub.Publishing;

using System.Threading.Tasks;
using NServiceBus;

class PublishAtStartup
{
    public async Task Publish(EndpointConfiguration endpointConfiguration)
    {
        #region publishAtStartup
        // Other config
        var builder = Host.CreateApplicationBuilder();
        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
        var host = builder.Build();
        await host.StartAsync();
        var messageSession = host.Services.GetRequiredService<IMessageSession>();
        await messageSession.Publish(new MyEvent());

        #endregion

    }
}

public class MyEvent
{
}