## Implementing custom unit of work

In scenarios, when custom unit of work is needed (e.g. to commit NHibernate transactions, or call `SaveChanges` on a RavenDB session without polluting handers logic) it can be implemented using a [dedicated pipeline behavior](/samples/pipeline/unit-of-work).