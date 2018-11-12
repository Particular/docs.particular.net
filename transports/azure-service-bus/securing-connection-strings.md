---
title: Securing Connection Strings To Namespaces
component: ASB
versions: '[7,)'
tags:
- Azure
- Transport
- Security
reviewed: 2017-05-05
related:
 - nservicebus/operations
 - transports/upgrades/asb-6to7
redirects:
 - nservicebus/azure-service-bus/secure-credentials
 - nservicebus/azure-service-bus/securing-connection-strings
---

include: legacy-asb-warning


## Namespace Aliases

Versions 6 and below allows routing of messages across different namespaces by adding connection string information behind the `@` sign in any address notation. As address information is included in messages headers, the headers include both the queue name as well as the connection string. For instance, the `ReplyTo` header value has of the following structure:

```
[queue name]@Endpoint=sb://[namespace name].servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[key]
```

In certain scenarios this could lead to insecure behavior and result in a leak of such connection strings, for example when messages are exchanged with untrusted parties (native outgoing integration, messages export, etc) or when body content is added to [log files](/nservicebus/logging/) which are then shared.

To prevent this kind of accidental leaking, Versions 7 and above can map an alias to a namespace connection string. By default the connection strings are still passed around. To override the default behavior use the `UseNamespaceNamesInsteadOfConnectionStrings()` configuration API setting.

snippet: enable_use_namespace_alias_instead_of_connection_string

When this setting is enabled, `ReplyTo` header will no longer contain raw connection string and will be structured as following

```
[queue name]@[alias]
```

NOTE: Using namespace aliases is currently NOT compatible with ServiceControl. A [ServiceControl transport adapter](/servicecontrol/transport-adapter/) is required in order to leverage both.

## Configuration

To perform a cross-namespace routing, connection string has to be mapped to a corresponding alias. For example:

snippet: namespace_routing_registration

Note: All endpoints in a system need to be configured with the same namespace aliases and connection string information.

For a detailed explanation on configuring namespace mappings for namespace routing and partitioning, see [Multiple Namespaces Support](multiple-namespaces-support.md).


## Backward compatibility

include: asb-credential-warning

Internally the transport (Versions 7 and above) uses namespace alias to refer to namespaces. Even when using the `ConnectionString(string connectionString)` method on the configuration API directly, as shown below, it will cause the transport to add a mapping between a namespace alias `default` and the provided connection string internally.

snippet: map_default_logical_alias_to_connection_string

Without enabling the `UseNamespaceAliasInsteadOfConnectionStrings()` behavior, the transport will ensure that all outbound headers are converted to the `queueName@connectionString` format before delivering messages. This ensures backward compatibility among endpoints of different versions.

By calling `UseNamespaceAliasNameInsteadOfConnectionString()` the transport will change its behavior. Instead of embedding connection strings in headers, namespace aliases will be used instead.

Any incoming message can have headers in either format, the transport will automatically convert connection strings on the wire to namespace alias for internal use.
