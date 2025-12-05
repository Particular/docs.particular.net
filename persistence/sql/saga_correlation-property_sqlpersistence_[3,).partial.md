For NServiceBus sagas, an attempt will be made to determine the correlation property at compile time by analyzing the `ConfigureHowToFindSaga` method. There are a few unsupported scenarios where this is impossible and an exception will be thrown:

* Use of an external method or delegate
* Branching or looping logic inside the `ConfigureHowToFindSaga` method
* Non-matching correlation properties for multiple message types

In these cases, either redesign the saga to avoid these patterns or specify the correlation property with a [`[SqlSaga]` attribute](#correlation-ids-specifying-correlation-id-using-an-attribute).


### Specifying correlation ID using an attribute

In rare cases where the correlation property cannot be inferred from the `ConfigureHowToFindSaga` method, it can be specified with the `[SqlSaga]` attribute:

snippet: correlation-with-attribute


### Transitional correlation ID

In cases where business requirements dictate that the correlation property for a saga must change, a transitional correlation property can be used to gradually make that change over time. This takes into account in-flight messages and in-progress sagas that do not contain the new data.

If an incoming message cannot be mapped to a saga data instance using the correlation property, a saga that has a defined _transitional_ correlation property will also query against the additional column for a match. Once all sagas have been updated to contain the transitional correlation property, the old correlation property can be retired and the transitional property can become the new standard correlation property.

To define a transitional correlation property on a saga, use the `[SqlSaga]` attribute:

snippet: transitional-correlation-with-attribute
