using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.MessageMutator;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint2";
        var endpointConfiguration = new EndpointConfiguration("Samples.UsernameHeader.Endpoint2");
        endpointConfiguration.UseTransport(new LearningTransport());

        #region component-registration-receiver

        var principalAccessor = new PrincipalAccessor();
        var mutator = new SetCurrentPrincipalBasedOnHeaderMutator(principalAccessor);
        endpointConfiguration.RegisterMessageMutator(mutator);

        endpointConfiguration.RegisterComponents(c =>
        {
            //Register the accessor in the container so that the handler can access it
            c.AddSingleton<IPrincipalAccessor>(principalAccessor);
        });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}