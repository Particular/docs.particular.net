---
title: Using adapter to centralize monitoring of multi-instance SQL Server
component: SCTransportAdapter
reviewed: 2017-05-11
related:
 - servicecontrol
 - servicecontrol/plugins
---

## Prerequisistes

 1. [Install ServiceControl](/servicecontrol/installation.md).
 1. Using [ServiceControl Management](/servicecontrol/license.md#servicecontrol-management-app) tool, set up ServiceControl to monitor endpoints using SQL Server transport.
	 
	* Use `Particular.ServiceControl.SQL` as the instance name (make sure there is no other instance of SC running with the same name).
	* Use local `ServiceControl` on local SQL Server Express as the database (ServiceControl Manager will automatically create queue tables in the database). 
 1. Ensure the `ServiceControl` process is running before running the sample.
 1. In the same SQL Server instance, create databases for the endpoints: `sales`, `shipping` and `adapter`  
 1. [Install ServicePulse](/servicepulse/installation.md)

NOTE: In order to connect to a different SQL Server instance, ensure all database connection strings are updated in the sample.

## Running the project

 1. Start the projects: Adapter, Sales and Shipping (right-click on the project, select the `Debug > Start new instance` option). Make sure adapter starts first because on start-up it creates a queue that is used for heartbeats.
 1. Open ServicePulse and select the Endpoints Overview. `Samples.ServiceControl.SqlServerTransportAdapter.Shipping` endpoint should be visible in the Active Endpoints tab as it has the Heartbeats plugin installed
 1. Go to the Sales console and press `o` to create an order.
 1. Notice the Shipping endpoint receives the `OrderAccepted` event from Sales and publishes `OrderShipped` event.
 1. Notice the Sales endpoint logs that it processed the `OrderShipped` event. 
 1. Go to the Sales console and press `f` to simulate message processing failure.
 1. Press `o` to create another order. Notice the `OrderShipped` event fails processing in Sales and is moved to the error queue
 1. Press `f` again to disable message processing failure simulation in Sales.
 1. Go to the Shipping console and press `f` to simulate message processing failure.
 1. Go back to Sales and press `o` to create yet another order. Notice the `OrderAccepted` event fails in Shipping and is moved to the error queue.
 1. Press `f` again to disable message processing failure simulation in Shipping.
 1. Open ServicePulse and select the Failed Messages view.
 1. Notice the existence of one failed message group with two messages. Open the group.
 1. One of the messages is `OrderAccepted` which failed in `Shipping`, the other is `OrderShipped` which failed in `Sales`.
 1. Press the "Retry all" button.
 1. Go to the Shipping console and verify that the `OrderAccepted` event has been successfully processed.
 1. Go to the Sales console and verify that both `OrderShipped` events have been successfully processed.
 1. Shut down the Shipping endpoint.
 1. Open ServicePulse and notice a red label next to the heart icon. Click on the that icon to open the Endpoints Overview. Notice that `Samples.ServiceControl.SqlServerTransportAdapter.Shipping` is now displayed in the Inactive Endpoints tab.


## Code walk-through 

The solution consists of four projects.

### Shared

The Shared project contains the message contracts and the physical topology definition. The topology is defined in the `Connections` class via a method that takes the name of the queue table ([physical address](/nservicebus/sqlserver/addressing.md)) and returns the connection string to be used to access that queue.

snippet: GetConnectionString

The `StartsWith` comparison ensures that the [satellite](/nservicebus/satellites/) queues are correctly addressed. The `poison` queue is used by the adapter for unrecoverable failures. 

This topology is used in business endpoints (Sales, Shipping) as well as in the Adapter.

### Sales and Shipping

The Sales and Shipping projects contain endpoints that simulate execution of business process. The process consists of two events: `OrderAccepted` published by Sales and subscribed by Shipping and `OrderShipped` published by Shipping and subscribed by Sales.

The Sales and Shipping endpoints use separate databases and their transports are configured in the [multi-instance](/nservicebus/sqlserver/deployment-options.md#modes-overview-multi-instance) mode using the topology definition from the `Connections` class.

The business endpoints include message processing failure simulation mode (toggled by pressing `f`) which can be used to generate failed messages for demonstrating message retry functionality.

The Shipping endpoint has the Heartbeats plug-in installed to enable uptime monitoring via ServicePulse.

### Adapter

The Adapter project hosts the `ServiceControl.TransportAdapter`. The adapter has two sides: endpoint-facing and ServiceControl-facing. Each side can use different transport but in this sample both use SQL Server transport:

snippet: AdapterTransport

because the purpose is to keep ServiceControl unaware of specific details of endpoints' transport topology.
The endpoint-facing side config enables the [multi-instance](/nservicebus/sqlserver/deployment-options.md#modes-overview-multi-instance) mode of SQL Server transport using the shared topology.

snippet: EndpointSideConfig

The ServiceSontrol-facing side config specifies the connection string to the database used by ServiceControl:

snippet: SCSideConfig

Because the ServiceControl has been installed under a non-default instance name (`Particular.ServiceControl.SQL`) the control queue name needs to be overridden in the adapter configuration:

snippet: ControlQueueOverride

## How it works

The purpose of the adapter is to isolate ServiceControl from specifics of physical deployment topology of the business endpoints (such as [multi-instance](/nservicebus/sqlserver/deployment-options.md#modes-overview-multi-instance) mode. In order to do so, the adapter provides a ServiceControl interface to the business endpoints.

### Heartbeats

The heartbeat messages arrive at adapter's `Particular.ServiceControl` queue. From there there are moved to `Particular.ServiceControl.SQL` queue in ServiceControl database. In case of problems (e.g. destination database being down) the forward attempts are repeated configurable number of times after which messages are dropped to prevent the queue from growing indefinitely.

### Audits

The audit messages arrive at adapter's `audit` queue. From there there are moved to `audit` queue in ServiceControl database and ingested by ServiceControl.

### Retries

If a message fails all recoverability attempts in a business endpoint, it is moved to the `error` queue located in the adapter database. The adapter enriches the message by adding `ServiceControl.RetryTo` header pointing to the adapter's input queue in ServiceControl database (`ServiceControl.SqlServer.Adapter.Retry`). Next the message is moved to the `error` queue in ServiceControl database and ingested into ServiceControl RavenDB store. 

When retrying, ServiceControl looks for `ServiceControl.RetryTo` header and, if it finds it, it sends the message to the queue from that header instead of the ultimate destination.

The adapter picks up the message and forwards it to the destination using its endpoint-facing transport.


