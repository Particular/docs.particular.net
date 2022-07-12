---
title: GDPR compliance document
summary: Information about PII stored by NServiceBus
versions: "[5,)"
component: core
isLearningPath: true
reviewed: 2020-03-30
---

When operating an NServiceBus-based application, the platform will collect various pieces of information that are necessary to fulfill its operations. It is possible that some of this information will need to be considered when evaluating the application for GDPR compliance.

PII stands for personally identifiable information (also know as personal data) which means any information relating to an identifiable person who can be directly or indirectly identified. 

## Headers

partial: headers

These system headers are not configurable and should be considered present on all messages that originate from within the application. 

If the application makes use of [custom headers](/samples/header-manipulation/#adding-headers-when-sending-a-message) the custom code implementing them will need to be evaluated on an individual basis.


## Logs

NServiceBus has built-in logging that can collect PII information. Application logging [can be configured](/nservicebus/logging/) to reduce or eliminate this data from the application's log files.

#### Subscription entries
If the application is using the [publish/subscribe capabilities of NServiceBus](/nservicebus/messaging/publish-subscribe/), and the [MSMQ transport](/transports/msmq) is being used, endpoint names will be captured in the log files if the logging infrastructure is set to DEBUG level.

#### Current Windows user name
When the application is running, NServiceBus can log the user account name that its process is currently running under. This data can be captured when logging infrastructure is set to the INFO level and the [MSMQ transport](/transports/msmq) is being used.

#### Message bodies
Although not standard, it is possible that an application's [message bodies will be logged](/nservicebus/logging/message-contents.md) along with the PII that they contain. This will occur only if the application's logging level is set to `DEBUG` and if the message types have had the `.ToString()` method overridden to write out the PII data.


## Startup diagnostics

During endpoint startup, NServiceBus will write a diagnostics file for debugging and support purposes. This file will include the following data that could contain PII.

| Name | Description |
| :---------------| :-: |
| Hosting - Machine Name | `RuntimeEnvironment.MachineName` |
| Hosting - HostName | `Dns.GetHostUserName()` |
| Hosting - UserName | `Environment.UserName` |
| Receiving - LocalAddress | Endpoint Name |
| Receiving - LogicalAddress | Endpoint Name |
| Receiving - Satellites - Name| Satellite Name |
| Receiving - Satellites - ReceivingAddress | Satellite Address |

The diagnostics file will also include a listing of all message type names, endpoint names, and queue names.

More information on where to find the diagnostics file, and how to customize it, can be found in the [hosting documentation](/nservicebus/hosting/startup-diagnostics.md).

## ServiceControl

ServiceControl, using audits and error consumption, saves a set of information related to consumed messages in its database:

| Property | Document | Description |
| :------------------ | :-: | :-: |
| OriginatingEndpoint | CustomChecks | Endpoint Name, HostId, HostName |
| MessageMetadata/SendingEndpoint | FailedMessages | Endpoint Name, HostId, HostName |
| MessageMetadata/ReceivingEndpoint | FailedMessages | Endpoint Name, HostId, HostName |
| Headers | FailedMessages | All of the headers like in the Headers section |
| EndpointsDetails | KnownEndpoints | Endpoint Name, HostId, HostName |
| MessageMetadata/SendingEndpoint | ProcessedMessages | Endpoint Name, HostId, HostName |
| MessageMetadata/ReceivingEndpoint | ProcessedMessages | Endpoint Name, HostId, HostName |
| Headers | ProcessedMessages | All of the headers like in the Headers section |

When this data needs to be deleted, contact [Particular Support](https://particular.net/support) for assistance. 


## General

Data points such as "Endpoint Name" have been mentioned multiple times in this document. It's important to note that in many situations these pieces of data do not need to be considered with regards to GDPR. However, if that data has been configured to include PII, they will. It is best to avoid naming things in a manner that includes PII.

There are situations where this is not possible. For example, when using MSMQ as a transport the [computer's name will be included in the name of the queues](/transports/msmq/full-qualified-domain-name.md). This is a requirement of MSMQ and, as a result, means that queue names should receive the same vetting as machine names.