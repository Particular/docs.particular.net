---
title: Running locally without Azure Cloud Services emulator
summary: Debugging endpoints without Azure Cloud Services emulator
component: CloudServicesHost
reviewed: 2020-08-23
---

include: cloudserviceshost-deprecated-warning

Debugging endpoints hosted with Cloud Services requires starting up Azure Cloud Service emulator and loading endpoint in the emulator. It's a resource intensive operation and if no scale out of roles is required, can be done by loading endpoints using a host process provided by NServiceBus instead.

Following these steps will enable local execution of endpoints without Azure Cloud Service emulator:

Partial: running-locally
