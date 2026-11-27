In version 9.1 and later, `WriterCreator` accepts a `Func<TextWriter, JsonWriter>`. The persister passes in the `TextWriter` it serializes the saga data into, so the custom `JsonWriter` writes directly into the persister's buffer:

snippet: SqlPersistenceCustomTextWriter

The `TextWriter` belongs to the persister and is valid only while the saga data is serialized:

- Do not keep a reference to the `TextWriter`.
- Do not close the `TextWriter` before serialization completes. The persister disposes the returned `JsonWriter`, which may close the `TextWriter`.

In version 9.1 and later, the overload that accepts a `Func<StringBuilder, JsonWriter>` is obsolete. If both overloads are configured, the `TextWriter` overload is used. Version 9.0 supports only the `StringBuilder` overload:
