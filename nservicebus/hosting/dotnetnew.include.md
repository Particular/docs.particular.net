## dotnet new information


DANGER: `dotnet new` is still in preview. As such expect to suffer some friction due to bugs, lacking or incorrect documentation and the changing nature of the features and APIs.


### Command execution

The `dotnet` command line operations can be executed from within any Console and the [Visual Studio 2017 Developer Command Prompt](https://msdn.microsoft.com/en-us/library/ms229859.aspx).


### Reference material

 * [dotnet command documentation](https://docs.microsoft.com/en-nz/dotnet/core/tools/dotnet)
 * [dotnet-new documentation](https://docs.microsoft.com/en-nz/dotnet/core/tools/dotnet-new)
 * [Template spec](https://github.com/dotnet/templating/wiki/%22Runnable-Project%22-Templates)
 * [How to create templates](https://blogs.msdn.microsoft.com/dotnet/2017/04/02/how-to-create-your-own-templates-for-dotnet-new/)


### List installed templates

To list the currently installed templates run `dotnet new` with no parameters



### Template install

Where `[TemplatePackageName]` is the name of the NuGet package that contains the template and `[Version]` is a NuGet version number.

```ps
dotnet new --install [TemplatePackageName]::[Version]
```

The wildcard `*` can be used to target the current released version.

```ps
dotnet new --install [TemplatePackageName]::*
```

This command is currently in preview and [not documented](https://github.com/dotnet/docs/issues/2315).


### Install Location

The downloaded NuGets are cached on on disk at:

```
%USERPROFILE%\.templateengine\dotnetcli\vDOTNETVERSION\packages
```

With the list of installed templates listed in:

```
%USERPROFILE%\.templateengine\dotnetcli\vDOTNETVERSION\settings.json
```


### Template uninstall

There is currently [no method for uninstalling a specific template](https://github.com/dotnet/templating/issues/893). The current hack recommended by Microsoft is to use reset the installed templates to default list using:

```ps
dotnet new --debug:reinit
```