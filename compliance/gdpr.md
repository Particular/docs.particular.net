---
title: GDPR compliance document
summary: Information about PII stored by NServiceBus
versions: "[5,)"
tags:
 - GDPR
 - Compliance
reviewed: 2018-05-17
---

When operating an NServiceBus based application, the platform will collect various pieces of information that are necessary to fulfill its operations. It is possible that some of this information will need to be considered when evaluating the application for GDPR compliance.

PII stands for personally identifiable information (also know as personal data) which mean any information relating to an identifiable person who can be directly or indirectly identified. 

## Headers

partial:headers

These system headers are not configurable and should be considered present on all messages that originate from within the application. 

If the application makes use of [custom headers](/samples/header-manipulation/#adding-headers-when-sending-a-message) the custom code implementing them will need to be evaluated on an individual basis.


## Logs

NServiceBus has built in logging that can collect PII information. It is possible to [configure application logging](/nservicebus/logging/) to reduce or eliminate this data from the application's log files.

#### Subscription entries
If the application is using the [publish-subscribe capabilities of NServiceBus](/nservicebus/messaging/publish-subscribe/), endpoint names will be captured in the log files if the logging infrastructure is set to XXXX level.

#### Current Windows user name
When the application is running NServiceBus can log the user account name that its process is currently running under. This data will be captured when logging infrastructure is set to the XXXX level.

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
| Receiving - Sattelites - Name| Sattelite Name |
| Receiving - Sattelites - ReceivingAddress | Sattelite Address |

The diagnostics file will also include a listing of all message type names, endpoint names and queue names.

More information on where to find the diagnostics file, and how to customize it, can be found in the [Hosting documentation](/nservicebus/hosting/#startup-diagnostics).

## ServiceControl

ServiceControl using audits and error consumption saves set of information related to consumed messages in it's database (RavenDB):

| Property | Document | Description |
| :------------------ | :-: | :-: |
| OriginatingEndpoint | CustomChecks | Endpoint Name, HostId, HostName |
| MessageMetadata/SendingEndpoint | FailedMessages | Endpoint Name, HostId, HostName |
| MessageMetadata/RecievingEndpoint | FailedMessages | Endpoint Name, HostId, HostName |
| Headers | FailedMessages | All of the headers like in the Headers section |
| EndpointsDetails | KnownEndpoints | Endpoint Name, HostId, HostName |
| MessageMetadata/SendingEndpoint | ProcessedMessages | Endpoint Name, HostId, HostName |
| MessageMetadata/RecievingEndpoint | ProcessedMessages | Endpoint Name, HostId, HostName |
| Headers | ProcessedMessages | All of the headers like in the Headers section |

When this data needs to be deleted, contact [Particular Support](https://particular.net/support) for assistance. 