---
title: Configuration API License in V3 and V4
summary: Configuration API License in V3 and V4
tags:
- NServiceBus
- BusConfiguration
- V3
- V4
---

The methods of assigning the license to an endpoint are all detailed in the [How to install your license file](/nservicebus/license-management) article. You can also specify a license at configuration time:

* `LicensePath( string partToLicenseFile )`: configures the endpoint to use the license file found at the supplied path;
* `License( string licenseText )`: configures the endpoint to use the supplied license, where the license text is the content of a license file.