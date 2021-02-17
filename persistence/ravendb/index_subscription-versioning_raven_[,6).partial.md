## Subscription persister and message versioning

The behavior of the RavenDB subscription persistence differs from other NServiceBus persisters in the way it handles versioning of message assemblies. It's important to understand this difference, especially when using a deployment solution that automatically increments assembly version numbers with each build.

To learn about message versioning as it relates to the RavenDB subscription persister, refer to [RavenDB subscription versioning](subscription-versioning.md).