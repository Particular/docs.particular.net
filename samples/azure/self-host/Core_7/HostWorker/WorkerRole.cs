using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using NServiceBus;
using NServiceBus.Logging;

public class WorkerRole :
    RoleEntryPoint
{
    private static IEndpointInstance endpoint;

    public override bool OnStart()
    {
        StartEndpoint().GetAwaiter().GetResult();

        return base.OnStart();
    }

    public override void OnStop()
    {
        Trace.TraceInformation("HostWorker is stopping");

        endpoint.Stop().GetAwaiter().GetResult();
        base.OnStop();

        Trace.TraceInformation("HostWorker has stopped");
    }

    static async Task StartEndpoint()
    {
        var configuration = new EndpointConfiguration("SelfHostedEndpoint");
        configuration.DefineCriticalErrorAction(OnCriticalError);

        if (SafeRoleEnvironment.IsAvailable)
        {
            var host = SafeRoleEnvironment.CurrentRoleName;
            var instance = SafeRoleEnvironment.CurrentRoleInstanceId;
            var displayName = $"{host}_{instance}";
            configuration
                .UniquelyIdentifyRunningInstance()
                .UsingNames(instance, host)
                .UsingCustomDisplayName(displayName);
        }

        var connectionString = RoleEnvironment.GetConfigurationSettingValue("HostWorker.ConnectionString");
        var persistence = configuration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString(connectionString);
        var transport = configuration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString(connectionString);
        transport.SerializeMessageWrapperWith<NewtonsoftSerializer>();
        transport.DelayedDelivery().DisableTimeoutManager();
        configuration.SendFailedMessagesTo("error");
        configuration.DisableNotUsedFeatures();

        var scanner = configuration.AssemblyScanner();
        scanner.ExcludeAssemblies("System.ValueTuple.dll");

        endpoint = await Endpoint.Start(configuration)
            .ConfigureAwait(false);

        Trace.TraceInformation("HostWorker has been started");
    }

    static Task OnCriticalError(ICriticalErrorContext context)
    {
        if (Environment.UserInteractive)
        {
            Thread.Sleep(10000); // so that user can see on their screen the problem
        }

        var message = $"The following critical error was encountered by NServiceBus:\n{context.Error}\nNServiceBus is shutting down.";
        LogManager.GetLogger(typeof(WorkerRole)).Fatal(message);
        Environment.FailFast(message, context.Exception);

        return Task.FromResult(0);
    }
}