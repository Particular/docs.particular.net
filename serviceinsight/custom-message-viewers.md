---
title: Custom message viewer plugin model in ServiceInsight
reviewed: 2020-03-16
summary: How to create custom viewers via an external plugin
component: ServiceInsight
---

Starting with version 2.4.0, custom message body viewers can be plugged into ServiceInsight. A custom message viewer is useful when displaying message bodies that are not supported by default by ServiceInsight. ServiceInsight supports displaying message bodies in the following formats: XML, json, and hexadecimal. If the message stored in ServiceControl is, for example encrypted, the message body cannot be properly displayed by ServiceInsight. Custom message body viewers can be deployed to ServiceInsight to provide custom visual formatting for message bodies.

### Create custom viewers

To create a custom message body viewer it is necessary to:

- Create a new C# class library project targeting .NET Framework 4.8
- Configure the project to use WPF
- Reference the `ServiceInsight.Sdk` file (can be found in the installation directory of ServiceInsight).
- Create a view model class (that can inherit from `Screen` class from Caliburn.Micro), and implement the `ICustomMessageBodyViewer` interface.
- Create a XAML UserControl as the view which will be bound to the view model.

Note that the view and the view model should follow certain convention that allow them to be found. For this to work, the UserControl should be named `*View` and the corresponding ViewModel should be named `*ViewModel`.

To plugin in the custom viewer an Autofac Module is required to wire up the plugin into the ServiceInsight bootstrap porcess:

- In the custom viewer assembly, add a class that inherits from `Autofac.Module` and implement the `Load` method
- In the `Load` method register the view and the view model, along with other components that need to be injected in them, in the container. The components can be registered as:
  - `implemented interfaces`
  - `as self`
- The view should always be registered as `SingleInstance`.

## Plugin Installation

To deploy the custom message body viewer the artifacts needs to be deployed to one of the following locations:

- The ServiceInsight installation directory
- The user profile roaming folder, `%APPDATA%\Particular Software\ServiceInsight\MessageViewers`
- The all users profile folder, `%ALLUSERSPROFILE%\Particular\ServiceInsight\MessageViewers`

For more information, refer to the ServiceInsight custom message body viewer [sample](/samples/serviceinsight/messageviewer).