---
title: Monitor third-party systems with custom checks
summary: Monitoring third-party systems which are exposed as HTTP endpoints with custom checks.
component: CustomChecks
reviewed: 2018-02-23
related:
 - servicecontrol/plugins
 - monitoring/custom-checks/in-servicepulse
redirects:
 - samples/custom-checks/monitoring3rdparty
---


## Code walk-through

When integrating with third-party systems, often messages fail when those systems are down. The Particular Platform has extensibility to enable the monitoring of third-party systems which are exposed as HTTP endpoints. This sample shows how to achieve this with custom checks.


### Third-party system console

The third-party system console application simulates a HTTP service running on `http://localhost:57789`. Verify that the third-party system is running by opening the url in a browser. When no error is received, the third-party system is running correctly.


### The sample console

The sample console hosts an endpoint instance which has a custom check associated with it.


### The custom check

The monitoring capability implements a `PeriodicCheck` which calls a defined URI every time the specified interval is elapsed. When the third-party system doesn't respond in a timely fashion a `CheckResult.Failed` event is sent to ServiceControl.

snippet: thecustomcheck
