---
title: Monitor third-party systems with custom checks
summary: Monitoring third-party systems which are exposed as HTTP endpoints with custom checks.
component: CustomChecks
reviewed: 2024-02-02
related:
 - monitoring/custom-checks/in-servicepulse
redirects:
 - samples/custom-checks/monitoring3rdparty
---

External, third-party systems becoming unavailable might cause message-processing failures. The Particular Platform supports monitoring of third-party systems exposed as HTTP endpoints. This sample shows how to set up such monitoring using custom checks.

include: platformlauncher-windows-required

downloadbutton

## Running the sample

Running the project will result in 3 console windows:

1. **3rdPartySystem**: Represents the third-party system by simulating an HTTP service running on `http://localhost:57789`. At startup, the custom check is returning success, but the state can be toggled between success and failure by pressing <kbd>Enter</kbd>.
1. **Monitor3rdParty**: The endpoint containing the custom check. The success or failure of the third-party system is continuously written to the console.
1. **PlatformLauncher**: Runs an in-process version of ServiceControl and ServicePulse. When the ServiceControl instance is ready, a browser window will be launched displaying the Custom Checks view in ServicePulse.

Try toggling the status of the third-party system by pressing <kbd>Enter</kbd> in the **3rdPartySystem** window, and watch the change in the output of the **Monitor3rdParty** window.

The status of the custom check is also reported to ServiceControl, and can be viewed in ServicePulse. In the browser window, navigate to the ServicePulse **Custom Checks** page to see the status change from success to error. The ServicePulse **Dashboard** page provides the overall status of custom checks.

## The custom check

The monitoring logic implements a `PeriodicCheck` which calls a configured URI whenever the specified interval elapses. When the third-party system doesn't respond in a timely fashion, a `CheckResult.Failed` event is sent to ServiceControl.

snippet: thecustomcheck
