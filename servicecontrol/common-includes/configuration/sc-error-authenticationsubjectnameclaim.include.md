### ServiceControl/Authentication.SubjectNameClaim

_Added in version 6.18.0_

The token claim used as the human-readable display name in the [authorization audit log](/servicecontrol/security/configuration/authorization.md#authorization-audit-log). Requests are rejected if this claim is missing from the token.

| Context | Name |
| --- | --- |
| **Environment variable** | `SERVICECONTROL_AUTHENTICATION_SUBJECTNAMECLAIM` |
| **App config key** | `ServiceControl/Authentication.SubjectNameClaim` |
| **SCMU field** | N/A |

| Type | Default value |
| --- | --- |
| string | `preferred_username` |
