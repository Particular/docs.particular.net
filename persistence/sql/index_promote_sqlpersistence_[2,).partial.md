
### Promotion

As stated above, scripts are created in the target project output directory. Generally this directory will be excluded from source control. To add created scripts to source control they can be "promoted".

WARNING: The target directory will be deleted and recreated as part of each build. Be sure to choose a path that is for script promotion only.

Some token replacement using [MSBuild variables](https://msdn.microsoft.com/en-us/library/c02as0cs.aspx) is supported.

 * `$(SolutionDir)`: The directory of the solution.
 * `$(ProjectDir)`: The directory of the project

All tokens are drive + path and include the trailing backslash `\`.

snippet: PromoteScripts

The path calculation is performed relative to the current project directory. So, for example, a value of `PromotedSqlScripts` (with no tokens) would evaluate as `$(ProjectDir)PromotedSqlScripts`.
