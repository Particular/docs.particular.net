---
title: Microsoft Entra ID authentication
summary: Set up authentication with Microsoft Entra ID for ServiceControl and ServicePulse
reviewed: 2025-12-11
component: ServiceControl
---

This guide explains how to configure Microsoft Entra ID (formerly Azure Active Directory) and ServiceControl to enable authentication for ServicePulse.

## Prerequisites

- Administrator permissions on the Microsoft Entra ID tenant
- ServiceControl 6.9.0 or later
- ServicePulse 2.5.0 or later

## Configure Microsoft Entra ID

Register ServiceControl and ServicePulse as applications in Microsoft Entra ID to allow users to authenticate.

### Create the ServiceControl app registration

This app registration represents the ServiceControl API and defines the permissions that ServicePulse will request when users sign in.

1. Navigate to the [Azure Portal](https://portal.azure.com/).
2. Open **Microsoft Entra ID** and select **Manage** > **App registrations**.
3. Click **+ New registration**.
4. Configure the registration:
   - **Name**: `ServiceControl API`
   - **Supported account types**: Accounts in this organizational directory only (single tenant)
   - Click **Register**.

> [!NOTE]
> Select **Accounts in any organizational directory** (multi-tenant) if users from multiple Entra ID tenants need access to ServicePulse.

5. On the **Overview** page, copy the **Directory (tenant) ID**. This is used to construct the authority URLs.
6. Select **Manage** > **Expose an API**.
7. Next to **Application ID URI**, click **Add** and save the default value (e.g., `api://{application-id}`).
8. Copy the **Application ID URI**. This is used for `ServiceControl/Authentication.Audience`.
9. Under **Scopes defined by this API**, click **Add a scope** and configure:
   - **Scope name**: `api.access`
   - **Who can consent?**: Admins and users
   - **Admin consent display name**: `Full access to ServiceControl API`
   - **Admin consent description**: `Allows ServicePulse to call ServiceControl`
   - **State**: Enabled
   - Click **Add scope**.

### Create the ServicePulse app registration

This app registration represents ServicePulse as a client application that users will sign into.

1. In **App registrations**, click **+ New registration**.
2. Configure the registration:
   - **Name**: `ServicePulse`
   - **Supported account types**: Accounts in this organizational directory only (single tenant)
   - **Redirect URI**:
     - **Platform**: Single-page application (SPA)
     - **URI**: The URL where ServicePulse is hosted (e.g., `https://servicepulse.example.com/`)
   - Click **Register**.

> [!WARNING]
> Redirect URIs must use HTTPS in production environments. HTTP is only acceptable for local development (e.g., `http://localhost:9090/`).

3. Copy the **Application (client) ID**. This is used for `ServiceControl/Authentication.ServicePulse.ClientId`.
4. Select **Manage** > **API permissions**.
5. Click **+ Add a permission**.
6. Select the **APIs my organization uses** tab.
7. Select **ServiceControl API**.
8. Under **Delegated permissions**, check **api.access**.
9. Click **Add permissions**.

> [!NOTE]
> If ServicePulse is accessed from multiple URLs (e.g., localhost during development and a production URL), add each URL as a redirect URI in the ServicePulse app registration under **Manage** > **Authentication**.

## Configure ServiceControl

Add the Entra ID application details to the ServiceControl Error instance configuration to enable authentication. The same settings apply to Audit and Monitoring instances, using their respective prefixes.

### Collected values

During the Entra ID configuration, the following values should have been collected:

| Value | Source | Used for |
|-------|--------|----------|
| Directory (tenant) ID | ServiceControl API app registration > Overview | Authority URLs |
| Application ID URI | ServiceControl API app registration > Expose an API | `Authentication.Audience` and `Authentication.ServicePulse.ApiScopes` |
| Application (client) ID | ServicePulse app registration > Overview | `Authentication.ServicePulse.ClientId` |

### Using App.config

Add the following settings to the ServiceControl configuration file:

```xml
<add key="ServiceControl/Authentication.Enabled" value="true" />
<add key="ServiceControl/Authentication.Authority" value="https://login.microsoftonline.com/{tenant-id}" />
<add key="ServiceControl/Authentication.Audience" value="{application-id-uri}" />
<add key="ServiceControl/Authentication.ServicePulse.ClientId" value="{client-id}" />
<add key="ServiceControl/Authentication.ServicePulse.Authority" value="https://login.microsoftonline.com/{tenant-id}/v2.0" />
<add key="ServiceControl/Authentication.ServicePulse.ApiScopes" value="[&quot;{application-id-uri}/api.access&quot;]" />
```

Replace the placeholder values:

| Placeholder | Example value |
|-------------|---------------|
| `{tenant-id}` | `a1b2c3d4-e5f6-7890-abcd-ef1234567890` |
| `{application-id-uri}` | `api://a1b2c3d4-e5f6-7890-abcd-ef1234567890` |
| `{client-id}` | `f9e8d7c6-b5a4-3210-fedc-ba0987654321` |

### Using environment variables

Environment variables can be used instead of App.config, which is useful for containerized deployments. Environment variables take precedence over App.config settings.

```powershell
$env:SERVICECONTROL_AUTHENTICATION_ENABLED = "true"
$env:SERVICECONTROL_AUTHENTICATION_AUTHORITY = "https://login.microsoftonline.com/{tenant-id}"
$env:SERVICECONTROL_AUTHENTICATION_AUDIENCE = "{application-id-uri}"
$env:SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_CLIENTID = "{client-id}"
$env:SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_AUTHORITY = "https://login.microsoftonline.com/{tenant-id}/v2.0"
$env:SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_APISCOPES = '["{application-id-uri}/api.access"]'
```

### Audit and Monitoring instances

To enable authentication on Audit and Monitoring instances, configure the same settings using their respective prefixes. Only the base authentication settings are required; the ServicePulse settings are only needed on the Error instance.

**Audit instance:**

```xml
<add key="ServiceControl.Audit/Authentication.Enabled" value="true" />
<add key="ServiceControl.Audit/Authentication.Authority" value="https://login.microsoftonline.com/{tenant-id}" />
<add key="ServiceControl.Audit/Authentication.Audience" value="{application-id-uri}" />
```

**Monitoring instance:**

```xml
<add key="Monitoring/Authentication.Enabled" value="true" />
<add key="Monitoring/Authentication.Authority" value="https://login.microsoftonline.com/{tenant-id}" />
<add key="Monitoring/Authentication.Audience" value="{application-id-uri}" />
```

## Verify the configuration

Once configured, restart the ServiceControl instances. When accessing ServicePulse, users will be redirected to Microsoft Entra ID to sign in.

