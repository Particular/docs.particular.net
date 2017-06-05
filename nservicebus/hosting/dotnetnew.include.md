## dotnet new information


### Reference material

 * [dotnet-new documentation](https://docs.microsoft.com/en-nz/dotnet/core/tools/dotnet-new)
 * [Template spec](https://github.com/dotnet/templating/wiki/%22Runnable-Project%22-Templates)
 * [How to create templates](https://blogs.msdn.microsoft.com/dotnet/2017/04/02/how-to-create-your-own-templates-for-dotnet-new/)


### List installed templates

To list the currently installed templates run `dotnet new` with no parameters


### Template install

```
dotnet new --install TemplatePackageName::Version
```

The wildcard `*` can be used to target the current released version.

```
dotnet new --install TemplatePackageName::*
```

This command is currently in preview and [not documented](https://github.com/dotnet/docs/issues/2315).


### Template uinstall

There is currently [no method for uninstalling a specific template](https://github.com/dotnet/templating/issues/893). The current hack recommended by Microsoft is  is to use reset the installed templates to default list using:

```
dotnet new --debug:reinit
```