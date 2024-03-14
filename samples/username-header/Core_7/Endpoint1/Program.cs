﻿using System;
using System.Security.Principal;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint1";
        var endpointConfiguration = new EndpointConfiguration("Samples.UsernameHeader.Endpoint1");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region component-registration-sender

        var principalAccessor = new PrincipalAccessor();
        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.RegisterSingleton<IPrincipalAccessor>(principalAccessor);
                components.ConfigureComponent<AddUserNameToOutgoingHeadersMutator>(DependencyLifecycle.InstancePerCall);
            });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        #region send-message

        async Task SendMessage(int userNumber)
        {
            var identity = new GenericIdentity($"FakeUser{userNumber}");
            principalAccessor.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);

            var message = new MyMessage();
            await endpointInstance.Send("Samples.UsernameHeader.Endpoint2", message);
        }

        await Task.WhenAll(SendMessage(1), SendMessage(2));

        #endregion

        Console.WriteLine("Message sent. Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop();
    }
}