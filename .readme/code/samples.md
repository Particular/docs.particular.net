### Samples

Our main goal is to provide the user with a smooth F5 experience when using the platform, and that includes samples, as it might be the user's first introduction to the platform.

#### When to write a sample

Any of the following, or combination thereof, could indicate that something should be a sample

* When there are multiple non-trivial moving pieces that would be mitigated by being able to download a runnable VS solution.
* When illustrating how Particular products/tools interact with 3rd-party products/tools.
* It is a sample of a significant feature of the Particular platform. e.g. Databus, encryption, pipeline etc.

Do not write a sample when:

* The only difference to an existing sample is a minor API usage.

#### Recommendations

* Samples should illustrate a feature or scenario with as few moving pieces as possible. For example, if the sample is "illustrating IOC with MVC" then "adding SignalR" to that sample will only cause confusion. In general, the fewer NuGet packages required to get the point across the better.
* Do not "document things inside a sample". A sample is to show how something is used, not to document it. Instead update the appropriate documentation page and link to it. As a general rule, if you add any content to a sample, where that guidance could possibly be applicable to other samples, then that guidance should probably exist in a documentation page.
* Start a sample with paragraph(s) that summarize why the sample would be useful or interesting. Think about more than what the sample _does_, but also what additional scenarios it can enable. After this summary, put the `downloadbutton` directive in a paragraph by itself, which will be rendered as a large **Download the sample now** button.

#### Conventions

* Samples are located [here](https://github.com/Particular/docs.particular.net/tree/master/samples).
* They are linked to from the home page and are rendered [here](https://docs.particular.net/samples/).
* Any directory in that structure with a sample.md will be considered a "root for a sample" or Sample Root.
* A Sample Root may not contain a sample.md in subdirectories.
* Each directory under the Sample Root will be rendered on the site as a downloadable zip with the directory name being the filename.
* A sample.md can use snippets from within its Sample Root but not snippets defined outside that root.
* A sample must obey rules that are verified by [Integrity Tests](#integrity-tests).
* Samples targeting .NET Core should be able to run across Windows, macOS, and Linux. To ensure that's the case, you can run the sample using WSL (Windows Subsystem for Linux) in VS Code. VS Code can be [configured to use WSL as the default development environment](https://code.visualstudio.com/docs/remote/wsl). If a sample cannot be designed to support one or more platforms add a note to the `sample.md`-file with the platforms that are unsupported and the reasoning.
* Samples contain no calls to `ConfigureAwait(bool)` unless they are explicitly required.
* Samples should define the `LangVersion` property to match the [default version](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version#defaults) of the lowest .NET TFM used in the solution.
* Only one sample should be defined for a major version, multiple samples for minor versions within a major should not be used. Users can update to a minor to use a new feature, so it's not worth the maintenance cost to maintain multiple projects.

#### References

Since users often use our samples to kick start their own projects, we want them to always use the latest versions of their dependencies. This is also important since we internally use our samples for smoke testing.

See the [NuGet package reference guidelines](#nuget-package-references) for more details on how to achieve this.

#### Startup projects

When a sample is zipped the [VS startup projects](https://msdn.microsoft.com/en-us/library/a1awth7y.aspx) are also configured. This is done by using [SetStartupProjects](https://github.com/SimonCropp/SetStartupProjects). By default startable projects are [detected though interrogating the project settings](https://github.com/SimonCropp/SetStartupProjects/blob/master/src/SetStartupProjects/StartProjectFinder.cs). To override this convention and hard-code the list of startup projects add a file named `{SolutionName}.StartupProjects.txt` in the same directory as the solution file. It should contain the relative paths to the project files you would like to use for startup projects.

For example if the solution "TheSolution.sln" contains two endpoints and you only want to start `Endpoint1` the content of `TheSolution.StartupProjects.txt` would be:

```text
Endpoint1\Endpoint1.csproj
```

To apply this convention on your local clone of the docs repo use the [set startup projects linkpad script](#setstartuplinq).

#### Bootstrapping a sample

At the moment the best way to get started on a sample is to copy an existing one. Ideally one that is similar to what you are trying to achieve.

A good sample to start with is the [Default Logging Sample](https://github.com/Particular/docs.particular.net/tree/master/samples/logging/default), since all it does is enable logging. You can then add the various moving pieces to the copy.

#### Screenshots

Avoid using screenshots in samples unless it adds significant value over what can be expressed in text. They have the following problems:

* More time consuming to update than text
* Not search-able
* Prone to an inconsistent feel as different people take screenshots at different sizes, different zoom levels and with different color schemes for the app in question
* Add significantly to the page load time.

The most common misuse of screenshots is when capturing console output. **DO NOT DO THIS**. Put the text inside a formatted code section instead.