---
title: Shared Hosting in Azure Cloud Services
summary: Using Azure Cloud Services to host multiple NServiceBus endpoints on a shared pool of machines.
tags:
- Azure
- Cloud
- Hosting
- Worker Roles
related:
 - samples/azure/shared-host
redirects:
 - nservicebus/shared-hosting-nservicebus-in-windows-azure-cloud-services
---

If real scale is what you're looking for, as in tens, hundreds or even thousands of machines hosting each endpoint, than cloud services is the deployment model you'll need. But very often, you only want this scale when you are eventually successful, not when you are just starting out. To support this scenario, we've created the `AsA_Hos`t endpoint role for Azure cloud services.

This role allows you to co-locate multiple endpoints on the same set of machines, while preserving the regular worker role programming model so that you can easily put each endpoint on it's own role again when required later.


## How it works

**Prerequisites** This approach assumes you already have your endpoints [hosted in worker roles](hosting-in-azure-cloud-services.md). The rest of this article will focus on how to transition from a multi worker environment to a shared hosting environment.

Instead of having our endpoints packaged & deployed by the Azure infrastructure, we will package them ourselves (as zip files), and put them in a well known location (in Azure blob storage).

Then we'll add a new worker role to the cloud services solution that will act as the host. This host will be configured to pull the endpoints from the well known location, extract them to disk and run them.


## Preparing the endpoint

Assuming you have a working endpoint hosted in a worker role. Open your cloud services project, expand `Roles` and click remove on the worker role that you're preparing.

NOTE: Visual studio will remove any configuration setting from the Azure configuration settings file. If you had configuration overrides in place that effect the way your endpoint behaves, make sure you move those to the app.config file first or apply the alternative override system for shared hosts, see `Configuration concerns` further down this article for more details on this approach.

The role entry point also doubles as a host process for our endpoint, one that is aware of the service runtime and role context. This functionality needs to be replaced by another process in order to run the endpoint in a similar context as it would have when it was a separate role. This replacement host process is available on NuGet as the `NServiceBus.Hosting.Azure.HostProcess` package, please install it in your worker role project.

You'll notice that an NServiceBus.Hosting.Azure.HostProcess.exe is now referenced. The beauty of this exe is that it can also run on your machine, so outside the context of a service runtime, aka you can debug your endpoint locally without starting the Azure emulator by adding this exe to the debug path in the project properties.

Next you need to pack the build output as a zip file so that the NServiceBus.Hosting.Azure.HostProcess.exe is in the root of the archive. (Just zip the debug or release folder)

Finally go to your Azure storage account and create a private container called `endpoints` and put the zip file in there. We'll configure the host role entry point to download endpoints from this container later.


## Creating the host

Once you have prepared and uploaded all your endpoints, you can add a new worker role project to your solution. This worker role will serve as a host for all your endpoints.

In this worker role you need to reference the assembly that contains the Azure role entry point integration. The recommended way of doing this is by adding a NuGet package reference to the `NServiceBus.Hosting.Azure` package to your project.

To integrate the NServiceBus dynamic host into the worker role entry point, all you need to do is create a new instance of `NServiceBusRoleEntrypoint` and call it's `Start` and `Stop` methods in the appropriate `RoleEntryPoint` override.

```
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
```

Next to starting the role entry point, you also need to define how you want your endpoint to behave. In this case we want hosting behavior, so that it will not run an endpoint itself but instead host other endpoints. To do so just specify the `AsA_Host` role.

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Host { }

The host entry point does require some configuration, you need to tell it in what storage account to look for endpoints and how often it should do so, furthermore you need to tell Azure to provision some space on the local disk as well, where the host can put the downloaded and extracted endpoints.

Please add the following configuration settings entries to your `.csdef` file

* DynamicHostControllerConfig.ConnectionString: The connectionstring to your storage account

And specify a local storage resource with the name endpoints as well.

	<WorkerRole name="VideoStore.Host" vmsize="Small">
    	<Imports>
      		<Import moduleName="Diagnostics" />
    	</Imports>
    	<ConfigurationSettings>
      		<Setting name="DynamicHostControllerConfig.ConnectionString" />      		
    	</ConfigurationSettings>
    	<LocalResources>
      		<LocalStorage name="endpoints" cleanOnRoleRecycle="true" sizeInMB="1000" />
    	</LocalResources>
	</WorkerRole>

Other configuration settings are available as well if you need more fine grained control on how the host works:

* `DynamicHostControllerConfig.Container`: The container where the endpoint packages are stored in the storage account, defaults to `endpoints`
* `DynamicHostControllerConfig.AutoUpdate`: Turn auto update on or off, defaults to true. Note that if you set it to false you need to reboot for the host to pick new endpoints or versions of endpoints.
* `DynamicHostControllerConfig.UpdateInterval`: The time between checks if updates are available, in milliseconds, defaults to 600000
* `DynamicHostControllerConfig.LocalResource`: The name of the local storage resource where the zip archives will be extracted, defaults to `endpoints`
* `DynamicHostControllerConfig.TimeToWaitUntilProcessIsKilled`: When updating an endpoint to a new version, the host will kill the current process. Sometimes this fails or takes a very long time. This property specifies how long the host should wait, if this time elapses without the process going down, the host will reboot the machine (by throwing an exception). Default value: 10000.
* `DynamicHostControllerConfig.RecycleRoleOnError`: By default Azure role instances will reboot when an exception is thrown from the role entrypoint, but not when thrown from a child process. If you want the role instance to reboot in this case as well, set RecycleRoleOnError on true. Then the host will start monitoring the child process for errors and request a recycle when it throws.


## Configuration concerns

The Azure configuration system applies to all instances of all roles. It has a built in way to separate role types, but not role instance and definitely no separation for processes on those instances. This means that a configuration override put in the service configuration file will automatically apply to all endpoints hosted on those roles. This is obviously not desirable, and can be dealt with in 2 ways.

* Put your configuration settings in the app.config. As autoupdate is available you can easily manage it this way as changing a configuration means uploading a new zip to your Azure storage account and the hosts will update themselves automatically. (This is the default)
* Alternatively you can separate the configuration settings in the service configuration file by convention. The `.AzureConfigurationSource(prefix)` overload allows you to set a prefix in every endpoint that will be prepended to it's configuration settings. Call this configuration method with a prefix of your choice and you can still use the configuration settings file for your hosted endpoints.
