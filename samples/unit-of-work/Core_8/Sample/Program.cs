using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.UnitOfWork";
        var endpointConfiguration = new EndpointConfiguration("Samples.UnitOfWork");
        endpointConfiguration.UseTransport(new LearningTransport());
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
                components.AddTransient<CustomManageUnitOfWork>();
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