---
title: Setting a automatic expiration for ServiceControl data
summary: How to configure ServiceControl to automatically purge old data after a user defined expiration period
tags:
- ServiceControl
- Expiration
- Configuration
---
ServiceControl is primarly a monitoring tool whose role is to intercept and store all the traffic, in term of messages, that flows into the monitored system; given its nature is expected that the quantity of messages and data stored by ServiceControl increases without limit.

By default ServiceControl purges old data periodically, checking each minute and deleting data older than 30 days.

it is possible to control the above behavior using the following settings:

* ExpirationProcessTimerInSeconds, the default is once a minute;
* HoursToKeepMessagesBeforeExpiring, the default is 30 days;

In order to change the ServiceControl behavior the above settings must be added to the ServiceControl configuration file, located in the installation directory, e.g.:

```xml 
	<add key="ExpirationProcessTimerInSeconds" value="new-integer-value" />
	<add key="HoursToKeepMessagesBeforeExpiring" value="new-integer-value" />
```