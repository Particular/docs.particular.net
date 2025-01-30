---
title: MassTransit Error Management - Early Access
summary: How to manage errors from MassTransit systems
reviewed: 2025-01-23
---

The Particular Service Platform is now offering its error management capabilities for MassTransit endpoints on RabbitMQ and Azure Service Bus under the [early access license - for free](https://particular.net/eula/early_access).

This functionality automatically detects and ingests messages from the [error](https://masstransit.io/documentation/concepts/exceptions#error-pipe) and [dead-letter](https://masstransit.io/documentation/concepts/exceptions#dead-letter-pipe) queues for all endpoints running in a MassTransit system.  
The platform provides an aggregated view of the information necessary to detect, diagnose, and fix problems causing the errors as well as the ability to send failed messages to be reprocessed.

![MassTransit Fault Management](masstransit-overview-s.png  "width=715")

This is done with a container called the [MassTransit Connector for ServiceControl](/servicecontrol/masstransit/) which runs alongside the existing MassTransit system and monitors for any failed messages that occur within it.

![Particular Service Platform architecture](architecture-overview-diagram-masstransit.svg)

### Managing errors

After the ingestion, failed messages are available via ServicePulse which is the UI for the platform. It enables navigating the list of errors, displaying details of the failed message (including exception details), as well as sending the message to be reprocessed.

![Managing failures with ServicePulse](masstransit-servicepulse.gif)

In addition, ServicePulse offers more advanced features such as [retry redirects](/servicepulse/redirect.md) and [failed message editing](/servicepulse/intro-editing-messages.md).

<div class="text-center inline-download hidden-xs"><a id='masstransit-sample' target="_blank" href='https://github.com/particular/MassTransitShowcaseDemo/' class="btn btn-primary btn-lg"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span> See it in action</a>
</div>
