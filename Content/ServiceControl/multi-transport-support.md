--
title: Multi Transport Support
subject: How to configure ServiceControl to use non-MSMQ Transports
originalUrl: https://github.com/Particular/ServiceControl/wiki/Multi-Transport-Support
tags:
- ServiceControl
- Multi-Transport
- Configuration
---

### Configuring ServiceControl to use non-MSMQ Transports:

1. Download this NuGet for the relevant transport including any dependencies and put the dlls in the ServiceControl bin folder: (```C:\Program Files (x86)\Particular Software\ServiceControl```)
   * [RabbitMQ](https://www.nuget.org/packages/NServiceBus.RabbitMQ/)
   * [ActiveMQ](https://www.nuget.org/packages/NServiceBus.ActiveMQ/)
   * [SQL Server](https://www.nuget.org/packages/NServiceBus.SqlServer/)
   * [Windows Azure Storage Queues](https://www.nuget.org/packages/NServiceBus.Azure/)
   * [Windows Azure ServiceBus](https://www.nuget.org/packages/NServiceBus.Azure/)
* Stop the ServiceControl service (from an admin cmd line, run ```net stop Particular.ServiceControl```)
used the default install location)
* Open ServiceControl.dll.config in notepad and locate:    
   ```
   <connectionStrings>
       <add name="NServiceBus/Transport" connectionString="type connection string here" />
   </connectionStrings>
   ```
* Update the ```"type connection string here"``` text above with the correct connection string for your transport
* To use the new transport locate:    
   ```
    <add key="ServiceControl/TransportType" value="Fully qualified type name of your transport here" />
   ```
* Update the ```"Fully qualified type name of your transport here"``` text above with the type for your transport. Eg for RabbitMQ it should be ```NServiceBus.RabbitMQ, NServiceBus.Transports.RabbitMQ```
* Save ServiceControl.dll.config
* The necessary queues for ServiceControl in the desired transport need to be created. This can be done by uninstalling and re-installing the ServiceControl service.
   * To uninstall the ServiceControl service, from a command line with administrator privileges, run the following command: 
      ```
      NServiceBus.Host.exe -uninstall -serviceName="Particular.ServiceControl"
      ```
   * Re-Install the ServiceControl service by running: 

      ```
      NServiceBus.Host.exe -install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -description="Particular Software ServiceControl for NServiceBus (version 1.0.0)"
      ``` 

   * Start the ServiceControl service by running: ```"net start Particular.ServiceControl"```
   * Ensure Particular.ServiceControl windows service has started and is functioning properly (try to access the main HTTP API URI exposed by ServiceControl, e.g. do an HTTP GET on [http://localhost:33333/api](http://localhost:33333/api)

**Inform [ServicePulse Beta team](mailto:pulsebeta@nservicebus.com) of any issues**
