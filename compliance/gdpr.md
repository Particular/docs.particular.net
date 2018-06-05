---
title: GDPR compliance document
summary: Information about potential PII stored by NServiceBus
versions: "[5,)"
tags:
 - GDPR
 - Compliance
reviewed: 2018-05-17
---

NServiceBus as a platform collects various information that are necessary for fulfilling it's options. The purpose of this information is to inform customers what where and what kind of information are stored that could be perceived as personal identifiable information.


## Headers


partial:headers

## Logs

Logging may collect the following information that could be PII:

| Sample Entries that could be PII | Description |
| :------------------ |
| May contain subscription entries | Contains information about which endpoint subscribe to given message type | 
| May contain information about priviledges of a current windows user | Contains the name of the windows user under which the process is running |

More information on where to find log files and how to configure NSB on the level of logs can be found in the [logging documentation](/nservicebus/logging/).

## Startup diagnostics



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

When in need of deleting this information contact [Particular Support](https://particular.net/support). 