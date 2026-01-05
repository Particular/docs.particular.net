---
title: Install ServicePulse in IIS
summary: Describes how to manually install ServicePulse in IIS
reviewed: 2024-06-27
component: ServicePulse
---

## Prerequisites

These instructions assume the following:

- ServiceControl has been installed and is listening on `http://localhost:33333/api`.
- A ServiceControl Monitoring instance has been installed and is listening on `http://localhost:33633`.
- ServicePulse has been installed.

## Basic setup

Steps

1. Create a directory for the ServicePulse files
1. Extract the ServicePulse files
1. Disable/Remove ServicePulse
1. Remove the `netsh` url restriction
1. Create a website in IIS referring to the ServicePulse directory

> [!NOTE]
> When performing an upgrade, it is recommended to delete your existing environment and start with a clean install. See [Upgrading ServicePulse hosted in IIS](#upgrading-servicepulse-hosted-in-iis) for more details.

### Detailed steps

By default, ServicePulse is [installed](installation.md) as a Windows Service that will self-host the ServicePulse web application.

It is possible to manually install ServicePulse using IIS following these steps:

1. Extract the ServicePulse files using the following command at a command prompt:

```shell
ServicePulse.Host.exe --extract --outPath="C:\inetpub\websites\ServicePulse"
```

> [!NOTE]
> The `ServicePulse.Host.exe` executable is located in the ServicePulse installation directory. By default, this directory can be found at `%programfiles(x86)%\Particular Software\ServicePulse`

1. Once the ServicePulse files are successfully extracted, configure a new IIS website whose physical path points to the location where the files have been extracted. Configure it to use port `9090`.
1. When using IIS to host ServicePulse, the ServicePulse.Host service is not used. To remove the service, uninstall ServicePulse using Add/Remove Programs.
1. Use the following command on an elevated command prompt to remove the URLACL that was created by the ServicePulse installer to use port 9090 without any restrictions.

```shell
netsh http delete urlacl http://+:9090/
```

> [!NOTE]
> Make sure that the ServicePulse Windows Service is not running and that the URLACL has been removed or else IIS will not be able to use port 9090.

> [!NOTE]
> If TLS is to be applied to ServicePulse then ServiceControl also must be configured for TLS. This can be achieved by reverse proxying ServiceControl through IIS as outlined [below](#advanced-configuration-servicecontrol).

## Advanced configuration

ServicePulse relies on the ServiceControl and ServiceControl Monitoring REST APIs. Both can be exposed. It is possible to add a [reverse proxy](https://en.wikipedia.org/wiki/Reverse_proxy) to the ServicePulse website using the Microsoft [URL Rewrite IIS extension](https://www.iis.net/downloads/microsoft/url-rewrite).

### ServiceControl

> [!NOTE]
> If ServiceControl is configured with a hostname other than `localhost` then change the hostname value back to `localhost`.

Installation steps:

1. Install the IIS [Application Request Routing extension](https://www.iis.net/downloads/microsoft/application-request-routing).
1. In IIS Manager, click the server node in the Connections pane, double-click "Application Request Routing Cache", then in the Actions pane, click "Server Proxy Settings", and check "Enable proxy". Click "Apply" to save changes.
1. Go to the root directory for the website created in the basic configuration.
1. Edit `js\app.constants.js` and change the `serviceControlUrl` value from `http://localhost:33333/api` to `api/`.
1. Open the IIS management tool.
1. Select the root directory from within the IIS management tool.
1. Open or create a `web.config` file
1. Add the following rewrite rules to the top of the file:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
    <system.webServer>
        <rewrite>
            <rules>
                 <rule name="Rewrite main instance API URL" stopProcessing="true">
                    <match url="^api/(.*)" />
                    <action type="Rewrite" url="http://localhost:33333/api/{R:1}" />
                </rule>
            </rules>
        </rewrite>
    </system.webServer>
</configuration>
```

> [!WARNING]
> When ServiceControl is configured to run on localhost:port, it is protected from being accessed via the server IP address. By exposing the REST API via the reverse proxy configuration, this protection is no longer in place. To address this, it's recommended to configure the IIS website with an appropriate authentication provider, such as Windows Integrated Authentication.

It is also recommended that the IIS website be configured to use TLS if an authorization provider is used.

### ServiceControl monitoring

When using [monitoring capabilities](/monitoring) the following steps should be followed to create a reverse proxy to access the monitoring API from IIS.

Installation steps:

1. Install the IIS [Application Request Routing extension](https://www.iis.net/downloads/microsoft/application-request-routing).
1. In IIS Manager, click the server node in the Connections pane, double-click "Application Request Routing Cache", then in the Actions pane, click "Server Proxy Settings", and check "Enable proxy". Click "Apply" to save changes.
1. Go to the root directory for the website created in the basic configuration.
1. Edit `js\app.constants.js` and change the `monitoring_urls` value from `http://localhost:33633/` to `monitoring/`.
1. Open the IIS management tool.
1. Select the root directory from within the IIS management tool.
1. Open or create a `web.config` file
1. Add the following rewrite rules to the top of the file:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
    <system.webServer>
        <rewrite>
            <rules>
                <rule name="Rewrite monitoring API URL" stopProcessing="true">
                    <match url="^monitoring/(.*)" />
                    <action type="Rewrite" url="http://localhost:33633/{R:1}" />
                </rule>
            </rules>
        </rewrite>
    </system.webServer>
</configuration>
```

> [!WARNING]
> When ServiceControl is configured to run on localhost:port, it is protected from being accessed via the server IP address. By exposing the REST API via the reverse proxy configuration, this protection is no longer in place. To address this, it's recommend to configure the IIS website with an appropriate authentication provider, such as Windows Integrated Authentication.

### Role-based security

After executing the steps outlined above, ServicePulse requires authentication before accessing any functionality.

However, once authenticated, authorization rules are not checked, so users have access to all functionality. To restrict access to specific features, use the `IIS URL Authorization` feature. The following snippet can be placed in the `web.config` file in the root of the website to restrict access based on roles:

snippet: RoleBasedSecurity

There are three roles defined:

- `SPReaders`: members can read all content but cannot trigger any actions.
- `SPFailedMessages`: members can manage failed messages (retry, delete, groups etc.).
- `SPMonitoring`: members can manage monitoring (e.g. enabling/disabling heartbeat monitoring for a particular endpoint).

## Upgrading ServicePulse hosted in IIS

When ServicePulse is hosted in IIS, the upgrade process is as follows:

1. Go to the root directory of the IIS website.
1. View and record the current ServicePulse configuration, specifically the value of `serviceControlUrl`. For versions 1.3 and below, this parameter is set in `config.js`. Between versions 1.3 and 1.31.0, it is set in `app\js\app.constants.js`. For versions 1.31.1 and above, it is set in `js\app.constants.js`.
1. Remove everything from the root folder.
1. Install the new version of ServicePulse using the standard instructions.
1. Extract the files from the `ServicePulse.Host.exe` using the following command, but replace `<recordedvalue>` with the value saved from Step 2, and `<webroot>` with the path to the root directory of the IIS website.

```shell
ServicePulse.Host.exe --extract --serviceControlUrl="<recordedvalue>" --outPath="<webroot>"
```

1. Optionally, remove or disable the unneeded Windows service by uninstalling ServicePulse via Add/Remove Programs.
1. The installer will add the URLACL which could restrict access and will need to be removed as described in the basic steps above.

## Adding MIME types for web fonts

If 404 errors occur when serving webfonts, it is possible the MIME type for web fonts have not been configured. Add the following MIME type declarations via IIS Manager (HTTP Headers tab of website properties):

| Extension | Mime Type                     |
| --------- | ----------------------------- |
| .eot      | application/vnd.ms-fontobject |
| .ttf      | application/octet-stream      |
| .svg      | image/svg+xml                 |
| .woff     | application/font-woff         |
| .woff2    | application/font-woff2        |

> [!NOTE]
> Some of these MIME types will already be set up on newer versions of IIS. Verify that all the listed MIME types are present.
