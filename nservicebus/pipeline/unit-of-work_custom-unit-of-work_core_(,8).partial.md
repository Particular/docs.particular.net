## Implementing custom unit of work

A unit of work allows for shared code, that wraps handlers, to be reused in a way that doesn't pollute the individual handler code. For example, committing NHibernate transactions, or calling `SaveChanges` on a RavenDB session.

### IManageUnitsOfWork

To create a unit of work, implement the `IManageUnitsOfWork` interface.

snippet: UnitOfWorkImplementation

The semantics are that `Begin()` is called when the transport messages enters the pipeline. A transport message can consist of multiple application messages. This allows any setup that is required.

The `End()` method is called when the processing is complete. If there is an exception, it is passed into the method.

This gives a way to perform different actions depending on the outcome of the message(s).

partial: access-to-context

partial: imanageunitsofwork-outbox

partial: nulltask


### Registering custom unit of work

After implementing an `IManageUnitsOfWork`, it needs to be registered:

snippet: InstancePerUnitOfWorkRegistration