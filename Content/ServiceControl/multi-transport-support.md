---
title: Multi Transport Support
summary: How to configure ServiceControl to use non-MSMQ Transports
tags:
- ServiceControl
- Multi-Transport
- Configuration
---

### Configuring ServiceControl to use non-MSMQ Transports:

* Download this NuGet for the relevant transport including any dependencies and put the dlls in the ServiceControl bin folder: ("C:\Program Files (x86)\Particular Software\ServiceControl")
   * [RabbitMQ](https://www.nuget.org/packages/NServiceBus.RabbitMQ/)
   * [SQL Server](https://www.nuget.org/packages/NServiceBus.SqlServer/)
   * [Windows Azure Storage Queues](https://www.nuget.org/packages/NServiceBus.Azure/)
   * [Windows Azure ServiceBus](https://www.nuget.org/packages/NServiceBus.Azure/) 
    
* Stop the ServiceControl service (from an admin cmd line, run `net stop Particular.ServiceControl`)
used the default install location)
* Open ServiceControl.dll.config in notepad and locate:    

```   
<connectionStrings>
  <add name="NServiceBus/Transport" connectionString="type connection string here" />
</connectionStrings>
```
   
* Update the "type connection string here" text above with the correct connection string for your transport
* To use the new transport locate:    
   `<add key="ServiceControl/TransportType" value="Fully qualified type name of your transport here" />`
* Update the "Fully qualified type name of your transport here" text above with the type for your transport. Eg for RabbitMQ it should be `NServiceBus.RabbitMQ, NServiceBus.Transports.RabbitMQ`
* Save ServiceControl.dll.config
* The necessary queues for ServiceControl in the desired transport need to be created. This can be done by uninstalling and re-installing the ServiceControl service.
   * To uninstall the ServiceControl service, from a command line with administrator privileges, run the following command: `NServiceBus.Host.exe -uninstall -serviceName="Particular.ServiceControl"`
   * Re-Install the ServiceControl service by running: 

      `NServiceBus.Host.exe -install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl"` 
   * Start the ServiceControl service by running: `"net start Particular.ServiceControl"`
   * Ensure Particular.ServiceControl windows service has started and is functioning properly (try to access the main HTTP API URI exposed by ServiceControl, e.g. do an HTTP GET on http://localhost:33333/api
* NOTE: When deploying using a packaging technology, like Windows Azure Cloud Services projects, make sure that the ServiceControl plugins become part of the package before executing the deployement. For example, this can be done by referencing the assemblies in a worker role project and setting "copy local" to true.

**Please inform [ServicePulse Beta team](mailto:pulsebeta@nservicebus.com) of any issues**
