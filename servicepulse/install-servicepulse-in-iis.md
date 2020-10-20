---
title: Install ServicePulse in IIS
summary: Describes how to manually install ServicePulse in IIS
reviewed: 2019-12-25
component: ServicePulse
---


## Prerequisites

These instructions assume the following:

* ServiceControl has been installed and is listening on `http://localhost:33333/api`.
* ServicePulse has been installed.


## Basic setup

Steps

 1. Create a directory for the ServicePulse files
 1. Extract the ServicePulse files
 1. Disable/Remove ServicePulse
 1. Remove the `netsh` url restriction
 1. Make sure that [WebSocket support is enabled for IIS](https://docs.microsoft.com/en-us/iis/get-started/whats-new-in-iis-8/iis-80-websocket-protocol-support)
 1. Create a website in IIS referring to the ServicePulse directory

### Detailed steps

By default, ServicePulse is [installed](installation.md) as a Windows Service that will self-host the ServicePulse web application.

It is possible to manually install ServicePulse using IIS following these steps:

1. Extract the ServicePulse files using the following command at a command prompt:

```dos
ServicePulse.Host.exe --extract --outPath="C:\inetpub\websites\ServicePulse"
```

Note: `ServicePulse.Host.exe` can be found in the ServicePulse installation directory. The default location for this directory is `%programfiles(x86)%\Particular Software\ServicePulse`

2. Once the ServicePulse files are successfully extracted, configure a new IIS website whose physical path points to the location where the files have been extracted. Configure it to use port `9090`.

3. When using IIS to host ServicePulse, the ServicePulse.Host service is not used. To remove the service, uninstall ServicePulse using Add/Remove Programs.

4. Use the following command on an elevated command prompt to remove the URLACL that was created by the ServicePulse installer to use port 9090 without any restrictions.

```dos
netsh http delete urlacl http://+:9090/
```

NOTE: Make sure that the ServicePulse Windows Service is not running and that the URLACL has been removed or else IIS will not be able to use port 9090.

NOTE: If SSL is to be applied to ServicePulse then ServiceControl also must be configured for SSL. This can be achieved by reverse proxying ServiceControl through IIS as outlined below.


## Advanced configuration

ServicePulse relies on the ServiceControl REST API. It is possible to add a [reverse proxy](https://en.wikipedia.org/wiki/Reverse_proxy) to the ServiceControl website using the Microsoft [URL Rewrite extenstion](https://www.iis.net/downloads/microsoft/url-rewrite) IIS extension.

This is useful to lock down access to ServicePulse or to expose the website over a single port.

NOTE: If ServiceControl is configured with a host other than `localhost` then change the hostname value back to `localhost`.

Installation Steps:

 1. Install the IIS [URL Rewrite extension](https://www.iis.net/downloads/microsoft/url-rewrite) extension
 1. Go to the root directory for the website created in the basic configuration
 1. Create a new subdirectory called `api`
 1. Edit `app\js\app.constants.js` and change the `serviceControlUrl` value from `http://localhost:33333/api` to `/api`
 1. Open the IIS management tool
 1. Select the api subdirectory from within the IIS management tool
 1. Click `URL Rewrite`
 1. Add a new URL Rewrite Rule
 1. Choose `Reverse Proxy` from the list of rule templates
 1. Enter `localhost:33333/api` into the inbound field and leave SSL offload enabled then click OK to add the rule
 1. The website will now answer on `/api` as though it were directly accessing ServiceControl. Verify this by opening the reverse proxy url in a browser `http://localhost:9090/api/` (9090 is the port chosen for the ServicePulse website).
 1. Restrict access to the website

The procedure above should result in a `web.config` file in the newly created `/api` directory similar to this:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
    <system.webServer>
        <rewrite>
            <rules>
                <rule name="ReverseProxyInboundRule1"
                      stopProcessing="true">
                    <match url="(.*)" />
                    <action type="Rewrite"
                            url="http://localhost:33333/api/{R:1}" />
                </rule>
            </rules>
        </rewrite>
    </system.webServer>
</configuration>
```

WARNING: The default configuration for ServiceControl only allows access to the REST API via localhost. By exposing the REST API via the reverse proxy configuration, this protection is no longer in place. To address this, it is recommended that the IIS website be configured with one of the IIS authentication providers such as Windows integration authentication.

It is also recommended that the IIS website be configured to use SSL if an authorization provider is used.


### Role-based security

After executing the steps outlined above, ServicePulse requires authentication before accessing any functionality. It does not check authorization rules though, so every authenticated user can do anything. The `IIS URL Authorization` feature can be used to restrict access to specific features. The following snippet can be placed in the `web.config` file in the root of the website to restrict access based on roles:

snippet: RoleBasedSecurity

There are three roles defined:
 * `SPReaders` members can read all the content but cannot trigger any actions
 * `SPFailedMessages` members can manage the failed messages (retry, delete, groups etc.)
 * `SPMonitoring` members can manage monitoring (e.g. enabling/disabling heartbeat monitoring for a particular endpoint)

### ServiceControl monitoring

When using monitoring capabilities the following steps should be followed to create a reverse proxy to access the monitoring API from IIS.

Installation steps:

 1. Install the IIS [Application Request Routing](https://www.iis.net/downloads/microsoft/application-request-routing) extension.
 1. Go to the root directory for the website created in the basic configuration.
 1. Create a new subdirectory called `monitoring`.
 1. Edit `app\js\app.constants.js` and change the `monitoring_urls` value from `http://localhost:33633/` to `/monitoring`.
 1. Open the IIS management tool.
 1. Select the monitoring subdirectory from within IIS management tool.
 1. Click the `URL Rewrite`.
 1. Add a new URL Rewrite Rule.
 1. Choose `Reverse Proxy` from the list of rule templates.
 1. Enter `localhost:33633/` into the inbound field and leave SSL offload enabled then click OK to add the rule.
 1. The website will now answer on `/monitoring` as though it were directly accessing ServiceControl monitoring. Verify this by opening the reverse proxy url in a browser `http://localhost:9090/monitoring/` (9090 is the port chosen for the ServicePulse website).
 1. Restrict access to the website.

The procedure above should result in a `web.config` file in the newly created `/monitoring` directory similar to this:

```
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
    <system.webServer>
        <rewrite>
            <rules>
                <rule name="ReverseProxyInboundRule1" stopProcessing="true">
                    <match url="(.*)" />
                    <action type="Rewrite" url="http://localhost:33633/{R:1}" />
                </rule>
            </rules>
        </rewrite>
    </system.webServer>
</configuration>
```

### Configuring reverse proxy in a non-root directory

Due to a [bug in SignalR](https://github.com/SignalR/SignalR/issues/3649) in Microsoft.AspNet.SignalR.JS version 2.2.0, usage of IIS as a reverse proxy in a virtual directory requires an additional URL Rewrite Rule on the `/api/` subdirectory. This rule makes sure that SignalR uses the correct path when hosted within a virtual directory. This rule should look as follows:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
    <system.webServer>
        <rewrite>
            <outboundRules>
                <rule name="Update Url property" preCondition="JSON" enabled="true" stopProcessing="true">
                    <match filterByTags="None" pattern="\&quot;Url\&quot;:\&quot;(.+?)\&quot;" />
                    <conditions>
                        <add input="{URL}" pattern="(.*)/api/" />
                    </conditions>
                    <action type="Rewrite" value="&quot;Url&quot;:&quot;{C:1}{R:1}&quot;" />
                </rule>
                <preConditions>
                    <preCondition name="JSON">
                        <add input="{URL}" pattern="/api/messagestream/negotiate" />
                        <add input="{RESPONSE_CONTENT_TYPE}" pattern="application/json" />
                    </preCondition>
                </preConditions>
            </outboundRules>
        </rewrite>
    </system.webServer>
</configuration>
```


### Limitations

If ServiceControl is secured with an authentication module other that Windows Authentication, ServiceInsight will not be able to connect to the REST API exposed via IIS. ServiceInsight version 1.4 or greater is required to use Windows authentication.

Older versions of ServiceInsight can still be used locally, bypassing the security by connecting to the ServiceControl port directly using the `http://localhost:33333/api` URL.


## Upgrading ServicePulse hosted in IIS

When ServicePulse is hosted in IIS the upgrade process is as follows:

 1. Go to the root directory of the IIS website,
 1. View and record the current ServicePulse configuration, specifically the value of `serviceControlUrl`. Prior to version 1.3 this was set in `config.js`. For version 1.3 and above the `app\js\app.constants.js` contains this configuration.
 1. In the advanced config above, the api directory is configured to be created. In the upgrade remove everything except that api directory or manually create it again.
 1. Install the new version of ServicePulse using the standard instructions.
 1. Extract the files from the `ServicePulse.Host.exe` using the following command, replacing the recorded values from step 2 with the values from the `app\js\app.constants.js` and `<webroot>` with the path to the root directory of the IIS website.
```dos
ServicePulse.Host.exe --extract --serviceControlUrl="<recordedvalue>" --outPath="<webroot>"
```
 1. Optionally remove or disable the unneeded Windows service by uninstalling ServicePulse via Add/Remove Programs.
 1. The installer will add the URLACL which could restrict access and will need to be removed as described in the basic steps above.


## Adding MIME types for web fonts

If 404 errors occur when serving webfonts, it is possible the MIME type for web fonts have not been configured. Add the following MIME type declarations via IIS Manager (HTTP Headers tab of website properties):

Extension | Mime Type
------------ | -------------
.eot | application/vnd.ms-fontobject
.ttf  | application/octet-stream
.svg | image/svg+xml               
.woff | application/font-woff       
.woff2 | application/font-woff2

NOTE: Some of these MIME types will already be set up on newer versions of IIS. Verify that all the listed MIME types are present.