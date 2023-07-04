---
title: Health check notifications
summary: Describes how to setup email notifications for failing ServiceControl internal health checks
component: ServicePulse
reviewed: 2022-01-13
related:
---

Every ServiceControl instance performs periodic internal health checks  and raises notifications when they fail. By default, notifications are published as [integration events](/servicecontrol/contracts.md) but it's also possible to deliver them as email messages.


NOTE: Email notifications require ServicePulse version 1.29 or later, and ServiceControl version 4.17 or later.

Email notification settings can be managed from the Configuration page by selecting the Health Check Notifications tab. When configured emails will be sent only when health checks fail like remote instance cannot be reached or audit ingestion stopped etc.
It would not send an email for a message going to the error queue.

![Email health checks configuration](images/email-notifications.png)
