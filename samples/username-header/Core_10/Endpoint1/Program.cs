using System;
using System.Security.Principal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus.MessageMutator;

Console.Title = "Endpoint1";

var endpointConfiguration = new EndpointConfiguration("Samples.UsernameHeader.Endpoint1");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var builder = Host.CreateApplicationBuilder(args);

#region component-registration-sender

// Register both services

var principalAccessor = new PrincipalAccessor();
builder.Services.AddSingleton<IPrincipalAccessor>(principalAccessor);

var serviceProvider = builder.Services.BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<AddUserNameToOutgoingHeadersMutator>>();
var mutator = new AddUserNameToOutgoingHeadersMutator(principalAccessor, logger);
endpointConfiguration.RegisterMessageMutator(mutator);
#endregion

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

#region send-message

async Task SendMessage(int userNumber)
{
    var identity = new GenericIdentity($"FakeUser{userNumber}");
    principalAccessor.CurrentPrincipal = new GenericPrincipal(identity, []);

    var messageSession = host.Services.GetRequiredService<IMessageSession>();

    var message = new MyMessage();
    await messageSession.Send("Samples.UsernameHeader.Endpoint2", message);
}

await Task.WhenAll(SendMessage(1), SendMessage(2));

#endregion

Console.WriteLine("Message sent. Press any key to exit");
Console.ReadKey();

await host.StopAsync();