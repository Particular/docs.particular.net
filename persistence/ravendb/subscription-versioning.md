---
title: Subscription versioning
component: raven
versions: '[4,6)'
reviewed: 2025-05-13
redirects:
 - nservicebus/ravendb/subscription-versioning
---

include: dtc-warning

include: cluster-configuration-info

The default behavior of the RavenDB subscription persistence differs from other persisters in the way it handles versioning of message assemblies. It's important to understand this difference, especially when using a deployment solution that automatically increments assembly version numbers with each build.

## Technical details

Most other persisters store subscriptions by message type alone, but the RavenDB persister additionally takes the message assembly's major version into account as well.

At a high level, the RavenDB document id for a stored subscription uses this format:

```csharp
// Pseudo-code only
subscriptionDocumentId = $"Subscriptions/{HashOf(messageType.Name + messageType.AssemblyVersion.Major)}";
```

As a result, if the subscription storage contains 3 subscribers for `MyMessage, MyAssembly, Version 1.0.0.0` but a publisher attempts to publish `MyMessage, MyAssembly, Version 2.0.0.0`, no subscription document will be found, and the event will not be published to any subscribers.

This becomes an issue when the version increments as a result of an automated deployment pipeline, such as when a build server uses a `YYYY.MM.DD.BuildNumber` version format for all builds. At the beginning of a new year, message publishers will begin looking for a new major version and, as a result, fail to publish events to the correct subscribers.

Because the version has been baked in to the RavenDB document id since the very first version, any attempt to modify the RavenDB persister to work like other persisters would be very high risk. This issue does not affect the vast majority of customers, and fairly simple workarounds to the problem exist. On the flip side, attempting to convert the storage format for existing subscriptions would run the risk of causing a message loss scenario for all customers.

## Guidelines

 1. If possible, do not version message assemblies at all. Design message contracts to be backward and forward compatible so that explicit versioning is not required.
 1. Instead of versioning with the [AssemblyVersionAttribute](https://msdn.microsoft.com/en-us/library/system.reflection.assemblyversionattribute.aspx), use the [AssemblyFileVersionAttribute](https://msdn.microsoft.com/en-us/library/system.reflection.assemblyfileversionattribute.aspx) or [AssemblyInformationalVersionAttribute](https://msdn.microsoft.com/en-us/library/system.reflection.assemblyinformationalversionattribute.aspx) instead, while leaving the `AssemblyVersionAttribute` fixed. The RavenDB persister only uses the Major version of the `AssemblyVersionAttribute` as part of the subscription document id.

## Non-versioned subscriptions support

In Versions 4.2 and above support for non-versioned subscriptions has been introduced. Non-versioned subscription are an opt-in feature due to their incompatibility with the default subscription storage behavior. To enable support for storing subscriptions without storing the message version change endpoint configuration as follows:

snippet: DisableSubscriptionVersioning

> [!NOTE]
> Due to the breaking nature of this feature it is important to take into account the impact of enabling it on pre-existing installations. Once enabled all the stored subscriptions, if any, will be immediately ignored and publishers will stop publishing events until all subscribers are restarted and new subscription requests reach the publisher endpoint. Opting in this new feature has no effects for newly deployed publisher endpoints that never received any subscription requests.

In Version 5.0 and above choosing a subscription versioning strategy is required to use RavenDB subscription storage. In order to continue using legacy versioned subscriptions, change the endpoint configuration as follows:

snippet: LegacySubscriptionVersioning

> [!WARNING]
> If neither subscription versioning option is selected, an exception will be thrown.
