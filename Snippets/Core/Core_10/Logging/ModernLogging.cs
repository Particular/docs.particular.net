namespace Core.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

public class MyMessage : IMessage { }

#region InjectingILoggerInterface

public class MyHandler(ILogger<MyHandler> logger) : IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        logger.LogInformation("Handling message");
        return Task.CompletedTask;
    }
}

#endregion

partial class HighPerformanceLogging
{
    #region UsingLoggerMessageSourceGenerator
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Processing message {MessageId}")]
    static partial void LogProcessingMessage(ILogger logger, string messageId);
    #endregion
}

class Startup
{
    #region ConfiguringRollingFileLogger
    void ConfigureHost()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Services.Configure<RollingLoggerProviderOptions>(options =>
        {
            options.Directory = @"C:\logs";
            options.LogLevel = LogLevel.Debug;
            options.NumberOfArchiveFilesToKeep = 10;
            options.MaxFileSizeInBytes = 10L * 1024 * 1024;
        });
    }
    #endregion
}

#region UsingEndpointLoggingScope
public class MyBackgroundService(
    ILogger<MyBackgroundService> logger,
    EndpointLoggingScope endpointScope) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (logger.BeginEndpointScope(endpointScope))
        {
            logger.LogInformation("Doing background work");
        }

        await Task.CompletedTask;
    }
}
#endregion
