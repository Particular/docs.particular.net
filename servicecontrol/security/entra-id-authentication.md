---
title: Microsoft Entra ID Authentication Guide
summary: Set up authentication with Microsoft Entra ID for ServiceControl and ServicePulse
reviewed: 2026-01-13
component: ServiceControl
related:
- servicecontrol/security/configuration/authentication
- servicepulse/security/configuration/authentication
---

This guide explains how to configure Microsoft Entra ID (formerly Azure Active Directory) as the identity provider for ServiceControl and ServicePulse.

## Prerequisites

- Administrator permissions on the Microsoft Entra ID tenant
- ServiceControl 6.9.0 or later
- ServicePulse 2.5.0 or later

## Overview

Two app registrations are required in Microsoft Entra ID:

1. **ServiceControl API** - Represents the ServiceControl API that ServicePulse will call
2. **ServicePulse** - Represents the ServicePulse single-page application that users sign into

## Step 1: Register the ServiceControl API

Follow Microsoft's guide to [register an application](https://learn.microsoft.com/en-us/entra/identity-platform/quickstart-register-app) with the following settings:

| Setting                 | Value                                                          |
|-------------------------|----------------------------------------------------------------|
| Name                    | `ServiceControl API`                                           |
| Supported account types | Accounts in this organizational directory only (single tenant) |

After registration, collect these values from the app registration:

| Value                 | Location      | Used for                          |
|-----------------------|---------------|-----------------------------------|
| Directory (tenant) ID | Overview page | Authority URLs                    |
| Application ID URI    | Expose an API | `Authentication.Audience` setting |

### Expose the API

Follow Microsoft's guide to [expose a web API](https://learn.microsoft.com/en-us/entra/identity-platform/quickstart-configure-app-expose-web-apis) and add a scope with these settings:

| Setting                    | Value                                        |
|----------------------------|----------------------------------------------|
| Application ID URI         | Accept the default (`api://{app-id}`)        |
| Scope name                 | `api.access`                                 |
| Who can consent            | Admins and users                             |
| Admin consent display name | `Full access to ServiceControl API`          |
| Admin consent description  | `Allows ServicePulse to call ServiceControl` |

## Step 2: Register ServicePulse

Follow Microsoft's guide to [register an application](https://learn.microsoft.com/en-us/entra/identity-platform/quickstart-register-app) with the following settings:

| Setting                 | Value                                                                            |
|-------------------------|----------------------------------------------------------------------------------|
| Name                    | `ServicePulse`                                                                   |
| Supported account types | Accounts in this organizational directory only (single tenant)                   |
| Redirect URI - Platform | Single-page application (SPA)                                                    |
| Redirect URI - URI      | The URL where ServicePulse is hosted (e.g. `https://servicepulse.example.com/`) |

After registration, collect this value:

| Value                   | Location      | Used for                                      |
|-------------------------|---------------|-----------------------------------------------|
| Application (client) ID | Overview page | `Authentication.ServicePulse.ClientId` setting |

> [!WARNING]
> Redirect URIs must use HTTPS in production. HTTP is only acceptable for local development. If ServicePulse is accessed from multiple URLs, add each as a redirect URI under **Manage** > **Authentication**.

### Grant API permissions

Follow Microsoft's guide to [configure a client application to access a web API](https://learn.microsoft.com/en-us/entra/identity-platform/quickstart-configure-app-access-web-apis):

1. In the ServicePulse app registration, go to **API permissions**
2. Add a permission for **ServiceControl API** (under My APIs)
3. Select the **api.access** delegated permission

## Step 3: Configure ServiceControl

Use the collected values to configure ServiceControl. For Entra ID, the authority URLs follow this pattern:

- **ServiceControl authority**: `https://login.microsoftonline.com/{tenant-id}`
- **ServicePulse authority**: `https://login.microsoftonline.com/{tenant-id}/v2.0`

The following table summarizes how Entra ID values map to ServiceControl settings:

| Entra ID value                       | ServiceControl setting                   |
|--------------------------------------|------------------------------------------|
| Directory (tenant) ID                | Used in `Authentication.Authority` URL   |
| Application ID URI                   | `Authentication.Audience`                |
| Application ID URI + `/api.access`   | `Authentication.ServicePulse.ApiScopes`  |
| ServicePulse Application (client) ID | `Authentication.ServicePulse.ClientId`   |

See [Authentication Configuration](configuration/authentication.md) for all settings and configuration examples, including App.config and environment variable formats.

> [!NOTE]
> All ServiceControl instances (Primary, Audit, and Monitoring) must be configured with the same authority and audience values. ServicePulse settings are only required on the Primary instance.

## Verify the configuration

After configuring ServiceControl, restart all instances. When accessing ServicePulse:

1. The browser should redirect to the Microsoft sign-in page
2. After signing in, ServicePulse should load and display data from ServiceControl

If authentication fails, check the ServiceControl logs for token validation errors.
