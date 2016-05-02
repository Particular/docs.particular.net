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

Transport, versions 6 and below, uses raw connection string provided into message headers. It could result in a leak of sensitive information, f.i. logging it.

In order to prevent it, transport, versions 7 and above, allows for creating a logical name for each connection string. The name is mapped to the physical connection string, and connection strings are always referred to by their logical name.   
In the event of an error or when logging only the logical name will be used avoiding sharing of sensitive information.

To enable this behavior, apply the following configuration:

snippet: enable_use_namespace_name_instead_of_connection_string

After enabling this behavior, if `ReplyTo` message header contains the value `queueName@connectionString`, it will be replaced by `queueName@namespaceName`.

Using `AddNamespace(string name, string connectionString)` configuration API is possible to register namespaces mapping logical names with connection strings, as shown below for `SingleNamespacePartitioning` strategy:

snippet: map_logical_name_to_connection_string 

For detailed explanation about all supported partitioning strategies and how to configure namespaces mapping for them see related [article](multiple-namespaces-support.md).

## How to upgrade endpoints

Transport guarantees backward compatibility between older transport versions (below to version 7) and newer versions (version 7 and above).  
In order to be able to enable `UseNamespaceNameInsteadOfConnectionString` follow the next steps:
- upgrade all endpoints to version 7 or above.
- switch on `UseNamespaceNameInsteadOfConnectionString` feature one endpoint at time. 
