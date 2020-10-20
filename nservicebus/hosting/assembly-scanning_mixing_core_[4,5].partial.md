
### Including assemblies using pattern matching:

snippet: ScanningIncludeByPattern

`AllAssemblies` helper class can be used to create a list of assemblies either by creating a denylist using the method `Except` or an allowlist by using Matching or a combination of both.

NOTE: The `Except`, `Matching` and `And` methods behave like `string.StartsWith(string)`.


### Mixing includes and excludes:

snippet: ScanningMixingIncludeAndExclude
