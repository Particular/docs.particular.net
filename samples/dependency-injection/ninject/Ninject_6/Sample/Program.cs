using System;
using System.Threading.Tasks;
using Ninject;
using NServiceBus;

public class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Ninject";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Ninject");

        var kernel = new StandardKernel();
        kernel.Bind<MyService>()
            .ToConstant(new MyService());
        endpointConfiguration.UseContainer<NinjectBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingKernel(kernel);
            });

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();


        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}