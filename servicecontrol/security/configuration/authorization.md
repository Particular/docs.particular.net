---
title: ServiceControl Role-Based Access Control
summary: Restrict what authenticated users can do in ServiceControl and ServicePulse using roles
reviewed: 2026-07-01
component: ServiceControl
related:
- servicecontrol/security/configuration/authentication
- servicepulse/security/configuration/authorization
---

Once [authentication](authentication.md) is enabled, ServiceControl can additionally enforce _authorization_: restricting what each authenticated user is allowed to do based on the roles carried in their access token. This is known as role-based access control (RBAC).

Authentication answers _who is calling_; authorization answers _what they are allowed to do_. Authorization requires authentication to be enabled first.

> [!NOTE]
> Role-based authorization is disabled by default. When it is disabled — but authentication is enabled — every authenticated user is granted every permission. Enable authorization to restrict actions per user.

## How it works

Each ServiceControl instance (Primary, Audit, and Monitoring) is a separate process that validates the caller's token and makes its own authorization decisions. When a request arrives:

1. The instance reads the user's roles from a configured claim in the token.
2. Each role is expanded into a set of _permissions_.
3. The requested API action is allowed only if the user's roles grant the matching permission.
4. The decision is written to the [authorization audit log](#authorization-audit-log).

ServicePulse never learns the internal permission vocabulary. Instead, each instance exposes a per-user manifest of the routes the current token may call (`GET /api/my/routes` on the Primary instance and the equivalent on Monitoring). ServicePulse uses this manifest to [show or hide UI actions](/servicepulse/security/configuration/authorization.md). Because the enforcement logic and the manifest are computed from the same inputs, what ServicePulse displays cannot drift from what the server actually allows.

## Permission model

Permissions are strings in the format `instance:resource:action`, for example:

- `error:messages:retry` — retry failed messages on the error (Primary) instance
- `audit:message:view` — view audited messages on the Audit instance
- `monitoring:endpoint:delete` — remove a monitored endpoint on the Monitoring instance

The `instance` segment (`error`, `audit`, or `monitoring`) namespaces permissions per process, so each instance authorizes only the actions it serves.

## Built-in roles

ServiceControl ships with three built-in roles. Assign one or more of these role names to users in the identity provider.

| Role     | Grants                                                                                                                                                                    |
|----------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `reader` | Read-only access to everything (every `view` permission across all instances).                                                                                           |
| `admin`  | Everything `reader` grants, plus full management of the configuration and administration areas (licensing, notifications, retry redirects, throughput, and connections). |
| `writer` | Full access — every permission on every instance, including message triage actions such as retry, edit, archive, and restore.                                            |

A user with no recognized role has no permissions and, in ServicePulse, sees a read-only or empty interface for the affected areas.

> [!NOTE]
> Role names are matched case-insensitively. Custom roles are not currently supported; only `reader`, `admin`, and `writer` grant permissions.

## Reading roles from the token

Identity providers place roles in different claims and in different shapes. Use the [`Authentication.RolesClaim`](/servicecontrol/servicecontrol-instances/configuration.md#authentication-servicecontrolauthentication-rolesclaim) setting to tell ServiceControl where to find them. Both a flat claim name and a dotted path into a nested JSON object are supported.

| Identity provider                                | Typical `RolesClaim` value | Notes                                                                                    |
|--------------------------------------------------|----------------------------|------------------------------------------------------------------------------------------|
| Keycloak (realm roles)                           | `realm_access.roles`       | Roles are nested inside the `realm_access` object; a dotted path reaches them.           |
| Keycloak (with a realm-role mapper) / generic    | `roles`                    | A flat top-level array of role strings.                                                  |
| Microsoft Entra ID (app roles)                   | `roles`                    | Configure app roles named `reader`, `admin`, or `writer` and assign them to users.       |
| AWS Cognito (groups)                             | `cognito:groups`           | Name the Cognito groups `reader`, `admin`, or `writer`.                                  |

The value at the configured path may be a single string or an array of strings; both are accepted. Malformed or missing claims yield no roles rather than an error.

> [!NOTE]
> The role names emitted by the identity provider must exactly match the [built-in role names](#built-in-roles) (`reader`, `admin`, `writer`), aside from casing. Map or rename provider-side roles accordingly.

## Enabling authorization

Enable authorization on **every** instance (Primary, Audit, and Monitoring), because each instance authorizes its own requests. For a complete list of settings and per-instance app config keys and environment variables, see:

- [Primary Instance authentication settings](/servicecontrol/servicecontrol-instances/configuration.md#authentication)
- [Audit Instance authentication settings](/servicecontrol/audit-instances/configuration.md#authentication)
- [Monitoring Instance authentication settings](/servicecontrol/monitoring-instances/configuration.md#authentication)

The following example enables authorization on the Primary instance and reads roles from a Keycloak token:

```xml
<!-- Authentication must already be enabled -->
<add key="ServiceControl/Authentication.Enabled" value="true" />
<add key="ServiceControl/Authentication.Authority" value="https://{keycloak-host}/realms/{realm}" />
<add key="ServiceControl/Authentication.Audience" value="{api-client-id}" />

<!-- Enable role-based authorization -->
<add key="ServiceControl/Authentication.RoleBasedAuthorizationEnabled" value="true" />
<add key="ServiceControl/Authentication.RolesClaim" value="realm_access.roles" />
```

## Authorization audit log

Every authorization decision — both allowed and denied — is written to a dedicated audit log so that access can be reviewed and, where required, retained for compliance. Entries are emitted under the log category `ServiceControl.Audit` and formatted as [Elastic Common Schema (ECS)](https://www.elastic.co/guide/en/ecs/current/index.html) JSON, so they can be ingested into Elastic/Kibana or most SIEM (Security Information and Event Management) systems without custom mapping.

Each entry records:

- The subject identifier and display name of the caller (see [audit identity claims](#authorization-audit-log-audit-identity-claims))
- The permission that was checked and the optional resource
- Whether access was allowed or denied, and the reason

Allowed decisions are logged at `Information` level and denied decisions at `Warning` level, so alerting can key on denials without parsing the payload.

### Audit identity claims

The audit log identifies the caller using two token claims, configurable per instance:

- [`Authentication.SubjectIdClaim`](/servicecontrol/servicecontrol-instances/configuration.md#authentication-servicecontrolauthentication-subjectidclaim) — a stable identifier for the user (default `sub`).
- [`Authentication.SubjectNameClaim`](/servicecontrol/servicecontrol-instances/configuration.md#authentication-servicecontrolauthentication-subjectnameclaim) — a human-readable display name (default `preferred_username`).

If either claim is missing from the token, the request is rejected. A missing claim usually means the identity provider is not configured to emit it; correct the provider or point these settings at claims the token does contain.

## Disabling authorization

To keep authentication (require a valid token) but grant every authenticated user full access, leave `Authentication.RoleBasedAuthorizationEnabled` set to `false` (the default). In this mode roles are not consulted and all permissions are granted.
