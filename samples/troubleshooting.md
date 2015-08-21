---
title: Troubleshooting
summary: Samples common issues and troubleshooting
tags:
- Samples
- Troubleshooting
---

## Prerequisites

All sample applications should work right away after download. If application is throwing exceptions that might signify **missing prerequisites**.


### Symptoms
For example, the following exception means that MSMQ wasn't configured correctly:
```
An unhandled exception of type 'System.InvalidOperationException' occurred in NServiceBus.Core.dll
Additional information: There is a problem with the input queue: FormatName:DIRECT=OS:HOST-NAME\private$\Samples.AsyncPages.Server. See the enclosed exception for details.
```

### Solution

The easiest and recommended solution is to download and run [platform installer](http://particular.net/downloads), which will handle all prerequisites.

Alternatively, you can handle prerequisites manually by following the [instructions](/platform/installer/offline.md).

## Serialization

NServiceBus uses custom [XmlSerializer](/nservicebus/serialization/xml.md) by default. If you don't explicitly specify otherwise it will be used in your endpoints, for more details refer to [dedicated page](/nservicebus/serialization/).

However, some sample applications are configured to use [JsonSerializer](/nservicebus/serialization/json.md). 

If you add a new endpoint to the sample application or change the configuration of an existing endpoint, then the receiving endpoint won't be able to deserialize the message.

### Symptoms

After successfully sending the message, the following exception is thrown:
 
```
NServiceBus.MessageDeserializationException
```

### Solution

Make sure all endpoints are configured to use the same serializer. Follow the documentation on specific serializers to verify and set up configuration (e.g. [XmlSerializer](/nservicebus/serialization/xml.md), [JsonSerializer](/nservicebus/serialization/json.md). 
).