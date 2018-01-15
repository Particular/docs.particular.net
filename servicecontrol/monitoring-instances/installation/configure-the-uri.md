---
title: Configure the URI
summary: How to configure a Monitoring instance to be exposed through a custom hostname and IP port
reviewed: 2017-07-29
redirects:
- servicecontrol/monitoring-instances/installation/setting-custom-hostname
---


## Changing the Monitoring instance URI

To set a custom hostname and IP port for an instance of the Monitoring instance HTTP API:

NOTE: Anyone who can access the Monitoring instance URL has complete access to the endpoint data stored by the Monitoring instance. This is  why the default is to only respond to the localhost. Consider carefully the implications of exposing a Monitoring instance via a custom or wildcard URI.


### Using ServiceControl Management

 1. Click the Configuration Icon for for the Service Instance to modify.
 1. Change the Host and Port number fields to the desired values.
 1. Click Save.

ServiceControl Management will then validate the settings changes and restart the service to apply the changes.


### Updating ServicePulse Configuration to Monitoring instance Custom Hostname

 1. Update the ServicePulse configuration file to access the updated Monitoring instance hostname & port number. By default, the ServicePulse configuration file is located in `[Program Files]\Particular Software\ServicePulse\app\config.js`.
 1. Update the value of the `monitoring_urls` parameter to the specified Monitoring instance hostname and IP port number.
 1. When next accessing ServicePulse, make sure to refresh the browser cache to allow ServicePulse to access the updated configuration settings.
