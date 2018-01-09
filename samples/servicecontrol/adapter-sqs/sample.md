---
title: Monitor SQS transport with ServiceControl adapter
summary: Use transport adapter to connect SQS-based system to ServiceControl
component: SCTransportAdapter
reviewed: 2018-01-09
related:
 - servicecontrol
 - servicecontrol/transport-adapter
 - servicecontrol/plugins
---


This sample shows how to configure ServiceControl to monitor endpoints and retry messages when using SQS transport. The user endpoint in this solution uses the SQS transport while ServiceControl runs on MSMQ.


## Prerequisistes

 1. [Install ServiceControl](/servicecontrol/installation.md). 
 2. Using [ServiceControl Management](/servicecontrol/license.md#servicecontrol-management-app) tool, set up ServiceControl to monitor endpoints using MSMQ transport:
	 
   * Add a new ServiceControl instance:
   * Use default `Particular.ServiceControl` as the instance name (ensure there is no other instance of SC running with the same name).

NOTE: If other ServiceControl instances have been running on this machine, it's necessary to specify a non-default port number for API. [Adjust ServicePulse settings](/servicepulse/host-config.md#changing-the-servicecontrol-url) accordingly to point to this location.
 
 3. Ensure the `ServiceControl` process is running before running the sample.
 4. [Install ServicePulse](/servicepulse/installation.md)
 5. Set up Amazon SQS using instructions from the [simple SQS sample](/samples/sqs/simple).


## Running the project

 1. Start the Adapter and Endpoint  projects.
 1. Open ServicePulse (by default it's available at `http://localhost:9090/#/dashboard`) and select the Endpoints Overview. `Samples.ServiceControl.SqsTransportAdapter.Endpoint` endpoint should be visible in the Active Endpoints tab as it has the Heartbeats plugin installed.
 1. Go to the Endpoint console and press `o` to send a message.
 1. Notice the endpoint receives its own message and successfully processes it.
 1. Press `f` to simulate message processing failure.
 1. Press `o` to create more messages.
 1. Notice messages failed processing.
 1. Open ServicePulse and select the Failed Messages view.
 1. Notice the existence of one failed message group. Open the group.
 1. Press the "Retry all" button.
 1. Go to the Endpoint console and verify that the message has been successfully processed.
 1. Shut down the Endpoint.
 1. Open ServicePulse and notice a red label next to the heart icon. Click on the that icon to open the Endpoints Overview. Notice that `Samples.ServiceControl.SqsTransportAdapter.Endpoint` is now displayed in the Inactive Endpoints tab.


## Code walk-through 

The code base consists of two projects.


### Endpoint

The Endpoint project contains an endpoint that simulates the execution of a business process by sending a message to itself. It includes a message processing failure simulation mode (toggled by pressing `f`) which can be used to generate failed messages for demonstrating message retry functionality. The endpoint uses the SQS transport.

The endpoint has the Heartbeats plugin installed to enable uptime monitoring via ServicePulse.


### Adapter

The Adapter project hosts the `ServiceControl.TransportAdapter`. The adapter has two sides: endpoint-facing and ServiceControl-facing. In this sample the endpoint-facing side uses SQS transport and the ServiceControl-facing side uses MSMQ:

snippet: AdapterTransport

The following code configures the adapter to use SQL Server transport when communicating with the business endpoints (Shipping).

snippet: EndpointSideConfig
