---
title: Configure Monitoring instance URI
summary: How to configure a Monitoring instance to be exposed through a custom hostname and IP port
reviewed: 2026-04-17
redirects:
- servicecontrol/monitoring-instances/installation/setting-custom-hostname
- servicecontrol/monitoring-instances/installation/configure-the-uri
---

> [!NOTE]
> By default, anyone who can access the Monitoring instance URL has a complete access to data stored by the instance. This is why the default URL on which the instance servers the requests equals to `localhost`. Before deploying to any environment security implications should be carefully considered follwoing [the platfrom hosting guidles](/servicecontrol/security/hosting-guide.md).

## Changing the Monitoring instance URI

The default values for hostname and IP port of a Monitoring instance can be changed using one of the following methods.

### Using ServiceControl Management

 1. Click the Configuration Icon for for the Service Instance to modify.
 1. Change the Host and Port number fields.
 1. Click Save.

ServiceControl Management will validate and restart the service to apply the changes.

### Updating ServicePulse Configuration to the Monitoring instance custom hostname

Refer to the [ServicePulse documention](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui) for guidance on how to configure ServicePulse to connect to ServiceControl Monitoring.
