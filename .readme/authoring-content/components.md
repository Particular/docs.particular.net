### Components

Two things are generally referred to as "components", which can be confusing:

* [Components](#componentsyaml), as defined in [components.yaml](components/components.yaml), is the true definition of components, and defines the topic for a page, as well as the collection of snippets that it will pull in. In the case of a component like MSMQ, the functionality started in Core but was moved out to the MSMQ transport, but all of these together use the component `MsmqTransport`.
* [NuGet Aliases](#nugetaliastxt) are frequently misidentified as components because they look the same. NuGet aliases are used to translate the name of a versioned directory like `MsmqTransport_1` into the NuGet package `NServiceBus.Transport.Msmq`. Multiple NuGet aliases can exist within a component, i.e. MSMQ will have versioned snippet directories `Core_6` and `MsmqTransport_1`, which are different NuGet Aliases, but both belong to the same component.

**When you are adding a new package,** you therefore need to add new entries to both components.yaml and nugetAlias.txt, which can be a source of confusion.

#### components.yaml

"Components" is a general term used to describe a deployable set of functionality. Components exist in [components/components.yaml](components/components.yaml). Note that over time a component may have moved between nugets or split into new nugets. For example, the ASB Data Bus or the Callbacks.

Sample Component:

```yaml
- Key: Callbacks
  Url: nservicebus/messaging/callbacks
  NugetOrder:
    - NServiceBus.Callbacks
    - NServiceBus
```

When adding a new component

##### Component Key

The component key allows for shorthand when referring to components in page headers.

##### Component URL

The component URL is the definitive source of documentation for a given component. This will eventually be used to link back to said documentation from both NuGet usages, samples and articles.

##### Component NugetOrder

Since components can be split over multiple different nugets, it is not possible to infer the order from the NuGet version alone. So we need to have a lookup index and the NugetOrder allows us to sensibly sort component versions. For example, NServiceBus.Callbacks.1.0.0 should sort higher than the version of Callbacks that exists in NServiceBus.5.0.0.

#### nugetAlias.txt

All NServiceBus-related NuGet packages (used in documentation) are listed in [components/nugetAlias.txt](components/nugetAlias.txt). The alias part of the NuGet is the key that is used to infer the version and component for all snippets. For example, [Snippets/Callbacks](Snippets/Callbacks) has, over its lifetime, existed in both the Core NuGet and the Callbacks NuGet. So the directories under Callbacks are indicative of the NuGet (alias) they exist in and then split over the multiple versions of a given NuGet.

Example aliases:

```text
ASP: NServiceBus.Persistence.AzureStorage
Autofac: NServiceBus.Autofac
Azure: NServiceBus.Azure
Callbacks: NServiceBus.Callbacks
```