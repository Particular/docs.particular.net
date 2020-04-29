---
title: Debugging NServiceBus in Visual Studio Code
summary: How to configure Visual Studio Code to build and debug multiple NServiceBus endpoints simultaneously
component: Core
reviewed: 2020-04-29
---

This article describes how to configure [Visual Studio Code](https://code.visualstudio.com/) (or "VS Code") to build an NServiceBus solution with multiple projects and debug multiple endpoints simultaneously.

NServiceBus versions 7 and above are designed to run on .NET Core, which means instead of using the full Visual Studio IDE, users can also use the cross-platform (and free) VS Code application to build systems with NServiceBus.

While Visual Studio contains specific support for .NET projects, VS Code is a more lightweight editor for any language, so some configuration is necessary for VS Code to know how to build and debug .NET projects. Because NServiceBus projects typically involve running multiple startup projects to test multiple endpoints simultaneously, a little extra configuration is needed.

## Prerequisities

* This article assumes knowledge of NServiceBus solutions.
* VS Code must have the [C# for Visual Studio Code (powered by OmniSharp)](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp) extension installed.

## Configuration files

The build and debug system in VS Code is controlled by two files located in a `.vscode` directory at the project root:

* _{Project Root}_
    * `.vscode/`
        * `launch.json`
        * `tasks.json`

The **launch.json** file describes different projects within the solution and how to launch each one.

The **tasks.json** file describes actions to take, the most common of which is to build the code. These tasks can be configured to run before launching a configuration in **launch.json**.

Both of these files can be generated from within VS Code, but the default values will need some tweaks for NServiceBus solutions.

## tasks.json

To create the initial file:

 1. Attempt to start debugging.
 2. Click **Configure Task** in the error dialog that appears.
 3. Select **Create tasks.json from template**.
 4. Select **.NET Core**.
 
The file created by VS Code may work as-is if the project contains only one solution file in the project's root directory.

The following **tasks.json** describes how to build the solution using `dotnet build`, only revealing the terminal if a problem is detected:

```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet build ${workspaceFolder}/path/to/SolutionName.sln",
            "type": "shell",
            "group": "build",
            "presentation": {
                "reveal": "silent"
            }
            "problemMatcher": "$msCompile"
        }
    ]
}
```

For an NServiceBus solution, use the content above, and adjust the path of the `sln` file.

For more information on **tasks.json**, see [Visual Studio Code: Integrate with External Tools via Tasks](https://code.visualstudio.com/docs/editor/tasks).

## launch.json

To create the initial file:

 1. In the Debug toolbar, click the **No Configurations** dropdown.
 1. Select **Add Configuration**.
 1. Select **.NET Core**.

The high-level structure of the **launch.json** file contains a collection of individual project objects in `configurations` and an optional [`compounds` collection](#launch-json-compounds) that lists multiple configurations that should be launched at the same time.

```json
{
    "version": "0.2.0",
    "configurations: [

    ],
    "compounds": [

    ]
}
```

### Console application

Here is an example configuration for an NServiceBus endpoint hosted as a console application:

```json
{
    "name": "Sales",
    "type": "coreclr",
    "request": "launch",
    "preLaunchTask": "build",
    "program": "${workspaceFolder}/path/to/Sales/bin/Debug/netcoreapp2.1/Sales.dll",
    "args": [],
    "cwd": "${workspaceFolder}/path/to/Sales",
    "stopAtEntry": false,
    "console": "externalTerminal"
}
```

Most of the values can be considered boilerplate, except for the `program` and `cwd` values, which should be adjusted for each project:

* `name`: Provides a name that appears for the project in VS Code's Debug dropdown.
* `type`: The value `coreclr` identifies the plugin that should handle debugging responsibilities.
* `request`: The value `launch` means a new process will be started, rather than attaching to an existing process.
* `preLaunchTask`: Indicates a task label from the **tasks.json** file. In this case, it ensures the solution is compiled before trying to launch it.
* `program`: Identifies the DLL to use as the entry point.
* `args`: Specifies any command-line arguments that are needed.
* `cwd`: Specifies the current working directory for the launched `dotnet` process.
* `stopAtEntry`: If set to true, the debugger will break at the entry point even when a breakpoint has not been set.
* `console`: Set to `externalTerminal` to use external console windows rather than VS Code's built-in terminal. This is useful when running multiple NServiceBus endpoints.

### Web application

For web application projects that need to launch a browser window, the configuration is a little more involved:

```json
{
    "name": "EShop.UI",
    "type": "coreclr",
    "request": "launch",
    "preLaunchTask": "build",
    "program": "${workspaceFolder}/path/to/Website/bin/Debug/netcoreapp2.1/Website.dll",
    "args": [],
    "cwd": "${workspaceFolder}/path/to/Website",
    "stopAtEntry": false,
    "internalConsoleOptions": "openOnSessionStart",
    "launchBrowser": {
        "enabled": true,
        "args": "${auto-detect-url}",
        "windows": {
            "command": "cmd.exe",
            "args": "/C start ${auto-detect-url}"
        },
        "osx": {
            "command": "open"
        },
        "linux": {
            "command": "xdg-open"
        }
    },
    "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
    },
    "sourceFileMap": {
        "/Views": "${workspaceFolder}/path/to/Website/Views"
    }
}
```

Like the console example, most of the values are boilerplate, except for the `program`, `cwd`, and `sourceFileMap` values, which should be adjusted for each project:

* `internalConsoleOptions`: Most solutions will only contain a single website project. If so, VS Code's integrated console can be used, as in this example. The `internalConsoleOptions` values then control whether the drawer for the internal console will become visible when debugging begins. The values `neverOpen` or `openOnFirstSessionStart` can also be used. In addition, it's possible to use the `console` property to launch an external console.
* `launchBrowser`: Contains instructions for launching a browser window on a platform-specific basis.
* `env`: Additional environment variables to pass to the application, including the common `ASPNETCORE_ENVIRONMENT` variable that affects how an ASP.NET Core web application will read configuration values.
* `sourceFileMap`: Additional source file mappings passed to the debugger, useful in this case to let the debugger know the path to MVC views.

### Attaching to a process

It's also possible to start a process from a terminal and then attach the debugger to the process with this configuration:

```json
{
    "name": ".NET Core Attach",
    "type": "coreclr",
    "request": "attach",
    "processId": "${command:pickProcess}"
}
```

### Compounds

Running multiple NServiceBus endpoints together requires the `compounds` element, which identifies a `name` to appear in VS Code's Debug menu and a list of configuration names specified elsewhere in the file:

```json
"compounds": [
    {
        "name": "Debug All",
        "configurations": [
            "Sales",
            "Billing",
            "Website"
        ]
    }
]
```

### Complete example

This example shows an entire **launch.json** file with **Sales** and **Billing** endpoints plus a **Website** project, all in a directory named `src` beneath the project root:

```json
{
   "version": "0.2.0",
   "configurations": [
        {
            "name": "Billing",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/Billing/bin/Debug/netcoreapp2.1/Billing.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/Billing",
            "stopAtEntry": true,
            "console": "externalTerminal"
        },
        {
            "name": "Sales",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/Sales/bin/Debug/netcoreapp2.1/Sales.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/Sales",
            "stopAtEntry": false,
            "console": "externalTerminal"
        },
        {
            "name": "Website",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/Website/bin/Debug/netcoreapp2.1/Website.dll",
            "args": [],
            "cwd": "${workspaceFolder}/src/Website",
            "stopAtEntry": false,
            "internalConsoleOptions": "openOnSessionStart",
            "launchBrowser": {
                "enabled": true,
                "args": "${auto-detect-url}",
                "windows": {
                    "command": "cmd.exe",
                    "args": "/C start ${auto-detect-url}"
                },
                "osx": {
                    "command": "open"
                },
                "linux": {
                    "command": "xdg-open"
                }
            },
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            },
            "sourceFileMap": {
                "/Views": "${workspaceFolder}/src/Website/Views"
            }
        },
        {
            "name": ".NET Core Attach",
            "type": "coreclr",
            "request": "attach",
            "processId": "${command:pickProcess}"
        }
    ,],
    "compounds": [
        {
            "name": "Debug All",
            "configurations": [
                "Billing",
                "Sales",
                "Website",
            ]
        }
    ]
}
```

## More information

See the following articles for more information on debugging with VS Code:

* [OmniSharp: Configuring launch.json for C# debugging](https://github.com/OmniSharp/omnisharp-vscode/blob/master/debugger-launchjson.md)
* [Visual Studio Code: Integrate with External Tools via Tasks](https://code.visualstudio.com/docs/editor/tasks)
