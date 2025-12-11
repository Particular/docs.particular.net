---
title: Microsoft Entra ID authentication
summary: Set up authentication with Microsoft Entra ID for ServiceControl and ServicePulse
reviewed: 2025-12-11
component: ServiceControl
---

This guide explains how to configure Microsoft Entra ID (formerly Azure Active Directory) and ServiceControl to enable authentication for ServicePulse.

## Configure Microsoft Entra ID

Register ServiceControl and ServicePulse as applications in Microsoft Entra ID to allow users to authenticate.

### Create the ServiceControl app registration

1. Navigate to the [Azure Portal](https://portal.azure.com/).
2. Open **Microsoft Entra ID** and select **Manage** > **App registrations**.
3. Click **+ New registration**.
4. Configure the registration:
   - **Name**: `ServiceControl API`
   - **Supported account types**: Accounts in this organizational directory only
   - Click **Register**.
5. On the **Overview** page, click **Endpoints** and copy the **OpenID Connect metadata document** URL (remove `/.well-known/openid-configuration` from the end). This is the authority URL used for `ServiceControl/Authentication.Authority`.
6. Select **Manage** > **Expose an API**.
7. Next to **Application ID URI**, click **Add** and save the default value.
8. Under **Scopes defined by this API**, click **Add a scope** and configure:
   - **Scope name**: `api.access`
   - **Who can consent?**: Admins and users
   - **Admin consent display name**: `Full access to ServiceControl API`
   - **Admin consent description**: `Allows ServicePulse to call ServiceControl`
   - **State**: Enabled
   - Click **Add scope**.
9. Copy the **Application ID URI**. This is used for `ServiceControl/Authentication.Audience` and as part of `ServiceControl/Authentication.ServicePulse.ApiScopes`.

### Create the ServicePulse app registration

1. In **App registrations**, click **+ New registration**.
2. Configure the registration:
   - **Name**: `ServicePulse`
   - **Supported account types**: Accounts in this organizational directory only
   - **Redirect URI**:
     - **Platform**: Single-page application (SPA)
     - **URI**: `http://localhost:5291/`
   - Click **Register**.
3. Copy the **Application (client) ID**. This is used for `ServiceControl/Authentication.ServicePulse.ClientId`.
4. Select **Manage** > **API permissions**.
5. Click **+ Add a permission**.
6. Select the **APIs my organization uses** tab.
7. Search for and select **ServiceControl API**.
8. Under **Delegated permissions**, enable **api.access**.
9. Click **Add permissions**.

## Configure ServiceControl

Add the Entra ID application details to the ServiceControl configuration to enable authentication.

### Using App.config

Add the following settings to the ServiceControl configuration file:

```xml
<add key="ServiceControl/Authentication.Enabled" value="true" />

<add key="ServiceControl/Authentication.Authority" value="https://login.microsoftonline.com/{tenant-id}" />
<add key="ServiceControl/Authentication.Audience" value="api://{app-id}" />

<add key="ServiceControl/Authentication.ServicePulse.ClientId" value="{client-id}" />
<add key="ServiceControl/Authentication.ServicePulse.Authority" value="https://login.microsoftonline.com/{tenant-id}/v2.0" />
<add key="ServiceControl/Authentication.ServicePulse.ApiScopes" value="[&quot;api://{app-id}/api.access&quot;]" />
```

Replace the placeholder values with the values copied from the Entra ID app registrations:

| Placeholder | Value |
|-------------|-------|
| `{tenant-id}` | The directory (tenant) ID from the app registration overview |
| `{app-id}` | The Application ID URI from the ServiceControl API registration |
| `{client-id}` | The Application (client) ID from the ServicePulse registration |

### Using environment variables

Environment variables can be used instead of App.config, which is useful for containerized deployments and local development. Convert setting names by replacing `/` and `.` with `_`.

| App.config key | Environment variable |
|----------------|---------------------|
| `ServiceControl/Authentication.Enabled` | `SERVICECONTROL_AUTHENTICATION_ENABLED` |
| `ServiceControl/Authentication.Authority` | `SERVICECONTROL_AUTHENTICATION_AUTHORITY` |
| `ServiceControl/Authentication.Audience` | `SERVICECONTROL_AUTHENTICATION_AUDIENCE` |
| `ServiceControl/Authentication.ServicePulse.ClientId` | `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_CLIENTID` |
| `ServiceControl/Authentication.ServicePulse.Authority` | `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_AUTHORITY` |
| `ServiceControl/Authentication.ServicePulse.ApiScopes` | `SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_APISCOPES` |

Environment variables take precedence over App.config settings.

```powershell
# Enable authentication
$env:SERVICECONTROL_AUTHENTICATION_ENABLED = "true"
$env:SERVICECONTROL_AUTHENTICATION_AUTHORITY = "https://login.microsoftonline.com/{tenant-id}"
$env:SERVICECONTROL_AUTHENTICATION_AUDIENCE = "api://{app-id}"

# ServicePulse settings
$env:SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_CLIENTID = "{client-id}"
$env:SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_AUTHORITY = "https://login.microsoftonline.com/{tenant-id}/v2.0"
$env:SERVICECONTROL_AUTHENTICATION_SERVICEPULSE_APISCOPES = '["api://{app-id}/api.access"]'
```

Once configured, ServiceControl enforces authentication and ServicePulse requires users to sign in through Microsoft Entra ID.

