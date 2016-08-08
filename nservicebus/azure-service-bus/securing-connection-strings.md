---
title: Securing Connection Strings To Namespaces
summary: 'Azure Service Bus Transport: Securing connection strings to namespaces.'
tags:
- Cloud
- Azure
- Transports
- Security
reviewed: 2016-04-26
related:
- nservicebus/upgrades/asb-6to7
redirects:
- nservicebus/azure-service-bus/secure-credentials
---


## Namespace Names

Versions 6 and below allows routing of messages across different namespaces by adding connection string information behind the `@` sign in any address notation. As address information is included in messages headers, the headers include both the queue name as well as the connection string. For instance, the `ReplyTo` header value has of the following structure:

```no-highlight
[queue name]@Endpoint=sb://[namespace name].servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[key]
```

In certain scenarios this could lead to insecure behavior and result in a leak of such connection strings, for example when messages are exchanged with untrusted parties or when body content is added to [log files](/nservicebus/logging/) which are then shared.

To prevent this kind of accidental leaking, Versions 7 and above can map a logical namespace name to a namespace connection string. By default the connection string is still being passed around, but that behavior can be changed to using the `UseNamespaceNamesInsteadOfConnectionStrings()` API Setting.

snippet: enable_use_namespace_name_instead_of_connection_string

If this setting is enabled, the resulting `ReplyTo` header will look like this:

```no-highlight
[queue name]@[namespacename]
```


## Configuration

Mapping between a namespace name and the corresponding connection string, to perform cross namespace routing, is done as follows:

snippet: namespace_routing_registration

Note: All endpoints in a system need to be configured with the same namespace names and connection string information.

For a detailed explanation about all ways to configure namespace mappings for namespace routing and namespace partitioning, see [Multiple Namespaces Support](multiple-namespaces-support.md).


## Backward compatibility

include: asb-credential-warning

Internally the transport (Version 7) uses the namespace name to refer to namespaces. Even when using the `ConnectionString(string connectionString)` method on the configuration API directly, as shown below, it will cause the transport to add a mapping between a namespace name `default` and the provided connection string internally.

snippet: map_default_logical_name_to_connection_string

Without enabling the `UseNamespaceNamesInsteadOfConnectionStrings()` behavior, the transport will ensure that all outbound headers are converted to the `queueName@connectionString` format before delivering the message. This ensures backward compatibility among endpoints of different versions.

By calling `UseNamespaceNameInsteadOfConnectionString()` the transport will change it's behavior, and send the namespace name instead.

Any incoming message can have headers in either format, the transport will automatically convert connection strings on the wire to namespace names for internal use.