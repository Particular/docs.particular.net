---
title: ServicePulse Role-Based Access Control
summary: How ServicePulse adapts its interface to the permissions granted to the signed-in user
reviewed: 2026-07-01
component: ServicePulse
related:
- servicepulse/security/configuration/authentication
- servicecontrol/security/configuration/authorization
---

When [role-based access control](/servicecontrol/security/configuration/authorization.md) is enabled in ServiceControl, ServicePulse tailors its interface to the permissions of the signed-in user. Tabs, pages, and action buttons that the user is not permitted to use are hidden or disabled, so users see only the actions they can actually perform.

> [!IMPORTANT]
> Authorization is configured entirely in ServiceControl. ServicePulse has no authorization configuration of its own; it reads the permitted actions from ServiceControl. See the [ServiceControl authorization guide](/servicecontrol/security/configuration/authorization.md).

## How it works

When a user signs in, ServicePulse adapts its interface to the user's [role](/servicecontrol/security/configuration/authorization.md#built-in-roles):

- Navigation tabs and pages the user cannot access are hidden.
- Action buttons (for example _Retry_, _Edit_, _Delete_, _Restore_) the user cannot use are shown as disabled, with a tooltip explaining why, rather than failing after a click.

ServiceControl enforces access on every request regardless of what ServicePulse displays, so hiding and disabling elements is a usability aid, not a security boundary. A user cannot bypass a restriction by calling the API directly.

## Reviewing your permissions

ServicePulse includes a **Your permissions** page under **Configuration** that lists each area of the product and shows, for the signed-in user, which capabilities are allowed and which are not. Use it to confirm that a user's roles map to the expected access, or to diagnose why an action is unavailable.

## Troubleshooting

### An action is unexpectedly disabled

The signed-in user's roles do not grant the required permission. Check the **Your permissions** page to see which capabilities are allowed, and verify the user's role assignments in the identity provider match the [built-in role names](/servicecontrol/security/configuration/authorization.md#built-in-roles).

### All actions are visible even though authorization is enabled

ServicePulse could not determine the user's permissions and is showing everything rather than blocking access. Confirm that ServiceControl is reachable and that `Authentication.RoleBasedAuthorizationEnabled` is `true` on each instance. ServiceControl still rejects any action the user is not permitted to perform.
