---
title: Service Fabric Persistence Sagas
reviewed: 2019-01-16
component: ServiceFabricPersistence
---


## Reliable collections

Saga data is stored in reliable dictionaries.


## Saga data serialization

Saga data in stored in JSON format using [Json.NET](https://www.newtonsoft.com/json). 

Saga data serialization can be configured by providing custom [JsonSerializerSettings](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) instance

snippet: ServiceFabricPersistenceSagaJsonSerializerSettings

custom [JsonReader](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm) instance

snippet: ServiceFabricPersistenceSagaReaderCreator

or custom [JsonWriter](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm) instance

snippet: ServiceFabricPersistenceSagaWriterCreator


## Saga data storage 

By default saga data stored in multiple reliable collections - one per saga data type. Reliable collection name can be changed at the level of saga data type

snippet: ServiceFabricPersistenceSagaWithCustomCollectionName

Saga data identifier used as a key in reliable dictionary gets calculated, among others, from saga data type name. As a result, renaming saga data class name changes storage identifier for every saga data instance. This is a problem especially when some saga data instances have already been stored for a given saga data type. In such scenarios it is necessary to provide stable saga data type name

snippet: ServiceFabricPersistenceSagaWithCustomSagaDataName

## Saga concurrency

When simultaneously handling messages, conflicts may occur. See below for examples of the exceptions which are thrown. _[Saga concurrency](/nservicebus/sagas/concurrency.md)_ explains how these conflicts are handled, and contains guidance for high-load scenarios.

include: saga-concurrency

### Creating saga data

Example exception:

```
The saga with the correlation id 'Name: {correlationProperty.Name} Value: {correlationProperty.Value}' already exists.
```

### Updating or deleting saga data

ServiceFabric persistence uses [optimistic concurrency control](https://en.wikipedia.org/wiki/Optimistic_concurrency_control) when updating or deleting saga data.

Example exception:

```
{nameof(SagaPersister)} concurrency violation: saga entity Id[{sagaData.Id}] already saved.
```
