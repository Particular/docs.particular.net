// using System;
// using AzureFunctions.KafkaTrigger.FunctionsHostBuilder;
// using Microsoft.Azure.Functions.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection;
// using NServiceBus;
//
// [assembly: FunctionsStartup(typeof(Startup))]
// public class Startup : FunctionsStartup
// {
//     public override void Configure(IFunctionsHostBuilder builder)
//     {
//         FunctionsAssemblyResolver.RedirectAssembly();
//
//         builder.Services.Add(new ServiceDescriptor(typeof(IMessageSession), provider =>
//         {
//             var cfg = new EndpointConfiguration("SendOnly");
//             cfg.SendOnly();
//
//             cfg.AssemblyScanner().ExcludeAssemblies("Google.Protobuf.dll", "Azure.Core.dll");
//
//             var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsServiceBus");
//             var transport = new AzureServiceBusTransport(connectionString);
//             var routing = cfg.UseTransport(transport);
//
//             routing.RouteToEndpoint(typeof(FollowUp), "Samples.KafkaTrigger.ConsoleEndpoint");
//
//             var endpoint = Endpoint.Start(cfg).GetAwaiter().GetResult();
//
//             return endpoint;
//         }, ServiceLifetime.Singleton));
//     }
// }