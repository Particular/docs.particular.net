---
title: Customizing ServiceControl Configuration
summary: How ServiceControl manages configuration and how to create and customize the ServiceControl configuration file.
tags:
- ServiceControl
- Configuration
---

ServiceControl allows you to create a configuration file with settings that override default settings.

When you first install ServiceControl, it is set to automatically start (as a Windows Service) using its internal default settings. Examples: the default `localhost` hostname and `33333` port number, and the embedded database location.

To override these default settings:

1. Stop the ServiceControl service.
1. Locate/create a configuration file named `ServiceControl.exe.config` in the ServiceControl installation folder.
1. Edit the configuration file and add the relevant settings to the `<appSettings>` section.
1. Start the ServiceControl service.
 
### Sample configuration file
  
* File name: `servicecontrol.exe.config`
* File path: `C:\Program Files (x86)\Particular Software\ServiceControl` (default ServiceControl installation folder)
 
 
```xml
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<configuration>
  <appSettings>
      <add key="ServiceControl/Hostname" value="my.domain-name.com" />
      <add key="ServiceControl/Port" value="33333" />
      <add key="ServiceControl/DbPath" value="x:\mydb" />
      ...
  </appSettings>
</configuration>
```

### Configuration Considerations

#### Using custom domain names and TCP port number

- See [Setting a Custom Hostname](setting-custom-hostname.md) for guidance and details.
- See [Securing ServiceControl](securing-servicecontrol.md) for a discussion on security implications of custom domain access and access limitation options.

#### Embedded database location and size

ServiceControl uses RavenDB as an embedded database. The database location has significant effect both on ServiceControl throughput (number of read/write operations it can perform) and on the storage capacity (the limiting factor being the available storage on the drive on which the embedded database is located. For more information, see '[Customize RavenDB Embedded Path and Drive](configure-ravendb-location.md)'.

#### Automatic expiration of messages 

ServiceControl consumes messages from the Audit and Error queues and stores these messages temporarily (by default, for 30 days) in its embedded database. You can set the message storage timespan by [setting automatic expiration for ServiceControl data](how-purge-expired-data.md).

#### Consuming messages from Audit & Error queues

ServiceControl consumes messages from the audit and error queues, and stores these messages locally in its own embedded database.
You can configure ServiceControl to forward any consumed messages into alternate queues, so that a copy of any message consumed by ServiceControl is available from these alternate queues. To do so, set the `ServiceBus/ErrorLogQueue` and `ServiceBus/AuditLogQueue` settings.

### Configuration Options

* `ServiceControl/LogPath` (string)

  The path for the ServiceControl logs. Default: `%LOCALAPPDATA%\Particular\ServiceControl\logs`

* `ServiceControl/Port` (int)

  The port to bind the embedded http server. Default: `33333`.

* `ServiceControl/Hostname` (string)

  The hostname to bind the embedded http server to, modify if you want to bind to a specific hostname, eg. sc.mydomain.com. Default: `localhost`

* `ServiceControl/VirtualDirectory` (string)

  The virtual directory to bind the embedded http server to, modify if you want to bind to a specific virtual directiory. Default: `empty`

* `ServiceControl/HeartbeatGracePeriod` (timespan)

  The period that defines whether an endpoint is considered alive or not. Default: `00:00:40` (40 secs)

* `ServiceControl/MaximumMessageThroughputPerSecond`
   This setting was introduced in version 1.5. The setting controls the maximum throughput of messages ServiceControl will handle per second and is necessary to avoid overloading the underlying message database. An apropriate limit ensures that the database can cope with number of insert operations. Otherwise the query performance would drop significantly and the message expiration process would stop working when under heavy insert load. Make sure to concudct thorough performance tests on your hardware before increasing this value.  Default: `350`. 
   
* `ServiceControl/ForwardAuditMessages` (bool `true`/`false`)

  Use this setting to configure whether processed audit messages are forwarded to another queue or not. Default `false`.  From v1.5 if this setting is not explicitly set to true of false a warning is shown in the logs at startup.
  See [Installation](installation.md) for details on how to set this at install time.

* `ServiceControl/ExpirationProcessTimerInSeconds` (int) 

  The number of seconds to wait between checking for expired messages.  The default prior to version 1.4 is `60` (1 minute), the new default is `600` (10 minutes).  Settings the value to 0 will disable the expiration process, this is not recommended and it is only provided for fault finding.

* `ServiceControl/ExpirationProcessBatchSize` (int) , Default `65512`  
 
  This setting was introduced in version 1.4. This minimum allowed value for this settings is 10240, there is no hardcoded maximum as this is heavily dependent on system performance.  

* `ServiceControl/HoursToKeepMessagesBeforeExpiring` (int)

  The number of hours to keep a message for before it is deleted, Default `720` (30 days)

* `ServiceControl/DbPath` (string)

  The path where the internal RavenDB is located. Default `%SystemDrive%\ProgramData\Particular\ServiceControl\`

* `ServiceControl/TransportType` (string .Net type)

  The transport type to run ServiceControl with. Default `NServiceBus.Msmq, NServiceBus.Core`

* `NServiceBus/Transport` (string)

  The connection string for the transport.

* `ServiceBus/AuditQueue` (string)

  The audit queue name. Default `audit`

* `ServiceBus/ErrorQueue` (string)

  The error queue name. Default `error`

* `ServiceBus/ErrorLogQueue` (string)

  The error queue name to use for forwarding error messages. Default `<ErrorQueue>.log`

* `ServiceBus/AuditLogQueue` (string)

  The audit queue name to use for forwarding audit messages. This only works if `ServiceControl/ForwardAuditMessages` is true. Default `<AuditQueue>.log`
