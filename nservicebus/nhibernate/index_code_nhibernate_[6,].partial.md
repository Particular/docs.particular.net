

### Passing Configuration in code

The following snippet tells NServiceBus to use a given `Configuration` object for all the persistence concerns

snippet:CommonNHibernateConfiguration

To specific configuration on a per-concern basis, use following

snippet:SpecificNHibernateConfiguration

NOTE: Combine both approaches to define a common configuration and override it for one specific concern.

WARNING: When using per-concern API to enable the NHibernate persistence, the `UseConfiguration` method still applies to the common configuration, not the specific concern being enabled. The following code will set up NHibernate persistence only for `GatewayDeduplication` concern but will override the default configuration **for all the concerns**.

snippet:CustomCommonNhibernateConfigurationWarning
