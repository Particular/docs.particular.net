---
title: JSON Serializer
summary: A json serializer that uses Json.NET.
related:
 - samples/serializers/json
---

Using [Json](https://en.wikipedia.org/wiki/Json) via an ILMerged copy of [Json.NET](http://www.newtonsoft.com/json).


## Usage

snippet:JsonSerialization


## Json.net versions

Over time the version of ILMerged Json.NET has changed.

| NServiceBus Version | Json.net Version |
|---|---|
| 3  | 4.0.8 |
| 4.0 | 4.5.11 |
| 4.1-5.X | 5.0.6 |


## Customization

Since Json.net is ILMerged the Json.net customization attributes are not supported. However certain customizations are still supported via standard .net attributes.


### Excluding members

Members can be exclude via the [IgnoreDataMemberAttribute](https://msdn.microsoft.com/en-us/library/system.runtime.serialization.ignoredatamemberattribute.aspx).

The attribute can be used as such

```
public class Person
{
    public string FamilyName { get; set; }
    public string GivenNames { get; set; }

    [IgnoreDataMember]
    public string FullName { get; set; }
}
```

Then when this is serialized.

```
Person person = new Person
{
    GivenNames = "John",
    FamilyName = "Smith",
    FullName = "John Smith"
};
```

The result will be

```
{"FamilyName":"Smith","GivenNames":"John"}
```


## Bson

Using [Bson](https://en.wikipedia.org/wiki/BSON) via the same ILMerged copy of Json.NET as above.


### Usage

snippet:BsonSerialization
