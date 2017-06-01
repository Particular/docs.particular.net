---
title: Windows Service 'dotnet new' Template
reviewed: 2017-06-01
component: ServiceTemplate
tags:
- Hosting
---

The `dotnet new` [template](https://github.com/Particular/NServiceBus.Templates) makes it easier to create a Windows Service host for NServiceBus. Following snippet shows how to install the template and instantiate a Visual Studio project based on that template

```
dotnet new --install NServiceBus.Template.WindowsService::*
dotnet new nsbservice --name MyWindowsService
```