---
title: Monitor mixed transports with ServiceControl adapter
summary: Centralize monitoring of mixed transport solution with the ServiceControl adapter
component: SCTransportAdapter
reviewed: 2017-08-11
related:
 - servicecontrol
 - servicecontrol/transport-adapter
 - servicecontrol/plugins
---


This sample shows how to configure ServiceControl to monitor endpoints and retry messages when using mixed transports. The main transport for the solution is MSMQ and this is the transport used by the ServiceControl. Some endpoints, however, use SQL Server transport.


## Prerequisistes

 1. [Install ServiceControl](/servicecontrol/installation.md). 
 2. Using [ServiceControl Management](/servicecontrol/license.md#servicecontrol-management-app) tool, set up ServiceControl to monitor endpoints using MSMQ transport:
	 
   * Add a new ServiceControl instance:
   * Use default `Particular.ServiceControl` as the instance name (ensure there is no other instance of SC running with the same name).

NOTE: If other ServiceControl instances have been running on this machine, it's necessary to specify a non-default port number for API. [Adjust ServicePulse settings](/servicepulse/host-config.md#changing-the-servicecontrol-url) accordingly to point to this location.
 
 3. Ensure the `ServiceControl` process is running before running the sample.
 4. In the local SQL Server Express instance, create database for the shipping endpoint: `shipping`.
 5. [Install ServicePulse](/servicepulse/installation.md)

NOTE: In order to connect to a different SQL Server instance, ensure all database connection strings are updated in the sample.


## Running the project

 1. Start the projects: Adapter, Sales and Shipping projects.
 1. Open ServicePulse (by default it's available at `http://localhost:9090/#/dashboard`) and select the Endpoints Overview. `Samples.ServiceControl.MixedTransportAdapter.Shipping` endpoint should be visible in the Active Endpoints tab as it has the Heartbeats plugin installed.
 1. Go to the Sales console and press `o` to send a message.
 1. Notice the Sales endpoint receives its own message and successfully processes it.
 1. Press `f` to simulate message processing failure.
 1. Go to the Shipping console and also press `f` to simulate message processing failure.
 1. Press `o` in Sales to create more messages.
 1. Notice both messages failed processing in their respective endpoints.
 1. Open ServicePulse and select the Failed Messages view.
 1. Notice the existence of one failed message group with two messages. Open the group.
 1. Press the "Retry all" button.
 1. Go to the Shipping console and verify that the message has been successfully processed.
 1. Go to the Sales console and verify that the message has been successfully processed.
 1. Shut down the Shipping endpoint.
 1. Open ServicePulse and notice a red label next to the heart icon. Click on the that icon to open the Endpoints Overview. Notice that `Samples.ServiceControl.MixedTransportAdapter.Shipping` is now displayed in the Inactive Endpoints tab.


## Code walk-through 

The following diagram shows the topology of the solution:

![Topology diagram](diagram.svg)

The code base consists of three projects.


### Sales

The Sales project contains an endpoint that simulates the execution of a business process by sending a message to itself. It includes a message processing failure simulation mode (toggled by pressing `f`) which can be used to generate failed messages for demonstrating message retry functionality. The Sales endpoint uses the MSMQ transport (same as ServiceControl).


### Shipping

The Shipping project also contains an endpoint that simulates the execution of a business process by sending a message to itself. It includes message processing failure simulation mode (toggled by pressing `f`) which can be used to generate failed messages for demonstrating message retry functionality.

The Shipping endpoint uses the SQL Server transport and requires an adapter in order to communicate with ServiceControl.

The Shipping endpoint has the Heartbeats plugin installed to enable uptime monitoring via ServicePulse.


### Adapter

The Adapter project hosts the `ServiceControl.TransportAdapter`. The adapter has two sides: endpoint-facing and ServiceControl-facing. In this sample the endpoint-facing side uses SQL Server transport and the ServiceControl-facind side uses MSMQ:

snippet: AdapterTransport

The following code configures the adapter to use SQL Server transport when communicating with the business endpoints (Shipping).

snippet: EndpointSideConfig


### Duplicates

By default NServiceBus transports use the highest supported transaction modes. In case of MSMQ and SQL Server transports this means `TransactionScope`. Because of that the Adapter requires the Distributed Transaction Coordinator (DTC) service to be configured. In this mode there is no risk of creating duplicate messages when moving messages between the transports. This is especially important for the messages that are selected for retry.
