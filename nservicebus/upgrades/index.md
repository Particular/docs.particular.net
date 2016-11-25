---
title: Upgrade Guides
reviewed: 2016-04-05
suppressRelated: true
redirects:
 - nservicebus/upgradeguides
---

When upgrading a major dependency like NServiceBus, every solution is different and will have its own unique challenges. It's important to have an upgrade plan and proceed in well defined steps while taking sufficient time to perform adequate regression tests after each step. 

The process of upgrading the endpoints is repeating a common sequence of steps. Being able to apply those steps consistently is key to the success of the upgrade plan. The recommended approach is to upgrade a simple and low risk endpoint first to ensure that the process is well understood before tackling the endpoints that make up the core of the solution. For example endpoints that send emails or generate documents are often good candidates for this. 

Here are a few things to consider when planning an upgrade project.

## Choosing an endpoint for upgrade
Due to the wire-compatibility guarantees, it is **not** necessary for every endpoint in the solution to use the same version of NServiceBus. This means a single endpoint with less complexity can be chosen for the upgrade and once deployed and stabalized, the process can be applied for the next endpoint.

This also means if the system is under development, a new endpoint can be developed and deployed with the new version of NServiceBus while endpoints in production are still running on the older version. This keeps the scope of changes to a minimum, which helps to reduce risk and to isolate potential problems if they arise.

While it is possible to have a distributed system running on multiple versions of NServiceBus, another factor to consider is the investment required to maintain codebases like these. It may be cheaper in the long run to maintain a codebase containing one version of NServiceBus than to invest in training and knowledge sharing around all the versions in use.

## Feature parity
Some features in newer version of NServiceBus are not available in prior versions and in order to benefit from them, the whole system needs to be running on the newer version of NServiceBus. An example of that is Multiple Deserialization feature in NServiceBus Version 6.

Ensure that any new features are adequately researched in regards to its impact on the upgrade process.

## Shared Code-bases
While upgrading one endpoint at a time is recommended, there's an issue with the shared library projects. If the endpoints in a solution share a common library then upgrading one endpoint might lead to changes in the common library and that necessitates changes in all of the other endpoints that rely on the common library at the same time. The recommended approach to dealing with this rippling change is to create a copy of the common library for the new endpoint and to upgrade it along with the endpoint. When the time comes to upgrade the second endpoint, change it's dependency to point to the new, upgraded, version of the common library. When using this approach, other changes to the common library should be minimized as they will need to be reflected in both codebases.   

## Updating dependencies
All NServiceBus dependencies for an endpoint project are managed via NuGet. Open the Manage NuGet Packages window for the endpoint project, switch to the Updates tab and look for packages that start with NServiceBus. Update each one to the latest Version 6 package.

Once packages have been updated the project will contain quite a few errors. This is expected as a lot of things have changed.

See also:

 * [NuGet Package Manager Dialog - Updating a Package](https://docs.nuget.org/ndocs/tools/package-manager-ui#updating-a-package)
 * [NuGet Package Manager Console - Updating a Package](https://docs.nuget.org/ndocs/tools/package-manager-console#updating-a-package)

