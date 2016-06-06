---
title: Cross-version compatibility testing with NServiceBus
summary: Develop service layers and long-running processes using test-driven development.
reviewed: 2016-06-06
related:
 - samples/unit-testing
---

In order to ensure that NServiceBus upgrades are safe and that endpoints in various versions can communicate with each other, it is necessary to extensively test multiple aspects of cross-version compatibility. As much as possible, those tests are automated. Some of them are available publicly in the [Particular/EndToEnd](https://github.com/Particular/EndToEnd/) repository.

Those tests might be useful for NServiceBus users in order to verify that the framework works correctly in an unusual scenario, for example when using an unusually complex data structure, in order to understand in detail how specific feature works, or to reproduce an issue they experienced. The provided code can be run on a local machine, the pre-requisites are listed in the [Readme](https://github.com/Particular/EndToEnd/blob/master/README.md) file in the repository.


## How it works?


### Pulling packages from NuGet

The tests cover all supported versions of NServiceBus and related packages (persistences, transports, etc.), including all minor versions. The available version numbers within a specified range are automatically determined based on the NuGet feeds. They are also automatically pulled to be stored locally before running the tests.


### Versions combinations

In order to ensure cross-version compatibility versions combinations are generated automatically within a given range. The test cases are executed across all combinations of minor versions within a specified range of versions.
