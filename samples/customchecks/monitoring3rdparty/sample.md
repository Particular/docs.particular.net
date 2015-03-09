---
title: Monitoring 3rd party systems with custom checks
summary: This sample shows how to monitor 3rd party systems which are exposed as HTTP endpoints with custom checks.
tags:
- CustomCheck
- ServicePulse
- ServiceControl
---

## Code walk-through 

When integrating with 3rd party systems, we often end up having many messages that fail when those systems are down. We then usually end up looking for external monitoring services. Why looking elsewhere when the Particular Platform already has the extensibility to enable you to monitor 3rd party systems which are exposed as HTTP endpoints. This sample shows how to achieve this with custom checks. 

Set both projects as Startup-Projects. Run the solution with elevated permissions.

### 3rd Party System console

The 3rd party system console application simulates a HTTP service running on `http://localhost:57789`. You can verify that the 3rd party system is running by opening the url in a browser. When you receive no error the 3rd system is running correctly.

### The sample console

The sample console hosts an endpoint instance which has a custom check associated with it. 

### The custom check

The custom check consists of an abstract base class which provides the monitoring capability. The monitoring capability implements a `PeriodicCheck` which calls a defined URI every time the specified interval is elapsed. When the 3rd system doesn't respond in a timely fashion a `CheckResult.Failed` is sent to ServiceControl.

<!-- import thecustomcheck -->

 Some places rely on external monitoring tools to do url monitoring, e.g. site24x7.com. Perhaps specify that this is one approach when that's not possible? Why write code if the users have some tool in place to monitor? cc / @gbiellem 
Also should we link the sample to this article? http://docs.particular.net/servicepulse/how-to-develop-custom-checks