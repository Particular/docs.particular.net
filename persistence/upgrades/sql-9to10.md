---
title: SQL Persistence Upgrade Version 9 to 10
summary: Migration instructions on how to upgrade to SQL Persistence version 10
reviewed: 2026-11-27
component: SqlPersistence
related:
- persistence/sql
- persistence/sql/saga
isUpgradeGuide: true
---

Version 10 turns APIs that are obsolete in version 9 into errors. Endpoints that compile without obsolete warnings on the latest version 9 do not need any changes for the APIs listed in this guide.

| Change | Action required |
|--------|-----------------|
| [`StringBuilder` overload of `WriterCreator`](#stringbuilder-overload-of-writercreator) | Use the `TextWriter` overload |

## `StringBuilder` overload of `WriterCreator`

The `SagaSettings.WriterCreator` overload that accepts a `Func<StringBuilder, JsonWriter>` is obsolete with a warning starting in version 9.1 and generates an error in version 10. It will be removed in version 11.

The `WriterCreator` overload that accepts a `Func<TextWriter, JsonWriter>`, added in version 9.1, passes in the `TextWriter` the persister serializes the saga data into, so the custom `JsonWriter` writes directly into the persister's buffer instead of into a separate `StringBuilder`.

Endpoints that do not call `WriterCreator` are not affected. No action is required.

Endpoints that [customize the saga JSON writer](/persistence/sql/saga.md#json-net-settings-custom-writer) with the `StringBuilder` overload must pass the `TextWriter` to the `JsonWriter` instead.

Before (version 9.0):

```csharp
var sagaSettings = persistence.SagaSettings();
sagaSettings.WriterCreator(
    writerCreator: builder =>
    {
        var writer = new StringWriter(builder);
        return new JsonTextWriter(writer)
        {
            Formatting = Formatting.None
        };
    });
```

After (version 9.1 and later):

```csharp
var sagaSettings = persistence.SagaSettings();
sagaSettings.WriterCreator(
    writerCreator: textWriter =>
    {
        return new JsonTextWriter(textWriter)
        {
            Formatting = Formatting.None
        };
    });
```

The `TextWriter` belongs to the persister and is valid only while the saga data is serialized. Do not keep a reference to it, and do not close it before serialization completes.
