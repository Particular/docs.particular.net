using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Logging;

public class ContinuousJob : IJobHost
{
    public ContinuousJob(IConfiguration configuration, IServiceCollection services)
    {
        this.configuration = configuration;
        this.services = services;
    }

    #region WebJobHost_Start
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        endpointConfiguration = new EndpointConfiguration("receiver");
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();

        var transportConnectionString = configuration.GetConnectionString("TransportConnectionString");

        var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString(transportConnectionString);

        endpoint = await Endpoint.Start(endpointConfiguration);
        #endregion

        _ = SimulateWork();
    }

    private async Task SimulateWork()
    {
        // sending here to simulate work
        await endpoint.SendLocal(new MyMessage());
        await endpoint.SendLocal(new MyMessage());
    }

    #region WebJobHost_Stop

    public async Task StopAsync()
    {
        await endpoint.Stop();
    }
    #endregion

    public Task CallAsync(string name, IDictionary<string, object> arguments = null, CancellationToken cancellationToken = default)
    {
        // this method is not needed for NServiceBus to run
        throw new NotImplementedException();
    }

    #region WebJobHost_CriticalError
    static async Task OnCriticalError(ICriticalErrorContext context)
    {
        var fatalMessage =
            $"The following critical error was encountered:\n{context.Error}\nProcess is shutting down.";
        Logger.Fatal(fatalMessage, context.Exception);

        if (Environment.UserInteractive)
        {
            // so that user can see on their screen the problem
            await Task.Delay(10_000);
        }

        Environment.FailFast(fatalMessage, context.Exception);

    }
    #endregion


    readonly IConfiguration configuration;
    readonly IServiceCollection services;
    EndpointConfiguration endpointConfiguration;
    IEndpointInstance endpoint;
    static ILog Logger = LogManager.GetLogger<ContinuousJob>();
}
