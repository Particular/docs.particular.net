---
title: Azure Service Bus Transport Batching
summary: Sanitization with Azure Service Bus and how it works
component: ASB
tags:
- Cloud
- Azure
- Transports
reviewed: 2016-05-02
---


## What is sanitization and why is it needed?

Entities in the Messaging namespace are subjects to the following limitations

Queues: 260
Topics: 260
	Entity names or path can contain only letters, numbers, periods (.), hyphens (-), and underscores (_). Also a slashes (/) in the path.

Subjects: 50
Rules: 50
	Entity names or path can contain only letters, numbers, periods (.), hyphens (-), and underscores (_).


## Version 6 and below

All entities treated the same for allowed characters (only letters, numbers, periods (.), hyphens (-), and underscores (_)). 

## Version 7 and above

Default: `ThrowOnFailingSanitization`

Backward compatible: `EndpointOrientedTopologySanitization`. 

<upgrade guide info>

Custom sanitization: implement `ISanitizationStrategy`.