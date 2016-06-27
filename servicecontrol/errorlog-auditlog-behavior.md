---
title: ServiceControl Forwarding Queues
summary: Details the ServiceControl Audit and Error forwarding behavior and configuration
tags:
- ServiceControl
---

### Audit and Error queues

ServiceControl consumes messages from the audit and error queues and stores these messages locally in its own embedded database. These input queues name can be customized via `ServiceBus/ErrorQueue` and `ServiceBus/AuditQueue` settings.

ServiceControl can also forward these messages to two forwarding queues.

* Error messages are optionally forwarded to the Error forwarding queue.
* Audit messages are optionally forwarded to the Audit forwarding queue. 

This behavior can be toggled through the ServiceControl Management utility or by directly changing the  `ServiceControl/ForwardAuditMessages` or `ServiceControl/ForwardErrorMessages` settings. See also [Customizing ServiceControl Configuration](creating-config-file.md#transport).


### Error and Audit Forwarding Queues 

The forwarding queues retain a copy of the original messages ingested by ServiceControl.

The queues are not directly managed by ServiceControl and are meant as points of external integration.

Note: If external integration is not required, it is highly recommend turning forwarding queues off.


### Changing the queues name manually and default naming

Changing the input queue names via the configuration file without considering the forwarding queues can cause issues. For example, in the configuration below `ServiceBus/ErrorQueue` has been set to `CustomErrorQueue`. This will cause ServiceControl to expect a queue named `CustomErrorQueue.log` to exist as well. If the corresponding forwarding queue does not exist then the ServiceControl service will not start.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <appSettings>
       <add key="ServiceBus/ErrorQueue" value="CustomErrorQueue" />
       <!-- "ServiceBus/ErrorLogQueue" will resolve to `CustomErrorQueue.log as it's not set`  -->
    </appSettings>
</configuration>
```

To avoid this confusion it is recommended the names of the output queues be explicitly configured using the `ServiceBus/ErrorLogQueue` and `ServiceBus/AuditLogQueue` settings. The recommended way to change the input and forwarding queues names is to use the command line options as detailed below. In this example all four queue names are being explicitly set and if any of the queues do not exist they will be created.

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <appSettings>
        <add key="ServiceBus/AuditQueue" value="audit" />
        <add key="ServiceBus/ErrorQueue" value="error" />
        <add key="ServiceBus/ErrorLogQueue" value="error.log" />
        <add key="ServiceBus/AuditLogQueue" value="audit.log" />   
        <add key="ServiceControl/ForwardAuditMessages" value="False" />
        <add key="ServiceControl/ForwardErrorMessages" value="False" />
    </appSettings>
</configuration>
```

NOTE: The app settings related to queue names are prefixed with "ServiceBus" not "ServiceControl".


### Test Error and Audit Forwarding Queues on startup

To confirm the availability of the forwarding queues an empty message is sent to these queues at when the service starts.