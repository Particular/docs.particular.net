

## .NET Core

.Net Core has some extra caveats to consider when using it in a windows service.


### System.ServiceProcess.ServiceController

The `ServiceBase` functionality is provided by the [System.ServiceProcess.ServiceController](https://www.nuget.org/packages/System.ServiceProcess.ServiceController/). However the current stable does not provide enough functionality so an [unstable package](https://dotnet.myget.org/feed/dotnet-core/package/nuget/System.ServiceProcess.ServiceController) is currently being used. The package is resolved using a [custom package source](https://docs.microsoft.com/en-us/nuget/schema/nuget-config-file#package-source-sections) in the `nuget.config` at the root of the sample solution.

```
<add key="dotnet-core" value="https://dotnet.myget.org/F/dotnet-core/api/v3/index.json" />
```


### Via a self contained exe

To use the installation approach described above a self contained exe is required. To produce an exe from a .NET Core console project [dotnet-publish](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish) can be used. This can be done by using the following command from the [Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console):

```
dotnet publish WindowsServiceHosting.Core.sln --framework netcoreapp2.0 --runtime win10-x64
```

The exe `Sample.Core.exe` will then exist in `Sample\bin\Debug\netcoreapp2.0\win10-x64`.


### Via dotnet.exe

A .NET Core console application project will, by default, produce a dll. This console dll can be executed via the [dotnet command](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet). When used with [sc create](https://technet.microsoft.com/en-us/library/cc990289.aspx) this equates to the following command:

```
sc.exe create nsbSampleService binpath= "\"C:\Program Files\dotnet\dotnet.exe\" \"[Full Bin Debug Directory]\Sample.Core.dll\""
```


### Related:

 * [dotnet-publish - Arguments](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish#arguments)
 * [.NET Core Runtime IDentifier (RID) catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog#windows-rids)
 * [Target frameworks](https://docs.microsoft.com/en-us/dotnet/standard/frameworks)

