---
title: How to install your license file
summary: 'Make sure your NServiceBus endpoints use your license: Fluent API, appconfig, subfolder in your BIN directory, or registry.'
originalUrl: http://www.particular.net/articles/license-management
tags:
- NServiceBus
- license
---

There are several ways to make sure that your NServiceBus endpoints pick up and use your license. The following options are available:
[Registry](#registry) , [subfolder in your BIN directory](#subfolder_in_BIN) , [app.config](#app.config) , or [fluent API](#fluent_api) .


<a id="registry" name="registry">Using the registry</a>
-------------------------------------------------------

NServiceBus V3.3 supports storing the license in a registry key. This allows you to install a license for the entire server, making it very easy to update. If you are managing many machines this also allows you to automatically roll out changes to all machines using group policies.The registry key where the license is stored is
[HKEYCURRENTUSER\\Software\\NServiceBus\\{Major.Minor}\\License].

**Version 3.3** - To install the license in the registry, use one of these options:

-   The LicenseInstaller.exe tool that comes with the NServiceBus
    install.
-   The
    [Install-License](managing-nservicebus-using-powershell.md)
    PowerShell commandlet.
-   If your trial license has expired and you are running in debug mode,
    the endpoint shows you a dialog that enables you to install the
    license.

**Version 4.0** : Starting from NServiceBus V4.0, the license file will be stored under HKLM\\Software\\NServiceBus\\{Major.Minor}\\License when installed using the
[Install-NServiceBusLicense](managing-nservicebus-using-powershell.md) PowerShell commandlet and the LicenseInstaller.exe tool that comes with the NServiceBus install. In order to install the license file under HKCU
(same location in version 3.3), please use the -c option on the LicenseInstaller.exe

<a id="subfolder_in_BIN" name="subfolder_in_BIN">Using a subfolder in your BIN directory</a>
--------------------------------------------------------------------------------------------

To have NServiceBus automatically pick up your License.xml file, place it in a subfolder named /License under your BIN folder.

<a name="app.config">Using the app.config settings</a>
------------------------------------------------------

As a developer you can specify the license in app.config:

-   Use the key **NServiceBus/LicensePath** to specify the path where
    NServiceBus looks for your license. For example:


```XML
<appSettings>
  <add key="NServiceBus/LicensePath" value="C:\NServiceBus\License\License.xml" />
</appSettings>
```


-   Use the key **NServiceBus/License** to transfer the actual
    HTML-encoded contents of your license. For example:


```XML
<appSettings>
  <add key="NServiceBus/License" value="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;
license id=&quot;1222e1d1-2222-4a46-b1c6-943c442ca710&quot; expiration=&quot;2013-11-30T00:00:00.0000000
&quot; type=&quot;Standard&quot; LicenseType=&quot;Standard&quot; LicenseVersion=&quot;4.0
&quot; MaxMessageThroughputPerSecond=&quot;Max&quot; WorkerThreads=&quot;Max
&quot; AllowedNumberOfWorkerNodes=&quot;Max&quot;&gt;
. . .
&lt;/license&gt;" />  
</appSettings>
```




<a id="fluent_api" name="fluent_api">Using the Fluent API</a>
-------------------------------------------------------------

As a developer you can specify the license to use in your configuration code:

-   Configure.LicensePath(licensePath) uses the license at the specified
    path. For example,


```C#
class ConfigureLicense : IWantCustomInitialization
{
  public void Init()
  {
    NServiceBus.Configure.Instance
      .LicensePath(@"C:\NServiceBus\License\license.xml");
  }
}
```


-   Configure.License(licenseText) uses the transferred license. For
    example, you could add the license file as an embedded resource in
    your assembly and provide its contents as shown below:


```C#
class ConfigureLicense : IWantCustomInitialization
{
   public void Init()
   {
     var licenseStream = Assembly.GetExecutingAssembly()
       .GetManifestResourceStream("FullyQualifiedNamespace.license.xml");
     using (StreamReader sr = new StreamReader(licenseStream))
     {
       NServiceBus.Configure.Instance.License(sr.ReadToEnd());
     }
   }
}
```




