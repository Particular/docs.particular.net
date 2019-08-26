---
title: Json.NET Serializer
summary: A JSON serializer that uses Newtonsoft Json.NET.
component: Newtonsoft
reviewed: 2019-08-26
related:
 - samples/serializers/newtonsoft
 - samples/serializers/newtonsoft-bson
---

This serialiser uses [JSON](https://en.wikipedia.org/wiki/Json) via a NuGet dependency on [Json.NET](https://www.newtonsoft.com/json).

partial: howcoreusesjson


## Usage

snippet: NewtonsoftSerialization


### Json.NET attributes

Json.NET attributes are supported.

For example

snippet: NewtonsoftAttributes

NOTE: by default Json.NET serializer adds the Byte Order Mark (BOM). To disable it, see [custom writter](
/nservicebus/serialization/newtonsoft.md#usage-custom-writer) section.

### Custom settings

Customizes the instance of [JsonSerializerSettings](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) used for serialization.

snippet: NewtonsoftCustomSettings


### Custom reader

Customize the creation of the [JsonReader](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm).

snippet: NewtonsoftCustomReader


### Custom writer

Customize the creation of the [JsonWriter](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm).

In the example below, the custom writer omits the [Byte Order Mark (BOM)](https://en.wikipedia.org/wiki/Byte_order_mark).

snippet: NewtonsoftCustomWriter


include: custom-contenttype-key

snippet: NewtonsoftContentTypeKey

## Inferring message type from $type

For integration scenarios where the sender is unable to add message headers, the serializer is able to infer the message type from the [`$type` property supported by Json.NET](https://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm). 

See [native integration with SqlTransport sample](/samples/sqltransport/native-integration) for more details.

## BSON

Customize to use the [Newtonsoft Bson serialization](https://www.newtonsoft.com/json/help/html/SerializeToBson.htm).

snippet: NewtonsoftBson


## Compatibility with the core JSON serializer

Up to NServiceBus version 6, a [JSON serializer based on Json.NET](json.md) was bundled with the core package. This section outlines the compatibility considerations when switching to this serializer.

### No support for `XContainer` and `XDocument` properties

In contrast to the bundled serializer `XContainer` and `XDocument` properties are no longer supported. If `XContainer` and `XDocument` properties are required [use a JsonConverter](https://www.newtonsoft.com/json/help/html/CustomJsonConverter.htm) as shown below:

snippet: XContainerJsonConverter

Configure the converter as follows:

snippet: UseConverter

###  No support for bundled logical messages

This serializer is not compatible with multiple bundled messages (when using the `Send(object[] messages)` APIs) sent from NServiceBus version 3 and below. If this scenario is detected then an exception with the following message will be thrown:

```
Multiple messages in the same stream are not supported.
```

The `AddDeserializer` API can help transition between serializers. See the [Multiple Deserializers Sample](/samples/serializers/multiple-deserializers/) for more information.


### Use of $type requires an assembly qualified name

The bundled serializer registers a custom serialization binder in order to not require the assembly name to be present when inferring message type from the [`$type` property supported by json.net](https://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm). Not having to specify the assembly name can be useful to reduce coupling when using the serializer for native integration scenarios as [demonstrated in this sample](/samples/sqltransport/native-integration).

To make the serializer compatible with this behavior use the following serialization binder:

snippet: KnownTypesBinder

and configured as follows:

snippet: KnownTypesBinderConfig
