---
title: Json.NET Serializer
summary: A JSON serializer that uses Newtonsoft Json.NET.
component: Newtonsoft
reviewed: 2016-03-17
related:
 - samples/serializers/newtonsoft
 - samples/serializers/newtonsoft-bson
---

Using [JSON](https://en.wikipedia.org/wiki/Json) via a NuGet dependency on [Json.NET](http://www.newtonsoft.com/json).


## How the NServiceBus core uses Json.net

The core of [NServiceBus uses Json.net](json.md). However it is [ILMerged](https://github.com/Microsoft/ILMerge) where this library has a standard dll and NuGet dependency. While ILMerging reduces versioning issues in the core it does cause several restrictions:

 * Can't use a different version of Json.net
 * Can't use Json.net attributes
 * Can't customize the Json.net serialization behaviors.

These restrictions do not apply to this serializer.


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

The only incompatibility with the [core serializer](json.md) is that this serializer does not support the serialization of `XContainer` and `XDocument` properties. If XML properties are required on messages strings should be used instead. If `XContainer` and `XDocument` properties are required [use a JsonConverter](newtonsoft.md#compatibility-with-the-core-json-serializer-use-a-jsonconverter-for-xcontainer-and-xdocument).

{{WARNING:
This serializer is not compatible with multiple bundled messages (when using the `Send(object[] messages)` APIs) sent from Versions 3 and below of NServiceBus. If this scenario is detected then an exception with the following message will be thrown: 

```
Multiple messages in the same stream are not supported.
```

The `AddDeserializer` API can help transition between serializers. See the [Multiple Deserializers Sample](/samples/serializers/multiple-deserializers/) for more information.

}}


### Use a JsonConverter for XContainer and XDocument


#### The JsonConverter

This is a custom [JsonConverter](http://www.newtonsoft.com/json/help/html/CustomJsonConverter.htm) that replicates the approach used by the [core serializer](json.md).

snippet: XContainerJsonConverter


#### Use the JsonConverter

At configuration time the JsonConverter can then be used with the following.

snippet: UseConverter