---
title: NServiceBus.Extensions.Logging Usage
summary: A sample that uses Microsoft.Extensions.Logging with NLog
component: Extensions.Logging
reviewed: 2020-03-04
tags:
 - logging
related:
 - nservicebus/logging
---

The sample shows how to configure an NServiceBus endpoint to use Microsoft.Extensions.Logging in combination with NLog.

Both Microsoft.Extensions.Logging and NServiceBus.Logging are logging abstractions. Both need to be configured.

The following logging chain is created:


- NServiceBus.Logging
  - NServiceBus.Extensions.Logging
    - Microsoft.Extensions.Logging
      - NLog.Extensions.Logging
        - NLog
          - Console output


### Configure NLog

NLog in this example is configured in code:

snippet: NLogConfiguragion

INFO: There is no preference on how NLog is configuredNLog. Based on the NLog documentation the most used method is via an [NLog configuration file](https://github.com/nlog/nlog/wiki/Configuration-file#configuration).

WARNING: It is important that NLog, Microsoft.Extensions.Logging and NServiceBus.Logging abstractions are initialized before `Endpoint.Create` or `Endpoint.Start` is invoked. If logging is not fully initialized it is not guaranteed that logging will be working as expected.

### Configuring logging abstractions

The following snippet shows how to initialize logging. NLog has its own provider extensions for Microsoft.Extensions.Logging and needs an `NLogLoggerFactory` provider that implements `Microsoft.Extensions.Logging.ILoggerFactory` instance so that `Microsoft.Extensions.Logging` can use NLog.

snippet: MicrosoftExtensionsLoggingNLog

After logging initialization endpoints can be started or created.
