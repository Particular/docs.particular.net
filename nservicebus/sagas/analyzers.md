---
title: Roslyn analyzers for sagas
summary: Details of the Roslyn analyzers that promote code quality in sagas.
component: Core
versions: '[7.7,)'
reviewed: 2024-11-05
---

Starting in NServiceBus version 7.7, [Roslyn analyzers](https://docs.microsoft.com/en-us/visualstudio/code-quality/roslyn-analyzers-overview) that analyze the code in sagas and make suggestions for improvements directly in the editor, are packaged with the NServiceBus package.

## Non-mapping expression used in ConfigureHowToFindSaga method

* **Rule ID**: NSB0003
* **Severity**: Warning, Error starting in NServiceBus version 8
* **Example message**: The ConfigureHowToFindSaga method should only contain mapping expressions (i.e. 'mapper.MapSaga().ToMessage<T>()') and not contain any other logic.

The `ConfigureHowToFindSaga` method is executed to determine the mappings between incoming messages and stored saga data. Arbitrary statements or calls to other methods, while they may be valid C#, are not valid in this method.

See [message correlation](message-correlation.md) for details on how to map incoming messages to stored saga data.

partial: simplify-mapping

## Message that starts the saga does not have a message mapping

* **Rule ID**: NSB0006
* **Severity**: Warning, Error starting in NServiceBus version 8
* **Example message**: Saga `MySaga` implements `IAmStartedByMessages<MyMessage>` but does not provide a mapping for that message type. In the `ConfigureHowToFindSaga` method, after calling `mapper.MapSaga(saga => saga.CorrelationPropertyName)`, add `.ToMessage<MyMessage>(msg => msg.PropertyName)` to map a message property to the saga correlation ID, or `.ToMessageHeader<MyMessage>("HeaderName")` to map a header value that will contain the correlation ID.

A message type identified by `IAmStartedByMessages<TMessage>` means that the message of type `TMessage` can start the saga. Because there may not yet be any saga data when this message is received, a message identified in this way **must** have an associated message mapping in the `ConfigureHowToFindSaga()` method; otherwise, it would be impossible to know if saga data had already been created.

The code fix will attempt to rewrite the `ConfigureHowToFindSaga()` method and generate the missing mapping. If the existing mapping expressions already identify a correlation id (i.e. `sagaData.OrderId`) and the message type being mapped has a property with a matching name (i.e. `message.OrderId`), then the mapping will automatically use that property name. Otherwise, the code fix will generate a mapping expression with placeholders to fill in.

## Saga data property is not writeable

* **Rule ID**: NSB0007
* **Severity**: Warning, Error starting in NServiceBus version 8
* **Example message**: Saga data property `MySagaData.MyProperty`does not have a public setter. This could interfere with loading saga data. Add a public setter.

Many saga persistence libraries use serialization and deserialization to store and load saga data. It's not always possible for serializers to set values unless the property is marked as `public` and has a `public` setter.

A saga data class is not a good place to employ specialized data access patterns to restrict write access to certain properties. Saga data classes should be considered internal storage, fully owned by the saga, and are best implemented as simple properties like `public string PropertyName { get; set; }` without any access modifiers.

## Saga message mappings are not needed for timeouts

* **Rule ID**: NSB0008
* **Severity**: Warning
* **Example message**: Message type `MyMessage` is mapped as `IHandleTimeouts<MyMessage>`, which do not require saga mapping because timeouts have the saga's Id embedded in the message. &#89;&#111;&#117; can remove this mapping expression.

When sagas request timeouts, the delayed message is stamped with a header that includes the full saga ID. This enables the lookup of the correct saga data without requiring a mapping in the `ConfigureHowToFindSaga()` method. Removing the mapping will have no effect on the saga's ability to find the correct saga data when the timeout is processed, and it can be safely removed.

## A saga cannot use the Id property for a Correlation ID

* **Rule ID**: NSB0009
* **Severity**: Warning, Error starting in NServiceBus version 8
* **Example message**: A saga cannot map to the saga data's Id property, regardless of casing. Select a different property (such as OrderId, CustomerId) that relates all of the messages handled by this saga.

The `Id` property of a saga (defined by the required `IContainSagaData` interface) is reserved for use by the saga. It cannot be used as a correlation ID by mapping to it in the `ConfigureHowToFindSaga()` method.

In addition, some saga persistence libraries, such as [SQL Persistence](/persistence/sql/), store the saga's `Id` value in a column, and the column names are commonly case-insensitive. This means that other casings of `Id` (e.g. `ID`, `id`, or even `iD`) are also not allowed.

## Message types should not be used as saga data properties

* **Rule ID**: NSB0010
* **Severity**: Warning
* **Example message**: Using the message type `MyMessage` to store message contents in saga data is not recommended, as it creates unnecessary coupling between the structure of the message and the stored saga data, making both more difficult to evolve.

When a saga receives a message, it can be tempting to insert the whole message into the saga data.

However, this creates an unintended coupling between the saga data and the message contract.

The saga data class is wholly owned and managed by the saga and represents the internal stored state of that saga. It must be able to be stored to disk and reloaded perhaps minutes, hours, or even days/years later. I.e. it must be durable.

On the other hand, the message is more ephemeral. It only needs to be stable long enough to deal with any messages currently in-flight at the time a new version of a software system is released. Its ownership is different and needs to be able to change over time.

By storing a message type inside the saga data, the ephemeral message structure must be locked down by the same rules as the saga data, making it harder for the saga and the other message endpoints it exchanges messages with to evolve.

## Correlation ID property must be a supported type

* **Rule ID**: NSB0011
* **Severity**: Error
* **Example message**: A saga correlation property must be one of the following types: string, Guid, long, ulong, int, uint, short, ushort

The correlation property represents the logical identity of the stored saga data. It needs to be something easily represented by nearly every saga persistence library. For example, `DateTime` is a bad correlation property type because it is represented differently on different storage systems, such as between relational SQL tables and NoSQL databases, or even in various amounts of precision in fractions of a second stored between different relational database systems.

Prior to NServiceBus version 7.7, this check was a runtime error. In NServiceBus version 7.7 and above, the analyzer diagnostic raises this feedback at compile time.

## Saga data classes should inherit ContainSagaData

* **Rule ID**: NSB0012
* **Severity**: Warning
* **Example message**: It's easier to inherit the class `ContainSagaData`, which contains all the necessary properties to implement `IContainSagaData`, than to implement IContainSagaData directly.

The generic class constraints on `Saga<TSagaData>` require the saga data class to implement the `IContainSagaData` interface, which specifies properties required by the saga infrastructure. However, it is much easier to directly inherit `ContainSagaData`, which already specifies these properties.

A benefit to inheriting the `ContainSagaData` class is that in NServiceBus version 7 and above, the implemented properties are decorated with `[EditorBrowsable(EditorBrowsableState.Never)]`, which means that those properties that are _only_ needed by the saga infrastructure will not appear in IntelliSense. So, it is less likely that one of these reserved properties will be used accidentally.

One exception comes when [using NHibernate's `[RowVersion]` attribute to control optimistic concurrency](/persistence/nhibernate/saga-concurrency.md#customizing-concurrency-behavior-explicit-version). This attribute is not compatible with derived classes. In this case, the saga data class must implement `IContainSagaData` directly. The `NSB0012` diagnostic can be suppressed for this scenario to remove the warning.

## Reply in Saga should be ReplyToOriginator

* **Rule ID**: NSB0013
* **Severity**: Info
* **Example message**: In a Saga, `context.Reply()` will reply to the sender of the immediate message, which isn't common. To reply to the message that started the saga, use the saga's `ReplyToOriginator()` method.

Using `context.Reply(…)` in a message handler is fairly common but less common (and can be confusing) when used in a saga.

Calling `.Reply(…)` always replies to the immediate message. Imagine a saga is started by `Msg1`, which sends `DoSomething` to an external handler, and that handler replies with a `DoSomethingResponse`. If the saga calls `context.Reply(…)` within the handler for `DoSomethingResponse`, the reply will be sent to the handler that processed `DoSomething`.

More often, a reply in a saga should instead use `.ReplyToOriginator(…)`. In the example above, this will go to the endpoint that originally sent the `Msg1` to start the saga, which is most often the desired effect when used in a saga.

## Saga should not have intermediate base class

* **Rule ID**: NSB0014
* **Severity**: Warning
* **Example message**: A saga should not have an intermediate base class and should inherit directly from NServiceBus.Saga<TSagaData>.

Sagas should not use a base class (i.e. `MySaga : MyAbstractSaga<TSagaData>`) to provide shared functionality to multiple saga types. While this may work for sagas using [Learning Persistence](/persistence/learning/), some persistence libraries such as [SQL Persistence](/persistence/sql/) are unable to generate database scripts when sagas are constructed in this way.

A better way to provide shared functionality to multiple saga types and reduce code duplication is to use [extension methods](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods).

## Saga should not implement IHandleSagaNotFound

* **Rule ID**: NSB0015
* **Severity**: Warning, Error starting in NServiceBus version 8
* **Example message**: A saga should not implement `IHandleSagaNotFound`, as this catch-all handler will handle messages where *any* saga is not found. Implement `IHandleSagaNotFound` on a separate class instead.

A ["saga not found" handler](/nservicebus/sagas/saga-not-found.md) provides a way to deal with messages that are not allowed to start a saga but cannot find existing saga data.

"Saga not found" handlers operate on all saga messages within an endpoint, no matter which saga the message was originally bound for. So it is misleading to implement `IHandleSagaNotFound` on a saga because it creates the impression that it will only handle not found messages for that _specific_ saga, which is false.

Instead, implement `IHandleSagaNotFound` on an independent class.

## Correlation property must match message mapping expression type

* **Rule ID**: NSB0016
* **Severity**: Error
* **Example message**: When mapping a message to a saga, the member type on the message and the saga property must match. `MySagaData.CorrelationProperty` is of type `string` and `MyMessage.CorrelationProperty` is of type `int`.

When mapping incoming message properties to the saga's correlation property, these must be the same type, or they can't be compared.

When the correlation value can be expressed in different ways, it's best to represent the saga's correlation ID as a string. Individual message mapping expressions can format incoming values of other types into a string to match the saga's correlation value, such as this example where one message type contains the value as a `Guid`:

snippet: SagaAnalyzerToMessageStringExpressions

When using [nullable reference types](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references), the nullability of the values also matters. A saga data class containing a nullable `string?` can accept a non-nullable `string` from a message, because the `string?` is more permissive than non-nullable `string`. However, the reverse is not true and will trigger the diagnostic.

## ToSaga mapping must point to a property

* **Rule ID**: NSB0017
* **Severity**: Error
* **Example message**: Mapping expressions for saga members must point to properties.

The "to saga" expression argument of the `MapSaga(…)` or `ToSaga(…)` methods must point directly to a property. Mapping to a field or an expression is not valid.

Mapping directly to a property is **valid**:

snippet: SagaAnalyzerToSagaPropertyOk

Mapping to a field or an arbitrary expression is **invalid**:

snippet: SagaAnalyzerToSagaFieldNotOk

Mapping to an arbitrary expression is also **invalid**:

snippet: SagaAnalyzerToSagaExpressionNotOk
