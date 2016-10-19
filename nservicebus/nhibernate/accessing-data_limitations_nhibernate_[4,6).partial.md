## Known limitations

Due of the way NServiceBus opens sessions, by passing an existing instance of a database connection, it is currently not possible to use NHibernate's second-level cache. Such behavior of NServiceBus is caused by [still-unresolved bug in NHibernate](https://nhibernate.jira.com/browse/NH-3023).