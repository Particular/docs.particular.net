using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint1";
        var endpointConfiguration = new EndpointConfiguration("Samples.UsernameHeader.Endpoint1");
        endpointConfiguration.UseTransport<LearningTransport>();

        #region ComponentRegistration

        endpointConfiguration.RegisterMessageMutator(new UsernameMutator());

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        #region SendMessage

        var identity = new GenericIdentity("FakeUser");
        Thread.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);
        var message = new MyMessage();
        await endpointInstance.Send("Samples.UsernameHeader.Endpoint2", message)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine("Message sent. Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}