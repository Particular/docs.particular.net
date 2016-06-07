---
title: Cross-version compatibility testing with NServiceBus
summary: Develop service layers and long-running processes using test-driven development.
reviewed: 2016-06-06
related:
 - samples/unit-testing
---

At times it is required to run different versions of NServiceBus across a distributed system. A common theme is when an update needs to be rolled out and these updates are usually rolled out gradually. The rest of the system should remain intact and functional. As such, it is essential that endpoints running different versions are cross-compatible.

In order to ensure endpoints using various versions of NServiceBus can communicate with each other, it is necessary to extensively test multiple aspects of cross-version compatibility. The automated tests are publicly available in the [Particular/EndToEnd](https://github.com/Particular/EndToEnd/) repository and are evaluated as part of pre-release activities.

Those tests might be useful for NServiceBus users in order to verify that the framework works correctly in an unusual scenario, for example when using an unusually complex data structure, in order to understand in detail how specific feature works, or to reproduce an issue they experienced. The provided code can be run on a local machine, the pre-requisites are listed in the [Readme](https://github.com/Particular/EndToEnd/blob/master/README.md) file in the repository.

## How it works?

The tests are standard integration-style tests. SQLServer Express or higher needs to be installed to run the tests as described in the [Readme](https://github.com/Particular/EndToEnd/blob/master/README.md) file.


### Pulling packages from NuGet

The tests cover all supported versions of NServiceBus and related packages (persistences, transports, etc.), including all minor versions. The available version numbers within a specified range are automatically determined based on the versions available on the NuGet feed. They are also automatically downloaded and stored locally as part of the test runs.


### Versions combinations

In order to ensure cross-version compatibility versions combinations are generated automatically within a given range. Each test case is executed across all combinations of minor versions within specified range of supported versions.
