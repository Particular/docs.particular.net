---
title: Writing Custom Checks
summary: Authoring one-off or periodic checks with the Custom Checks plugin
reviewed: 2018-01-26
component: CustomChecks
versions: 'CustomChecks:*'
---

To create a custom check, create a new custom check class:

snippet: CustomCheck

When the custom check executes, it should return a pass or fail status, and in the case of failure, a descriptive message. This status and descriptive message will be sent to ServiceControl and will appear in the [ServicePulse UI](in-servicepulse.md) and in [ServiceControl integration events](notification-events.md).

All custom checks are executed when the endpoint starts up. If the optional interval is specified then the custom check will be executed periodically.

snippet: PeriodicCheck
