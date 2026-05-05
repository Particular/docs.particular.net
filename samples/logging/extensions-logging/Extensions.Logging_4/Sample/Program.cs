using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NServiceBus.Extensions.Logging;

Console.Title = "ExtensionsLogging";

#region NLogConfiguration
var config = new LoggingConfiguration();

var consoleTarget = new ColoredConsoleTarget
{
    Layout = "${level}|${logger}|${message}${onexception:${newline}${exception:format=tostring}}"
};
config.AddTarget("console", consoleTarget);
config.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Debug, consoleTarget));

NLog.LogManager.Configuration = config;

#endregion

var builder = Host.CreateApplicationBuilder();

#region MicrosoftExtensionsLoggingNLog

builder.Logging.AddNLog(config);

#endregion

var endpointConfiguration = new EndpointConfiguration("Samples.Logging.ExtensionsLogging");

endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
var host = builder.Build();
var messageSession = host.Services.GetRequiredService<IMessageSession>();
await host.StartAsync();

var myMessage = new MyMessage();
await messageSession.SendLocal(myMessage);
Console.WriteLine("Press any key to exit");
Console.ReadKey();
await host.StopAsync();