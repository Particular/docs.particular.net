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

include: intro-keep-connection-string-safe

To enable this behavior, apply the following configuration:

snippet: enable_use_namespace_name_instead_of_connection_string

Using `AddNamespace(string name, string connectionString)` configuration API is possible to register namespaces mapping logical names with connection strings, as shown below for `SingleNamespacePartitioning` strategy:

snippet: map_logical_name_to_connection_string 

For detailed explanation about all supported partitioning strategies and how to configure namespaces mapping for them see related [article](multiple-namespaces-support.md)
