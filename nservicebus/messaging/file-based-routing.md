---
title: File-based message routing
summary: How to configure file-based message routing in NServiceBus
tags:
- routing
- route
- file
related:
- nservicebus/messaging/message-owner
- nservicebus/messaging/routing
---

Before version 6, NServiceBus used fixed [message ownership mappings](/nservicebus/messaging/message-owner.md) in the standard .NET configuration file. The advantage was that operations personnel can adjust the routing should the topology change. This is especially important in case of bus-like transports (e.g. MSMQ) where machine name is part of the routing information. Version 6 introduced [code-first API](/nservicebus/messaging/routing.md), which is a code first API however it still supports file-based routing.


## Configuration

To configure NServiceBus to use endpoint instance mapping from a file, use the following config

snippet:Routing-FileBased-Config

The type mapping is still specified in code via `RouteToEndpoint` calls. This is different from Version 5 approach where all the routing information was present inside the config file. In Version 6 only the mapping between endpoints and their instances is in the file. This separation supports responsibility division between developers and operations people as described [here](/nservicebus/messaging/routing.md) e.g. preventing accidental change of message-to-endpoint mappings.


## The file

The routing file is a simple XML file that has to be located either on local hard drive or a network drive. It is processed before the endpoint starts up and then re-processed in regular intervals so the changes in the document are reflected in the behavior of NServiceBus automatically (with slight delay caused by caching of routes). If the document is not present in its configured location when endpoint starts up, NServiceBus will refuse to run. If it is deleted when the endpoint is already running, it will continue to run normally (with last read routes). Here is the XSD document describing the format of the routing file

snippet:Routing-FileBased-Schema 


## Examples of routing files

Following are examples of routing files


### MSMQ

The MSMQ routing file contains additional attribute `Machine` which is necessary because MSMQ is a distributed technology with a node running on each endpoint's machine.

snippet:Routing-FileBased-MSMQ


### Broker transports 

The routing file for a broker transport does not need the `Machine` attribute but may contain other attributed specific to the transport technology. It also does not need to include non-scaled-out endpoints because NServiceBus assumes there is a single default instance of each endpoint (unless otherwise specified).

snippet:Routing-FileBased-Broker
