---
title: Hosting in Azure Cloud Services
summary: Using Azure Cloud Services to host NServiceBus.
tags:
 - Hosting
 - Worker Roles
 - Web Roles
 - Azure
 - Cloud
 - Logging
redirects:
 - nservicebus/hosting-nservicebus-in-windows-azure-cloud-services
related:
 - samples/azure/shared-host
 - nservicebus/lifecycle
---

The Azure Platform and NServiceBus make a perfect fit. On the one hand the Azure platform offers the scalable and flexible platform required, on the other hand NServiceBus makes development on this highly distributed environment a breeze.

If real scale is required (eg in tens, hundreds or even thousands of machines hosting each endpoint) than cloud services is the required deployment model.

NOTE: If this level of scale is not required, there are [other hosting options available](hosting.md).


## Cloud Services - Worker Roles

Reference the assembly that contains the Azure role entry point integration. The recommended way of doing this is by adding a NuGet package reference to the `NServiceBus.Hosting.Azure` package to the project.

NOTE: If self hosting, done later in this article for Web Roles, everything can be configured using the configuration API and the extension methods found in the `NServiceBus.Azure` package, no need to reference the hosting package in that case.

To integrate the NServiceBus generic host into the worker role entry point, create a new instance of `NServiceBusRoleEntrypoint` and call it's `Start` and `Stop` methods in the appropriate `RoleEntryPoint` override.

snippet:HostingInWorkerRole

Next to starting the role entry point, define the endpoint behavior. The role has been named `AsA_Worker`. Specify the transport to use, using the `UseTransport<T>`, as well the persistence using the `UsePersistence<T>` configuration methods.

snippet:ConfigureEndpoint

This will integrate and configure the default infrastructure:

 * Configuration setting will be read from the `app.config` file, merged with the settings from the service configuration file.
 * Logs will be sent to the `TraceLogger`, the latter should have been implemented with Azure diagnostic monitor trace listener by the visual studio tooling.
 * Subscriptions are persisted in the chosen persistence store (in case of the AzureStorageQueue transport, AzureServiceBus has it's own subscription facility).
 * Saga's are enabled by default and persisted in the chosen persistence store.
 * Timeouts are enabled by default and persisted in the chosen persistence store.


## Configuration override convention

Because Azure cloud services has it's own configuration model, but NServiceBus is typically used with it's configuration in app.config, a convention based override model is used. Where most of the configuration is in app.config, but any setting can be added 'by convention' to the service configuration file to override the original value in app.config. This makes it easy to develop locally (without the service runtime), but still make use of this feature in production.

NServiceBus makes extensive use of the .NET configuration section model, which allows it to apply default settings if specified anything in `app.config`. If nothing is specified then the following will apply:

```
public class AzureSubscriptionStorageConfig : ConfigurationSection
{
    [ConfigurationProperty("ConnectionString", IsRequired = false, DefaultValue = "UseDevelopmentStorage=true")]
    public string ConnectionString
    {
        get
        {
            return this["ConnectionString"] as string;
        }
        set
        {
            this["ConnectionString"] = value;
        }
    }
}
```

This setting can be overridden in the app.config file, by specifying this config section:

```
<configSections>
	<section name="AzureSubscriptionStorageConfig" type="NServiceBus.Config.AzureSubscriptionStorageConfig, NServiceBus.Hosting.Azure" />
</configSections>
<AzureSubscriptionStorageConfig ConnectionString="YourConnectionstring" />
```

When hosting in a Azure cloud service this setting can be overridden in the service configuration (`.cscfg`) file.

First define the setting in the service definition file (`.csdef`) and then specify the value for every cloud service deployment in the Cloud Services project.

`.csdef`

	<WorkerRole name="VideoStore.Sales">
	   <ConfigurationSettings>
	      <Setting name="AzureSubscriptionStorageConfig.ConnectionString"/>
	   </ConfigurationSettings>
	</WorkerRole>

The name to be used for the property override is always structured like this: `TagName.PropertyName`. If tags were nested it's a repetition of the pattern `ParentTagName.ChildTagName.PropertyName`. It's currently impossible to override parent tags that would contain multiple of the same child tag, therefore it's impossible to override `MessageEndpointMappings` in this way.

`.cscfg`

	<Role name="VideoStore.Sales">
	    <Instances count="2" />
	    <ConfigurationSettings>
	         <Setting name="AzureSubscriptionStorageConfig.ConnectionString" value="YourConnectionString"/>
	    </ConfigurationSettings>
	</Role>

The override order used in this example applies, lowest priority is the default value, then the app.config value is applied, and than the service configuration value is applied.


## Logging

The NServiceBus [logging](/nservicebus/logging/) integrates with the Azure Diagnostics service through a simple trace logger. In the past it would itself setup Azure diagnostics service and integrate with it directly, but this is no longer the case today. The primary reason for this is that Visual Studio tooling now sets up everything.

If the following trace listener is added to the `app.config`, all NServiceBus logs will be forwarded to the diagnostics service.

```
<system.diagnostics>
	<trace>
	<listeners>
		<add 
			type="Microsoft.WindowsAzure.Diagnostics.DiagnosticMonitorTraceListener, Microsoft.WindowsAzure.Diagnostics, Version=2.2.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
			name="AzureDiagnostics">
			<filter type="" />
		</add>
	</listeners>
	</trace>
</system.diagnostics>
```

Logging settings can than be controlled by configuring the Azure diagnostics service itself using a `.wadcfg` file. Check out the [MSDN documentation](https://msdn.microsoft.com/library/azure/hh411551.aspx) for more information on this topic.


## Cloud Services - Web Roles

Next to worker roles, cloud services also has a role type called 'Web Roles'. These are worker roles which have IIS configured properly, this means that they run a worker role process (the entry point is in `webrole.cs`) and an IIS process on the same codebase.

Usually NServiceBus is configured as a client in the IIS process. This needs to be approached in the same way as any other website, by means of self hosting. When self-hosting everything can be configure using the configuration API and the extension methods found in the `NServiceBus.Azure` package. No reference to the hosting package is required in that case.

The configuration API is used with the following extension methods to achieve the same behavior as the generic `AsA_worker`:

snippet:HostingInWebRole

A short explanation of each:

 * `AzureConfigurationSource`: overrides any settings known to the NServiceBus Azure configuration section within the app.config file with settings from the service configuration file.
 * `TraceLogger`: Redirects all [logging](/nservicebus/logging/) to the trace logger. This in turn can be configured for diagnostics monitor trace listener.
 * `UseTransport<AzureStorageQueueTransport>`: Sets [Azure storage queues](/nservicebus/azure/azure-storage-queues-transport.md) as the [transport](/nservicebus/transports).
 * `UsePersistence`: Configures [Azure storage](/nservicebus/azure/azure-storage-persistence.md) for [persistence](/nservicebus/persistence).


include:host-startup


## Handling critical errors


### Azure Host Versions 6.2.2 and above

Azure host is terminated on critical errors by default. When host is terminated, Azure Fabric will restart the host automatically.


### Azure Host Versions 6.2.1 and below

Azure host is not terminated on critical errors by default and only shuts down the bus. This would cause role not to process messages until host (role) is restarted. To address this, implement critical errors handling code that shuts down the host.

snippet:DefineCriticalErrorActionForAzureHost