Console.Title = "Receiver";

var endpointConfiguration = new EndpointConfiguration("FixMalformedMessages.Receiver");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.EnableInstallers();
endpointConfiguration.SendFailedMessagesTo("error");

#region DisableRetries

var recoverability = endpointConfiguration.Recoverability();

recoverability.Delayed(
    customizations: retriesSettings =>
    {
        retriesSettings.NumberOfRetries(0);
    });
recoverability.Immediate(
    customizations: retriesSettings =>
    {
        retriesSettings.NumberOfRetries(0);
    });

#endregion

#region RegisterFixBehavior

var pipeline = endpointConfiguration.Pipeline;

pipeline.Register(
    behavior: new FixMessageIdBehavior(),
    description: "Fix message Id");

#endregion

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press 'Enter' to finish.");
Console.ReadLine();

await endpointInstance.Stop();
