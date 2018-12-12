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

When integrating with third-party systems, often messages fail when those systems are down. The Particular Platform has extensibility to enable the monitoring of third-party systems which are exposed as HTTP endpoints. This sample shows how to achieve this with custom checks.

include: platformlauncher-windows-required

downloadbutton

## Running the sample

partial: running

Try toggling the status of the 3rd-party system by pressing <kbd>Enter</kbd> in the **Samples.CustomChecks.3rdPartySystem** window, and watch the change in output in the **Samples.CustomChecks.Monitor3rdParty** window.

partial: platform

## The custom check

The monitoring capability implements a `PeriodicCheck` which calls a defined URI every time the specified interval is elapsed. When the third-party system doesn't respond in a timely fashion a `CheckResult.Failed` event is sent to ServiceControl.

snippet: thecustomcheck
