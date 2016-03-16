---
title: Install ServicePulse in IIS
summary: Describes how to manually install ServicePulse in IIS.
tags:
- ServicePulse
- Security
- IIS
---


## Prerequisites

These instructions assume the following:

* ServiceControl has installed and is listening on `http://localhost:33333/api`.
* ServicePulse has been installed.


## Basic Setup

Steps

1. Create folder for ServicePulse files.
1. Extract ServicePulse files.
1. Disable/Remove ServicePulse.
1. Remove `netsh` url restriction.
1. Create ServicePulse website in IIS.
1. Refer to ServicePulse folder.


ServicePulse, by default, is [installed](installation.md) as a Windows Service that will self-host the ServicePulse web application.

It is possible to manually install ServicePulse using IIS following these steps:

* Extract ServicePulse files using, at a command prompt, the following command:

```
ServicePulse.Host.exe --extract --serviceControlUrl="http://localhost:33333/api" --outPath="C:\ServicePulse"
```

When using IIS to host ServicePulse the ServicePulse.Host service is not used. To remove the service uninstall ServicePulse from Add/Remove programs.

Use the following command on an elevated command prompt to remove the URLACL that was created by the ServicePulse installer to use port 9090 without any restrictions.

```
netsh http delete urlacl http://+:9090/
```

Note: `ServicePulse.Host.exe` can be found in the ServicePulse installation directory, whose default is `%programfiles(x86)%\Particular Software\ServicePulse`

Once all the ServicePulse files are successfully extracted configure a new IIS web site whose physical path points to the location where files have been extracted. Configure it to use port `9090`.

NOTE: Make sure that the ServicePulse windows service is not running and that the URLACL has been removed or else IIS will not be able to use port 9090.

NOTE: If SSL is to be applied to ServicePulse then ServiceControl also needs to be configured for SSL. This can be achieved by reverse proxying ServiceControl through IIS as outlined below.


## Advanced Configuration

ServicePulse relies on the ServiceControl REST API. It is possible to add a [reverse proxy](https://en.wikipedia.org/wiki/Reverse_proxy) to the ServiceControl web site using  the Microsoft [Application Request Routing](http://www.iis.net/downloads/microsoft/application-request-routing) IIS extension.

This is useful to lock down access to ServicePulse or to expose the web site over a single port.

Installation Steps:

1. Install the IIS [Application Request Routing](http://www.iis.net/downloads/microsoft/application-request-routing) extension.
1. Go to the root folder for the Web site created in the basic configuration.
1. Create a new subdirectory called `api`.
1. Edit `app.constants.js` and change the `serviceControlUrl` value from `http://localhost:33333/api` to `/api`.
1. Open the IIS management tool.
1. Select the api sub folder from within IIS management tool.
1. Click the `URL Rewrite`.
1. Add a new URL Rewrite Rule.
1. Choose `Reverse Proxy` from the list of rule templates.
1. Enter `localhost:33333/api` into the inbound field and leave SSL offload enabled then click OK to add the rule.
1. The website will now answer on `/api` as though it were directly accessing ServiceControl. Verify this by opening the reverse proxy url in a browser `http://localhost:9090/api/` (9090 is the port chosen for the ServicePulse web site).
1. Restrict access to website.

The procedure above should result in a `web.config` file in the newly created `/api` folder similar to this:

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
```
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