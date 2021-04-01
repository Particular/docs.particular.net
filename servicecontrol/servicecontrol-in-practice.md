---
title: Optimizing ServiceControl for use in different environments
summary: Tips for running ServiceControl efficiently
reviewed: 2020-03-26
isLearningPath: true
---

ServiceControl provides many capabilities such as endpoint monitoring, advanced debugging, and failed message management. These capabilities can be extended by adding optional [plugins](/servicecontrol/plugins/) into the endpoints being monitored. Each capability and plugin provides valuable information, but they have certain resource and performance costs.

Hardware, peak and average message throughput, and number of endpoints in the system all have an impact on the performance of ServiceControl. These factors can vary greatly between environments. Capabilities and plugins that provide value in one environment may have a negative impact if included in another environment. For example, the [Saga Audit](/servicecontrol/plugins/saga-audit.md) plugin provides additional information to support a development environment where message load is low. In a production environment, where there are many more saga instances to audit, the increased overhead is magnified and can have a significant performance impact. 

Each capability and plugin should be considered carefully for each environment to determine if the value that it provides in that environment outweighs the costs that it imposes.

Here are some considerations when installing and deploying ServiceControl for better performance.

## Hardware & installation considerations:

Read the [general hardware recommendations](/servicecontrol/servicecontrol-instances/hardware.md) for more details.

In addition:

* Read the [capacity planning](/servicecontrol/capacity-and-planning.md) and [troubleshooting](/servicecontrol/troubleshooting.md) guides for ServiceControl.
* Each environment should have a dedicated ServiceControl instance.
* Other applications or NServiceBus endpoints running on the same server as ServiceControl can compete for hardware resources and therefore negatively impact ServiceControl's performance. For optimal performance, run ServiceControl on a dedicated server.

## Message throughput considerations

Depending on the number of endpoints and message volume, audit messages can have a significant impact on performance. Turn off [message auditing](/nservicebus/operations/auditing.md) if it is not needed. The primary reason for the audit ingestion capability of ServiceControl is to support system analysis with ServiceInsight. If ServiceInsight is not in use, turn off messaging auditing for each endpoint. Message auditing may be important for some endpoints but not others.

NOTE: If message auditing is required without the use of ServiceInsight, configure endpoints and ServiceControl to use different audit queues. Audit messages going to an audit queue that is not managed by ServiceControl must be cleaned up manually.

Turn off [audit forwarding](/servicecontrol/errorlog-auditlog-behavior.md) if it is not needed. ServiceControl sends a copy of each audited message to a configured audit forwarding queue. If these messages are not being used, turn this feature off.

## Plugin considerations

[Plugins](/servicecontrol/plugins/) are installed in an endpoint and send data to ServiceControl. This communication uses messaging over the configured transport of the endpoint. Each instance of a plugin adds more messages to the ServiceControl queue which can delay the processing of each message and make ServiceControl less responsive.

### Heartbeats

Not all endpoints are mission critical and need to be monitored with [heartbeats](/monitoring/heartbeats/) using the same SLA. For endpoints that are less critical, [adjust the heartbeat interval](/monitoring/heartbeats/install-plugin.md). Increasing the interval ensures that ServiceControl is able to process heartbeats in a timely manner. Increasing the heartbeat interval for endpoints requires a corresponding increase in the [heartbeat grace period](/servicecontrol/creating-config-file.md#plugin-specific-servicecontrolheartbeatgraceperiod) in ServiceControl.

Heartbeat messages tend to be frequent, and a large backlog can occur if ServiceControl is offline for an extended period. When this happens, it can take ServiceControl some time to process old heartbeats when it restarts.

### Saga audit

The [Saga Audit](/servicecontrol/plugins/saga-audit.md) can add significant load on ServiceControl due to volume of messages it sends but is invaluable when diagnosing issues with Saga behavior. In order to use it in the production environment make sure to use ServiceControl 4.13.0 or later and configure the Saga Audit plugin to send messages to the `audit` queue. If necessary consider [scaling out the audit processing](/servicecontrol/servicecontrol-instances/distributed-instances.md).

## Performance considerations

Run a performance test using the expected peak and average message throughput for the environment to baseline the system. The baseline test should not include audit ingestion or any ServiceControl plugins.

Once this baseline has been established, follow these steps:

* Install and configure the Heartbeat plugin in each endpoint where it is needed. Re-run the performance test suite and monitor ServiceControl to ensure that it can effectively monitor the system under load. This may require adjustments to the Heartbeat interval. Re-run the performance tests after each adjustment.
* Turn on auditing for each endpoint that needs it and re-run the performance tests to assess impact.
* For each endpoint, turn on any required custom checks and re-run the performance tests to assess impact.

When an infrastructure outage occurs in a production environment it's possible that every message processed on every endpoint may end up in the error queue. It can take ServiceControl some time to ingest all of these messages. Once ingested, a bulk retry operation will consume additional network and disk I/O above the usual requirements. It is important to simulate these conditions as a part of performance testing to ensure that these times and resources are accounted for in recovery plans.

## Anti-virus checks

Exclude the ServiceControl [database directory](/servicecontrol/configure-ravendb-location.md) from anti-virus checks. ServiceControl uses an embedded database and produces a high level of I/O traffic. Some anti-virus software can add overhead to I/O operations causing a significant performance impact.

## Version downgrades

Do not downgrade releases of ServiceControl. ServiceControl uses an embedded database and changes to the internal data structures can occur between releases. Rolling back may cause index corruption or data loss. Perform testing in a lower environment before upgrading in production environments.

## Server monitoring

It is recommended to monitor the following metrics on the server hosting the ServiceControl instance:

* CPU usage
* Memory available
* Disk space free
* Disk queue length
* Disk read/write/total IO

These metrics can be viewed using *Resource Monitor* but it is advised to use server monitoring since it stores historic data in a durable format.

## Automated OS patching

ServiceControl instances are resilient to forced reboots as part of automated patching or hardware failures (i.e. Windows updates, security patches, failovers, e.g.) that can result in ungraceful termination. Forced reboots won’t result in a loss of messages or unrecoverable data corruption.
