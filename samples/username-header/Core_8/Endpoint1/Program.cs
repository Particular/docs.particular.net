using System;
using System.Security.Principal;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint1";
        var endpointConfiguration = new EndpointConfiguration("Samples.UsernameHeader.Endpoint1");
        endpointConfiguration.UseTransport(new LearningTransport());

        #region component-registration-sender

        var principalAccessor = new PrincipalAccessor();
        var mutator = new AddUserNameToOutgoingHeadersMutator(principalAccessor);
        endpointConfiguration.RegisterMessageMutator(mutator);

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        #region send-message

        async Task SendMessage(int userNumber)
        {
            var identity = new GenericIdentity($"FakeUser{userNumber}");
            principalAccessor.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);

            var message = new MyMessage();
            await endpointInstance.Send("Samples.UsernameHeader.Endpoint2", message)
                .ConfigureAwait(false);
        }

        await Task.WhenAll(SendMessage(1), SendMessage(2)).ConfigureAwait(false);

        #endregion

        Console.WriteLine("Message sent. Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}