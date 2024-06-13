---
title: Configure Monitoring instance URI
summary: How to configure a Monitoring instance to be exposed through a custom hostname and IP port
reviewed: 2024-07-09
redirects:
- servicecontrol/monitoring-instances/installation/setting-custom-hostname
- servicecontrol/monitoring-instances/installation/configure-the-uri
---


## Changing the Monitoring instance URI

To set a custom hostname and IP port for an instance of the Monitoring instance HTTP API:

> [!NOTE]
> Anyone who can access the Monitoring instance URL has complete access to the endpoint data stored by the Monitoring instance. This is why the default is to only respond on `localhost`. Carefully consider the implications before exposing a Monitoring instance via a custom or wildcard URI.


### Using ServiceControl Management

 1. Click the Configuration Icon for for the Service Instance to modify.
 1. Change the Host and Port number fields.
 1. Click Save.

ServiceControl Management will validate and restart the service to apply the changes.


### Updating ServicePulse Configuration to the Monitoring instance custom hostname

Refer to the [ServicePulse documention](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui) for guidance on how to configure ServicePulse to connect to ServiceControl Monitoring.
