
## Controlling schema

In some cases it may be necessary to take full control over creating the SQL structure used by the NHibernate persister. In these cases the automatic creation of SQL structures on install can be disabled as follows:


**For all persistence schema updates:**

snippet: DisableSchemaUpdate


**For Gateway schema update:**

snippet: DisableGatewaySchemaUpdate


**For Subscription schema update:**

snippet: DisableSubscriptionSchemaUpdate


**For Timeout schema update:**

snippet: DisableTimeoutSchemaUpdate

