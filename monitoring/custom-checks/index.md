---
title: Custom Checks
summary: Define a custom set of conditions that are checked on the endpoint.
reviewed: 2018-01-03
component: CustomChecks
versions: 'CustomChecks:*'
---


The Custom Checks plugin enables endpoint health monitoring by running custom code and reporting status (success or failure) to a ServiceControl instance.

```mermaid
graph LR

subgraph Endpoint
CustomChecks[Custom Checks]
end
	
CustomChecks -- Custom Check<br>Data --> SCQ[ServiceControl<br>Input Queue]

SCQ --> ServiceControl
```

These conditions are solution and/or endpoint specific. It is recommended that they include the set of explicit (and implicit) assumptions about what enables the endpoint to function as expected versus what will make the endpoint fail.

For example, custom checks can include checking that a third-party service provider is accessible from the endpoint host, verifying that resources required by the endpoint are above a defined minimum threshold, and more.

To enable custom checks in an environment:

1. [Install a ServiceControl instance](/servicecontrol/servicecontrol-instances/)
2. [Install the Custom Checks plugin in endpoints that will contain custom checks](install-plugin.md) and [write a custom check](writing-custom-checks.md)
3. [View the status of Custom Checks in ServicePulse](in-servicepulse.md)
4. Optionally [subscribe to integration events from ServiceControl when custom checks succeed or fail](notification-events.md)

