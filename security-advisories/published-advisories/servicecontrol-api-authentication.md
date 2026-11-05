---
title: Security Advisory 2026-10-09
summary: ServiceControl API accepts requests without a valid token, and returns the stored SMTP password
reviewed: 2026-10-14
---


This advisory discloses two security vulnerabilities that have been found in [ServiceControl](/servicecontrol/) and fixed in ServiceControl 6.21.2.

- ServiceControl versions 6.18.0 to 6.21.1 that have [authentication](/servicecontrol/security/configuration/authentication.md) enabled and [role-based authorization](/servicecontrol/security/configuration/authorization.md) disabled should be upgraded to version 6.21.2 or later.
- ServiceControl instances that have email notifications configured with an SMTP password should be upgraded to version 6.21.2 or later.
- Instances that do not match either case are not affected.

If there are any questions or concerns regarding this advisory, send an email to [security@particular.net](mailto:security@particular.net).

## ServiceControl API accepts requests without a valid token

A vulnerability has been fixed in the ServiceControl error, audit, and monitoring instances. When authentication is enabled and role-based authorization is disabled, the API accepts requests without a bearer token, with an invalid token, or with an expired token.

This vulnerability is tracked as [GHSA-gqfq-8749-hgcf](https://github.com/Particular/ServiceControl/security/advisories/GHSA-gqfq-8749-hgcf).

### Impact

An attacker can use the ServiceControl API as if they were a signed-in user. This includes reading failed and audited message bodies and headers, retrying, archiving, and deleting failed messages, changing message redirects, removing endpoints, and changing the email notification settings. With the SQL Server and PostgreSQL persisters, an attacker can also delete stored data.

### Exploitability

The exploitation of this vulnerability requires that all of the conditions below are met at the same time:

1. `Authentication.Enabled` is `true`.
1. `Authentication.RoleBasedAuthorizationEnabled` is `false`, which is the default.
1. The attacker can send HTTP requests to the ServiceControl instance.

Instances with authentication disabled are not affected by this vulnerability. These instances do not require a token by design, and their API is open to every caller that can reach it.

### Affected versions

ServiceControl versions 6.18.0 to 6.21.1 are affected. Versions 6.11.0 to 6.17.x are not affected.

### Risk mitigation

If it is not possible to immediately upgrade ServiceControl, use role-based authorization as a **temporary workaround**:

1. Make sure that the identity provider assigns a role to every user. Users without a role are denied access.
1. Enable [role-based authorization](/servicecontrol/security/configuration/authorization.md#how-to-enable-role-based-access-control) on every instance.

### Fix

Upgrade every ServiceControl instance (error, audit, and monitoring) to version 6.21.2 or later.

After the upgrade, requests without a valid token are rejected with `401 Unauthorized` on every endpoint that requires authentication. This affects:

- Scripts and tools that call the ServiceControl API without a token. Configure them to send a valid token.
- Container health checks or probes that call `/connection` on a monitoring instance. Use the root endpoint (`/`) instead. The health check in the monitoring container image uses the root endpoint from version 6.21.2.

## SMTP password returned by the email notification settings API

A vulnerability has been fixed in the ServiceControl error instance. The API that returns the email notification settings also returns the stored SMTP password.

This vulnerability is tracked as [GHSA-vw2m-7w4j-7jj6](https://github.com/Particular/ServiceControl/security/advisories/GHSA-vw2m-7w4j-7jj6).

### Impact

An attacker can read the password of the SMTP account that ServiceControl uses to send email notifications. With that password, the attacker can send email through the SMTP account.

> [!CAUTION]
> Treat the SMTP password of every affected instance as leaked. The password must be changed.

### Exploitability

The exploitation of this vulnerability requires that all of the conditions below are met at the same time:

1. Email notifications are configured with an SMTP account and password. Turning email notifications off does not remove a stored password.
1. The attacker can read the email notification settings through the ServiceControl API. With authentication disabled, which is the default, every caller that can reach the API can read them. With role-based authorization enabled, every role can read them.

Instances that never had email notifications configured with an SMTP password are not affected.

### Affected versions

ServiceControl versions 4.17.0 to 6.21.1 are affected.

### Risk mitigation

If it is not possible to immediately upgrade ServiceControl:

- Change the password of the SMTP account.
- If email notifications are not needed, remove the SMTP account and password from the email notification settings.
- If email notifications are needed, limit what the SMTP account can send. See [securing the SMTP account](/servicecontrol/security/smtp-account.md). The API can still return the new password until the upgrade. Change the password again after the upgrade.

### Fix

Upgrade the ServiceControl error instance to version 6.21.2 or later. From this version, the API does not return the stored SMTP password.

There is no patch release for versions 5.x and 4.x. To upgrade from version 5.x, see the [upgrade guide from version 5 to 6](/servicecontrol/upgrades/5to6.md).

[Versions 5.x and 4.x have reached end of life](/servicecontrol/upgrades/support-policy.md#end-of-life). Customers on version 5.x that cannot upgrade can contact [security@particular.net](mailto:security@particular.net) to see how Particular Software can best support them.

After the upgrade, change the password of the SMTP account. Then enter the new password in the email notification settings.

## Contact info

If there are any questions or concerns regarding this advisory, contact [security@particular.net](mailto:security@particular.net).
