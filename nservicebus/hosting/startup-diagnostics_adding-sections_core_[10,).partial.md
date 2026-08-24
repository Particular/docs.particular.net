To extend the startup diagnostics with custom sections:

snippet: CustomDiagnosticsSection

Starting in version 10.3, custom sections can be registered with a strongly-typed value and its `JsonTypeInfo<T>`. The section value must be a named type registered with a source-generated `JsonSerializerContext`:

snippet: CustomDiagnosticsSectionTypes

Registering sections with type information makes startup diagnostics serialization AOT-safe and trimming-safe. This is required when reflection-based serialization is disabled, such as in NativeAOT applications. In that case, a section registered with the object-based overload cannot be serialized. NServiceBus logs an error identifying the section, and the diagnostics document is not written. When reflection-based serialization is disabled, every section in the document must be registered with type information. A single legacy section prevents the complete document from being written.

The object-based overload remains available and continues to work when reflection-based serialization is enabled.

Use the factory overload when the diagnostics value is expensive to compute and should only be evaluated when the diagnostics are actually written. The factory is evaluated once, when the diagnostics document is written, and the resulting value is reused for every output target, including the log, the file, and any custom diagnostics writer. For cheap values, prefer the direct overload:

snippet: CustomDiagnosticsSectionFactory
