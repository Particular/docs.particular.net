---
title: JSON Serializer
summary: A JSON serializer that uses Json.NET.
reviewed: 2017-07-14
component: Json
versions: '(,7)'
related:
 - samples/serializers/json
---

include: json-deprecation

Using [JSON](https://en.wikipedia.org/wiki/Json) via an [ILMerged](https://github.com/Microsoft/ILMerge) copy of [Json.NET](http://www.newtonsoft.com/json).


## Usage

snippet: JsonSerialization


partial: version


## Customization

Since Json.net is ILMerged, the Json.net customization attributes are not supported. However, certain customizations are still supported via standard .NET attributes.


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

partial: encoding

## Inferring message type from $type

For integration scenarios where the sender is unable to add message headers the serializer is able to infer the message type from the [`$type` property supported by json.net](https://www.newtonsoft.com/json/help/html/SerializeTypeNameHandling.htm). By default json.net requires the property to contain the assembly qualified name of the message type but to make integration easier the serializer also registers a custom binder that allows only the full type name to be passed.  

See [native integration with SqlTransport sample](/samples/sqltransport/native-integration) for more details.

## Bson

partial: bson
