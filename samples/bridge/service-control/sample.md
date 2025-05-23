---
title: Use Bridge to connect to ServiceControl running on different transport
summary: Centralize monitoring of mixed transport solution with the Bridge
component: Bridge
reviewed: 2025-05-19
related:
 - servicecontrol
 - nservicebus/bridge
redirects:
 - samples/servicecontrol/adapter-mixed-transports
---

This sample shows how to configure ServiceControl to monitor endpoints and retry messages when using mixed transports. Both the endpoint and the ServiceControl instance use the [learning transport](/transports/learning/) (so that external dependencies are not needed) but with different storage folders. The same approach can be used to connect a single ServiceControl instance to multiple instances of the same transport, e.g. multiple databases used for SQL Server transport or to different transports.


## Running the project

 1. Start the **Bridge**, **Endpoint** and **PlatformLauncher** projects. ServicePulse should open automatically in the default browser.
 1. Go to the **Endpoint** console window and press <kbd>o</kbd> to send a message.
 1. Notice the endpoint receives its own message and successfully processes it.
 1. Press <kbd>f</kbd> to simulate message processing failure.
 1. Press <kbd>o</kbd> in to create more messages.
 1. Notice this time messages fail to be processed.
 1. Go to ServicePulse in the web browser and select the **Failed Messages** view.
 1. Notice the existence of one failed message group with two messages. Open the group.
 1. Press <kbd>f</kbd> in the Endpoint console to disable the failure simulation.
 1. Press the **Retry all** button in ServicePulse.
 1. Go to the **Endpoint** console and verify that the message has been successfully processed.
 1. Shut down the **Endpoint**.
 1. Open ServicePulse and notice a red label next to the heart icon. Click on that icon to open the Endpoints Overview. Notice that the Endpoint is now displayed in the Inactive Endpoints tab.


## Code walk-through

The code base consists of three projects.


### Endpoint

The Endpoint project contains an endpoint that simulates the execution of a business process by sending a message to itself. It includes a message processing failure simulation mode (toggled by pressing <kbd>f</kbd>) which can be used to generate failed messages for demonstrating message retry functionality. It uses the SQL Server transport.

The Endpoint has the heartbeats plugin installed to enable uptime monitoring via ServicePulse.


### Bridge

The Bridge project hosts the `NServiceBus.MessagingBridge`. The bridge has two sides: endpoint-facing and ServiceControl-facing.

snippet: ServiceControlTransport

snippet: EndpointSideConfig

### Duplicates

The bridge might introduce duplicates in the message flow when bridging two transports so endpoints need to account for that, e.g. by enabling the [Outbox](/nservicebus/outbox/) feature.
