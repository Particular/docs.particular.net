# How to Contribute

Before you start, ensure you have

 *  Created a [GitHub account](https://github.com/join)
 *  Signed the Particular [Contributor License Agreement](https://particular.net/contributors-license-agreement-consent).

There are two approaches to contributing.


## Via the GitHub Web UI

For simple changes, the GitHub web UI should suffice.

 1. Find the page you want to edit on https://docs.particular.net/.
 1. Click the `Edit Online` button. This will automatically fork the project so you can edit the file.
 1. Make the changes you require. Ensure you verify the changes in the `Preview` tab.
 1. Add a description of the changes.
 1. Click `Propose File Changes`.


## By Forking and Submitting a Pull Request

For more complex changes you should fork and then submit a pull request. This is useful if you are proposing multiple file changes

 1. [Fork](https://help.github.com/forking/) on GitHub.
 1. Clone the fork locally.
 1. Work on the feature.
 1. Push the code to GitHub.
 1. Send a Pull Request on GitHub.

For more information, see [Collaborating on GitHub](https://help.github.com/categories/63/articles) especially [using GitHub pull requests](https://help.github.com/articles/using-pull-requests).


## Reviewing a page

If, as part of editing a page, a full review of the content is done, the [reviewed header](#reviewed) should be updated. This date is used to render https://docs.particular.net/review.

As part of a full review the following should be done:

 * Spelling (US)
 * Grammar
 * Version specific language and content is correct
 * Language is concise
 * All links are relevant. No 3rd party links have redirects or 404s.
 * Are there any more links that can be added to improve the content
 * Content is correct up to and including the current released version
 * Tags are correct
 * Summary and title is adequate

**Note that for minor changes (e.g. individual spelling or grammar fixes) the reviewed header should NOT be updated.**


# Conventions


## Lower case  and `-` delimited

All content files (`.md`, `.png`, `.jpg` etc) and directories must be lower case.

All links pointing to them must be lower case.

Use a dash (`-`) to delimit filenames (e.g. `specify-endpoint-name.md`).


## Headers

Each document has a header. It is enclosed by `---` and is defined in a [YAML](https://en.wikipedia.org/wiki/YAML) document format.

The GitHub UI will [correctly render YAML](https://github.com/blog/1647-viewing-yaml-metadata-in-your-documents).

For example:

```
---
title: Auditing Messages
summary: Provides built-in message auditing for every endpoint.
versions: '[4,)'
tags:
- Auditing
- Forwarding Messages
related:
- samples/custom-checks/monitoring3rdparty
redirects:
- nservicebus/overview
---
```


### Title

```
title: Auditing With NServiceBus
```

**Must be 70 characters or less**, and 50-60 characters is recommended. https://moz.com/learn/seo/title-tag

Required. Used for the web page title tag `<head><title>`, displayed in the page content, and displayed in search results.

**Note: When considering what is a good title keep in mind that the parent context of a given page is fairly well known by other means. ie people can see where it exists in the menu and can see where in the hierarchy it is through the breadcrumbs. So it is often not necessary to include the parent title in the current pages title. For example when documenting "Publishers name configuration", for "Azure Service Bus Transport", then "Publishers name configuration" would be a sufficient title where "Publishers name configuration in Azure Service Bus Transport" is overly verbose and partially redundant.**


### Component

```
component: Core
```

Required when using partials views, recommended also when using snippets in multiple versions. Allows the rendering engine determine what versions of the given page should be generated. Specified by providing the [component key](#component-key).


### Versions

```
versions: '[4,)'
```

Optional. Used for specifying what versions the given page covers, especially relevant for features that are not available in all supported versions. Format is 'nuget_version_range'


### Reviewed

```
reviewed: 2016-03-01
```

Optional. Used to capture the last date that a page was fully reviewed. Format is `yyyy-MM-dd`.


### Summary

```
summary: Provides built-in message auditing for every endpoint.
```

Optional. Used for the meta description tag (`<meta name="description" />`) and displaying the search results.


### Tags

```
tags:
- Auditing
- Forwarding Messages
```

Optional. Used to flag the article as being part of a group of articles.

Tags are rendered in the articles content with the full list of tags being rendered at [https://docs.particular.net/tags](https://docs.particular.net/tags). Untagged articles will be rendered here [https://docs.particular.net/tags/untagged](https://docs.particular.net/tags/untagged)

Tags are interpreted in two ways.

* For inclusion in URLs:
   * Tag are lower case
   * Spaces are replaced with dashes (`-`)
* For display purposes:
   * Tags are lower case
   * Dashes (`-`) are replaced with spaces


### Hidden

```
hidden: true
```

Causes two things:

 * Stops search engines from finding the page using a `<meta name="robots" content="noindex" />`.
 * Prevents the page from being found in the docs search.


### Related

```
related:
- samples/custom-checks/monitoring3rdparty
```

A list of related pages for this page. These links will be rendered at the bottom of the page. Can include both samples and articles and they will be grouped as such when rendered in html.


### Suppress Related

```
suppressRelated: true
```

No related content will be displayed at the bottom of the article, including specifically included articles in the metadata, as well as any documents discovered by traversing the directory structure. This is intended for pages where tight control over the presentation of related material is desired.


### Redirects

```
redirects:
- nservicebus/overview
```

When renaming an existing article to a new name, add the `redirects:` section in the article header and specify the previous name for the article. If the old URL is linked anywhere, the new renamed article will automatically be served when the user clicks on it.

 * Values specified in the `redirects` section must be lower case.
 * Multiple values can be specified for the redirects, same as `tags`.
 * Values are fully qualified


### URL format for Redirects and Related

Should be the URL relative to the root with no beginning or trailing slash padding and no .md.


### UpgradeGuides

To Mark something as an upgrade guide use `isUpgradeGuide: true`

The `upgradeGuideCoreVersions` setting can optionally be used to filter which NSB core version tab the page show up in the search results

```
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
```


## An example header for an article

- In the following example, whenever the URLs `/servicecontrol/sc-si` or `/servicecontrol/debugging-servicecontrol` are being requested, the given article will be rendered.

```
---
title: ServiceInsight Interaction
summary: 'Using ServiceInsight Together'
tags:
- Invocation
- Debugging
redirects:
- servicecontrol/sc-si
- servicecontrol/debugging-servicecontrol
related:
- samples/azure/shared-host
---
```


## components.yaml

"Components" is a general term used to describe a deployable set of functionality. Components exist in [components/components.yaml](components/components.yaml). Note that over time a component may have moved between nugets or split into new nugets. For example, the ABS DataBus or the Callbacks.

Sample Component:

```
- Key: Callbacks
  Url: nservicebus/messaging/callbacks
  NugetOrder:
    - NServiceBus.Callbacks
    - NServiceBus
```


### Component Key

The component key allows for shorthand when referring to components in page headers.


### Component URL

The component URL is the definitive source of documentation for a given component. This will eventually be used to link back to said documentation from both NuGet usages, samples and articles.


### Component NugetOrder

Since components can be split over multiple different nugets, it is not possible to infer the order from the NuGet version alone. So we need to have a lookup index and the NugetOrder allows us to sensibly sort component versions. For example, NServiceBus.Callbacks.1.0.0 should sort higher than the version of Callbacks that exists in NServiceBus.5.0.0.


## nugetAlias.txt

All NServiceBus-related NuGet packages (used in documentation) are listed in [components/nugetAlias.txt](components/nugetAlias.txt). The alias part of the NuGet is the key that is used to infer the version and component for all snippets. For example, [Snippets/Callbacks](Snippets/Callbacks) has, over its lifetime, existed in both the Core NuGet and the Callbacks NuGet. So the directories under Callbacks are indicative of the NuGet (alias) they exist in and then split over the multiple versions of a given NuGet.

Example aliases:

```
ASP: NServiceBus.Persistence.AzureStorage
Autofac: NServiceBus.Autofac
Azure: NServiceBus.Azure
Callbacks: NServiceBus.Callbacks
```


## Menu

The menu is a YAML text document stored at [menu/menu.yaml](menu/menu.yaml).

Any sub-items that are prefixed with the title of the parent item will have that prefix removed

Example content:

```
- Name: NServiceBus
  Topics:
  - Url: platform
    Title: Getting Started
    Articles:
    - Url: platform
      Title: Particular Service Platform Overview
    - Title: NServiceBus Overview
      Articles:
      - Url: nservicebus/architecture/principles
        Title: Architectural Principles
      - Url: nservicebus/architecture
        Title: Bus versus broker architecture
```

Conventions:

 * Top level items the `Name` is used for the URL.
 * `Title` is required for all nodes other than top level.
 * Maximum of 4 levels deep.
 * URL is optional. if it does not exist it will render as an expandable node.


## URLs

The directory structure where a `.md` exists is used to derive the URL for that document.

So a file existing at `nservicebus\logging\nlog.md` will have a URL of `https://docs.particular.net/nservicebus/logging/nlog`.


### Index Pages

One exception to the URL rule is when a page is named `index.md`. In this case the `index.md` is omitted in the URL and only the directory structure is used.

So a file existing at `nservicebus\logging\index.md` will have a URL of `https://docs.particular.net/nservicebus/logging/`.


#### Related Pages on Index Pages

Like any page an index page can include [related pages](#related). However index pages will, by default, have all sibling and child pages included in the list of related pages. This is effectively a recursive walk of the file system for the directory the given index.md exists in.


### Linking

Links to other documentation pages should be relative and contain the `.md` extension.

The `.md` allows links to work inside the GitHub web UI. The `.md` will be trimmed when they are finally rendered.

Given the case of editing a page located at `\nservicebus\page1.md`:

- To link to the file `nservicebus\page2.md`, use `[Page 2 Text](Page2.md)`.
- To link to the file `\servicecontrol\page3.md`, use `[Page 3 Text](/servicecontrol/page3.md)`.

Don't link to `index.md` pages, instead link to the directory. So link to `/nservicebus/logging` and NOT `/nservicebus/logging/index.md`


## Markdown

The site is rendered using [GitHub Flavored Markdown](https://help.github.com/articles/github-flavored-markdown)


### [MarkdownPad](https://markdownpad.com/)

For editing markdown on the desktop (after cloning locally with Git) try [MarkdownPad](https://markdownpad.com/).


#### Markdown flavor

Ensure you enable `GitHub Flavored Markdown (Offline)` by going to

    Tools > Options > Markdown > Markdown Processor > GitHub Flavored Markdown (Offline)

Or click in the bottom left on the `M` icon to "hot-switch"


#### YAML

Don't render YAML front-matter by going to

    Tools > Options > Markdown > Markdown Settings

And checking `Ignore YAML Front-matter`


## EIP references 

[Enterprise Integration Patters (EIP)](http://www.enterpriseintegrationpatterns.com/) is a bible of messaging. We sometimes use the same or similar patterns, but name them differently. When describing such a pattern, it's useful to reference the related EIP pattern, to make it easier to understand.


### Terms we use and are aligned with EIP

 * Message
 * Message Endpoint
 * [Messaging Bridge](http://www.enterpriseintegrationpatterns.com/patterns/messaging/MessagingBridge.html) - Unfortunately we don't have any implementation other than MSMQ-SQL sample
 * [Command Message](http://www.enterpriseintegrationpatterns.com/patterns/messaging/CommandMessage.html)
 * [Event Message](http://www.enterpriseintegrationpatterns.com/patterns/messaging/EventMessage.html)
 * [Request-Reply](http://www.enterpriseintegrationpatterns.com/patterns/messaging/RequestReply.html) - not sure if we are 100% aligned here. We use the term *Full Duplex* to describe a non-synchronous request reply and only use *Request-Reply* or *Callback* for the synchronous variant
 * [Correlation ID](http://www.enterpriseintegrationpatterns.com/patterns/messaging/CorrelationIdentifier.html)
 * [Transactional Client](http://www.enterpriseintegrationpatterns.com/patterns/messaging/TransactionalClient.html)
 * [Competing Consumers](http://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html)
 * [Message Store](http://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageStore.html) - we have it in ServiceControl


### EIP terms and ideas we don't use but can

 * [Message Bus](http://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageBus.html) - we dropped using the *bus* word when referring to an *endpoint* (which is correct) but I think we can take advantage of this definition of the bus because it is aligned with our concepts.
 * [Dead Letter Channel](http://www.enterpriseintegrationpatterns.com/patterns/messaging/DeadLetterChannel.html) - only MSMQ implements it, we don't have it on NServiceBus level
 * [Datatype Channel](http://www.enterpriseintegrationpatterns.com/patterns/messaging/DatatypeChannel.html) - is a channel reserved for a single data/message type. This is something we should be selling users as a good practice
 * [Channel Adapter](http://www.enterpriseintegrationpatterns.com/patterns/messaging/ChannelAdapter.html) - this seems to be similar to the ADSD idea for integration components that pull data from various services in order to combine them into a message
 * [Messaging Gateway](http://www.enterpriseintegrationpatterns.com/patterns/messaging/MessagingGateway.html) and [Messaging Mapper](http://www.enterpriseintegrationpatterns.com/patterns/messaging/MessagingMapper.html) - we could prepare guidance based on them on how to use NSB in the application
 * [Control Bus](http://www.enterpriseintegrationpatterns.com/patterns/messaging/ControlBus.html) - I believe we should get this implemented in (close) future
 * [Test Message](http://www.enterpriseintegrationpatterns.com/patterns/messaging/TestMessage.html)


### Terms we use but have a different meaning or name

 * Message Channel - we call it a queue but EIP name seeps to be more appropriate since e.g. SQL transport does not use queues
 * [Point-to-Point](http://www.enterpriseintegrationpatterns.com/patterns/messaging/PointToPointChannel.html) - we use *Unicast* instead
 * [Publish-Subscribe](http://www.enterpriseintegrationpatterns.com/patterns/messaging/PublishSubscribeChannel.html) - we use *Multicast* instead. I think these two discrepancies are something we need to live with because we reserve *Publish/Subscribe* name to a logical pattern.
 * [Invalid Message Channel](http://www.enterpriseintegrationpatterns.com/patterns/messaging/InvalidMessageChannel.html) - we call it *error queue*
 * [Guaranteed Delivery](http://www.enterpriseintegrationpatterns.com/patterns/messaging/GuaranteedMessaging.html) - we call it *store and forward*
 * [Return Address](http://www.enterpriseintegrationpatterns.com/patterns/messaging/ReturnAddress.html) - we use *reply address* but I think we are close enough.
 * [Process Manager](http://www.enterpriseintegrationpatterns.com/patterns/messaging/ProcessManager.html) - we call it *Saga*
 * [Message Broker](http://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageBroker.html) - we use the term broker to describe a centralized transport mechanism where all message channels are on remote machine/cluster. EIP sees broker as a thing that also does routing based on message types (and/or content)
 * [Claim Check](http://www.enterpriseintegrationpatterns.com/patterns/messaging/StoreInLibrary.html) - we call it *Data bus*
 * [Event-Driven Consumer](http://www.enterpriseintegrationpatterns.com/patterns/messaging/EventDrivenConsumer.html) - we call it *message pump* or *IPushMessages*
 * [Message Dispatcher](http://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageDispatcher.html) - we call it *Distributor*
 * [Selective Consumer](http://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageSelector.html) - I believe our closest equivalent is a message handler
 * [Wire Tap](http://www.enterpriseintegrationpatterns.com/patterns/messaging/ControlBus.html) - we call it audit queue which confuses the purpose with the implementation. The thing is called *wire tap* and it is usually used to *audit* message flows.


## Samples


### When to write a sample

Any of the following, or combination thereof, could indicate that something should be a sample

 * When there are multiple non-trivial moving pieces that would be mitigated by being able to download a runnable VS solution.
 * When illustrating how Particular products/tools interact with 3rd-party products/tools.
 * It is a sample of a significant feature of the Particular platform. e.g. Databus, encryption, pipeline etc.

Do not write a sample when:

 * The only difference to an existing sample is a minor API usage.


### Recommendations

 * Samples should illustrate a feature or scenario with as few moving pieces as possible. For example, if the sample is "illustrating IOC with MVC" then "adding SignalR" to that sample will only cause confusion. In general, the fewer NuGet packages required to get the point across the better.
 * Do not "document things inside a sample". A sample is to show how something is used, not to document it. Instead update the appropriate documentation page and link to it. As a general rule, if you add any content to a sample, where that guidance could possibly be applicable to other samples, then that guidance should probably exist in a documentation page.


### Conventions

 * Samples are located here: https://github.com/Particular/docs.particular.net/tree/master/samples.
 * They are linked to from the home page and are rendered here: https://docs.particular.net/samples/.
 * Any directory in that structure with a sample.md will be considered a "root for a sample" or Sample Root.
 * A Sample Root may not contain a sample.md in subdirectories.
 * Each directory under the Sample Root will be rendered on the site as a downloadable zip with the directory name being the filename.
 * A sample.md can use snippets from within its Sample Root but not snippets defined outside that root.


### Startup projects

When a sample is zipped the [VS startup projects](https://msdn.microsoft.com/en-us/library/a1awth7y.aspx) are also configured. This is done by using https://github.com/ParticularLabs/SetStartupProjects. By default startable projects are [detected though interrogating the project settings](https://github.com/ParticularLabs/SetStartupProjects/blob/master/src/SetStartupProjects/StartProjectFinder.cs). To override this convention and hard code the list of startup projects add a file named `DefaultStartupProjects.txt` in the same directory as the solution file. It should contain the relative paths to the project files you would like to use for startup projects. 

For example if the solution contains two endpoints and you only want to start `Endpoint1` the content of `DefaultStartupProjects.txt` would be:

```
Endpoint1\Endpoint1.csproj
```

To apply this convention on your local clone of the docs repo use the [set startup projects linkpad script](#setstartuplinq).


### Bootstrapping a sample

At the moment the best way to get started on a sample is to copy an existing one. Ideally one that is similar to what you are trying to achieve.

A good sample to start with is the [Default Logging Sample](https://github.com/Particular/docs.particular.net/tree/master/samples/logging/default), since all it does is enable logging. You can then add the various moving pieces to the copy.


### Screenshots

Avoid using screenshots in samples unless it adds significant value over what can be expressed in text. They have the following problems:

 * More time consuming to update than text
 * Not search-able
 * Prone to an inconsistent feel as different people take screenshots at different sizes, different zoom levels and with different color schemes for the app in question
 * Add significantly to the page load time.

The most common misuse of screenshots is when capturing console output. **DO NOT DO THIS**. Put the text inside a formatted code section instead.


## Tutorials

Tutorials are similar to samples but optimized for new users to follow in a step-by-step fashion. Tutorials differ from samples in the following ways:

* Markdown file must be named `tutorial.md`
* No component specified in the header
* Focus on only the most recent version
* Rendered without button toolbar and component information at the top
* By default, solution download button is rendered at the end
  * An inline download button can be created instead using a `downloadbutton` directive on its own line within the tutorial markdown.
* Utilizes two collections of snippets, from the solution and also from an optional Snippets solution, allowing more granular or multi-phase snippets
* Allows use of personal voice (you/your/we/etc.) within `/tutorials` directory to foster collaborative tone with user


### Directory structure

An example directory structure for a tutorial might look like this:

```
{tutorial-name}/
  Snippets/ (optional)
    Snippets.sln
    Core_6
  Solution/
    {SolutionName}.sln
  tutorial.md
```

Tutorials can be grouped together in a parent directory with a normal article serving as a table of contents.


### Multi-lesson tutorials

For tutorials chained together to form multiple lessons, navigation can be created to combine a button linking to the next lesson with the Download Solution link.

```
- !!tutorial
  nextText: "Next Lesson: Sending a command"
  nextUrl: tutorials/intro-to-nservicebus/2-sending-a-command
```

The `nextText` parameter is optional, and will default to the title of the linked page if omitted.


## Markdown partials

Partials are version specific files that contain markdown.

They are only rendered in the target page when the version filter matches the convention for a give file.

Partial Convention: `filePrefix_key_nugetAlias_version.partial.md`

Make sure to use component alias (as defined in components.yaml file) in the partial name. For most components component alias will be identical to NuGet alias, however it's not always the case, e.g. the Callbacks feature has been moved out of core package to the dedicated NServiceBus.Callbacks package, so the there are two NuGet aliases that are related to this feature, but it's still the same component and has a single component alias. 

The NuGet alias in samples should match the prefix as defined by the samples solution directories.

Partials are rendered in the target page by using the following syntax

```
partial: PARTIAL_KEY
```

So an example directory structure might be as follows

<img src="partials.png" style='border:1px solid #000000' />

And to include the `endpointname` partial can be pulled into `sample.md` by including.

```
partial: endpointname
```


## Markdown includes

Markdown includes are pulled into the document prior to passing the content through the markdown conversion.


### Defining an include

Add a file anywhere in the docs repository that is suffixed with `.include.md`. For example, the file might be named `theKey.include.md`.


### Using an include

Add the following to the markdown: `include: theKey`


## Code Snippets


### Defining Snippets

There is a some code located here: https://github.com/Particular/docs.particular.net/tree/master/Snippets. Any directory containing an `_excludesnippets` file will have its snippets ignored.

File extensions scanned for snippets include:

 * `.config`
 * `.cs`
 * `.ps`
 * `.cscfg`
 * `.csdef`
 * `.html`
 * `.sql`
 * `.txt`
 * `.xml`


### Snippets are highlighted using highlightjs

 * [Documentation](https://highlightjs.readthedocs.io/)
 * [Language List](https://highlightjs.readthedocs.io/en/latest/css-classes-reference.html#language-names-and-aliases)


### Inline Code

 * https://highlightjs.org/static/demo/
 * https://github.com/isagalaev/highlight.js/tree/master/src/languages

| language     | key            |
|--------------|----------------|
| c#           | `cs`           |
| xml          | `xml`          |
| command line | `dos`          |
| powershell   | `ps`           |
| json         | `json`         |
| sql          | `sql`          |


**Always use fenced code blocks with a language.** If no language is defined then highlightjs will guess the language and it regularly gets it wrong.


#### Using comments

Any code wrapped in a convention-based comment will be picked up. The comment needs to start with `startcode` which is followed by the key.

```
// startcode ConfigureWith
var configure = Configure.With();
// endcode
```

For non-code snippets apply a similar approach as in code, using comments appropriate for a given file type. For plain-text files an extra empty line is required before `endcode` tag.

|Tag        |XML-based                    |PowerShell            |SQL script             |Plain text          |
|-----------|-----------------------------|----------------------|-----------------------|--------------------|
|**Open**   |`<!-- startcode name -->`|`# startcode name`|`-- startcode name`|`startcode name`|
|Content    |                             |                      |                       |                    |
|**Close**  |`<!-- endcode -->`       |`# endcode`       |`-- endcode`       |`endcode`       |


#### Using regions

Any code wrapped in a named C# region will be picked up. The name of the region is used as the key.

```
#region ConfigureWith
var configure = Configure.With();
#endregion
```


### Snippet versioning

Snippets are versioned. These versions are used to render snippets in a tabbed manner.

<img src="tabbed_snippets.png" style='border:1px solid #000000' />

Versions follow the [NuGet versioning convention](https://docs.nuget.org/create/versioning#specifying-version-ranges-in-.nuspec-files). If either `Minor` or `Patch` is not defined they will be rendered as an `x`. For example, Version `3.3` would be rendered as `3.3.x` and Version `3` would be rendered as `3.x`.

Snippet versions are derived in two ways


#### Version suffix on snippets

Appending a version to the end of a snippet definition as follows:

```
#region ConfigureWith 4.5
var configure = Configure.With();
#endregion
```

Or version range:

```
#region MySnippetName [1.0,2.0]
My Snippet Code
#endregion
```


#### Convention based on the directory

If a snippet has no version defined then the version will be derived by walking up the directory tree until if finds a directory that is suffixed with `_Version` or `_VersionRange`. For example:

 * Snippets extracted from `docs.particular.net\Snippets\Snippets_4\TheClass.cs` would have a default version of `(≥ 4.0.0 && < 5.0.0)`.
 * Snippets extracted from `docs.particular.net\Snippets\Snippets_4\Special_4.3\TheClass.cs` would have a default version of `(≥ 4.3.0 && < 5.0.0)`.
 * Snippets extracted from `docs.particular.net\Snippets\Special_(1.0,2.0)\TheClass.cs` would have a default version of `(> 1.0.0 && < 2.0.0)`.


#### Pre-release marker file

If a file named `prerelease.txt` exists in a versioned directory then a `-pre` will be added to the version.

For example, if there is a directory `docs.particular.net\Snippets\{Component}\Snippets_6\` and it contains a `prerelease.txt` file then the version will be `(≥ 6.0.0-pre)`


### Using Snippets

The keyed snippets can then be used in any documentation `.md` file by adding the text

**snippet: KEY**

Then snippets with the key (all versions) will be rendered in a tabbed manner. If there is only a single version then it will be rendered as a simple code block with no tabs.

For example:

<pre>
<code >To configure the bus call
snippet: ConfigureWith</code>
</pre>

The resulting markdown will be:

    To configure the bus call
    ```
    var configure = Configure.With();
    ```


### Code indentation

The code snippets will do smart trimming of snippet indentation.

For example, given this snippet:

<pre>
&#8226;&#8226;#region DataBus
&#8226;&#8226;var configure = Configure.With()
&#8226;&#8226;&#8226;&#8226;.FileShareDataBus(databusPath);
&#8226;&#8226;#endregion
</pre>

The two leading spaces (&#8226;&#8226;) will be trimmed and the result will be

```
var configure = Configure.With()
••.FileShareDataBus(databusPath)
```

The same behavior will apply to leading tabs.


#### Do not mix tabs and spaces

If tabs and spaces are mixed there is no way for the snippets to work out what to trim.

So given this snippet:

<pre>
&#8226;&#8226;#region DataBus
&#8226;&#8226;var configure = Configure.With()
&#10137;&#10137;.FileShareDataBus(databusPath);
&#8226;&#8226;#endregion
</pre>

where &#10137; is a tab, the resulting markdown will be

<pre>
var configure = Configure.With()
&#10137;&#10137;.FileShareDataBus(databusPath)
</pre>

Note that none of the tabs have been trimmed.


### Explicit variable typing versus 'var'

Use `var` everywhere.


### Snippets are compiled

The code used by snippets and samples is compiled on the build server. The compilation is done against the versions of the packages referenced in the samples and snippets projects. When a snippet doesn't compile, the build will break so make sure snippets are compiling properly. Samples and snippets should not reference unreleased NuGet packages.


## Unreleased NuGet packages

There are some scenarios where documentation may require unreleased or beta NuGet packages. For example, when creating a PR against documentation for a feature that is not yet released. In this case, it is ok for a PR to reference an unreleased NuGet and have that PR fail to build on the build server. Once the NuGet packages have been released that PR can be merged.

In some cases it may be necessary to have merged documentation for unreleased features. In this case the NuGet packages should be pushed to the [Particular feed on MyGet](https://www.myget.org/feed/Packages/particular). The feed is included by default in the [Snippets nuget.config](https://github.com/Particular/docs.particular.net/blob/master/Snippets/nuget.config#L14).


## Alerts

Sometimes it is necessary to draw attention to items you want to call out in a document.

This is achieved through bootstrap alerts https://getbootstrap.com/components/#alerts

There are several keys each of which map to a different colored alert

| Key              | Color  |
|------------------|--------|
| `SUCCESS`        | green  |
| `NOTE` or `INFO` | blue   |
| `WARNING`        | yellow |
| `DANGER`         | red    |

Keys can be used in two manners


### Single-line

This can be done with the following syntax

    KEY: the note text.

For example, this

    NOTE: Some sample note text.

will be rendered as

    <p class="alert alert-info">
       Some sample note text.
    </p>


### Multi-line

Sometimes it is necessary to group markdown elements inside a note. This can be done with the following syntax

    {{KEY:
    Inner markdown elements
    }}

For example, this

    {{NOTE:
    * Point one
    * Point Two
    }}

will be rendered as

    <p class="alert alert-info">
    * Point One
    * Point Two
    </p>


## Headings

The first (and all top level) headers in a `.md` page should be a `h2` (i.e. `##`) with sub-headings under it being `h3`, `h4`, etc.


## Spaces

* Two empty lines before a heading and any other text
* Add an empty line after a heading
* Add an empty line between paragraphs


## Anchors

One addition to standard markdown is the auto creation of anchors for headings.

So if you have a heading like this:

    ## My Heading

it will be converted to this:

    <h2>
      <a name="my-heading"/>
      My Heading
    </h2>

Which means elsewhere in the page you can link to it with this:

    [Goto My Heading](#My-Heading)


## Images

Images can be added using the following markdown syntax

    ![Alt text](/path/to/img.jpg "Optional title")

With the minimal syntax being

    ![](/path/to/img.jpg)


### Image sizing

Image size can be controlled by adding the text `width=x` to the end of the title

For example

    ![Alt text](/path/to/img.jpg "Optional title width=x")

With the minimal syntax being

    ![](/path/to/img.jpg "width=x")

This will result in the image being re-sized with the following parameters

    width="x" height="auto"

It will also wrap the image in a clickable lightbox so the full image can be accessed.


### Maintaining images

When creating images, strive to keep sources in order to update and re-create images later. Whenever possible use mermaid. When using LucidChart make sure to keep the sources.


### mermaid

The support for [mermaid](https://knsv.github.io/mermaid/) is provided as an extension to [Markdig](https://github.com/lunet-io/markdig). Markdig converts the diagram definition from .md to HTML, and then mermaid JavaScript library converts the definition to SVG format on the fly.

Diagram images are generated using the  using a pseudocode syntax like this:
<pre><code>
```mermaid
_mermaid_diagram_definition_
```
</code></pre>

For example:
<pre><code>
```mermaid
graph TB
A[ExchangeA] --> B[ExchangeB]
A --> D[ExchangeD]
B --> C[ExchangeC]
B --> Q1[Queue1]
D --> Q2[Queue2]
```
</code></pre>

The diagrams can be created and verified using the [online editor](http://knsv.github.io/mermaid/live_editor/).


#### Messaging Graph Style

Diagrams that represent messages and events being passed between endpoint should follow some basic style rules.

Endpoints should be represented as nodes with rounded corners. Messages should be represented as nodes. To show an endpoint sending a message to another endpoint use two edges. The first edge goes from the sender to the message being sent. The second edge goes from the message to the receiver. Like this:
<pre><code>
```mermaid
graph LR
a(EndpointA)
b(EndpointB)
a-->SomeCommand
SomeCommand-->b
```
</code></pre>

Showing an endpoint publishing an event is similar but should use a dotted edge. Events can be delivered to multiple recipients. Use a separate edge for each one. Like this:
<pre><code>
```mermaid
graph LR
a(EndpointA)
b(EndpointB)
c(EndpointC)
a-.->AnEvent
AnEvent-->b
AnEvent-->c
```
</code></pre>

There are two css classes (`event` and `message`) that should be applied to message nodes in these diagrams. To apply these, use the `class` keyword in mermaid:

<pre><code>
```mermaid
graph LR

Endpoint1-->SomeCommand
Command-->Endpoint2
Endpoint2-.->AnEvent
AnEvent-.->Endpoint3
Endpoint3 -.->AnotherEvent
AnotherEvent -.->Endpoint1
AnotherEvent -.->Endpoint4

class SomeCommand message;
class AnEvent,AnotherEvent event;
```
</code></pre>


### LucidChart

Another option is using [LucidChart](https://www.lucidchart.com). LucidChart allows to export and import Visio (VDX) formatted documents. Visio formatted documents can be used to generate images and should be committed along with the images. To generate images from LucidChart (or a Visio document), export the image as PNG, using the "Crop to content" option.


## Some Useful Characters

 * Ticks are done with `&#10004;` &#10004;
 * Crosses are done with `&#10006;` &#10006;


## More Information

 * [Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)


# Writing Style


## Language Preferences

For consistency, prefer American English.

No personal voice. I.e. no "we", "you", "your", "our" etc.


## Version Language

Avoid ambiguity.

 * Range: **Versions X and above** and **Versions Y and below** and **Version X to Version Y**.
 * Singular: **Version X** and NOT **VX** or **version X**.


## Embedding videos

There is a CSS class that will properly style videos: `video-container`

Use it as follows:

```
<div class="video-container">
<iframe src="https://www.youtube.com/embed/QolL1Oum72Q" frameborder="0" allowfullscreen></iframe>
</div>
```



## Terminology


### Bus

The word `Bus` should be avoided in documentation. Some replacements include:
 * When referring to the topology, use `federated` (for which the opposite term is `centralized`)
 * When referring to the NServiceBus instance, the general thing that sends or publishes messages, use `endpoint instance` or `endpoint` (when it is clear from the context that you are talking about an instance rather than a logical concept)
 * When referring specifically to the `IBus` interface use `message session` or `message context` (depending if you are talking about just sending a messages from external component or from inside a handler)

The word `Bus` is allowed when a particular piece of documentation refers specifically to version 5 or below and discusses low level implementation details.


# Links to 3rd parties


## RavenDB

Avoid deep link into the RavenDB documentation since it is a maintenance pain. For example don't link to `https://ravendb.net/docs/article-page/3.0/Csharp/client-api//session/transaction-support/dtc-transactions#transaction-storage-recovery` since when RavenDB 4 is release `article-page/3.0/Csharp` is invalid and requires an update. Also the RavenDB documentation does not maintain structure between versions. e.g. `https://ravendb.net/docs/article-page/2.0/Csharp/client-api//session/transaction-support/dtc-transactions#transaction-storage-recovery` is a 404. So we can't trust that "just change the version" will work. Instead link to the RavenDB docs search: `https://ravendb.net/docs/search/latest/csharp?searchTerm=THE-SEARCH-TERM`. So for the above example it would be `https://ravendb.net/docs/search/latest/csharp?searchTerm=Transaction-storage-recovery`.


# Utilities

Under https://github.com/Particular/docs.particular.net/tree/master/tools there are several utilities to help with the management of this repository. All are in the form of [LINQPad](https://www.linqpad.net/) scripts.


## nugets.linq

Uses nuget.exe to update all NuGet packages in all solutions to the newest patch version. This script takes 10-20 minutes depending on bandwidth. The script will **not** update to the newest minor or major versions.


## projectStandards.linq

Remove redundant content from sln and csproj files.

Enforces the [Resharper](https://www.jetbrains.com/resharper/) settings to be correct for every solution. The standard is a placeholder .settings file that pull in the [Shared.DotSettings](https://github.com/Particular/docs.particular.net/blob/master/tools/Shared.DotSettings) file as a layer.


## setStartup.linq

Sets the correct startup projects for every solution. This is persisted in an `.suo` file for each solution. Since `.suo` files are not committed to source control, if a re-clone is done this script will need to be re-run.


# Git management/behavior

In general the quality of the git history is not important in this repository. The reason for this is that the standard usages of a clean history (blame, supporting old versions, support branches etc) do not apply to a documentation repository. As such there are several recommendations based on that:

 * If pushed to GitHub **do not** re-write history. Even locally it is probably not worth the effort.
 * **Do not** force push.
 * Optionally merge commits immediately prior to merging a PR.

So if following the [Git pretty flow chart](http://justinhileman.info/article/git-pretty/) you should usually end in the "It's safest to let it stay ugly" end point.


# Additional Resources

 * [GitHub Flow in the Browser](https://help.github.com/articles/github-flow-in-the-browser/)
 * [General GitHub documentation](https://help.github.com/)
 * [GitHub pull request documentation](https://help.github.com/send-pull-requests/)
 * [Forking a Repo](https://help.github.com/articles/fork-a-repo)
 * [Using Pull Requests](https://help.github.com/articles/using-pull-requests)
 * [Markdown Table generator](http://www.tablesgenerator.com/markdown_tables)
