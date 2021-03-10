Azure Table Persistence supports the same set of types as [Azure Table Storage](https://docs.microsoft.com/en-us/rest/api/storageservices/understanding-the-table-service-data-model) and additional types that can be serialized into JSON using [Json.NET](https://www.newtonsoft.com/json). When a saga containing a property of an unsupported type is persisted, an exception containing the following information is thrown: `The property type 'the_property_name' is not supported on Azure Table Storage and it cannot be serialized with JSON.NET`.

#### Customization

Saga data serialization can be configured by providing a custom [JsonSerializerSettings](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) instance:

snippet: AzurePersistenceSagasJsonSerializerSettings

or with a custom [JsonReader](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm) instance:

snippet: AzurePersistenceSagasReaderCreator

or with a custom [JsonWriter](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm) instance:

snippet: AzurePersistenceSagasWriterCreator