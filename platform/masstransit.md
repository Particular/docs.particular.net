---
title: The Particular Platform for MassTransit
summary: How the Particular Platform works with MassTransit endpoints
reviewed: 2024-09-04
component: ServiceControl
---

The Particular Service Platform provides recoverability features for MassTransit endpoints on RabbitMQ and Azure Service Bus

It auto-detects and ingests messages from the [error](https://masstransit.io/documentation/concepts/exceptions#error-pipe) and [dead-letter](https://masstransit.io/documentation/concepts/exceptions#dead-letter-pipe) queues for all endpoints running in your system. The platform provides an aggregated view of the information necessary to detect, diagnose, and fix problems causing the failures as well as the ability to schedule failed messages for re-processing.

![MassTransit Fault Management](masstransit-overview-s.png  "width=715")

The [MassTransit Connector for ServiceControl](/servicecontrol/masstransit/) is part of the Particular Service Platform, which adds error queue and dead letter queue monitoring to MassTransit systems. This container runs alongside the existing MassTransit system and monitors for any faulted messages that occur within the system.

![Particular Service Platform architecture](architecture-overview-diagram-masstransit.svg)

### Managing failures

After the ingestion, failing messages are available via ServicePulse which is the UI for the platform. It enables navigating the list of failures, displaying details of the failed messsage (including exception details), as well as scheduling message for reprocessing.

![Managing failures with ServicePulse](masstransit-servicepulse.gif)

In addition, ServicePulse offers more advanced features such as [retry redirects](/servicepulse/redirect.md) and [failure message editting](/servicepulse/intro-editing-messages.md).

<div class="text-center inline-download hidden-xs"><a id='masstransit-sample' target="_blank" href='https://github.com/particular/MassTransitShowcaseDemo/' class="btn btn-primary btn-lg"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span> See it in action</a>
</div>