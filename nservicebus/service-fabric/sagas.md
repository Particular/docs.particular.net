---
title: Service Fabric Persistence Sagas
reviewed: 2017-03-20
component: ServiceFabricPersistence
---

TODO: describe saga persister [SUBJECT TO CHANGE, need to decide if a signle or multiple collections]


## Reliable collections

When using the Service Fabric Persistence with a reliable service, saga data is stored using a reliable dictionary called `sagas`.  

## Saga data serialization settings

Saga data gets stored in reliable collection in json format. Service Fabric Persister uses Json.NET for the purpose of serialization. 

It is possible to customize serialization by providing custom [JsonSerializerSettings](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) instance 

snippet: ServiceFabricPersistenceSagaJsonSerializerSettings

custom [JsonReader](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm) instance

snippet: ServiceFabricPersistenceSagaReaderCreator

or custom [JsonWriter](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm) instance

snippet: ServiceFabricPersistenceSagaWriterCreator


