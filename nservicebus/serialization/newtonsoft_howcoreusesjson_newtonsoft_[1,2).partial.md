## How the NServiceBus core uses Json.net

The core of [NServiceBus uses Json.net](json.md). However it is [ILMerged](https://github.com/Microsoft/ILMerge) where this library has a standard dll and NuGet dependency. While ILMerging reduces versioning issues in the core it does cause several restrictions:

 * Can't use a different version of Json.net
 * Can't use Json.net attributes
 * Can't customize the Json.net serialization behaviors.

These restrictions do not apply to this serializer.
