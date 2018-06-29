For NServiceBus sagas, in most cases the correlation property can be inferred at compile time by inspecting the intermediate language (IL) for calls to `.ToSaga(sagaData => sagaData.CorrelationPropertyName). There are a few unsupported instances where this is impossible and an exception will be thrown:

* Use of an external method or delegate
* Branching or looping logic inside the `ConfigureHowToFindSaga` method
* Non-matching correlation properties for multiple message types

In these cases, either redesign the saga to avoid these patterns, specify the correlation property with a [`[SqlSaga]` attribute](#correlation-ids-specifying-correlation-id-using-attribute), or use the [`SqlSaga` base class](sqlsaga.md).


### Specifying correlation id using attribute

In rare cases where the correlation property cannot be inferred from the `ConfigureHowToFindSaga` method, it can be specified with the `[SqlSaga]` attribute:

snippet: correlation-with-attribute


### Transitional Correlation Id

In cases where business requirements dictate that the correlation property for a saga needs to change, a transitional correlation property can be used to gradually make that change over time, taking into account that there may be in-flight messages and already-running sagas that are not updated with the new data.

If an incoming message cannot be mapped to a saga data instance using the correlation property, a saga that has a defined _transitional_ correlation property will also query against the additional column for a match. Once all sagas have been updated to contain the transitional id, the old correlation property can be retired and the transitional property can become the new standard correlation property.

To define a transitional correlation property on a saga, use the `[SqlSaga]` attribute:

snippet: transitional-correlation-with-attribute

A transitional correlation property can also be expressed as a class property when using the [SqlSaga base class](sqlsaga.md#correlation-ids-correlation-and-transitional-ids).