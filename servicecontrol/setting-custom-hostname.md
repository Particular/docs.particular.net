---
title: Configure the ServiceControl URI
summary: How to configure ServiceControl to be exposed through a custom hostname and IP port
reviewed: 2024-05-28
---

To set a custom hostname and IP port for an instance of the ServiceControl service:

> [!WARN]
> Anyone who can access the ServiceControl URL has complete access to the message data stored within ServiceControl. This is  why the default is to only respond to the localhost. Consider carefully the implications of exposing ServiceControl via a custom or wildcard URI.

## Using ServiceControl Management

 1. Click the Configuration Icon for for the Service Instance to modify.
 1. Change the Host and Port number fields to the desired values.
   - Can be set to `*` to allow remote connections using any hostname
 1. Click Save.

See also:

- Setting [ServiceControl/HostName](/servicecontrol/creating-config-file.md#host-settings-servicecontrolhostname)
- Setting [ServiceControl.Audit/HostName](/servicecontrol/audit-instances/creating-config-file.md#host-settings-servicecontrolhostname)
 
ServiceControl Management will then validate the settings changes and restart the service to apply the changes.


## Updating ServicePulse Configuration to ServiceControl Custom Hostname

Refer to the [ServicePulse documention](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui) for guidance on how to configure ServicePulse to connect to ServiceControl.
