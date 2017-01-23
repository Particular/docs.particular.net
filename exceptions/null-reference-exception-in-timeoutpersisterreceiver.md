---
title: Null Reference Exception in TimeoutPersisterReceiver.Poll
summary: Null Reference Exception in TimeoutPersisterReceiver.Poll
tags:
- Exception
---

# Null Reference Exception in TimeoutPersisterReceiver.Poll

This exception can occur due to:

* Incomplete assembly scanning
* Version mismatch between NServiceBus and the selected persistence
* Selected a persistence that does not provide a specific storage feature


## Incomplete assembly scanning

When using custom [assembly scanning](/nservicebus/hosting/assembly-scanning) it is important to include all relevant assemblies that provide logic including extensions to NServiceBus like packages provided by the community or custom packages.

Make sure all relevant assemblies are included.

## Version mismatch between NServiceBus and the used persistence

NServiceBus versions 4.4.8, 4.5.7, 4.6.10, 4.7.7, 5.0.6, 5.1.4 and 5.2.8 introduced a new interface called `IPersistTimeoutsV2` and is used by persistence implementations. If you did not update NServiceBus to at least version 5.2.8 but are using the latest version of a transport that is using `IPersistTimeoutsV2` then this exception can occur.

Please update NServiceBus to at least the latest patch version to resolve this issue.


## Selected a persistence that does not provide a specific storage feature

Not all persistence options support all features. MSMQ, for example, does not support timeout persistence. When timeout persistence is needed this exception can occur if the persistence does not have an implementation for timeouts.
