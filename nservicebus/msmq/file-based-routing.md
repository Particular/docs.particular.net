---
title: File-based instance mapping
reviewed: 2016-06-01
tags:
- routing
- route
related:
- nservicebus/messaging/message-owner
- nservicebus/messaging/routing
---

Before Version 6, NServiceBus used fixed [message ownership mappings](/nservicebus/messaging/message-owner.md) in the standard .NET configuration file. The advantage was that operations personnel can adjust the routing should the topology change. This is especially important in case of bus-like transports (e.g. MSMQ) where machine name is part of the routing information. Version 6 introduced [code-first API](/nservicebus/messaging/routing.md), which is a code first API however it still supports file-based routing. To avoid hard coding of machine names with the newly introduced routing API, MSMQ offers a file-based instance mapping file to resolve machine names dynamically at runtime.


## Configuration

The instance mapping file is used in addition to the [code-first API](/nservicebus/messaging/routing.md) which only specifies logical endpoints (without machine name) as message destinations. The mapping file can be used to map a logical endpoint to one or multiple physical endpoints (e.g. running on different machines or with different queue names). The instance mapping file can be adjusted at runtime, allowing redirecting messages to different machines or scaling out at runtime without restarting the endpoint.

When using MSMQ as the transport, NServiceBus will automatically look for an `instance-mapping.xml` file in `AppDomain.BaseDirectoy`. You can change the following default settings:


### RefreshInterval

Specifies the interval between route data refresh attempts.

Default: 30 seconds

snippet: Routing-FileBased-RefreshInterval


### FilePath

Specifies the path and file name of the instance mapping file. This can be a realtive or an absolute file path. Relative file paths are resolved from `AppDomain.BaseDirectoy`.

Default: `instance-mapping.xml`

snippet: Routing-FileBased-FilePath


## The file

The routing file is a simple XML file that has to be located either on local hard drive or a network drive. It is processed before the endpoint starts up and then re-processed in regular intervals so the changes in the document are reflected in the behavior of NServiceBus automatically (with slight delay caused by caching of routes). If the document is not present in its configured location when endpoint starts up, NServiceBus will not search again for the file during runtime. If it is deleted or corrupted while the endpoint is already running, NServiceBus will continue to run normally with last successfully read routes. 


### Examples of routing files

Following are examples of instance mapping configurations for the given sample routing:

snippet:Routing-FileBased-Config


#### Machine name

The mapping file contains a `Machine` attribute which needs to be used when the receiving endpoint runs on a different machine. If no machine attribute is specified, MSMQ assumes the receiving endpoint is located on the same machine.

snippet:Routing-FileBased-MSMQ


#### Instance discriminator

The instance discriminator attribute can be used if multiple uniquely addressable endpoints run on the same machine and messages should be sent to their unique input queue instead of the shared input queue.

snippet:Routing-FileBased-Broker