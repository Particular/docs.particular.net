## Mark sagas with SagaAttribute to enable source generation

* **Rule ID**: NSB0025
* **Severity**: Info
* **Example message**: Mark saga class `MySaga` with SagaAttribute to enable generation of handler registration methods.

Starting with NServiceBus version 10.2.0, marking a saga with the `[Saga]` attribute allows the NServiceBus source generator to create methods that register the saga explicitly, so an endpoint can run with assembly scanning disabled. The diagnostic is raised on concrete saga classes that are not marked.

This diagnostic is informational by default because the attribute is optional. It can be raised to an error to guarantee that no saga is missed. See [convention-based handlers](/nservicebus/handlers/convention-based.md) for the source generation details.

## SagaAttribute should be applied to concrete saga classes

* **Rule ID**: NSB0026
* **Severity**: Error
* **Example message**: SagaAttribute is applied to base class `MyBaseSaga`, but should be placed on the concrete saga class.

The source generator registers the saga type that is actually handled by the endpoint, which is the concrete class. Marking an abstract saga class, or a saga class that is used as a base class for another saga, would register a type that cannot be instantiated or is not the type the endpoint runs.

Move the `[Saga]` attribute to the concrete saga class. Note that sharing functionality through a saga base class also triggers [NSB0014](#saga-should-not-have-intermediate-base-class).

## SagaAttribute should be applied to classes implementing Saga

* **Rule ID**: NSB0027
* **Severity**: Error
* **Example message**: SagaAttribute is applied to base class `MyClass`, but should be placed on a concrete saga class implementing `Saga<T>`.

The `[Saga]` attribute only has meaning on a saga. Applying it to a class that does not inherit from `Saga<TSagaData>` generates no registration, so the attribute is either on the wrong class or the class was intended to be a saga but does not inherit from `Saga<TSagaData>`.

To mark a convention-based message handler that does not implement `IHandleMessages<T>`, use the `[Handler]` attribute instead.
