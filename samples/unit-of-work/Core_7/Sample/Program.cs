using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;

class Program
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
        Console.Title = "Samples.UnitOfWork";
        var endpointConfiguration = new EndpointConfiguration("Samples.UnitOfWork");
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            customizations: immediate =>
            {
                immediate.NumberOfRetries(0);
            });
        recoverability.Delayed(
            customizations: delayed =>
            {
                delayed.NumberOfRetries(0);
            });

        #region ComponentRegistration

        endpointConfiguration.RegisterComponents(
            registration: components =>
            {
                components.ConfigureComponent<CustomManageUnitOfWork>(DependencyLifecycle.InstancePerCall);
            });

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await Runner.Run(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}