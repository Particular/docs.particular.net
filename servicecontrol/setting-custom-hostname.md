---
title: Configure the ServiceControl URI
summary: How to configure ServiceControl to be exposed through a custom hostname and IP port
reviewed: 2021-11-15
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

Refer to the [ServicePulse documention](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui) for guidance on how to configure ServicePulse to connect to ServiceControl.
