---
title: Upgrade Guides
summary: General guidelines when upgrading NServiceBus from one major version to another
reviewed: 2020-06-29
suppressRelated: true
redirects:
 - nservicebus/upgradeguides
related:
 - persistence/upgrades
 - transports/upgrades
---

Every codebase is different and will have unique challenges when upgrading a major dependency like NServiceBus. It's important to have an upgrade plan and proceed in well-defined steps while taking sufficient time to perform regression tests after each stage. 

The process of upgrading endpoints consists of a common sequence of steps. Being able to apply those steps consistently is the key to a successful upgrade. 


## Choosing an endpoint for upgrade

The recommended approach is to upgrade one endpoint at a time. Start with simple, low-risk endpoints to ensure that the process is well understood, before tackling the more complex and critical endpoints. For example, endpoints that send emails or generate documents tend to be easy to upgrade. 

Thanks to wire-compatibility guarantees, it is not necessary for every endpoint in the solution to use the same version of NServiceBus. This means that a single endpoint can be upgraded, tested, and deployed to production before upgrading another one. This keeps the scope of changes to a minimum, reducing risk and isolating potential problems. In the same way, a new endpoint can be added and deployed with a newer version of NServiceBus, while endpoints in production remain on an older version. 

Note however that while NServiceBus guarantees wire-compatibility on the transport level, there may be some limitations with the chosen persistence and it might be necessary to transform stored data as part of the upgrade. Verify the detailed upgrade recommendations for the selected persistence.

Another factor to consider is the investment required to maintain the codebase. It may be cheaper in the long run to maintain a codebase containing one version of NServiceBus than to invest in training and knowledge sharing around multiple versions.


## Feature parity

Some NServiceBus features are not available in older versions and they might require the whole system to use the new version. An example is the [multiple deserializers feature](/samples/serializers/multiple-deserializers/) in NServiceBus version 6.

Ensure that any new features are adequately researched with regard to their impact on the upgrade process and potential prerequisites.


## Shared codebases

While upgrading one endpoint at a time is recommended whenever possible, if multiple endpoints share a common library the process will be slightly different. For example, upgrading the endpoint might involve changes in the common library, which will impact all other endpoints that rely on that library. The recommended approach in this situation is to create a copy of the common library for the new endpoint and keep the old version until all endpoints have been upgraded. When the time comes to upgrade the second endpoint, update it to point to the new version of the common library.

Other changes to the common library, for example related to new business requirements, should be minimized before all endpoints are upgraded, as they will need to be duplicated in both places.   


## Updating dependencies

All NServiceBus dependencies for an endpoint project are managed via NuGet. Open the Manage NuGet Packages window for the endpoint project, switch to the Updates tab, and look for packages that start with NServiceBus. Update each one to use the appropriate version.

It is strongly recommended to upgrade by one major version at a time, e.g. from version 5 to 6, or from version 4 to 5, rather than skipping major versions.

See also:

 * [NuGet Package Manager Dialog - Updating a Package](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui#updating-a-package)
 * [NuGet Package Manager Console - Updating a Package](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console#updating-a-package)

