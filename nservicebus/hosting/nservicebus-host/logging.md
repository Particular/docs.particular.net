---
title: Logging
reviewed: 2016-11-04
---


## Versions 5 and above

As of Versions 5 and above, logging for the host is controlled with the [core logging API](/nservicebus/logging/).

Add the logging API calls as mentioned in the above article directly in the implementation of the `IConfigureThisEndoint.Customize` method.


## Versions 4 and below

To change the host's logging infrastructure, implement the `IWantCustomLogging` interface. In the `Init` method, configure the custom setup. To make NServiceBus use the logger, use the `NServiceBus.SetLoggingLibrary.Log4Net()` API described in the [logging documentation](/nservicebus/logging) and shown below:

snippet:CustomHostLogging

The host's [profiles](/nservicebus/hosting/nservicebus-host/profiles.md) mechanism can be used to specify different logging levels (`DEBUG`, `WARN`, etc.) or targets (`CONSOLE`, `FILE`, etc.).
