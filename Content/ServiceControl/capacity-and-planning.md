---
title: ServiceControl capacity and planning
summary: Details the ServiceControl capacity, throughput and storage considerations to plan and support production environments
tags:
- ServiceControl
---

ServiceControl is intended to be a monitoring tool for production environments and, as each production tool, the deployment must be carefully planned and mantained over time.




 stores its data in a RavenDB Embedded instance, since


bjhxbdvzvkzjfvb

---------------------------



Additional content for article:

Maximum capacity for Embedded RavenDB database: 16TB

By default, message expiration is turned on
Refer to details in Message Expiration article
You can specify a custom drive/path for location of SC DB

Avarage message handling, 800/s


ServiceControl is not intended for long-term archiving. By default, it implements an audited message expiration module whose expiration is set for 30 days (configurable).

Note that failed messages do not expire

You can extend or reduce that period based on your storage capabilities and needs, but you need to take account of this fact.

ServiceControl is intended to serve as the backend for ServicePulse (i.e. production monitoring on endpoints, failed messages etc.) and ServiceInsight (advanced visualizations and debugging), so it deals mainly with recent-past information

If you wish to store the data in long term archiving storage, or in a specialized BI database, you can easily do so in several ways:

By querying the ServiceControl HTTP API: This will provide a JSON stream of audited messages (headers, body and context) you can then import into another database for various purposes

By activating audit.log queuing in ServiceControl, which copies the audited messages to the audit.log queue

this is turned off by default, as opposed to copying failed messages fo error.log which is on by default