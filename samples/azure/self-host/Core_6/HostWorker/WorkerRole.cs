using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using NServiceBus;
using NServiceBus.Logging;

public class WorkerRole :
    RoleEntryPoint
{
    #region AzureSelfHost_StartEndpoint

    public override bool OnStart()
    {
        StartEndpoint().GetAwaiter().GetResult();
        return base.OnStart();
    }

    static async Task StartEndpoint()
        #endregion
    {
        var configuration = new EndpointConfiguration("SelfHostedEndpoint");

        #region AzureSelfHost_CriticalError

        configuration.DefineCriticalErrorAction(
            onCriticalError: context =>
            {
                if (Environment.UserInteractive)
                {
                    // so that user can see on their screen the problem
                    Thread.Sleep(10000);
                }

                var message = $"Critical error encountered:\n{context.Error}\nNServiceBus is shutting down.";
                LogManager.GetLogger(typeof(WorkerRole)).Fatal(message);
                Environment.FailFast(message, context.Exception);
                return Task.CompletedTask;
            });

        #endregion

        #region AzureSelfHost_DisplayName

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

        #endregion

        #region AzureSelfHost_ConnectionString

        var connectionString = RoleEnvironment.GetConfigurationSettingValue("HostWorker.ConnectionString");
        var persistence = configuration.UsePersistence<AzureStoragePersistence>();
        persistence.ConnectionString(connectionString);
        var transport = configuration.UseTransport<AzureStorageQueueTransport>();
        transport.ConnectionString(connectionString);

        #endregion

        transport.SerializeMessageWrapperWith<NewtonsoftSerializer>();
        transport.DelayedDelivery().DisableTimeoutManager();
        configuration.SendFailedMessagesTo("error");
        configuration.DisableNotUsedFeatures();
        configuration.UseSerialization<NewtonsoftSerializer>();

        endpoint = await Endpoint.Start(configuration)
            .ConfigureAwait(false);

        Trace.TraceInformation("HostWorker has been started");

        await endpoint.SendLocal(new MyMessage())
            .ConfigureAwait(false);

        Trace.TraceInformation("Message sent out");
    }

    public override void OnStop()
    {
        Trace.TraceInformation("HostWorker is stopping");

        endpoint.Stop().GetAwaiter().GetResult();
        base.OnStop();

        Trace.TraceInformation("HostWorker has stopped");
    }

    static IEndpointInstance endpoint;
}