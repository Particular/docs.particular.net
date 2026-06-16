---
title: Licensing ServicePulse
summary: Configure, manage and troubleshoot ServicePulse license
reviewed: 2026-06-16
component: ServicePulse
related: 
- servicecontrol/license
- nservicebus/licensing
---

Licensing for ServicePulse depends on the [licensing configuration of ServiceControl](/servicecontrol/license), meaning that once the license is correctly configured for ServiceControl, nothing else needs to be modified in ServicePulse.
More specifically, the license information is read by ServicePulse from the ServiceControl Error instance via the HTTP API.

## Behavior

The license is constanly checked while using the application, so there is no need to restart or refresh the page once the license file has been modified.

### Expired license

If the license has expired (or is corrupted), then all the features of the application will be disabled, except for the ability to request a new license, request a trial extension (for trial licenses only) or contact customer support.
Once a new license has been attached, then all functionalities will be restored.

## Troubleshooting

### ServiceControl license was updated, but ServicePulse reports the license has expired

The license information depends on the license information on the ServiceControl Error instance. Such instances read the license file during startup, which is cached for 8 hours. Therefore, either wait for the cache to expire or restart the ServiceControl Error instance manually so that ServicePulse reflects the new license.
