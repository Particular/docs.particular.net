## Known limitations

NServiceBus opens a session by passing an existing instance of a database connection. Therefore, it is not possible to use NHibernate's second-level cache. The reason why sessions are opened this way in NServiceBus is because of an [unresolved bug in NHibernate](https://nhibernate.jira.com/browse/NH-3023).
