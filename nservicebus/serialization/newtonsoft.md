---
title: Json.NET Serializer
summary: A JSON serializer that uses Newtonsoft Json.NET.
component: Newtonsoft
reviewed: 2017-10-13
related:
 - samples/serializers/newtonsoft
 - samples/serializers/newtonsoft-bson
---

Using [JSON](https://en.wikipedia.org/wiki/Json) via a NuGet dependency on [Json.NET](http://www.newtonsoft.com/json).

partial: howcoreusesjson


## Usage

snippet: NewtonsoftSerialization


### Json.net attributes

Json.net attributes are supported.

For example

snippet: NewtonsoftAttributes


### Custom Settings

Customizes the instance of [JsonSerializerSettings](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) used for serialization.

snippet: NewtonsoftCustomSettings


### Custom Reader

Customize the creation of the [JsonReader](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm).

snippet: NewtonsoftCustomReader


### Custom Writer

Customize the creation of the [JsonWriter](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm).

snippet: NewtonsoftCustomWriter


include: custom-contenttype-key

snippet: NewtonsoftContentTypeKey


## BSON

Customize to use the [Newtonsoft Bson serialization](http://www.newtonsoft.com/json/help/html/SerializeToBson.htm).

snippet: NewtonsoftBson


## Compatibility with the core JSON serializer

### No support for `XContainer` and `XDocument` properties

In contrast to the [core serializer](json.md) is that this serializer does not support the serialization of `XContainer` and `XDocument` properties. If XML properties are required on messages strings should be used instead. If `XContainer` and `XDocument` properties are required [use a JsonConverter](newtonsoft.md#compatibility-with-the-core-json-serializer-use-a-jsonconverter-for-xcontainer-and-xdocument).

{{WARNING:
This serializer is not compatible with multiple bundled messages (when using the `Send(object[] messages)` APIs) sent from Versions 3 and below of NServiceBus. If this scenario is detected then an exception with the following message will be thrown:

```
Multiple messages in the same stream are not supported.
```

The `AddDeserializer` API can help transition between serializers. See the [Multiple Deserializers Sample](/samples/serializers/multiple-deserializers/) for more information.

}}


#### Use a JsonConverter for XContainer and XDocument


##### The JsonConverter

This is a custom [JsonConverter](http://www.newtonsoft.com/json/help/html/CustomJsonConverter.htm) that replicates the approach used by the [core serializer](json.md).

snippet: XContainerJsonConverter


##### Use the JsonConverter

At configuration time the JsonConverter can then be used with the following.

snippet: UseConverter

### Use of $type requires an assembly qualified name

The [core serializer](json.md) is registering a custom serialization binder to not require the assembly name to be present when inferring message type from the special [`$type` property supported by json.net](https://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm). Not having to specify the assembly name can be useful to reduce coupling when using the serializer for native integration scenarios like [demonstrated in this sample](/samples/sqltransport/native-integration).

To make the serializer compatible with this behavior the following serialization binder can be used:

snippet: KnownTypesBinder

and configured using the following configuration:

snippet: KnownTypesBinderConfig