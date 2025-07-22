using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

Console.Title = "ExternallyManagedMode";

// Configure NServiceBus endpoint
var endpointConfiguration = new EndpointConfiguration("Samples.DependencyInjection.ExternallyManagedMode");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

// Create ServiceCollection for external container
var serviceCollection = new ServiceCollection();

// Register dependencies
serviceCollection.AddSingleton<Greeter>();
serviceCollection.AddSingleton<MessageSender>();

// Create endpoint with externally managed container
var endpointWithExternallyManagedContainer = EndpointWithExternallyManagedContainer
    .Create(endpointConfiguration, serviceCollection);

// Register the message session
serviceCollection.AddSingleton(p => endpointWithExternallyManagedContainer.MessageSession.Value);

// Build the service provider
using var serviceProvider = serviceCollection.BuildServiceProvider();

// Start the endpoint
var endpointInstance = await endpointWithExternallyManagedContainer.Start(serviceProvider);

// Get services and send messages
var messageSender = serviceProvider.GetRequiredService<MessageSender>();

await messageSender.SendMessage();

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop();
