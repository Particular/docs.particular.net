---
title: Jil Serializer
summary: A JSON serializer that uses Jil.
component: Jil
reviewed: 2018-09-05
related:
 - samples/serializers/jil
---

This sample demonstrates serialization using [JSON](https://en.wikipedia.org/wiki/Json) via a NuGet dependency on [Jil](https://github.com/kevin-montrose/Jil).


## Usage

snippet: JilSerialization


### Custom settings

Customizes the instance of `Options` used for serialization.

snippet: JilCustomSettings


### Custom reader

Customize the creation of the [JsonReader](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm).

snippet: JilCustomReader


### Custom writer

Customize the creation of the [JsonWriter](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm).

snippet: JilCustomWriter


include: custom-contenttype-key

snippet: JilContentTypeKey


## Currently not supported

Usages of `DataBusProperty<T>` are not supported since it doesn't have a default constructor. However usage of the [databus convention](/nservicebus/messaging/databus) is supported.