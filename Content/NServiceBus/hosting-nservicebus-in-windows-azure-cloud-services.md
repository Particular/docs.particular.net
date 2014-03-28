---
title: Hosting NServiceBus in Windows Azure Cloud Services
summary: Using Windows Azure Cloud Services to host NServiceBus.
tags: 
- Windows Azure
- Cloud
---

The Windows Azure Platform and NServiceBus make a perfect fit. On the one hand the azure platform offers us the scalable and flexible platform that we are looking for in our designs, on the other hand NServiceBus makes development on this highly distributed environment a breeze.

If real scale is what you're looking for, as in tens, hundreds or even thousands of machines hosting each endpoint, than cloud services is the deployment model you'll need.

**Note:** if you don't need the scale offered by cloud services, there are [other hosting options available.](/nservicebus/hosting-nservicebus-in-windows-azure)


Cloud Services - Worker Roles
-----------------

First you need to reference the assembly that contains the windows azure role entry point integration. The recommended way of doing this is by adding a nuget package reference to the `NServiceBus.Hosting.Azure` package to your project.

**Note:** If self hosting, like we'll do later in this article for Web Roles, you can configure everything using the fluent configuration API and the extension methods found in the `NServiceBus.Azure` package, no need to reference the hosting package in that case.

To integrate the NServiceBus generic host into the worker role entry point, all you need to do is create a new instance of `NServiceBusRoleEntrypoint` and call it's `Start` and `Stop` methods in the appropriate `RoleEntryPoint` override.

    public class WorkerRole : RoleEntryPoint
    {
        private NServiceBusRoleEntrypoint nsb = new NServiceBusRoleEntrypoint();

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

Next to starting the role entry point, you also need to define how you want your endpoint to behave. As we're inside worker roles most of the time, the role has been conveniently named `AsA_Worker`. Furthermore you also need to specify the transport that you want to use, using the `UsingTransport<T>` interface.

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureStorageQueue> { }

This will integrate and configure the default infrastructure for you, being:

* Configuration setting will be read from your app.config file, merged with the settings from the service configuration file.
* Depending on the specified profile (`Development` or `Production`), logs will be sent to respectively the `ConsoleLogger` or the `TraceLogger`, the latter should have been implemented with azure diagnostic monitor trace listener by the visual studio tooling.
* Subscriptions are persisted in windows azure storage (in case of the AzureStorageQueue transport, AzureServiceBus has it's own subscription facility)
* Saga's are enabled by default and persisted in windows azure storage
* Timeouts are enabled by default and persisted in windows azure storage


Configuration override convention
---------------------------------

Because windows azure cloud services has it's own configuration model, but nservicebus is typically used with it's configuration in app.config, we've decided to go for a convention based override model. Where most of the configuration is in app.config, but you can add any setting 'by convention' to the service configuration file to override the original value in app.config. This makes it easy to develop locally (without the service runtime), but still make use of this feature in production.

NServiceBus makes extensive use of the .net configuration section model, which allows it to apply default settings if you do not specify anything in app.config. So if you don't specify anything, than the following will apply:

	public class AzureProfileConfig : ConfigurationSection
    {
        [ConfigurationProperty("Profiles", IsRequired = false, DefaultValue = "NServiceBus.Development")]
        public string Profiles
        {
            get
            {
                return this["Profiles"] as string;
            }
            set
            {
                this["Profiles"] = value;
            }
        }
    }

You can then override this setting in your app.config file, by specifying this config section like this.

	<configSections>
	  <section name="AzureProfileConfig" type="NServiceBus.Config.AzureProfileConfig, NServiceBus.Hosting.Azure" />
	</configSections>
	<AzureProfileConfig Profiles="NServiceBus.Development" />

When hosting in a windows azure cloud service, you can override this setting again in the service configuration (.cscfg) file. 

First you need to define the setting in the service definition file (.csdef) and then specify the value for every cloud service deployment you have in your Cloud Services project.

`.csdef`

	<WorkerRole name="VideoStore.Sales">
	   <ConfigurationSettings>
	      <Setting name="AzureProfileConfig.Profiles"/>
	   </ConfigurationSettings>
	</WorkerRole>

The name to be used for the property override is always structured like this: `TagName.PropertyName`. If tags were nested it's simply a repetition of the pattern `ParentTagName.ChildTagName.PropertyName`. It's currently impossible to override parent tags that would contain multiple of the same child tag, therefore it's impossible to override `MessageEndpointMappings` in this way.

`.cscfg`

	<Role name="VideoStore.Sales">
	    <Instances count="2" />
	    <ConfigurationSettings>
	         <Setting name="AzureProfileConfig.Profiles" value="NServiceBus.Production"/>
	    </ConfigurationSettings>
	</Role>

The override order used in this example applies, lowest priority is the default value, then the app.config value is applied, and than the service configuration value is applied.

Logging
-------

The NServiceBus logging integrates with the Windows Azure Diagnostics service through a simple trace logger. In the past it would itself setup azure diagnostics service and integrate with it directly, but this is no longer the case today. The primary reason for this is that Visual Studio tooling now sets everything up for you anyway.

If the following trace listener is added to your app.config, all nservicebus logs should be forwarded to the diagnostics service.

	<system.diagnostics>
		<trace>
		  <listeners>
		    <add type="Microsoft.WindowsAzure.Diagnostics.DiagnosticMonitorTraceListener, Microsoft.WindowsAzure.Diagnostics, Version=2.2.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
		      name="AzureDiagnostics">
		      <filter type="" />
		    </add>
		  </listeners>
		</trace>
	</system.diagnostics>

Logging settings can than be controlled by configuring the windows azure diagnostics service itself using a .wadcfg file. Check out the (msdn documentation)[http://msdn.microsoft.com/en-us/library/windowsazure/hh411551.aspx] for more information on this topic.

Cloud Services - Web Roles
-----------------

Next to worker roles, cloud services also has a role type called 'Web Roles'. These are simply worker roles which have IIS configured properly, this means that they run a worker role process (the entry point is in webrole.cs) and an IIS process on the same codebase.

Usually you will want to run NServiceBus as a client in the IIS process though. This needs to be approached in the same way as any other website, by means of self hosting. When  selfhosting you can configure everything using the fluent configuration API and the extension methods found in the `NServiceBus.Azure` package, no need to reference the hosting package in that case.

The fluent API is used with the following extension methods to achieve the same behavior as the generic `AsA_worker`:

	Configure.With()
            ...
            .AzureConfigurationSource()
            .TraceLogger()

            .UseTransport<AzureStorageQueue>()

			.AzureSubscriptionStorage()
			.AzureSagaPersister()
			.UseAzureTimeoutPersister()
			...

a short explanation of each:

* AzureConfigurationSource: Tells nservicebus to override any settings from the app.config file with settings from the service configuration file.
* TraceLogger: Redirects all logs to the trace logger (which in turn should be configured for diagnostics monitor trace listener.
* UseTransport<AzureStorageQueue>: Sets azure storage queues as the transport
* AzureSubscriptionStorage: Configures azure storage for persistence of subscriptions.
* AzureSagaPersister: Configures azure storage for persistence of saga's.
* UseAzureTimeoutPersister: Configures azure storage for persistence of timeout state.


Sample
------

Want to see these persisters in action? Checkout the [Video store sample.](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/VideoStore.AzureStorageQueues.Cloud) and more specifically, the `VideoStore.Sales` endpoint
