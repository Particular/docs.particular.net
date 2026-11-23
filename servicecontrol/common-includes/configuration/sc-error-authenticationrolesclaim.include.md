### ServiceControl/Authentication.RolesClaim

_Added in version 6.18.0_

The token claim from which user roles are read. Supports a flat claim name (for example `roles`) or a dotted path into a nested JSON object claim (for example `realm_access.roles`). See [reading roles from the token](/servicecontrol/security/configuration/authorization.md#reading-roles-from-the-token).

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUTHENTICATION_ROLESCLAIM` |
| **App config key** | `ServiceControl/Authentication.RolesClaim` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `roles` |
