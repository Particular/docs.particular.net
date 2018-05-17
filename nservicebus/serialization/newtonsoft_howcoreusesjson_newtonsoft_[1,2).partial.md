## How the NServiceBus core uses Json.NET

The core of [NServiceBus uses Json.NET](json.md). However it is [ILMerged](https://github.com/Microsoft/ILMerge) whereas this library has a standard dll and NuGet dependency. While ILMerging reduces versioning issues in the core it does cause several restrictions:

 * Can't use a different version of Json.NET
 * Can't use Json.NET attributes
 * Can't customize the Json.NET serialization behaviors.

These restrictions do not apply to this serializer.
