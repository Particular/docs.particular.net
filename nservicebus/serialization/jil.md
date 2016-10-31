---
title: Jil Serializer
summary: A JSON serializer that uses Jil.
component: Jil
reviewed: 2016-10-31
related:
 - samples/serializers/jil
---

Using [JSON](https://en.wikipedia.org/wiki/Json) via a NuGet dependency on [Jil](https://github.com/kevin-montrose/Jil).


## Usage

snippet:JilSerialization


### Custom Settings

Customizes the instance of `Options` used for serialization.

snippet: JilCustomSettings


### Custom Reader

Customize the creation of the [JsonReader](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonReader.htm).

snippet:JilCustomReader


### Custom Writer

Customize the creation of the [JsonWriter](http://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_JsonWriter.htm).

snippet:JilCustomWriter


include: custom-contenttype-key

snippet:JilContentTypeKey

