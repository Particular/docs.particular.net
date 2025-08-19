using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

Console.Title = "UnitOfWork";

var endpointConfiguration = new EndpointConfiguration("Samples.UnitOfWork");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var recoverability = endpointConfiguration.Recoverability();
recoverability.Immediate(
    customizations: immediate => { immediate.NumberOfRetries(0); });
recoverability.Delayed(
    customizations: delayed => { delayed.NumberOfRetries(0); });

#region ComponentRegistration

endpointConfiguration.RegisterComponents(
    registration: components => { components.AddTransient<CustomManageUnitOfWork>(); });

#endregion

var endpointInstance = await Endpoint.Start(endpointConfiguration);
await Runner.Run(endpointInstance);
await endpointInstance.Stop();