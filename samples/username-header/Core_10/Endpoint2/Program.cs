using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.MessageMutator;

Console.Title = "Endpoint2";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.UsernameHeader.Endpoint2");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region component-registration-receiver
// Register both services
builder.Services.AddSingleton<IPrincipalAccessor, PrincipalAccessor>();
builder.Services.AddSingleton<SetCurrentPrincipalBasedOnHeaderMutator>();

var serviceProvider = builder.Services.BuildServiceProvider();
var principalAccessor = serviceProvider.GetRequiredService<IPrincipalAccessor>(); // Use the interface type here
var mutator = serviceProvider.GetRequiredService<SetCurrentPrincipalBasedOnHeaderMutator>();

endpointConfiguration.RegisterMessageMutator(mutator);

endpointConfiguration.RegisterComponents(c =>
{
    //Register the accessor in the container so that the handler can access it
    c.AddSingleton<IPrincipalAccessor>(principalAccessor);
});

#endregion

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();

