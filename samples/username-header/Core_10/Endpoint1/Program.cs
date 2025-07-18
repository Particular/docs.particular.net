using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.MessageMutator;


var principalAccessor = new PrincipalAccessor();

var builder = Host.CreateApplicationBuilder(args);
var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(x =>
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.UsernameHeader.Endpoint1");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());


        #region component-registration-sender

        builder.Services.AddSingleton<IPrincipalAccessor>(principalAccessor);
        var serviceProvider = builder.Services.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<AddUserNameToOutgoingHeadersMutator>>();
        var mutator = new AddUserNameToOutgoingHeadersMutator(principalAccessor, logger);
        endpointConfiguration.RegisterMessageMutator(mutator);

        #endregion
        return endpointConfiguration;
    }).Build();


await host.StartAsync();


#region send-message

async Task SendMessage(int userNumber)
{
    var identity = new GenericIdentity($"FakeUser{userNumber}");
    principalAccessor.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);

    var messageSession = host.Services.GetRequiredService<IMessageSession>();

    var message = new MyMessage();
    await messageSession.Send("Samples.UsernameHeader.Endpoint2", message);
}

await Task.WhenAll(SendMessage(1), SendMessage(2));

#endregion

Console.WriteLine("Message sent. Press any key to exit");
Console.ReadKey();

await host.StopAsync();