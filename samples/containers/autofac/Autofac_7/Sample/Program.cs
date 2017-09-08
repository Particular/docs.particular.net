using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using NServiceBus;

static class Program
{
    static void Main()
    {
        //required to prevent possible occurrence of .NET Core issue https://github.com/dotnet/coreclr/issues/12668
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Autofac";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Autofac");

        var builder = new ContainerBuilder();

        IEndpointInstance endpoint = null;
        builder.Register(x => endpoint)
            .As<IEndpointInstance>()
            .SingleInstance();

        builder.RegisterInstance(new MyService());

        var container = builder.Build();

        endpointConfiguration.UseContainer<AutofacBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingLifetimeScope(container);
            });

        #endregion

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        #region EndpointAssignment
        endpoint = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        #endregion

        var endpointInstance = container.Resolve<IEndpointInstance>();
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
