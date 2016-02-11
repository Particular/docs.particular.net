---
title: Json.NET Serializer
summary: A json serializer that uses Newtonsoft Json.NET.
related:
 - samples/serializers/newtonsoft
---

Using [Json](https://en.wikipedia.org/wiki/Json) via a nuget dependency on [Json.NET](http://www.newtonsoft.com/json).


## The nuget package  [![NuGet Status](http://img.shields.io/nuget/v/NServiceBus.Newtonsoft.Json.svg?style=flat)](https://www.nuget.org/packages/NServiceBus.Newtonsoft.Json/)

https://nuget.org/packages/NServiceBus.Newtonsoft.Json/

    PM> Install-Package NServiceBus.Newtonsoft.Json


## But doesn't the NServiceBus core use Json.net

The core of [NServiceBus uses Json.net](json.md). However it is ILMerged where this library has a standard dll and nuget dependency. While ILMerging reduces versioning issues in the core it does cause several restrictions

 * Cant use a different version of Json.net
 * Cant use Json.net attributes
 * Cant customize the Json.net serialization behaviors.

These restrictions do no apply to this serializer.


## Comparability with the core json serializer 

The only incompatibility with the [core serializer](json.md) is that this serializer does not support the serialization of `XContainer` and `XDocument` properties. If  xml properties are required on your messages strings should be used instead. 


## Usage

snippet:NewtonsoftSerialization


### Json.net attributes

Json.net attributes are supported.

For example

snippet:NewtonsoftAttributes


### Custom Settings

Customizes the instance of `JsonSerializerSettings` used for serialization.

snippet: NewtonsoftCustomSettings


### Custom Reader

Customize the creation of the `JsonReader`.

snippet:NewtonsoftCustomReader


### Custom Writer

Customize the creation of the `JsonWriter`.

snippet:NewtonsoftCustomWriter