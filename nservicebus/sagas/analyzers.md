---
title: Roslyn analyzers for sagas
summary: Details of the Roslyn analyzers that promote code quality in sagas.
component: Core
versions: '[7.7,)'
reviewed: 2022-03-04
---

Starting in NServiceBus version 7.7, [Roslyn analyzers](https://docs.microsoft.com/en-us/visualstudio/code-quality/roslyn-analyzers-overview) are packaged with the NServiceBus package that analyze the code in sagas and make suggestions for improvements, right in the editor.

## Non-mapping expression used in ConfigureHowToFindSaga method

* **Rule ID**: NSB0003
* **Severity**: Warning, Error starting in NServiceBus version 8
* **Example message**: The ConfigureHowToFindSaga method should only contain mapping expressions (i.e. 'mapper.MapSaga().ToMessage<T>()') and not contain any other logic.

The `ConfigureHowToFindSaga` method is executed to determine the mappings between incoming messages and stored saga data. Arbitrary statements or calls to other methods, while they may be valid C#, are not valid in this method.

See [message correlation](/nservicebus/sagas/message-correlation.md) for details of how to map incoming messages to stored saga data.

## Saga mapping expressions can be simplified

* **Rule ID**: NSB0004
* **Severity**: Warning
* **Example message**: The saga mapping contains multiple .ToSaga(…) expressions which can be simplified using mapper.MapSaga(…).ToMessage<T>(…) syntax.

The original NServiceBus saga mapping API required repeating the `.ToSaga(…)` expressions for each call to `.ConfigureMapping(…)`.

The IDE will raise a diagnostic for mapping expressions like this:

snippet: SagaAnalyzerComplexMapping

The analyzer will also offer a code fix that will automatically rewrite the code to look like this:

snippet: SagaAnalyzerSimplifiedMapping

The simplified syntax removes duplication and reduces confusion since the `.ToSaga(…)` mappings in the old syntax must match in order to be valid.

## Saga can only define a single correlation property on the saga data

* **Rule ID**: NSB0005
* **Severity**: Error
* **Example message**: The saga can only map the correlation ID to one property on the saga data class.

When using the `.ConfigureMapping<T>(…).ToSaga(…)` mapping pattern, all of the `.ToSaga(…)` expressions must agree and point to the same property on the saga data class.

This was already a runtime error in NServiceBus 7.6 and below, but the analyzer raises the error as more direct feedback at compile time.

Once all the `.ToSaga(…)` expressions agree, [NSB0004: Saga mapping expressions can be simplified](#saga-mapping-expressions-can-be-simplified) will be invoked, and the code fix can be used to simplify the saga mapping expression so that `.ToSaga(…)` mappings are not duplicated.

## Message that starts the saga does not have a message mapping

* **Rule ID**: NSB0006
* **Severity**: Warning, Error starting in NServiceBus version 8
* **Example message**: Saga MySaga implements `IAmStartedByMessages<MyMessage>` but does not provide a mapping for that message type. In the ConfigureHowToFindSaga method, after calling `mapper.MapSaga(saga => saga.CorrelationPropertyName)`, add `.ToMessage<MyMessage>(msg => msg.PropertyName)` to map a message property to the saga correlation ID, or `.ToMessageHeader<MyMessage>("HeaderName")` to map a header value that will contain the correlation ID.

A message type identified by `IAmStartedByMessages<TMessage>` means that type of message can start the saga. Because there may not yet be any saga data when this message is received, a message identified in this way **must** have an associated message mapping in the `ConfigureHowToFindSaga()` method, otherwise it would be impossible to know if saga data had already been created.

The code fix will attempt to rewrite the `ConfigureHowToFindSaga()` method and generate the missing mapping. If the existing mapping expressions already identify a correlation id (i.e. `sagaData.OrderId`) and the message type being mapped has a property with a matching name (i.e. `message.OrderId`) then the mapping will automatically use that property name. Otherwise, the code fix will generate a mapping expression with placeholders to fill in.

## Saga data property is not writeable

* **Rule ID**: NSB0007
* **Severity**: Warning, Error starting in NServiceBus version 8
* **Example message**: Saga data property `MySagaData.MyProperty`does not have a public setter. This could interfere with loading saga data. Add a public setter.

Many saga persistence libraries use serialization and deserialization to store and load saga data, respectively. It's not always possible for serializers to set values unless the property is both marked as `public` and also has a `public` setter.

A saga data class is not a good place to employ specialized data access patterns to restrict write access to certain properties. Saga data classes should be thought of as being an internal storage fully owned by the saga, and are best implemented as simple properties like `public string PropertyName { get; set; }` without any specialized access modifiers.

## Saga message mappings are not needed for timeouts

* **Rule ID**: NSB0008
* **Severity**: Warning
* **Example message**: Message type `MyMessage` is mapped as `IHandleTimeouts<MyMessage>`, which do not require saga mapping because timeouts have the saga's Id embedded in the message. &#89;&#111;&#117; can remove this mapping expression.

When sagas request timeouts, the delayed message is stamped with a header that includes the full saga ID. This enables the lookup of the correct saga data without requiring a mapping in the `ConfigureHowToFindSaga()` method. Removing the mapping will have no effect on the saga's ability to find the correct saga data when the timeout is processed, and it can be safely removed.

## A saga cannot use the Id property for a Correlation ID

* **Rule ID**: NSB0009
* **Severity**: Warning, Error starting in NServiceBus version 8
* **Example message**: A saga cannot map to the saga data's Id property, regardless of casing. Select a different property (such as OrderId, CustomerId) that relates all of the messages handled by this saga.

The `Id` property of a saga (defined by the required `IContainSagaData` interface) is reserved for use by the saga. It cannot be used as a correlation id by mapping to it in the `ConfigureHowToFindSaga()` method.

In addition, some saga persistence libraries, such as [SQL Persistence](/persistence/sql/), store the saga's `Id` value in a column, and the column names are commonly case-insenstiive. This means that other casings of `Id` (`ID`, `id`, or even `iD`) are also not allowed.

## Message types should not be used as saga data properties

* **Rule ID**: NSB0010
* **Severity**: Warning
* **Example message**: Using the message type `MyMessage` to store message contents in saga data is not recommended, as it creates unnecessary coupling between the structure of the message and the stored saga data, making both more difficult to evolve.

When a saga receives a message, it can be convenient and even tempting to insert the whole thing into the saga data.

However, this creates unintended couling between the saga data and the message contract.

The saga data class is wholly owned and managed by the saga, and represents the internal stored state of that saga. It must be able to be stored to disk and reloaded perhaps minutes, hours, or even days/years later. It must be durable.

On the other hand, the message is more ephemeral. It only needs to be stable long enough to deal with any messages currently in-flight at the time a new version of a software system is released. Its ownership is different, and needs to be more nimble and able to change over time.

By storing a message type inside the saga data, the ephemeral message structure must be locked down by the same rules as the saga data, making it harder for the saga and the other message endpoints it exchanges messages with to evolve.

## Correlation ID property must be a supported type

* **Rule ID**: NSB0011
* **Severity**: Error
* **Example message**: A saga correlation property must be one of the following types: string, Guid, long, ulong, int, uint, short, ushort

The correlation property represents the logical identity of the stored saga data. It needs to be something easily represented by nearly every saga persistence library. For example, `DateTime` would make a bad correlation property type, because it is represented so differently on different storage systems, such as between relational SQL tables and NoSQL databases, or even in different amounts of precision in fractions of a second that are stored between different relational database systems.

Prior to NServiceBus version 7.7, this check was a runtime error. In NServiceBus version 7.7 and above, the analyzer diagnostic raises this feedback to compile time.

## Saga data classes should inherit ContainSagaData

* **Rule ID**: NSB0012
* **Severity**: Warning
* **Example message**: It's easier to inherit the class ContainSagaData, which contains all the necessary properties to implement IContainSagaData, than to implement IContainSagaData directly.

remarks

## Reply in Saga should be ReplyToOriginator

* **Rule ID**: NSB0013
* **Severity**: Info
* **Example message**: In a Saga, context.Reply() will reply to the sender of the immediate message, which isn't common. To reply to the message that started the saga, use the saga's ReplyToOriginator() method.

remarks

## Saga should not have intermediate base class

* **Rule ID**: NSB0014
* **Severity**: Warning
* **Example message**: A saga should not have an intermediate base class and should inherit directly from NServiceBus.Saga<TSagaData>.

remarks

## Saga should not implement IHandleSagaNotFound

* **Rule ID**: NSB0015
* **Severity**: Warning, Error starting in NServiceBus version 8
* **Example message**: A saga should not implement IHandleSagaNotFound, as this catch-all handler will handle messages where *any* saga is not found. Implement IHandleSagaNotFound on a separate class instead.

remarks

## Correlation property must match message mapping expression type

* **Rule ID**: NSB0016
* **Severity**: Error
* **Example message**: When mapping a message to a saga, the member type on the message and the saga property must match. `MySagaData.CorrelationProperty` is of type `string` and `MyMessage.CorrelationProperty` is of type `int`.

remarks

## ToSaga mapping must point to a property

* **Rule ID**: NSB0017
* **Severity**: Error
* **Example message**: Mapping expressions for saga members must point to properties.

remarks

## Saga mapping expressions can be simplified

* **Rule ID**: NSB0018
* **Severity**: Info
* **Example message**: This saga mapping expression can be rewritten using mapper.MapSaga(…).ToMessage<T>(…) syntax which avoids duplicate .ToSaga(…) expressions.

remarks

