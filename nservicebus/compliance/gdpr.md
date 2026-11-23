---
title: GDPR Compliance
summary: Information about personally identifiable information (PII) collected and stored by NServiceBus and ServiceControl
versions: "[5,)"
component: core
isLearningPath: true
reviewed: 2026-10-16
---

The [General Data Protection Regulation](https://en.wikipedia.org/wiki/General_Data_Protection_Regulation) is a data protection and privacy regulation of the European Union. The regulation contains provisions and requirements related to the processing of [personally identifiable information (PII)](https://en.wikipedia.org/wiki/Personal_data) of individuals located in the European Economic Area.

When operating an NServiceBus-based application, the platform will collect various pieces of information that are necessary to fulfill its operations. It is possible that some of this information will need to be considered when evaluating the application for GDPR compliance.

## Headers

| Name | Description |
| :------------------ | :--------- |
| OriginatingEndpoint | Contains endpoint name |
| OriginatingMachine | Contains the machine from which the message originated |
| ReplyToAddress | Contains the queue name that should get the answer |

These [system headers](/nservicebus/messaging/headers.md) can contain endpoint, machine, and queue names. The machine name used by NServiceBus [can be overridden](/nservicebus/hosting/override-machine-name.md).

If the application makes use of [custom headers](/samples/header-manipulation/#adding-headers-when-sending-a-message) the custom code implementing them will need to be evaluated on an individual basis.


## Logs

NServiceBus has built-in logging that can collect PII information. Application logging [can be configured](/nservicebus/logging/) to reduce or eliminate this data from the application's log files.

#### Subscription entries
If the application is using the [publish/subscribe capabilities of NServiceBus](/nservicebus/messaging/publish-subscribe/), and the [MSMQ transport](/transports/msmq) is being used, endpoint names will be captured in the log files if the logging infrastructure is set to DEBUG level.

#### Current Windows user name
When the application is running, NServiceBus can log the user account name that its process is currently running under. This data can be captured when logging infrastructure is set to the INFO level and the [MSMQ transport](/transports/msmq) is being used.

#### Message bodies

Although not standard, it is possible that an application's [message bodies will be logged](/nservicebus/logging/#logging-message-contents) along with the PII that they contain. NServiceBus logs the result of a message's `ToString()` method when sending at `DEBUG` level or lower. Overriding this method can expose PII in logs. Application code and custom logging integrations may also log message contents at other levels.


## Startup diagnostics

During endpoint startup, NServiceBus can write a diagnostics file for debugging and support purposes. This file can include the following data that could contain PII. The diagnostics structure varies by NServiceBus version and hosting model.

| Name | Description |
| :---------------| :-: |
| Hosting - Machine Name | `RuntimeEnvironment.MachineName` |
| Hosting - HostName | `Dns.GetHostName()` |
| Hosting - UserName | `Environment.UserName` |
| Receiving - LocalAddress | Endpoint Name |
| Receiving - LogicalAddress | Endpoint Name |
| Receiving - Satellites - Name| Satellite Name |
| Receiving - Satellites - ReceivingAddress | Satellite Address |

The diagnostics file will also include a listing of all message type names, endpoint names, and queue names.

More information on where to find the diagnostics file, and how to customize it, can be found in the [hosting documentation](/nservicebus/hosting/startup-diagnostics.md).

## Message data

Message bodies may contain PII as well as headers. When [auditing is enabled](/nservicebus/operations/auditing.md), NServiceBus forwards a copy of each successfully processed message to the audit queue. Messages that exhaust their retries are [forwarded to the error queue](/nservicebus/recoverability/configure-error-handling.md).

[Message property encryption](/nservicebus/security/property-encryption.md) encrypts selected properties; the rest of the message remains in clear text. It does not encrypt message headers.

When using [Claim Check (DataBus)](/nservicebus/messaging/claimcheck/), payloads are stored separately and messages contain references to them. Review the retention and cleanup of that external storage as well as the messages themselves.

## ServiceControl

ServiceControl stores message bodies and headers consumed from audit and error queues, in addition to metadata such as the following:

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

[Audit data expires according to the configured retention period](/servicecontrol/audit-instances/configuration.md#data-retention). [Failed message data becomes eligible for expiration after a successful retry or manual archival](/servicecontrol/servicecontrol-instances/configuration.md#data-retention). Unresolved failed messages are not automatically deleted by this retention policy.

[ServiceControl retention](/servicecontrol/how-purge-expired-data.md) applies to its database, not to audit or error forwarding queues. Contact [Particular Support](https://particular.net/support) for assistance with deleting data outside the normal retention process.


## General

Data points such as "Endpoint Name" have been mentioned multiple times in this document. It's important to note that in many situations these pieces of data do not need to be considered with regards to GDPR. However, if that data has been configured to include PII, they will. It is best to avoid naming things in a manner that includes PII.

There are situations where this is not possible. For example, when using MSMQ as a transport the [computer's name will be included in the name of the queues](/transports/msmq/full-qualified-domain-name.md). This is a requirement of MSMQ and, as a result, means that queue names should receive the same vetting as machine names.
