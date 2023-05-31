## NServiceBus Windows Service

This template makes it easier to create a [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) host for an NServiceBus endpoint.

The template can be used via the following command:

snippet: nsbservice-usage

This will create a new directory named `MyWindowsService` containing a Windows Service `.csproj` also named `MyWindowsService`.

To add to an existing solution:

snippet: nsbservice-addToSolution


### Options

snippet: nsbservice-options

#### Target Framework

The target framework for the project.

`-f|--framework`

Default: `net462`

#if-version [4,5)
Supported: `netcoreapp3.1`, `netcoreapp2.1`, `net48`, `net472`, `net471`, `net47`, `net462`, `net461`
#end-if

#if-version [3,4)
Supported: `netcoreapp2.1`, `net472`, `net471`, `net47`, `net462`, `net461`, `net46`, `net452`
#end-if

#if-version [2,3)
Supported: `net471`, `net47`, `net462`, `net461`, `net452`
#end-if

#if-version [1,2)
Supported: `net47`, `net462`, `net452`
#end-if

NOTE: When installing an endpoint created from this template as a service, the `--run-as-service` parameter must be set on the command line. See [Windows Service Installation](/nservicebus/hosting/windows-service.md) for details.