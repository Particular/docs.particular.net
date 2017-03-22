---
title: Service Fabric Persistence Sagas
reviewed: 2017-03-22
component: ServiceFabricPersistence
---

## Reliable collections

When using the Service Fabric Persistence with a reliable service, saga data is stored using a reliable dictionary called `sagas`.  

## Saga data serialization

Service Fabric Persistence stores saga data in JSON format using uses Json.NET. 

Saga data serialization can be configured by providing custom [JsonSerializerSettings](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) instance 

snippet: ServiceFabricPersistenceSagaJsonSerializerSettings

custom [JsonReader](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm) instance

snippet: ServiceFabricPersistenceSagaReaderCreator

or custom [JsonWriter](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm) instance

snippet: ServiceFabricPersistenceSagaWriterCreator

## Saga data storage 

By default saga data stored in multiple reliable collections - one per saga data type. Reliable collection name can be changed at the level of saga data type using `ServiceFabricSagaAttribute.CollectionName`

snippet: ServiceFabricPersistenceSagaWithCustomCollectionName

Saga data storage identifier includes saga data type name. Renaming saga data class name changes storage identifier for every saga data instance. This is a problem especially when some saga data instances have already been stored for a given saga data type. In such scenarios it is necessary to provide stable saga data type name via `ServiceFabricSagaAttribute.CollectionName`

snippet: ServiceFabricPersistenceSagaWithCustomSagaDataName
