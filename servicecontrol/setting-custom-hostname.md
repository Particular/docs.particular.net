---
title: Configure the URI
summary: How to configure ServiceControl to be exposed through a custom hostname and IP port
reviewed: 2016-11-09
---


## Changing the ServiceControl URI

To set a custom hostname and IP port for an instance of the ServiceControl service:

NOTE: Anyone who can access the ServiceControl URL has complete access to the message data stored within ServiceControl. This is  why the default is to only respond to the localhost. Consider carefully the implications of exposing ServiceControl via a custom or wildcard URI.


### Using ServiceControl Management

 1. Click the Configuration Icon for for the Service Instance to modify.
 1. Change the Host and Port number fields to the desired values.
 1. Click Save.

ServiceControl Management will then validate the settings changes and restart the service to apply the changes.


### Updating ServicePulse Configuration to ServiceControl Custom Hostname

 1. Update the ServicePulse configuration file to access the updated ServiceControl hostname & port number. By default, the ServicePulse configuration file is located in `[Program Files]\Particular Software\ServicePulse\app\app.constants.js`.
 1. Update the value of the `service_control_url` parameter to the specified ServiceControl hostname and IP port number.
 1. When next accessing ServicePulse, make sure to refresh the browser cache to allow ServicePulse to access the updated configuration settings.
