---
title: Monitor mixed transports with ServiceControl adapter
summary: Centralize monitoring of mixed transport solution with the ServiceControl adapter
component: SCTransportAdapter
reviewed: 2022-07-11
related:
 - servicecontrol
 - servicecontrol/transport-adapter
 - servicecontrol/plugins
redirects:
 - samples/servicecontrol/adapter-sqlserver-multi-instance
 - samples/servicecontrol/adapter-sqlserver-multi-schema
---

This sample shows how to configure ServiceControl to monitor endpoints and retry messages when using mixed transports. The endpoint in this solution uses the SQL Server transport while ServiceControl runs on the learning transport. The same approach can be used to connect a single service control instance to multiple instances of the same message broker e.g. multiple databases used for SQL Server transport.


## Prerequisites

include: sql-prereq

## Running the project

 1. Start the Adapter, Endpoint and PlatformLauncher projects.
 1. ServiceControl should open automatically in the default browser.
 1. Go to the Endpoint console and press `o` to send a message.
 1. Notice the endpoint receives its own message and successfully processes it.
 1. Press `f` to simulate message processing failure.
 1. Press `o` in to create more messages.
 1. Notice this time messages fail to be processed.
 1. Go to ServicePulse and select the Failed Messages view.
 1. Notice the existence of one failed message group with two messages. Open the group.
 1. Press `f` in the Endpoint console to disable the failure simulation.
 1. Press the "Retry all" button in ServicePulse.
 1. Go to the Endpoint console and verify that the message has been successfully processed.
 1. Shut down the Endpoint.
 1. Open ServicePulse and notice a red label next to the heart icon. Click on that icon to open the Endpoints Overview. Notice that the Endpoint is now displayed in the Inactive Endpoints tab.


## Code walk-through 

The code base consists of three projects.


### Endpoint

The Endpoint project contains an endpoint that simulates the execution of a business process by sending a message to itself. It includes a message processing failure simulation mode (toggled by pressing `f`) which can be used to generate failed messages for demonstrating message retry functionality. It uses the SQL Server transport.

The Endpoint has the heartbeats plugin installed to enable uptime monitoring via ServicePulse.


### Adapter

The Adapter project hosts the `ServiceControl.TransportAdapter`. The adapter has two sides: endpoint-facing and ServiceControl-facing. In this sample the endpoint-facing side uses SQL Server transport and the ServiceControl-facing side uses the Learning transport:

snippet: AdapterTransport

The following code configures the adapter to use SQL Server transport when communicating with the business endpoints.

snippet: EndpointSideConfig


### Duplicates

ServiceControl Transport Adapter might introduce duplicates in the message flow when adapting two transports that cannot be configured to participate in an atomic transaction.
