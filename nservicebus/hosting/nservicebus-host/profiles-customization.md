---
title: NServiceBus Host Profiles Customization
summary: Create custom NServiceBus host profiles and customize default NServiceBus host profiles.
component: Host
reviewed: 2025-10-03
---

include: host-deprecated-warning

## Custom profile

In certain scenarios, it is useful to define additional host profiles, for example when there are several testing and staging environments, each with different configuration. To define a custom profile, create a class that implements the `NServiceBus.IProfile` interface:

snippet: defining_profile

Note that the profile definition does not contain any configuration. The configuration for the profile is provided in profile behaviors.

## Profile behaviors

Profile behaviors specify the configuration for a given profile. They are created by implementing the `NServiceBus.Hosting.Profiles.IHandleProfile<T>` interface, where `T` is the specific profile type:

```csharp
class IntegrationProfileHandler : IHandleProfile<Integration>
{
   public void ProfileActivated(EndpointConfiguration config)
   {
      config.EnableInstallers();
   }
}
```

Profile behaviors can be defined using multiple classes for the same profile or using a single class for multiple profiles.

Profile behaviors can also be used to customize the configuration of a specific element, for example, an email component with the following requirements:

* Production profile: use an SMTP server
* Integration profile: write emails to disk
* Lite profile: do nothing

This can be achieved with the following implementation:

snippet: profile_behavior

NServiceBus will find the provided behaviors for the email component at runtime and invoke only methods appropriate for the profile that it's currently using. As a result, for each environment a different implementation of the email component will be registered with dependency injection and used in the system.

## Profile vs. endpoint configuration

In some situations, profile behavior implementation might depend on the endpoint configuration.

For example, NServiceBus Host uses this information to configure publishers. Endpoints that don't publish messages don't need subscription storage. The Lite profile configures an in-memory subscription storage, but the Integration and Production profiles should use the configuration specified in the endpoint configuration, such as RavenDB or NHibernate.

Endpoint configuration can be accessed to customize profile behaviors as follows:

snippet: dependent_profile
