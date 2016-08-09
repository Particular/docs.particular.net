---
title: Json.NET Serializer
summary: A JSON serializer that uses Newtonsoft Json.NET.
component: Newtonsoft
reviewed: 2016-03-17
related:
 - samples/serializers/newtonsoft
related:
 - samples/serializers/newtonsoft-bson
---

Using [JSON](https://en.wikipedia.org/wiki/Json) via a nuget dependency on [Json.NET](http://www.newtonsoft.com/json).


## The nuget package

https://www.nuget.org/packages/NServiceBus.Newtonsoft.Json/

    PM> Install-Package NServiceBus.Newtonsoft.Json


## How the NServiceBus core uses Json.net

The core of [NServiceBus uses Json.net](json.md). However it is ILMerged where this library has a standard dll and nuget dependency. While ILMerging reduces versioning issues in the core it does cause several restrictions:

 * Can't use a different version of Json.net
 * Can't use Json.net attributes
 * Can't customize the Json.net serialization behaviors.

These restrictions do not apply to this serializer.


## Comparability with the core JSON serializer

The only incompatibility with the [core serializer](json.md) is that this serializer does not support the serialization of `XContainer` and `XDocument` properties. If XML properties are required on messages strings should be used instead.


{{WARNING:
This serializer is not compatible with multiple bundled messages (when using the `bus.Send(object[] messages)` APIs) sent from Versions 3 and below of NServiceBus. If this scenario is detected then an exception with the following message will be thrown: 

```no-highlight
Multiple messages in the same stream are not supported.
```

The `AddDeserializer` API can help transition between serializers. See the [Multiple Deserializers Sample](/samples/serializers/multiple-deserializers/) for more information.

}}




## Usage

snippet:NewtonsoftSerialization


### Json.net attributes

Json.net attributes are supported.

For example

snippet:NewtonsoftAttributes


### Custom Settings

Customizes the instance of [JsonSerializerSettings](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonSerializerSettings.htm) used for serialization.

snippet: NewtonsoftCustomSettings


### Custom Reader

Customize the creation of the [JsonReader](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm).

snippet:NewtonsoftCustomReader


### Custom Writer

Customize the creation of the [JsonWriter](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm).

snippet:NewtonsoftCustomWriter


## BSON

Customize to use the [Newtonsoft Bson serialization](http://www.newtonsoft.com/json/help/html/SerializeToBson.htm).

snippet: NewtonsoftBson
