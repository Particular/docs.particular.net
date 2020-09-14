---
title: Monitor third-party systems with custom checks
summary: Monitoring third-party systems which are exposed as HTTP endpoints with custom checks.
component: CustomChecks
reviewed: 2020-09-14
related:
 - servicecontrol/plugins
 - monitoring/custom-checks/in-servicepulse
redirects:
 - samples/custom-checks/monitoring3rdparty
---

External, third-party systems becoming unavailable might cause message-processing failures. The Particular Platform supports monitoring of third-party systems exposed as HTTP endpoints. This sample shows how to set up such monitoring using custom checks.

include: platformlauncher-windows-required

downloadbutton

## Running the sample

partial: running

Try toggling the status of the third-party system by pressing <kbd>Enter</kbd> in the **Samples.CustomChecks.3rdPartySystem** window, and watch the change in the output of the **Samples.CustomChecks.Monitor3rdParty** window.

partial: platform

## The custom check

The monitoring logic implements a `PeriodicCheck` which calls a configured URI whenever the specified interval elapses. When the third-party system doesn't respond in a timely fashion, a `CheckResult.Failed` event is sent to ServiceControl.

snippet: thecustomcheck
