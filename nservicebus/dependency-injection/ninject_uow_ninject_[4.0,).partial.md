## Unit of work

Its is possible to bind to use an _Unit of Work_ scope, which corresponds to the `DependencyLifecycle.InstancePerUnitOfWork` lifecycle, when registering components with `configuration.RegisterComponents(...)`.

In essence, bindings using _Unit of Work_ scope

 * will be instantiated only once per transport Message
 * will be disposed when message processing finishes

Bind the services in _Unit of Work_ scope using:

snippet: NinjectUnitOfWork

Services using `InUnitOfWorkScope()` can only be injected into code which is processing messages. To inject the service somewhere else (e.g. because of an user interaction) define conditional bindings:

snippet: NinjectConditionalBindings