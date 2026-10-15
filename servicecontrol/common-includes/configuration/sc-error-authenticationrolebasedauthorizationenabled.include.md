### ServiceControl/Authentication.RoleBasedAuthorizationEnabled

_Added in version 6.18.0_

Enables [role-based access control](/servicecontrol/security/configuration/authorization.md). When `false`, every authenticated user is granted every permission. Requires `Authentication.Enabled` to be `true`; the instance refuses to start if this is enabled while authentication is disabled.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUTHENTICATION_ROLEBASEDAUTHORIZATIONENABLED` |
| **App config key** | `ServiceControl/Authentication.RoleBasedAuthorizationEnabled` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| bool | `false` |
