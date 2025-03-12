using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.MessageMutator;


Console.Title = "Endpoint1";

var builder = Host.CreateApplicationBuilder(args);


var endpointConfiguration = new EndpointConfiguration("Samples.UsernameHeader.Endpoint1");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region component-registration-sender

// Register both services

builder.Services.AddSingleton<IPrincipalAccessor, PrincipalAccessor>();
builder.Services.AddSingleton<AddUserNameToOutgoingHeadersMutator>();
builder.Services.AddHostedService<InputLoopService>();
var serviceProvider = builder.Services.BuildServiceProvider();
var principalAccessor = serviceProvider.GetRequiredService<IPrincipalAccessor>(); // Use the interface type here
var mutator = serviceProvider.GetRequiredService<AddUserNameToOutgoingHeadersMutator>();

endpointConfiguration.RegisterMessageMutator(mutator);

#endregion

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();

