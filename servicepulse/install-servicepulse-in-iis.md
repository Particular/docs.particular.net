---
title: Install ServicePulse in IIS
summary: Describes how to manually install ServicePulse in IIS.
reviewed: 2016-09-02
component: ServicePulse
tags:
- Security
---


## Prerequisites

These instructions assume the following:

* ServiceControl has installed and is listening on `http://localhost:33333/api`.
* ServicePulse has been installed.


## Basic Setup

Steps

 1. Create directory for ServicePulse files.
 1. Extract ServicePulse files.
 1. Disable/Remove ServicePulse.
 1. Remove `netsh` url restriction.
 1. Create ServicePulse website in IIS.
 1. Refer to ServicePulse directory.


ServicePulse, by default, is [installed](installation.md) as a Windows Service that will self-host the ServicePulse web application.

It is possible to manually install ServicePulse using IIS following these steps:

* Extract ServicePulse files using, at a command prompt, the following command:

```dos
ServicePulse.Host.exe --extract --outPath="C:\inetpub\websites\ServicePulse"
```

When using IIS to host ServicePulse the ServicePulse.Host service is not used. To remove the service uninstall ServicePulse from Add/Remove programs.

Use the following command on an elevated command prompt to remove the URLACL that was created by the ServicePulse installer to use port 9090 without any restrictions.

```dos
netsh http delete urlacl http://+:9090/
```

Note: `ServicePulse.Host.exe` can be found in the ServicePulse installation directory, whose default is `%programfiles(x86)%\Particular Software\ServicePulse`

Once all the ServicePulse files are successfully extracted configure a new IIS web site whose physical path points to the location where files have been extracted. Configure it to use port `9090`.

NOTE: Make sure that the ServicePulse windows service is not running and that the URLACL has been removed or else IIS will not be able to use port 9090.

NOTE: If SSL is to be applied to ServicePulse then ServiceControl also needs to be configured for SSL. This can be achieved by reverse proxying ServiceControl through IIS as outlined below.


## Advanced Configuration

ServicePulse relies on the ServiceControl REST API. It is possible to add a [reverse proxy](https://en.wikipedia.org/wiki/Reverse_proxy) to the ServiceControl web site using  the Microsoft [Application Request Routing](https://www.iis.net/downloads/microsoft/application-request-routing) IIS extension.

This is useful to lock down access to ServicePulse or to expose the web site over a single port.

NOTE: If ServiceControl is configured with a different host then `localhost` then change the hostname value back to `localhost`.

Installation Steps:

 1. Install the IIS [Application Request Routing](https://www.iis.net/downloads/microsoft/application-request-routing) extension.
 1. Go to the root directory for the Web site created in the basic configuration.
 1. Create a new subdirectory called `api`.
 1. Edit `app.constants.js` and change the `serviceControlUrl` value from `http://localhost:33333/api` to `/api`.
 1. Open the IIS management tool.
 1. Select the api sub directory from within IIS management tool.
 1. Click the `URL Rewrite`.
 1. Add a new URL Rewrite Rule.
 1. Choose `Reverse Proxy` from the list of rule templates.
 1. Enter `localhost:33333/api` into the inbound field and leave SSL offload enabled then click OK to add the rule.
 1. The website will now answer on `/api` as though it were directly accessing ServiceControl. Verify this by opening the reverse proxy url in a browser `http://localhost:9090/api/` (9090 is the port chosen for the ServicePulse web site).
 1. Restrict access to website.

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

WARNING: The default configuration for ServiceControl only allows access to REST API via localhost. By exposing the REST API via the reverse proxy configuration this protection is no longer in place. To address this it is recommended that the IIS Web site be configured with one of the IIS authentication providers such as Windows integration authentication.

It is also recommended that the IIS web site be configured to use SSL if an authorization provider is used.


### Role-based security

After executing the steps outlined above, ServicePulse requires authentication before accessing any functionality. It does not check any authorization rules though, so every authenticated user can do anything. The `IIS URL Authorization` feature can be used to restrict access to specific features. The following snippet can be placed in the `web.config` file in the root of the web site to restrict access based on roles:

snippet: RoleBasedSecurity

There are three roles defined:
 * `SPReaders` members can read all the content but cannot trigger any actions
 * `SPFailedMessages` members can manage the failed messages (retry, archive, groups etc.)
 * `SPMonitoring` members can manage monitoring (e.g. enabling/disabling heartbeat monitoring for a particular endpoint)


### Configuring Reverse Proxy in a non-root directory

Due to a [bug in SignalR](https://github.com/SignalR/SignalR/issues/3649) in Microsoft.AspNet.SignalR.JS version 2.2.0, usage of IIS as a reverse proxy in a virtual directory requires an additional URL Rewrite Rule on the `/api/` sub directory. This rule makes sure that SignalR uses the correct path when hosted within a virtual directory. This rule should look as follows:

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

If ServiceControl is secured with an authentication module other that Windows Authentication  ServiceInsight will not be able to connect to the REST API exposed via IIS. ServiceInsight v1.4 or greater is required to use Windows authentication.

Older versions of ServiceInsight can still be used locally, bypassing the security by connecting to the ServiceControl port directly using the `http://localhost:33333/api` URL.


## Upgrading ServicePulse hosted in IIS

When ServicePulse is hosted in IIS the upgrade process is as follows:

 1. Go to the root directory of the IIS web site,
 1. View and record the the current ServicePulse configuration, specifically the value of `serviceControlUrl`. Prior to version 1.3 this was set in `config.js`. For Version 1.3 and above the `app\js\app.constants.js` contains this configuration.
 1. In the advanced config above the api directory is configured to be created. In the upgrade remove everything except that api directory. Or manually create it again.
 1. Install the new version of ServicePulse using the standard instructions.
 1. Extract the files from the `ServicePulse.Host.exe` using the following command line, replacing the recorded values from step 2 with the values from the `app.constants.js` and `<webroot>` with the path to the root directory of the IIS website.
```dos
ServicePulse.Host.exe --extract --serviceControlUrl="<recordedvalue>" --outPath="<webroot>"
```
 1. Optionally remove or disable the unneeded Windows Service by uninstalling ServicePulse via the Add/Remove applet in control panel.
 1. The installer will add the URLACL which could restrict access and will need to be removed as described in the basic steps.


## Adding Mime Types for Web Fonts

If 404 errors when serving webfonts it is possible MIME type for web fonts have not been configured. Add the following MIME type declarations via IIS Manager (HTTP Headers tab of website properties):

Extension | Mime Type
------------ | -------------
.eot | application/vnd.ms-fontobject
.ttf  | application/octet-stream
.svg | image/svg+xml               
.woff | application/font-woff       
.woff2 | application/font-woff2 

NOTE: Some of these mime types will already be setup on newer versions of IIS. Verify that all the listed mime types are present.
