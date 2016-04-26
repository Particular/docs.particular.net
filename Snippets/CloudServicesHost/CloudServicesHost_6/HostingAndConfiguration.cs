﻿namespace CloudServicesHost_6
{
    using System.Web;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using NServiceBus;
    using NServiceBus.Hosting.Azure;

    #region HostingInWorkerRole

    public class WorkerRole : RoleEntryPoint
    {
        NServiceBusRoleEntrypoint nsb = new NServiceBusRoleEntrypoint();

        public override bool OnStart()
        {
            nsb.Start();

            return base.OnStart();
        }

        public override void OnStop()
        {
            nsb.Stop();

            base.OnStop();
        }
    }

    #endregion

    #region ConfigureEndpoint

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
    {
        public void Customize(BusConfiguration builder)
        {
            builder.UseTransport<AzureServiceBusTransport>();
            builder.UsePersistence<AzureStoragePersistence>();
        }
    }


    #endregion

    #region HostingInWebRole

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.AzureConfigurationSource();
            busConfiguration.UseTransport<AzureStorageQueueTransport>();
            busConfiguration.UsePersistence<AzureStoragePersistence>();
            IStartableBus startableBus = Bus.Create(busConfiguration);
            IBus bus = startableBus.Start();
        }
    }
    #endregion
}
