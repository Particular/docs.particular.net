---
title: ServiceControl Role-Based Access Control
summary: Restrict what authenticated users can do in ServiceControl and ServicePulse using roles
reviewed: 2026-07-01
component: ServiceControl
related:
- servicecontrol/security/configuration/authentication
- servicepulse/security/configuration/authorization
---

Once [authentication](authentication.md) is enabled, ServiceControl can additionally enforce _authorization_: restricting what each authenticated user is allowed to do based on the roles assigned to them in the identity provider. This is known as role-based access control (RBAC).

Authentication answers _who is calling_; authorization answers _what they are allowed to do_. Authorization requires authentication to be enabled first.

> [!NOTE]
> Role-based authorization is disabled by default. When it is disabled — but authentication is enabled — every authenticated user is granted full access. Enable authorization to restrict actions per user.

## How it works

When authorization is enabled, ServiceControl reads the user's roles from their access token and, for each request, allows or denies the action based on those roles. [ServicePulse](/servicepulse/security/configuration/authorization.md) adapts its interface to match, hiding or disabling the actions a user is not permitted to perform.

## Built-in roles

Access is granted through three built-in roles. Assign one or more of these role names to users in the identity provider:

| Role     | Access                                                                                                                                                              |
|----------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `reader` | Read-only access to everything.                                                                                                                                    |
| `admin`  | Everything `reader` can do, plus managing the configuration and administration areas (licensing, notifications, retry redirects, throughput, and connections).     |
| `writer` | Full access, including message actions such as retry, edit, archive, and restore.                                                                                  |

Role names are matched case-insensitively. A user with none of these roles has no access to the affected areas.

> [!NOTE]
> Only `reader`, `admin`, and `writer` grant access. Custom role names are not supported, so provider-side roles must be named to match.

## How to enable role-based access control

1. Upgrade to ServiceControl 6.18.0 or later and ServicePulse 2.9.0 or later.
2. Enable [authentication](authentication.md) if it is not already enabled. Authorization has no effect without it.
3. In the identity provider, create roles named `reader`, `admin`, and `writer`, and assign them to users. Ensure the roles are included in the access token — see [reading roles from the token](#reading-roles-from-the-token) for where different providers place them.
4. On **every** ServiceControl instance (Primary, Audit, and Monitoring), set the following configuration keys:

    ```xml
    <!-- Turn on role-based authorization -->
    <add key="ServiceControl/Authentication.RoleBasedAuthorizationEnabled" value="true" />
    <!-- The token claim that carries the roles (see the table below) -->
    <add key="ServiceControl/Authentication.RolesClaim" value="roles" />
    ```

5. Restart all ServiceControl instances.

Each instance authorizes its own requests, so all instances must have authorization enabled. For the app config keys and environment variables of every instance type, see [Primary](/servicecontrol/servicecontrol-instances/configuration.md#authentication), [Audit](/servicecontrol/audit-instances/configuration.md#authentication), and [Monitoring](/servicecontrol/monitoring-instances/configuration.md#authentication) configuration.

## Reading roles from the token

Identity providers place roles in different claims and shapes. Use the [`Authentication.RolesClaim`](/servicecontrol/servicecontrol-instances/configuration.md#authentication-servicecontrolauthentication-rolesclaim) setting to tell ServiceControl where to find them. Both a flat claim name (for example `roles`) and a dotted path into a nested JSON object claim (for example `realm_access.roles`) are supported.

| Identity provider                             | Typical `RolesClaim` value | Notes                                                                              |
|-----------------------------------------------|----------------------------|-----------------------------------------------------------------------------------|
| Keycloak (realm roles)                        | `realm_access.roles`       | Roles are nested inside the `realm_access` object; a dotted path reaches them.     |
| Keycloak (with a realm-role mapper) / generic | `roles`                    | A flat top-level array of role strings.                                            |
| Microsoft Entra ID (app roles)                | `roles`                    | Configure app roles named `reader`, `admin`, or `writer` and assign them to users. |
| AWS Cognito (groups)                          | `cognito:groups`           | Name the Cognito groups `reader`, `admin`, or `writer`.                            |

The value at the configured path may be a single string or an array of strings; both are accepted.

## Authorization audit log

Every authorization decision — both allowed and denied — is written to a dedicated audit log so access can be reviewed and retained for compliance. Entries are emitted under the log category `ServiceControl.Audit` and formatted as [Elastic Common Schema (ECS)](https://www.elastic.co/guide/en/ecs/current/index.html) JSON, so they can be ingested into Elastic/Kibana or most SIEM (Security Information and Event Management) systems without custom mapping. Allowed decisions are logged at `Information` level and denied decisions at `Warning` level.

### Audit identity claims

The audit log identifies the caller using two token claims, configurable per instance:

- [`Authentication.SubjectIdClaim`](/servicecontrol/servicecontrol-instances/configuration.md#authentication-servicecontrolauthentication-subjectidclaim) — a stable identifier for the user (default `sub`).
- [`Authentication.SubjectNameClaim`](/servicecontrol/servicecontrol-instances/configuration.md#authentication-servicecontrolauthentication-subjectnameclaim) — a human-readable display name (default `preferred_username`).

If either claim is missing from the token, the request is rejected. A missing claim usually means the identity provider is not configured to emit it; correct the provider or point these settings at claims the token does contain.

## Disabling authorization

To keep authentication (require a valid token) but grant every authenticated user full access, leave `Authentication.RoleBasedAuthorizationEnabled` set to `false` (the default). In this mode roles are not consulted.
