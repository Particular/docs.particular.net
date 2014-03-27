---
title: Multi Transport Support
summary: How to configure ServiceControl to use non-MSMQ Transports
tags:
- ServiceControl
- Multi-Transport
- Configuration
---

### Configuring ServiceControl to use non-MSMQ Transports:

* First, download the NuGet package for the relevant transport including any dependencies.
   * *RabbitMQ*: [NServiceBus.RabbitMQ](https://www.nuget.org/api/v2/package/NServiceBus.RabbitMQ) and [RabbitMQ.Client](https://www.nuget.org/api/v2/package/RabbitMQ.Client)
   * *SQL Server*: [NServiceBus.SqlServer](https://www.nuget.org/api/v2/package/NServiceBus.SqlServer)
   * *Windows Azure Storage Queues*: [NServiceBus.Azure.Transports.WindowsAzureStorageQueues](https://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureStorageQueues/5.1.1) and [WindowsAzure.Storage](https://www.nuget.org/api/v2/package/WindowsAzure.Storage/2.1.0)
   * *Windows Azure ServiceBus*: [NServiceBus.Azure.Transports.WindowsAzureServiceBus](https://www.nuget.org/api/v2/package/NServiceBus.Azure.Transports.WindowsAzureServiceBus/5.1.1) and [WindowsAzure.ServiceBus](https://www.nuget.org/api/v2/package/WindowsAzure.ServiceBus/2.2.0)

* The NuGet packages you just downloaded are in fact zip files. Rename the nupkg files to have a zip extension, and take the dlls from the `/lib` folder and put them in the ServiceControl bin folder: (`"C:\Program Files (x86)\Particular Software\ServiceControl"`).
_NOTE:_ Some nuget packages may have several folders under `/lib` - make sure to take the dlls from only one of them, preferrably the one targetting the latest .NET framework (e.g. `/lib/net40`).

* Stop the ServiceControl service (from an admin cmd line, run `net stop Particular.ServiceControl`)

* Open `ServiceControl.dll.config` in a text editor and locate the connection strings section. Update it with a connection string suitable for the transport you are installing. For example, the connection strings section for a ServiceControl instance using the RabbitMQ transport would look like this:

```xml
<connectionStrings>
  <add name="NServiceBus/Transport" connectionString="host=localhost"/>
</connectionStrings>
```

* Still in `ServiceControl.dll.config`, update the `ServiceControl/TransportType` key to point at the new transport by  specifying its full type. For example, for RabbitMQ this setting would look like this:

   `<add key="ServiceControl/TransportType" value="NServiceBus.RabbitMQ, NServiceBus.Transports.RabbitMQ" />`

* The necessary queues for ServiceControl in the desired transport need to be created. This can be done by uninstalling and re-installing the ServiceControl service.
   * Navigate to where ServiceControl was installed (default is `"C:\Program Files (x86)\Particular Software\ServiceControl"`)
   * To uninstall the ServiceControl service, from a command line with administrator privileges, run the following command: `NServiceBus.Host.exe -uninstall -serviceName="Particular.ServiceControl"`
   * Re-Install the ServiceControl service by running: 

      `NServiceBus.Host.exe -install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl"` 
   * Start the ServiceControl service by running: `"net start Particular.ServiceControl"`
   * Ensure Particular.ServiceControl windows service has started and is functioning properly (try to access the main HTTP API URI exposed by ServiceControl, e.g. do an HTTP GET on `http://localhost:33333/api`

#### Troubleshooting

* Make sure all assemblies copied are unblocked, otherwise the .NET runtime will refuse to load them.

* If there are conflicts between assembly versions used by ServiceControl and the transport, you'll need to add a binding redirect in the ServiceControl.dll.config file. This is the case with Microsoft.WindowsAzure.Storage for example:

```
<dependentAssembly>
  <assemblyIdentity name="Microsoft.WindowsAzure.Storage" publicKeyToken="31bf3856ad364e35" culture="neutral" />
  <bindingRedirect oldVersion="0.0.0.0-2.1.0.0" newVersion="2.1.0.0" />
</dependentAssembly>
```

* When deploying using a packaging technology, like Windows Azure Cloud Services projects, make sure that the ServiceControl plugins become part of the package before executing the deployement. For example, this can be done by referencing the assemblies in a worker role project and setting "copy local" to true.

**Please inform [ServicePulse Beta team](mailto:pulsebeta@nservicebus.com) of any issues**
