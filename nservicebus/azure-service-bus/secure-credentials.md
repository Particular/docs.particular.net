---
title: Securing Credentials
summary: Configuring Azure ServiceBus transport to securing credentials.
component: ASB
tags:
- Cloud
- Azure
- Transports 
- Security
reviewed: 2016-04-26
related:
- nservicebus/upgrades/asb-6to7
---

include: asb-credential-warning

Versions 6 and below uses raw the connection string in message headers. For instance, a typical value for `ReplyTo` header could be:

`[queue name]@Endpoint=sb://[namespace name].servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[secrets here]`

This could potentially result in a leak of sensitive information, for example in [log files](/nservicebus/logging/) or in the [error queue](/nservicebus/errors/). In order to prevent this, Versions 7 and above includes the ability to map logical namespace name to namespace connection string. If a valid mapping between connection string and namespace name doesn't exist a [`KeyNotFoundException`](https://msdn.microsoft.com/en-us/library/system.collections.generic.keynotfoundexception.aspx) is thrown.

Mapping between a namespace name and the corresponding connection string is done as follows:

snippet: map_logical_name_to_connection_string

For detailed explanation about all supported partitioning strategies, and how to configure namespaces mapping, see [Multiple Namespaces Support](multiple-namespaces-support.md).
  
Using directly `ConnectionString(string connectionString)` configuration API, as shown below, transport adds a mapping between a namespace name `default` and the provided connection string.

snippet: map_default_logical_name_to_connection_string

To enable the same behavior also for outgoing messages, a new feature, provided in Version 7 and above, has to be enabled:

snippet: enable_use_namespace_name_instead_of_connection_string

Without enabling this behavior, transport converts back the value to `queueName@connectionString` before delivering the message, to ensure backward compatibility among endpoints of different versions. Enabling `UseNamespaceNameInsteadOfConnectionString` feature, transport ensures the same behavior, avoiding sharing of sensitive information, for all incoming and outgoing messages.