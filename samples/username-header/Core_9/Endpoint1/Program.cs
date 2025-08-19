using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.MessageMutator;


Console.Title = "Endpoint1";

var builder = Host.CreateApplicationBuilder(args);


var endpointConfiguration = new EndpointConfiguration("Samples.UsernameHeader.Endpoint1");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region component-registration-sender

// Register both services

var principalAccessor = new PrincipalAccessor();
builder.Services.AddSingleton<IPrincipalAccessor>(principalAccessor);

var serviceProvider = builder.Services.BuildServiceProvider();
builder.Services.AddHostedService<InputLoopService>();

var logger = serviceProvider.GetRequiredService<ILogger<AddUserNameToOutgoingHeadersMutator>>();
var mutator = new AddUserNameToOutgoingHeadersMutator(principalAccessor, logger);
endpointConfiguration.RegisterMessageMutator(mutator);
#endregion

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);


await builder.Build().RunAsync();

