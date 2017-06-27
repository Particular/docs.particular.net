


To use a given NHibernate `Configuration` object for all the persistence concerns:

snippet: CommonNHibernateConfiguration


WARNING: When using per-concern API to enable the NHibernate persistence, the `UseConfiguration` method still applies to the common configuration, not the specific concern being enabled. The following code will set up NHibernate persistence only for `GatewayDeduplication` concern but will override the default configuration **for all the concerns**.

snippet: CustomCommonNhibernateConfigurationWarning
