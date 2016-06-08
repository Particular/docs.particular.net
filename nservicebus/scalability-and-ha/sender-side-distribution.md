---
title: Scaling out with sender-side distribution
summary: 
tags:
- Scale Out
- Routing
- MSMQ
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

**TODO: Consider splitting the snippet above into 2 snippets, splitting endpoint mappings from file-based instance mapping.**

This creates mappings specifying that the `AcceptOrder` command is handled by the **Sales** endpoint, while the `SendOrder` command is handled by the **Shipping** endpoint.

Meanwhile, the logical-to-physical mappings will be configured in the Routes.xml file, as this information is an operational concern that must be changed for deployment to multiple machines.

WARNING: If a message is mapped in an App.config file via the `UnicastBusConfig/MessageEndpointMappings` configuration section, then that message cannot participate in sender-side distribution. The endpoint address specified by a message endpoint mapping is a physical address (`QueueName@MachineName`, where machine name is assumed to be `localhost` if omitted) which combines the message-to-owner-endpoint and endpoint-to-physical-address concerns in a way that can't be separated.


## Mapping physical endpoint instances

The routing configuration file specifies how logical endpoint names are mapped to physical queues on specific machines:

snippet:Routing-FileBased-MSMQ

By default, a round-robin distribution strategy is used to distribute messages between the available endpoint instances.


## Location of mapping file

The routing file is a simple XML file that has to be located either on local hard drive or a network drive. It is processed before the endpoint starts up and then re-processed in regular intervals (every 30 seconds by default) so the changes in the document are reflected in the behavior of NServiceBus automatically. If the document is not present in its configured location when endpoint starts up, NServiceBus will refuse to run. If it is deleted when the endpoint is already running, it will continue to run normally (with the last successfully read routes).

There are many different options to consider when deploying routing configuration.

Many endpoints can be configured to use one centralized mapping file on a network drive accessible by all, creating a single file that describes how messages flow across an entire system. Any given endpoint will not care if the file contains information for endpoints it does not need.

The mapping file can also be kept in a centralized location and replicated out to many servers on demand, allowing each endpoint to read the file from a location on the local disk.

Or, rather than having a centralized file, each endpoint can keep its own routing file containing only information for the endpoints it needs to know about, which can be deployed in the same directory as the endpoint binaries and only modified as part of a controlled deployment process.


## Decommissioning endpoint instances

When using sender-side distribution, message senders have no knowledge of the status of any of the worker instances. They simply send messages to one of the configured instances in a round-robin fashion, whether that instance is still running or not.

Therefore, when scaling down (removing a "target" endpoint instance from service), it is important to properly decommission the instance:

1. Change the routing file to remove the target endpoint instance.
2. Ensure that the updated routing information is distributed to all endpoint instances that might send a message to the target endpoint.
3. Allow time (30 seconds by default) for all endpoints to reread the mapping file, and ensure no new messages are arriving in the target instance's queue.
4. Allow the target endpoint instance to complete processing all messages in its queue.
5. Disable the target endpoint instance.
6. Ensure that no new messages arrive before removing the target instance completely.
