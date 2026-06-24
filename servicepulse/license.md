---
title: Licensing ServicePulse
summary: Configure, manage and troubleshoot ServicePulse license
reviewed: 2026-06-16
component: ServicePulse
related: 
- servicecontrol/license
- nservicebus/licensing
---

ServicePulse reads license information from the ServiceControl Error instance via the HTTP API when a route is loaded. Once the [license is updated in ServiceControl](/servicecontrol/license.md), the update will be detected by ServicePulse the next time the page is refreshed or navigation to a new route occurs.

## Behavior

The license is constantly checked while using the ServicePulse, so there is no need to restart or refresh the page once the license file has been modified and ServiceControl has re-read it.

### Expired license

If the license has expired (or is corrupted), all application features will be disabled, except for the ability to request a new license, request a trial extension (for trial licenses only), generate a usage report, or contact customer support.
Once a new license is attached, all functionality will be restored.

## Troubleshooting

### ServiceControl license was updated, but ServicePulse reports the license has expired

License information is reported to ServicePulse by the ServiceControl Error instance, which reads the license file at startup and caches it for 8 hours. Restarting the ServiceControl Error instance or waiting for the cache to expire will resolve the issue.
