---
title: NServiceBus Host Profiles customization 
summary: ''
tags:
- Profiles
- Hosting
- Logging
- Persistence
component: Host
reviewed: 2017-02-28
---

## Custom profile

To define a custom Profile create a class that implements the `NServiceBus.IProfile` marker interface:

snippet:defining_profile

Note that the profile definition does not contain any configuration. The actual configuration for the profile is provided in profile behaviors.


## Profile behaviors

Profile behaviors specify configuration for a given profile. They are created by implementing `NServiceBus.Hosting.Profiles.IHandleProfile<T>`, where `T` is the specific profile type:

```
class IntegrationProfileHandler : IHandleProfile<Integration>
{
   public void ProfileActivated(EndpointConfiguration config)
   {
      config.EnableInstallers();
   }
}
```

Profile behaviors might be defined using multiple classes for the same profile, or using a single class for multiple profiles.

Profile behaviors might also be used to customize configuration of a specific element, for example an email component might have the following requirements:

 * Production profile: use an SMTP server
 * Integration profile: write emails to disk
 * Lite profile: do nothing

That can be achieved with the following implementation:

snippet:profile_behavior


## Profile vs endpoint configuration

In some situations profile behavior implementation might depend on the endpoint configuration. 

For example, NServiceBus Host uses this information to configure publishers. Endpoints that don't publish messages don't need to have any subscription storage. Endpoints that are publishers should use storage that can be specified either in the profile or endpoint configuration. The Lite profile configures in memory subscription storage, but the Integration and Production profiles should use configuration specified in the endpoint configuration, such as RavenDB or NHibernate.

Endpoint's configuration can be accessed to customize profile behaviors in the following way:

snippet:dependent_profile