---
title: Multi Transport Support
summary: How to configure ServiceControl to use non-MSMQ Transports
tags:
- ServiceControl
- Transports
- Configuration
- RabbitMQ
- SQL Server
- Windows Azure
- Cloud
---

### Configuring ServiceControl to use non-MSMQ Transports:

* First, download the NuGet package for the relevant transport including any dependencies.
   * *RabbitMQ*: [NServiceBus.RabbitMQ](https://www.nuget.org/api/v2/package/NServiceBus.RabbitMQ) and [RabbitMQ.Client](https://www.nuget.org/api/v2/package/RabbitMQ.Client/3.2.1)
   * *SQL Server*: [NServiceBus.SqlServer](https://www.nuget.org/api/v2/package/NServiceBus.SqlServer)
   * *Windows Azure Storage Queues*: [NServiceBus.Azure.Transports.WindowsAzureStorageQueues](https://www.nuget.org/api/v2/package/NServiceBus.Azure.Transports.WindowsAzureStorageQueues/5.1.1) and [WindowsAzure.Storage](https://www.nuget.org/api/v2/package/WindowsAzure.Storage/2.1.0)
   * *Windows Azure ServiceBus*: [NServiceBus.Azure.Transports.WindowsAzureServiceBus](https://www.nuget.org/api/v2/package/NServiceBus.Azure.Transports.WindowsAzureServiceBus/5.1.1) and [WindowsAzure.ServiceBus](https://www.nuget.org/api/v2/package/WindowsAzure.ServiceBus/2.2.0)

* The NuGet packages you just downloaded are in fact zip files. Rename the nupkg files to have a zip extension, and take the dlls from the `/lib` folder and put them in the ServiceControl bin folder: (`"[Program Files]\Particular Software\ServiceControl"`).
<p class="alert alert-warning">
<strong>NOTE</strong><br/>
Some nuget packages may have several folders under `/lib` - make sure to take the dlls from only one of them, preferrably the one targetting the latest .NET framework (e.g. `/lib/net40`).
</p>
* Uninstall the ServiceControl service
  ```bat
  x:\Your_Installed_Path\ServiceControl.exe --uninstall
  ```

* Install ServiceControl with the following cmd line arguments:
  ```bat
  x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==<.net type>" -d="NServiceBus/Transport==<connectionstring>"
  ```

  For example, for RabbitMQ these settings would look like:
  ```bat
  x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==NServiceBus.RabbitMQ, NServiceBus.Transports.RabbitMQ" -d="NServiceBus/Transport==host=localhost"
  ```
  
* Start the ServiceControl service by running: `"net start Particular.ServiceControl"`
* Ensure Particular.ServiceControl windows service has started and is functioning properly (try to access the main HTTP API URI exposed by ServiceControl, e.g. do an HTTP GET on `http://localhost:33333/api`

#### Troubleshooting

* Make sure all assemblies copied are unblocked, otherwise the .NET runtime will refuse to load them.
* When deploying using a packaging technology, like Windows Azure Cloud Services projects, make sure that the ServiceControl plugins become part of the package before executing the deployement. For example, this can be done by referencing the assemblies in a worker role project and setting "copy local" to true.

**Please inform [ServicePulse Beta team](mailto:pulsebeta@nservicebus.com) of any issues**
