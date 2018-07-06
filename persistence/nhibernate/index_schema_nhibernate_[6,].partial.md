
## Controlling schema

In some cases it may be necessary to take full control over creating the SQL structure used by the NHibernate persister. In these cases the automatic creation of SQL structures on install can be disabled as follows:


**For all persistence schema updates:**

snippet: DisableSchemaUpdate


**For gateway schema update:**

snippet: DisableGatewaySchemaUpdate


**For subscription schema update:**

snippet: DisableSubscriptionSchemaUpdate


**For timeout schema update:**

snippet: DisableTimeoutSchemaUpdate

