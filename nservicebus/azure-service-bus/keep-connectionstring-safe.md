---
title: Keep connection string safe
summary: Configuring Azure ServiceBus transport to secure connection strings. 
component: ASB
tags:
- Cloud
- Azure
- Transports 
- Security
reviewed: 2016-04-26
---

Transport, versions 6 and below, uses raw connection string provided into message headers.  
For instance, a typical value for `ReplyTo` header could be:  

`[queue name]@Endpoint=sb://[namespace name].servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[secrets here]`
  
It could result in a leak of sensitive information, f.i. logging it.  
In order to prevent it, transport, versions 7 and above, introduces the needs to map logical namespace name to namespace connection string. This configuration allows transport to convert connection string value to namespace name, if mapping exists, for each incoming message, using the logical name internally.
If a valid mapping between connection string and namespace name doesn't exist, transport throws a `KeyNotFoundException`.

To configure mapping between a namespace name and the corresponding connection string is possible through `AddNamespace(string name, string connectionString)` configuration API, as shown below for `SingleNamespacePartitioning` strategy:

snippet: map_logical_name_to_connection_string

For detailed explanation about all supported partitioning strategies and how to configure namespaces mapping for them see related [article](multiple-namespaces-support.md).  
Using directly `ConnectionString(string connectionString)` configuration API, as shown below, transport adds a mapping between a namespace name `default` and the provided connection string.

snippet: map_default_logical_name_to_connection_string 

In the event of an error or when logging only, the logical name will be used avoiding sharing of sensitive information.

To enable the same behavior also for outgoing messages, a new feature, provided by transport version 7 and above, has to be switched on:

snippet: enable_use_namespace_name_instead_of_connection_string

Without enabling this behavior, transport converts back the value to `queueName@connectionString` before delivering the message, to ensure backward compatibility among endpoints of different versions.  
Enabling `UseNamespaceNameInsteadOfConnectionString` feature, transport ensures the same behavior, avoiding sharing of sensitive information, for all incoming and outgoing messages.

For explanation about how to upgrade endpoints to enable this feature, see [upgrade guide article](/nservicebus/upgrades/asb-6to7.md)
