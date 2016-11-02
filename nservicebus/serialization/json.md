---
title: JSON Serializer
summary: A json serializer that uses Json.NET.
reviewed: 2016-08-23
component: Core
related:
 - samples/serializers/json
---

Using [Json](https://en.wikipedia.org/wiki/Json) via an [ILMerged](https://github.com/Microsoft/ILMerge) copy of [Json.NET](http://www.newtonsoft.com/json).


## Usage

snippet:JsonSerialization


partial: version


## Customization

Since Json.net is ILMerged the Json.net customization attributes are not supported. However certain customizations are still supported via standard .NET attributes.


### Excluding members

Members can be exclude via the [IgnoreDataMemberAttribute](https://msdn.microsoft.com/en-us/library/system.runtime.serialization.ignoredatamemberattribute.aspx).

The attribute can be used as such

```cs
public class Person
{
    public string FamilyName { get; set; }
    public string GivenNames { get; set; }

    [IgnoreDataMember]
    public string FullName { get; set; }
}
```

Then when this is serialized.

```cs
Person person = new Person
{
    GivenNames = "John",
    FamilyName = "Smith",
    FullName = "John Smith"
};
```

The result will be

```json
{"FamilyName":"Smith","GivenNames":"John"}
```


## Bson

partial: bson