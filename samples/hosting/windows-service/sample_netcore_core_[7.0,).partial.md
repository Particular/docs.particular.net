

## .NET Core

.NET Core has some extra caveats to consider when using it in a Windows service.


### System.ServiceProcess.ServiceController

The `ServiceBase` functionality is provided by the [System.ServiceProcess.ServiceController](https://www.nuget.org/packages/System.ServiceProcess.ServiceController/) package.


### Via a self contained exe

To use the installation approach described above, a self contained exe is required. [dotnet-publish](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish) can be used to produce an exe from a .NET Core console project. This can be done by using the following command from the [Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console):

```
dotnet publish WindowsServiceHosting.Core.sln --framework netcoreapp2.0 --runtime win10-x64
```

`Sample.Core.exe` will then exist in `Sample\bin\Debug\netcoreapp2.0\win10-x64\publish`.


### Via dotnet.exe

A .NET Core console application project will, by default, produce a dll. The console application can be executed via the [dotnet command](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet). When used with [sc create](https://technet.microsoft.com/en-us/library/cc990289.aspx) this equates to the following command:

```
sc.exe create nsbSample binpath= "\"C:\Program Files\dotnet\dotnet.exe\" \"[Full Directory]\Sample.Core.dll\""
```


### Related:

 * [dotnet-publish - Arguments](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish#arguments)
 * [.NET Core Runtime IDentifier (RID) catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog#windows-rids)
 * [Target frameworks](https://docs.microsoft.com/en-us/dotnet/standard/frameworks)

