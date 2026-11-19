### ServiceControl.Audit/Authentication.RolesClaim

_Added in version 6.18.0_

The token claim from which user roles are read. Supports a flat claim name or a dotted path into a nested JSON object claim. See [reading roles from the token](/servicecontrol/security/configuration/authorization.md#reading-roles-from-the-token).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUDIT_AUTHENTICATION_ROLESCLAIM` |
| **App config key** | `ServiceControl.Audit/Authentication.RolesClaim` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `roles` |
