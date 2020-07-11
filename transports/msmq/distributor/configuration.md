---
title: Configuring Distributor and Workers
summary: Describes how to configure the distributor and its workers.
component: distributor
reviewed: 2020-07-11
redirects:
 - nservicebus/msmq/distributor/configuration
---


partial: versions

Warning: It is not recommended to use Outbox on the workers. Outbox is designed to work with distributed transactions (MSDTC).

## Distributor configuration

The distributor requires an additional queue where workers can send their status updates. This is the **control** queue. The default address is `endpoint_name.distributor.control`.

```
EndpointName.distributor.control
```

The distributor stores metadata about the worker availability in the the queue with the suffix `.distributor.storage`.

NOTE: It is valid for messages to be in the queue when the system is idle. The number of the messages should be the sum of the capacity of all workers. If each worker had a maximum concurrency level of 8 and there are 4 workers then there will be 32 message in the queue.


### When hosting endpoints in NServiceBus.Host.exe

When running with [NServiceBus.Host.exe](/nservicebus/hosting/), the following profiles start the endpoint with the distributor functionality:

partial: distributor

partial: master

### When self-hosting

When [self-hosting](/nservicebus/hosting/) the endpoint, use this configuration:

snippet: ConfiguringDistributor

## Worker configuration

Any NServiceBus endpoint can run as a worker node. To activate it, create a handler for the relevant messages and ensure that the `app.config` file contains routing information for the distributor.


### When hosting in NServiceBus.Host.exe

If hosting the endpoint with `NServiceBus.Host.exe`, to run as a worker, use this command line:

partial: worker

Configure the name of the master node server as shown in this `app.config` example. Note the `MasterNodeConfig` section:

```xml
<configuration>
  <configSections>
    <!-- Other sections go here -->
    <section name="MasterNodeConfig"
             type="NServiceBus.Config.MasterNodeConfig, NServiceBus.Core" />
  </configSections>
  <!-- Other config options go here -->
  <MasterNodeConfig Node="MachineWhereDistributorRuns"/>
</configuration>
```

For more information, see `DistributorControlAddress` and `DistributorDataAddress` in the [Override distributor queues (advanced)](#advanced-override-distributor-queues) section.


### When self-hosting

When self-hosting the endpoint, the following code will enlist the endpoint with a distributor.

snippet: ConfiguringWorker

Similar to self-hosting, when running NServiceBus prior to version 6, ensure the `app.config` of the worker contains the `MasterNodeConfig` section to point to the hostname where the distributor process is running.

## Outbox

Enabling Outbox can result in increasing the enlisted capacity of a worker when errors occur. The distributor model is originally designed to work with distributed transactions (MSDTC). Consider using [Sender Side Distribution](/transports/msmq/sender-side-distribution.md) instead. When enabling Outbox on a worker or master, explicitly set transaction mode to "Sends atomic with receive" as the Outbox sets the default transaction mode to "Receive Only".

```c#
var transport = endpointConfiguration.UseTransport<MsmqTransport>();
transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
```

Outbox is not required on the distributor; the forwarding of messages to workers is purely a transport operation and outbox will not participate in the forwarding logic.

## Advanced

### Override distributor queues

The distributor process uses two queues for its runtime operation. The `DataInputQueue` is the queue where the client processes send their messages. The `ControlInputQueue` is the queue where the worker nodes send their control messages.


The NServiceBus default values for these settings can be overridden, as shown in the `UnicastBusConfig` section below:

```xml
<UnicastBusConfig DistributorControlAddress="EndpointName.Distributor.Control@MachineWhereDistributorRuns"
                  DistributorDataAddress="EndpointName@MachineWhereDistributorRuns">
  <MessageEndpointMappings>
    <!-- regular entries -->
  </MessageEndpointMappings>
</UnicastBusConfig>
```


### Prioritizing on message type

Similar to standard NServiceBus routing, it is not desirable to have high priority messages to get stuck behind lower priority messages, so just as it is possible to have separate NServiceBus processes for different message types, it is also possible to set up different distributor process instances (with separate queues) for various message types.

In this case, name the queues after the message type. For example: `SubmitPurchaseOrder.StrategicCustomers.Sales` becomes the name of the distributor data queue and the input queues of each of the workers.
