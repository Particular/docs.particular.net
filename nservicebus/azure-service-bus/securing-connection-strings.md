---
title: Securing Connection Strings
summary: 'Azure Service Bus Transport: Securing connection strings.'
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

Versions 6 and below use a raw connection string in message headers. For instance, `ReplyTo` header value was of the following structure:

`[queue name]@Endpoint=sb://[namespace name].servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[key]`

This could potentially result in a leak of connection strings, for example in [log files](/nservicebus/logging/). To prevent this, Versions 7 and above can map logical namespace name to namespace connection string. If a valid mapping between a namespace name and connection string doesn't exist a [`KeyNotFoundException`](https://msdn.microsoft.com/en-us/library/system.collections.generic.keynotfoundexception.aspx) is thrown.

Mapping between a namespace name and the corresponding connection string is done as follows:

snippet: map_logical_name_to_connection_string

For detailed explanation about all supported partitioning strategies, and how to configure namespaces mapping, see [Multiple Namespaces Support](multiple-namespaces-support.md).
  
Using directly `ConnectionString(string connectionString)` configuration API, as shown below, will cause transport to add a mapping between a namespace name `default` and the provided connection string.

snippet: map_default_logical_name_to_connection_string

To enable the same behavior for outgoing messages as well, use `UseNamespaceNamesInsteadOfConnectionStrings()` configuration API:

snippet: enable_use_namespace_name_instead_of_connection_string

Without enabling this behavior, transport converts back the value to `queueName@connectionString` before delivering the message, to ensure backward compatibility among endpoints of different versions. By calling `UseNamespaceNameInsteadOfConnectionString()` configuration API, transport ensures the same behavior, avoiding sharing of connections strings, for all incoming and outgoing messages.
