### ServiceControl/Authentication.ServicePulse.OfflineAccessScopeEnabled

_Added in version 6.18.3_

Whether ServicePulse should request the `offline_access` scope. ServiceControl composes the complete scope string ServicePulse uses to sign in from `ServicePulse.ApiScopes` plus the fixed `openid profile email` scopes and, unless this setting is `false`, `offline_access`.

Some identity providers reject the entire authorization request if a client requests a scope it isn't permitted to use. If the client/app registration used by ServicePulse isn't allowed to request `offline_access`, set this to `false` so ServicePulse omits it instead of failing to reach the login page. Leave this at its default in all other cases: without `offline_access`, some identity providers don't issue a refresh token, so ServicePulse falls back to a hidden-iframe silent renew that depends on third-party cookies, which browsers like Safari and Brave restrict. When that fallback fails, ServicePulse re-authenticates with a full-page redirect at every token expiry instead of renewing silently.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_OFFLINEACCESSSCOPEENABLED` |
| **App config key** | `ServiceControl/Authentication.ServicePulse.OfflineAccessScopeEnabled` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `true` |
