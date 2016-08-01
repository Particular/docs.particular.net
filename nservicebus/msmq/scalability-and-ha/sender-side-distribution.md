---
title: Scaling out with sender-side distribution
summary: How to scale out with sender-side distribution when using the MSMQ transport.
tags:
- Scale Out
- Routing
- MSMQ
redirects:
- nservicebus/messaging/file-based-routing
related:
- nservicebus/messaging/message-owner
- nservicebus/messaging/routing
---

NServiceBus endpoints using the MSMQ transport are unable to use the competing consumers pattern to scale out by adding additional worker instances. Sender-side distribution is a method of scaling out an NServiceBus endpoint using the MSMQ transport, without relying on a centralized [distributor](distributor/) assigning messages to available workers.

NOTE: Sender-side distribution is available in NServiceBus Versions 6 and above.

When using sender-side distribution:

* Multiple endpoint instances (deployed to different servers) are capable of processing a message that requires scaled-out processing.
* A client sending a message is aware of all the endpoint instances that can process the message.
* The client sends the message to a worker endpoint instance based on round-robin distribution, or a custom distribution strategy.

Using sender-side distribution requires two parts. The first part maps message types to logical endpoints, and occurs in code. The second part maps logical endpoints to physical endpoint instances running on a specific machine.


## Mapping logical endpoints

To map message types to logical endpoints, use the following config:

snippet:Routing-FileBased-Config

This creates mappings specifying that the `AcceptOrder` command is handled by the **Sales** endpoint, while the `SendOrder` command is handled by the **Shipping** endpoint.

Meanwhile, the logical-to-physical mappings will be configured in the Routes.xml file, as this information is an operational concern that must be changed for deployment to multiple machines.

WARNING: If a message is mapped in an App.config file via the `UnicastBusConfig/MessageEndpointMappings` configuration section, then that message cannot participate in sender-side distribution. The endpoint address specified by a message endpoint mapping is a physical address (`QueueName@MachineName`, where machine name is assumed to be `localhost` if omitted) which combines the message-to-owner-endpoint and endpoint-to-physical-address concerns in a way that can't be separated.


## Mapping physical endpoint instances

The routing configuration file specifies how logical endpoint names are mapped to physical queues on specific machines:

snippet:Routing-FileBased-MSMQ

By default, a round-robin distribution strategy is used to distribute messages between the available endpoint instances.


## Mapping file

The routing file is a simple XML file that has to be located either on a local hard drive or a network drive. When using MSMQ as the transport, NServiceBus will automatically look for an `instance-mapping.xml` file in `AppDomain.BaseDirectoy`.

The mapping file is processed before the endpoint starts up and then re-processed at regular intervals so the changes in the document are reflected in the behavior of NServiceBus automatically. If the document is not present in its configured location when endpoint starts up, NServiceBus will refuse to run. If it is deleted when the endpoint is already running, it will continue to run normally (with the last successfully read routes).

There are many different options to consider when deploying routing configuration.

* Many endpoints can be configured to use one centralized mapping file on a network drive accessible by all, creating a single file that describes how messages flow across an entire system. Any given endpoint will not care if the file contains information for endpoints it does not need.
* The mapping file can be kept in a centralized location and replicated out to many servers on demand, allowing each endpoint to read the file from a location on the local disk.
* Each endpoint can keep its own routing file containing only information for the endpoints it needs to know about, which can be deployed in the same directory as the endpoint binaries and only modified as part of a controlled deployment process.

You can change the following default settings:
 
 
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


## Decommissioning endpoint instances

When using sender-side distribution, message senders have no knowledge of the status of any of the worker instances. They simply send messages to one of the configured instances in a round-robin fashion, whether that instance is still running or not.

Therefore, when scaling down (removing a "target" endpoint instance from service), it is important to properly decommission the instance:

1. Change the routing file to remove the target endpoint instance.
1. Ensure that the updated routing information is distributed to all endpoint instances that might send a message to the target endpoint.
1. Allow time (30 seconds by default) for all endpoints to reread the mapping file, and ensure no new messages are arriving in the target instance's queue.
1. Allow the target endpoint instance to complete processing all messages in its queue.
1. Disable the target endpoint instance.
1. Check the input queue of the decommissioned instance for leftover messages and move them to other instances if necessary.
