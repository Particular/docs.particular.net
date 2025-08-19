using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddAzureServiceBusTopology(builder.Configuration);

var endpointConfiguration = new EndpointConfiguration("Samples.AzureServiceBus.Options.Publisher");

#region OptionsLoading
var section = builder.Configuration.GetSection("AzureServiceBus");
var topologyOptions = section.GetSection("Topology").Get<TopologyOptions>()!;
var topology = TopicTopology.FromOptions(topologyOptions);
var transport = new AzureServiceBusTransport(section["ConnectionString"]!, topology)
{
    Topology =
    {
        // Validation is already done by the generic host so we can disable in the transport
        OptionsValidator = new TopologyOptionsDisableValidationValidator()
    }
};
endpointConfiguration.UseTransport(transport);
#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<Worker>();

var app = builder.Build();
await app.RunAsync();