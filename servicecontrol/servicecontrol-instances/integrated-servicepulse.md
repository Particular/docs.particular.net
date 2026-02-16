---
title: Integrated ServicePulse
summary: A guide to hosting ServicePulse and ServiceControl within the same host process.
reviewed: 2026-02-16
component: ServiceControl
---

Version 6.12 of ServiceControl and above include integrated ServicePulse. Integrated ServicePulse is hosted in the same host process as the ServiceControl Error instance, and is automatically configured to use it.

Benefits:
- No need for a standalone ServicePulse installation. Install and manage ServiceControl and ServicePulse in one place.
- No need to configure ServicePulse with ServiceControl Error instance url. Integrated ServicePulse is preconfigured to connect to the ServiceControl installation it runs in.
- No need to upgrade ServicePulse. Each new version of ServiceControl includes the latest ServicePulse. Every time ServiceControl is upgraded, integrated ServicePulse is upgraded as well.

Drawbacks:
- Can not use built-in ServicePulse reverse proxy. Enable [security features of ServiceControl](/servicecontrol/security/) to secure access to ServiceControl data.
- Can not use a single ServicePulse instance to control multiple ServiceControl installations. Each ServiceControl Error instance can be configured with a dedicated integrated ServicePulse.

>[!NOTE]
>Standalone ServicePulse installations continue to function normally and can be used in conjunction with integrated ServicePulse.

## Enable integrated ServicePulse

Integrated ServicePulse can be [enabled in the ServiceControl Error instance configuration](/servicecontrol/servicecontrol-instances/configuration.md#servicecontrolenableintegratedservicepulse).

When upgrading an existing ServiceControl Error instance via ServiceControl Management, the user is prompted to enable integrated ServicePulse.

![ServiceControl Management Error instance upgrade screen](/servicecontrol/servicecontrol-instances/upgrade-enable-integrated-servicepulse.png)

When upgrading an existing ServiceControl Error instance via Powershell or docker, update the configuration manually to enable integrated ServicePulse.

>[!NOTE]
>Once integrated ServicePulse is enabled, standalone ServicePulse installations can be safely removed.

## Configuring integrated ServicePulse

Integrated ServicePulse shares settings with the ServiceControl Error instance it is hosted with (the hosting instance).

- All host settings (such as [host name](/servicecontrol/servicecontrol-instances/configuration.md#servicecontrolhostname) and [port number](/servicecontrol/servicecontrol-instances/configuration.md#servicecontrolport)) are shared with the hosting instance. Integrated ServicePulse is available at the root url (`http://localhost:33333/` in a default installation).
- All security settings are shared with the hosting instance. There is no need to enable [header forwarding](/servicecontrol/security/configuration/forward-headers.md), [CORS](/servicecontrol/security/configuration/cors.md), or [ServicePulse specific authorization configuration](/servicecontrol/servicecontrol-instances/configuration.md#servicecontrolauthenticationservicepulseclientid).
- Integrated ServicePulse is automatically configured to connect to the hosting instance. This configuration is read-only and cannot be changed.
- Connection to a ServiceControl Monitoring instance can be [configured via the ServicePulse UI](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui).

## Upgrade integrated ServicePulse

There is no need to upgrade integrated ServicePulse separately. Each release of ServiceControl contains the latest version of ServicePulse, and each new release of ServicePulse will trigger a release of ServiceControl. Upgrading the ServiceControl Error instance will upgrade integrated ServicePulse automatically.