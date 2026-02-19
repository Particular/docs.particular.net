### Code Snippets

#### Defining Snippets

There is a some code located [here](https://github.com/Particular/docs.particular.net/tree/master/Snippets). Any directory containing an `_excludesnippets` file will have its snippets ignored.

File extensions scanned for snippets include:

* `.config`
* `.cs`
* `.cscfg`
* `.csdef`
* `csproj`
* `.html`
* `.sql`
* `.txt`
* `.xml`
* `.xsd`
* `ps1`
* `.ps`
* `.json`
* `.proto`
* `.config`
* `.yml`
* `.yaml`
* `Dockerfile`

#### Snippets are highlighted using highlightjs

* [Documentation](https://highlightjs.readthedocs.io/)
* [Language List](https://highlightjs.readthedocs.io/en/latest/css-classes-reference.html#language-names-and-aliases)

#### Inline Code

* [highlight.js demo](https://highlightjs.org/static/demo/)
* [highlight.js languages](https://github.com/highlightjs/highlight.js/tree/main/src/languages)

| language     | key            |
|--------------|----------------|
| c#           | `cs`           |
| xml          | `xml`          |
| command line | `shell`        |
| powershell   | `ps`           |
| json         | `json`         |
| sql          | `sql`          |

**Always use fenced code blocks with a language.** If no language is defined then highlightjs will guess the language and it regularly gets it wrong.

> **_NOTE:_** For `.razor` files, since the `#region` keyword that is used to identify snippets can only be used inside of a razor code block, the language of the snippet will be changed from `razor` to `cs`, due to the current limitations of highlightjs for `.razor` files.

##### Using comments

Any code wrapped in a convention-based comment will be picked up. The comment needs to start with `startcode` which is followed by the key.

```c#
// startcode ConfigureWith
var configure = Configure.With();
// endcode
```

For non-code snippets apply a similar approach as in code, using comments appropriate for a given file type. For plain-text files an extra empty line is required before `endcode` tag.

|Tag        |XML-based                    |PowerShell            |SQL script             |Plain text          |Dockerfile / Compose / YAML         |
|-----------|-----------------------------|----------------------|-----------------------|--------------------|--------------------|
|**Open**   |`<!-- startcode name -->`|`# startcode name`|`-- startcode name`|`startcode name`|`# startcode name`
|Content    |                             |                      |                       |                    |
|**Close**  |`<!-- endcode -->`       |`# endcode`       |`-- endcode`       |`endcode`       |`# endcode`

##### Using regions

Any code wrapped in a named C# region will be picked up. The name of the region is used as the key.

```c#
#region ConfigureWith
var configure = Configure.With();
#endregion
```

#### Snippet versioning

Snippets are versioned. These versions are used to render snippets in a tabbed manner.

<img src="tabbed_snippets.png" style='border:1px solid #000000' />

Versions follow the [NuGet versioning convention](https://docs.nuget.org/create/versioning#specifying-version-ranges-in-.nuspec-files). If either `Minor` or `Patch` is not defined they will be rendered as an `x`. For example, version `3.3` would be rendered as `3.3.x` and version `3` would be rendered as `3.x`.

Snippet versions are derived in two ways

##### Version suffix on snippets

Appending a version to the end of a snippet definition as follows:

```c#
#region ConfigureWith 4.5
var configure = Configure.With();
#endregion
```

Or version range:

```c#
#region MySnippetName [1.0,2.0]
// My Snippet Code
#endregion
```

##### Convention based on the directory

If a snippet has no version defined then the version will be derived by walking up the directory tree until if finds a directory that is suffixed with `_Version` or `_VersionRange`. For example:

* Snippets extracted from `docs.particular.net\Snippets\Snippets_4\TheClass.cs` would have a default version of `(≥ 4.0.0 && < 5.0.0)`.
* Snippets extracted from `docs.particular.net\Snippets\Snippets_4\Special_4.3\TheClass.cs` would have a default version of `(≥ 4.3.0 && < 5.0.0)`.
* Snippets extracted from `docs.particular.net\Snippets\Special_(1.0,2.0)\TheClass.cs` would have a default version of `(> 1.0.0 && < 2.0.0)`.

##### Pre-release marker file

If a file named `prerelease.txt` exists in a versioned directory then a `-pre` will be added to the version.

For example, if there is a directory `docs.particular.net\Snippets\{Component}\Snippets_6\` and it contains a `prerelease.txt` file then the version will be `(≥ 6.0.0-pre)`

#### Using Snippets

The keyed snippets can then be used in any documentation `.md` file by adding the text

```markdown
snippet: KEY
```

Then snippets with the key (all versions) will be rendered in a tabbed manner. If there is only a single version then it will be rendered as a simple code block with no tabs.

For example:

```markdown
    To configure the bus call
    snippet: ConfigureWith
```

The resulting markdown will be:

```markdown
    To configure the bus call
    ```
    var configure = Configure.With();
    ```
```

#### Code indentation

The code snippets will do smart trimming of snippet indentation.

For example, given this snippet:

```c#
••#region DataBus
••var configure = Configure.With()
••••.FileShareDataBus(databusPath);
••#endregion
```

The two leading spaces (••) will be trimmed and the result will be

```c#
var configure = Configure.With()
••.FileShareDataBus(databusPath)
```

The same behavior will apply to leading tabs.

##### Do not mix tabs and spaces

If tabs and spaces are mixed there is no way for the snippets to work out what to trim.

So given this snippet:

```c#
••#region DataBus
••var configure = Configure.With()
➙➙.FileShareDataBus(databusPath);
•#endregion
```

where &#10137; is a tab, the resulting markdown will be

```c#
var configure = Configure.With()
➙➙.FileShareDataBus(databusPath)
```

Note that none of the tabs have been trimmed.

#### Explicit variable typing versus 'var'

Use `var` everywhere.

#### Snippets are compiled

The code used by snippets and samples is compiled on the build server. The compilation is done against the versions of the packages referenced in the samples and snippets projects. When a snippet doesn't compile, the build will break so make sure snippets are compiling properly. Samples and snippets should not reference unreleased NuGet packages.