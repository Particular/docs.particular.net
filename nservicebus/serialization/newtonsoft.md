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

## Inferring message type from $type

For integration scenarios where the sender is unable to add message headers the serializer is able to infer the message type from the [`$type` property supported by json.net](https://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm). 

See [native integration with SqlTransport sample](/samples/sqltransport/native-integration) for more details.

## BSON

Customize to use the [Newtonsoft Bson serialization](http://www.newtonsoft.com/json/help/html/SerializeToBson.htm).

snippet: NewtonsoftBson


## Compatibility with the core JSON serializer

Up to Version 6 of NServiceBus a [JSON serializer based on Json.net](json.md) was bundled inside the core package. This section outlines the compatibility considerations when switching to this serializer.

### No support for `XContainer` and `XDocument` properties

In contrast to the bundled serializer `XContainer` and `XDocument` properties are no longer supported. If `XContainer` and `XDocument` properties are required [use a JsonConverter](newtonsoft.md#compatibility-with-the-core-json-serializer-use-a-jsonconverter-for-xcontainer-and-xdocument) as shown below:

snippet: XContainerJsonConverter

Configure the converter like shown below:

snippet: UseConverter

###  No support for bundled logical messages

This serializer is not compatible with multiple bundled messages (when using the `Send(object[] messages)` APIs) sent from Versions 3 and below of NServiceBus. If this scenario is detected then an exception with the following message will be thrown:

```
Multiple messages in the same stream are not supported.
```

The `AddDeserializer` API can help transition between serializers. See the [Multiple Deserializers Sample](/samples/serializers/multiple-deserializers/) for more information.


### Use of $type requires an assembly qualified name

The bundled serializer registers a custom serialization binder in order to not require the assembly name to be present when inferring message type from the [`$type` property supported by json.net](https://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm). Not having to specify the assembly name can be useful to reduce coupling when using the serializer for native integration scenarios like [demonstrated in this sample](/samples/sqltransport/native-integration).

To make the serializer compatible with this behavior use the following serialization binder:

snippet: KnownTypesBinder

and configured as follows:

snippet: KnownTypesBinderConfig