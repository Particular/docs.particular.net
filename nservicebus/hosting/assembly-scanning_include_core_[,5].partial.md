### Including assemblies

snippet: ScanningListOfAssemblies

### Controlling the exact types that NServiceBus uses

snippet: ScanningListOfTypes

### Specifying the directory to scan

snippet: ScanningCustomDirectory

### Including assemblies using pattern matching

snippet: ScanningIncludeByPattern

The `AllAssemblies` helper class can be used to create a list of assemblies by creating: a) a _deny list_ using the method `Except`, b) an _allow list_ by using `Matching`, or c) a combination of both.

NOTE: The `Except`, `Matching` and `And` methods behave like `string.StartsWith(string)`.

### Mixing includes and excludes

snippet: ScanningMixingIncludeAndExclude
