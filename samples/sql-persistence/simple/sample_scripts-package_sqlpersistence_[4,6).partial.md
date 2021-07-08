Note that only `ServerShared` has the [NServiceBus.Persistence.Sql NuGet package](https://www.nuget.org/packages/NServiceBus.Persistence.Sql) directly referenced. This will cause the script directory `ServerShared\bin\Debug\[TFM]\NServiceBus.Persistence.Sql\[Variant]` to be populated at build time.

This is a test for only for V4-5.

#variant 45-control
This is the control for V4-5
#end-variant
#variant 45-alternate
This is the alternate for V4-5
#end-variant
