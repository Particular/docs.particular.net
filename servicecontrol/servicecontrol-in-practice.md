---
title: ServiceControl in Practice
summary: Considerations for configuring ServiceControl for use in production environments
tags:
- ServiceControl
---


ServiceControl has many capabilities which can be extended by adding optional plugins into the endpoints of the system being monitored. Each capability and plugin can provide valuable information but also comes with a cost in terms of resource usage and performance. There are other factors that can influence the resource and performance profile of ServiceControl including hardware, message throughput, and number of endpoints. These factors can vary greatly between environments. Capabilities and plugins that provide value in one environment may have a negative impact if included in another environment.

For example, the [Saga Audit](/servicecontrol/plugins/saga-audit.md) plugin provides additional information to support a development environment where message load is low. In a production environment, where are there are many more saga instances to audit, the increased overhead is magnified and can have a significant performance impact.  

Each capability and plugin needs to be considered for each environment to determine if the value that it provides in that environment outweighs the costs that it imposes. 


## General Approach

When configuring ServiceControl for a new environment follow these steps:

- Read the [Capacity Planning](/servicecontrol/capacity-and-planning.md) and [Troubleshooting](/servicecontrol/troubleshooting.md) guides for ServiceControl.
- Turn off Auditing and remove ServiceControl plugins from Endpoints in the new environment.
- Run a performance test using the expected peak and average message throughput for the environment to baseline the system. This test suite should include simulating infrastructure outages that will cause large numbers of message processing failures. 
- For each Endpoint, install and configure the Heartbeat plugin (if needed). Re-run the performance test suite and monitor ServiceControl to ensure that it is able to effectively monitor the system under load. This may require adjustments to the Heartbeat Interval. Re-run the performance tests after each adjustment.
- For each endpoint, turn on Auditing (if needed) and re-run the performance tests to determine impact.
- For each endpoint, turn on any required Custom Checks and re-run the performance tests to determine impact.

NOTE: There are some factors that influence ServiceControl resource usage and performance that are less likely to change between environments but are specific to the system being monitored such as choice of transport and average message size. 


## Specific Considerations

- Each environment should have a dedicated ServiceControl instance.
- In production, ServiceControl should run on a **dedicated machine**.
- Turn off [Message Auditing](/nservicebus/operations/auditing.md) if it is not needed. The audit ingestion capability of ServiceControl is primarily to support system analysis with ServiceInsight. If ServiceInsight is not deployed in an environment then configure ServiceControl to read audit messages from an empty queue or turn off messaging auditing for each endpoint. Message auditing may be important for some endpoints but not other others. 
- Turn off [Audit Forwarding](/servicecontrol/errorlog-auditlog-behavior.md) if it is not needed. ServiceControl sends a copy of each audited message to configured Audit Forwarding queue. If these messages are not being used, turn this feature off.
- Plugins communicate between the Endpoint they installed in to ServiceControl using messaging and the configured transport of your system. Each instance of a plugin adds more messages to the ServiceControl queue which can delay the processing of each message and make ServiceControl less responsive.
  - [Heartbeats](/servicepulse/intro-endpoints-heartbeats.md) - Not all endpoints are mission critical and need to be monitored with heartbeats. For Endpoints that do need to be monitored, adjust the Heartbeat Interval. Increasing the interval ensures that ServiceControl is able to process heartbeats in a timely manner but also increases the window that an Endpoint can be unavailable for before being detected by ServiceControl. Heartbeat messages tend to be frequent and a large backlog can occur if ServiceControl is offline for an extended period. When this happens, it can take ServiceControl some time to process old heartbeats when it restarts. 
  - [Saga Audit](/servicecontrol/plugins/saga-audit.md) - the saga audit plugin produces a lot of data. It's use outside of a development environment is not recommended.
- Exclude the ServiceControl [database directory](/servicecontrol/configure-ravendb-location.md) from anti-virus checks.
- Do not downgrade releases of ServiceControl. ServiceControl uses an embedded database and changes to the internal data structures can occur between releases. Rolling back may cause index corruption or data loss. Perform testing in a lower environment before upgrading in production environments.