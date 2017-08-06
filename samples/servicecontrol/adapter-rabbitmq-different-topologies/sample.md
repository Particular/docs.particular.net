---
title: Monitor RabbitMQ direct routing topology with ServiceControl adapter
summary: Configure RabbitMQ direct routing topology monitoring with the ServiceControl adapter
component: SCTransportAdapter
reviewed: 2017-08-02
related:
 - servicecontrol
 - servicecontrol/transport-adapter
 - servicecontrol/plugins
---


This sample shows how to configure ServiceControl to monitor endpoints and retry messages when using the RabbitMQ direct routing topology, the default topology of endpoints using RabbitMQ on NServiceBus Version 6. However, since ServiceControl is built on NServiceBus Version 5 and can only use the conventional routing topology, a kind of "transport adapter" is required to bridge the routing topologies. 


## Prerequisistes

 1. [Install ServiceControl](/servicecontrol/installation.md).
 2. Install RabbitMQ broker on localhost.
 3. Using [ServiceControl Management](/servicecontrol/license.md#servicecontrol-management-app) tool, set up ServiceControl to monitor endpoints using RabbitMQ transport:
	 
   * Add a new ServiceControl instance.
   * Use default `Particular.ServiceControl` as the instance name (make sure there is no other instance of SC running with the same name).
   * Specify `host=localhost` as a connection string. ServiceControl Management Utility will automatically create queues and exchanges on the broker.

NOTE: If other ServiceControl instances have been running on this machine, it's necessary to specify a non-default port number for API. [Adjust ServicePulse settings](/servicepulse/host-config.md#changing-the-servicecontrol-url) accordingly to point to this location.
 
 4. Ensure the `ServiceControl` process is running before running the sample.
 5. [Install ServicePulse](/servicepulse/installation.md)

NOTE: In order to connect to a different RabbitMQ broker, ensure all connection strings in the sample are updated.


## Running the project

 1. Start the projects: Adapter, Sales and Shipping (right-click on the project, select the `Debug > Start new instance` option). Make sure the adapter starts first because on start-up it creates a queue that is used for heartbeats.
 1. Open ServicePulse (by default it's available at `http://localhost:9090/#/dashboard`) and select the Endpoints Overview. `Samples.ServiceControl.RabbitMQAdapter.Shipping` endpoint should be visible in the Active Endpoints tab as it has the Heartbeats plugin installed.
 1. Go to the Sales console and press `o` to send a message.
 1. Notice the Sales endpoint receives its own message and successfully processes it.
 1. Press `f` to simulate message processing failure.
 1. Go to the Shipping console and also press `f` to simulate message processing failure.
 1. Press `o` in both Sales and Shipping to create more messages.
 1. Notice both messages failed processing in their respective endpoints.
 1. Open ServicePulse and select the Failed Messages view.
 1. Notice the existence of one failed message group with two messages. Open the group.
 1. Press the "Retry all" button.
 1. Go to the Shipping console and verify that the message has been successfully processed.
 1. Go to the Sales console and verify that the message has been successfully processed.
 1. Shut down the Shipping endpoint.
 1. Open ServicePulse and notice a red label next to the heart icon. Click on the that icon to open the Endpoints Overview. Notice that `Samples.ServiceControl.RabbitMQAdapter.Shipping` is now displayed in the Inactive Endpoints tab.


## Code walk-through 

The code base consists of three projects.


### Sales Endpoint

The Sales project contains an endpoint that simulates the execution of a business process by exchanging messages with the Sales endpoint. It includes a message processing failure simulation mode (toggled by pressing `f`) which can be used to generate failed messages that demonstrate message retry functionality. The Sales endpoint uses the RabbitMQ direct routing topology and requires an adapter in order to communicate with ServiceControl.

The following code configures the Sales endpoint to communicate with the adapter:

snippet: SalesConfiguration


### Shipping Endpoint

The Shipping project also contains an endpoint that simulates the execution of a business process by exchanging messages with the Shipping endpoint. It includes message processing failure simulation mode (toggled by pressing `f`) which can be used to generate failed messages that demonstrate message retry functionality.

The Shipping endpoint uses the RabbitMQ direct routing topology and requires an adapter in order to communicate with ServiceControl.

The Shipping endpoint has the Heartbeats plugin installed to enable uptime monitoring via ServicePulse.

The following code configures the Shipping endpoint to communicate with the adapter:

snippet: ShippingConfiguration


### Adapter

The Adapter project hosts the `ServiceControl.TransportAdapter`. The adapter has two sides: endpoint-facing and ServiceControl-facing. In this sample the ServiceControl-facing side uses the RabbitMQ transport with the default conventional routing topology:

snippet: AdapterTransport

The following code configures the adapter to use the RabbitMQ direct routing topology when communicating with the business endpoints:

snippet: EndpointSideConfig

The following code configures names of the queues used by the adapter and ServiceControl:

snippet: AdapterQueueConfiguration

include: adapter-how-it-works
