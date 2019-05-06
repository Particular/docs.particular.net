---
title: Configure the Monitoring instance URI
summary: How to configure a Monitoring instance to be exposed through a custom hostname and IP port
reviewed: 2019-05-06
redirects:
- servicecontrol/monitoring-instances/installation/setting-custom-hostname
---


## Changing the Monitoring instance URI

This article describes the steps required to set a custom hostname and IP port for an instance of the Monitoring HTTP API.

NOTE: Anyone who can access the Monitoring instance URL has complete access to the endpoint data stored by the Monitoring instance. This is  why the default is to only respond to `localhost`. Consider carefully the implications of exposing a Monitoring instance via a custom or wildcard URI.


### Using ServiceControl Management

 1. Click the Configuration Icon for for the Monitoring instance to modify.
 1. Change the Host Name and Port Number fields to the desired values.
 1. Click Save.

ServiceControl Management will then validate the settings changes and restart the service to apply the changes.


### Updating ServicePulse Configuration to the Monitoring instance custom hostname

 1. Update the ServicePulse configuration file to access the updated Monitoring instance hostname and port number. By default, the ServicePulse configuration file is located in `[Program Files]\Particular Software\ServicePulse\app\config.js`.
 1. Update the value of the `monitoring_urls` parameter to the specified Monitoring instance hostname and port number.
 1. When next accessing ServicePulse, make sure to refresh the browser cache to allow ServicePulse to access the updated configuration settings.
