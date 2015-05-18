---
title: RabbitMQ.Client Nuget usage
summary: Explains why the RabbitMQ.Client Nuget version range is inconsistent with most other NServiceBus extension nugets.
---

The NServiceBus.RabbitMQ library uses the [RabbitMQ.Client](http://www.nuget.org/packages/RabbitMQ.Client/) to communicate with the underlying queueing system. However due to the API/nuget versioning strategy of RabbitMQ.Client there are some complexities involved when using this dll from external libraries like NServiceBus.RabbitMQ.

Most libraries NServiceBus integrates with follow [SemVer](http://semver.org/). This means that those libraries do not make breaking changes in Minor or Patch releases. So the nuget range for these libraries is generally `≥ CurrentMajor.0.0 && < NextMajor.0.0`. So it is safe for the consumer of these libraries to move between any Minor of the current Major version.

The RabbitMQ.Client library does not follow SemVer. This means they are free to make breaking changes in Major, Minor or Patch released. 

For example between versions 3.5.1 and 3.5.2 of RabbitMQ.Client the public type `RabbitMQ.Client.Framing.Impl.Connection` had two public fields and three public methods removed (luckily none of these affect the API surface area used by NServiceBus.RabbitMQ). 

RabbitMQ.Client's current policy of allowing "breaking changes in any version" makes it difficult for us to select a sensible default when it comes to determining the nuget dependency version or version range.

## If NServiceBus.RabbitMQ targets a specific version of RabbitMQ.Client

If NServiceBus.RabbitMQ targets a specific version of RabbitMQ.Client the result is that consumers of NServiceBus.RabbitMQ are locked into the same version of RabbitMQ.Client as NServiceBus.RabbitMQ. Hence they cannot get new features or bugs fixes in  RabbitMQ.Client until an updated version of NServiceBus.RabbitMQ is shipped.

## If NServiceBus.RabbitMQ targets a "current Major" version range of RabbitMQ.Client

Our standard approach to targeting nuget ranges is to target "any Minor in the current Major". If this approach is used with NServiceBus.RabbitMQ it means that consumers of NServiceBus.RabbitMQ can move between versions of RabbitMQ.Client as specified in that range. Unfortunately there is a chance that RabbitMQ.Client will ship a breaking change that affects the subset of API that NServiceBus.RabbitMQ uses. If this does occur the most likely outcome is one of the following exceptions on startup:

* [MissingMethodException](https://msdn.microsoft.com/en-us/library/system.missingmethodexception.aspx)
* [MissingFieldException](https://msdn.microsoft.com/en-us/library/system.missingfieldexception.aspx)
* [TypeLoadException](https://msdn.microsoft.com/en-us/library/system.typeloadexception.aspx) 

## The current nuget dependency

In the currently released version targets a "current Minor" range i.e. `≥ CurrentMajor.CurrentMinor.0 && < CurrentMajor.NextMinor.0`. This is a compromise between the above two approaches. It allows some movement between RabbitMQ.Client versions while reducing the chance of breaking API changes. It also results in fewer deployments of NServiceBus.RabbitMQ to keep in sync.

So, after an upgrade of the RabbitMQ.Client nuget, if you have any problems with the above exceptions it is most likely a breaking API change. In that scenario please [Contact Us](http://particular.net/contactus) or raise an issue in the [NServiceBus.RabbitMQ GitHub Repository](https://github.com/Particular/NServiceBus.RabbitMQ).