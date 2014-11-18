---
title: How to configure ServicePulse's port
summary: Steps to configure ServicePulse's port
tags:
- ServicePulse
- HowTo
---
**Work In Progress**

**Changin ServicePulse's port**
The port is set in the registry: HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Particular.ServicePulse\imagepath 

To modify it, replace :9090 to the desired port and then restart service.

NOTE:   if you upgrade ServicePulse you need to check this entry was not overwritten by the installer.
