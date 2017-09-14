---
title: Monitor 3rd party systems with custom checks
summary: Monitoring 3rd party systems which are exposed as HTTP endpoints with custom checks.
component: CustomChecks
reviewed: 2016-03-21
related:
 - servicecontrol/plugins
 - servicepulse/intro-endpoints-custom-checks
redirects:
 - samples/custom-checks/monitoring3rdparty
---


## Code walk-through

When integrating with 3rd party systems, often messages fail when those systems are down. The Particular Platform has extensibility to enable the monitoring 3rd party systems which are exposed as HTTP endpoints. This sample shows how to achieve this with custom checks.


### 3rd Party System console

The 3rd party system console application simulates a HTTP service running on `http://localhost:57789`. Verify that the 3rd party system is running by opening the url in a browser. When no error is received, the 3rd system is running correctly.


### The sample console

The sample console hosts an endpoint instance which has a custom check associated with it.


### The custom check

The monitoring capability implements a `PeriodicCheck` which calls a defined URI every time the specified interval is elapsed. When the 3rd system doesn't respond in a timely fashion a `CheckResult.Failed` is sent to ServiceControl.

snippet: thecustomcheck
