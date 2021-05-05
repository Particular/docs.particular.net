---
title: Health check notifications
summary: Describes how to setup email notifications for failing ServiceControl health checks
component: ServicePulse
reviewed: 2021-05-05
related:
---

Every ServiceControl instance performs periodic health checks and raises notifications in case any of them fail. By default, notifications get published as [integration events](/servicecontrol/contracts.md) but it's also possible to get them delivered as email messages.

Email notification settings can be managed from the Configuration page by selecting the Health Check Notifications tab.

![Email health checks configuration](images/email-notifications.png)