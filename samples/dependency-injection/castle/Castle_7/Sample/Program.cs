using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;

static class Program
{
    static async Task Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        Console.Title = "Samples.Castle";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Castle");

        var container = new WindsorContainer();
        var registration = Component.For<MyService>()
            .Instance(new MyService());
        container.Register(registration);

        endpointConfiguration.UseContainer<WindsorBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingContainer(container);
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