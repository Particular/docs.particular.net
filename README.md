# How to Contribute

Before you start ensure you have

 *  Created a [GitHub account](https://github.com/join)
 *  Signed the Particular [Contributor License Agreement](http://www.particular.net/contributors-license-agreement-consent).

There are two approaches to contributing.

## Via the GitHub Web UI  

For simple changes the GitHub web UI should suffice.

 1. Find the page you want to edit on http://docs.particular.net/.
 1. Click the `Improve this doc`. This will automatically fork the project so you can edit the file.
 1. Make the changes you require. Ensure you verify your changes in the `Preview` tab.
 1. Add a description of your changes.
 1. Click `Propose File Changes`.

## By Forking and Submitting a Pull Request

For more complex changes you should fork and then submit a pull request. This is useful if you are proposing multiple file changes

 1. [Fork](https://help.github.com/forking/) on GitHub.
 1. Clone your fork locally.
 1. Work on your feature.
 1. Push the up to GitHub.
 1. Send a Pull Request on GitHub.

For more information see [Collaborating on GitHub](https://help.github.com/categories/63/articles) especially [using GitHub pull requests](https://help.github.com/articles/using-pull-requests). 

# Conventions

## Lower case  and `-` delimited

All content files (`.md`,`.png`,`.jpg` etc) and directories must be lower case. 

All links pointing to them must be lower case.

To delimiter file names use a dash (`-`).

## Headers

Each document has a header. It is enclosed by `---` and is defined in a [YAML](http://en.wikipedia.org/wiki/YAML) document.

The GitHub  UI will [correctly render YAML](https://github.com/blog/1647-viewing-yaml-metadata-in-your-documents).

For example:

```
---
title: Auditing With NServiceBus
summary: 'Provides built-in message auditing for every endpoint.'
tags:
- Auditing
- Forwarding Messages
---
```

### Title

Required. Used for the web page title tag `<head><title>`, displayed in the page content, and displayed in search results.

### Summary

Required. Used for the meta description tag (`<meta name="description"`) and displaying on the search results.

### Tags

Optional. Used to flag the article as being part of a group of articles.

Tags are rendered in the articles content with the full list of tags being rendered at [http://docs.particular.net/tags](http://docs.particular.net/tags). Untagged articles will be rendered here [http://docs.particular.net/tags/untagged](http://docs.particular.net/tags/untagged)

Tags are interpreted in two ways. 

* For inclusion of URLs:
   * Tag are lower case
   * Spaces are replaced with dashes (`-`) 
* For display purposes: 
   * Tags are lower case
   * Dashes (`-`) are replaced with dashes spaces 

### Redirects

When renaming an existing article to a new name, please add the `redirects:` section in the article header and specify the previous name for the article. If the old Url is linked anywhere, when the user clicks on it, the new renamed article will automatically be served.
 
* Values specified in the `redirects` section must be lower cased.
* Multiple values can be specified for the redirects, same as `tags`.
* Values are fully qualified

## An example header for an article

- In the following example, whenever the urls `/servicematrix/sm-si` or `/servicematrix/debugging-servicematrix` are being requested, the given article will be rendered.

```
---
title: ServiceMatrix and ServiceInsight Interaction
summary: 'Using ServiceMatrix and ServiceInsight Together'
tags:
- ServiceMatrix
- ServiceInsight
- Invocation
- Debugging

redirects:
- servicematrix/sm-si
- servicematrix/debugging-servicematrix

---

```

## Menu

The menu is a YAML text document stored at [menu/menu.yaml](menu/menu.yaml).

## URLs

The directory structure where a `.md` exists is used to derive the URL for that document. 

So a file existing at `nservicebus\logging\nlog.md` will have a resultant URL of `http://docs.particular.net/nservicebus/logging/nlog`.

### Index Pages

One exception to this rule is when a page is named `index.md`. In this case the `index.md` is omitted in the resultant URL and only the directory structure is used.

So a file existing at `nservicebus\logging\index.md` will have a resultant URL of `http://docs.particular.net/nservicebus/logging/`.

### Linking

Links to other documentation pages should be relative and contain the `.md` extension. 

The `.md` allows links to work inside the GitHub web UI. The `.md` will be trimmed when they are finally rendered.

Given the case of editing a page located at `\nservicebus\page1.md`:

To link to the file `nservicebus\page2.md`, use `[Page 2 Text](Page2.md)`.

To link to the file `\servicecontrol\page3.md`, use `[Page 3 Text](/servicecontrol/page3.md)`.

Don't link to `index.md` pages, instead link to the directory. So link to `/nservicebus/logging` and NOT `/nservicebus/logging/index.md`

## Markdown 

The site is rendered using [GitHub Flavored Markdown](https://help.github.com/articles/github-flavored-markdown)

### [MarkdownPad](http://markdownpad.com/)

For editing markdown on your desktop (after cloning locally with Git) try [MarkdownPad](http://markdownpad.com/).

#### Markdown flavor

Ensure you enable `GitHub Flavored Markdown (Offline)` by going to 

    Tools > Options > Markdown > Markdown Processor > GitHub Flavored Markdown (Offline)

Or click in the bottom left no the `M` icon to "hot-switch"

#### Yaml

Don't render YAML Front-Matter by going to  

    Tools > Options > Markdown > Markdown Settings 

And checking `Ignore YAML Front-matter`

## Samples

### Conventions

 * Samples are located here https://github.com/Particular/docs.particular.net/tree/master/samples.
 * They are linked to from the home page and are rendered here http://docs.particular.net/samples/.
 * Any directory in that structure with a sample.md will be considered a "root for a sample" or Sample Root.
 * A Sample Root may not contain an sample.md in subdirectories.
 * Each directory under the Sample Root will be rendered on the site as a downloadable zip with the directory name being the filename.
 * A sample.md can use snippets from within its Sample Root but not snippets defined outside that root.  

### Recommendations

 * Avoid using screen shots in samples as they cause extra effort when the sample needs to be updated.
 * Samples should illustrate a feature or scenario with as few moving pieces as possible. For example if the sample is "illustrating IOC with MVC" then "adding signalr" to that sample will only cause confusion. In general the fewer nugets you need to get the point across the better.
 * Do not "document things inside a sample". A sample is "to show how something is used" not to document it. Instead update the appropriate documentation page and link to it. As a general rule if you add any content to a sample, where that guidance could possible be applicable to other samples, then that guidance should probably exist in a documentation page.

### Bootstrapping a sample

At the moment the best way to get started on a sample is to copy an existing one. Ideally one that has similarities to what you are trying to achieve. 

A good sample to start with is the [Default Logging Sample](https://github.com/Particular/docs.particular.net/tree/master/samples/logging/default), since all it does is enable logging. You can then add the various moving pieces to your copy.

## Code Snippets

### Defining Snippets

There is a some code located here https://github.com/Particular/docs.particular.net/tree/master/Snippets. All `.cs`, `.xml`, `.sql` and `.config` files in that directory are parsed for code snippets

Snippets are compiled against the versions of the packages referenced in the snippets project. When a snippet doesn't compile the build will break. Make sure the snippets you use are compiling properly. There is one exception to this rule: If you are writing snippets for features which are not yet released.

#### Using comments

Any code wrapped in a convention based comment will be picked up. The comment needs to start with `startcode` which is followed by the key.

```
// startcode ConfigureWith
var configure = Configure.With();
// endcode
```

#### Using regions 

Any code wrapped in a named C# region will pe picked up. The name of the region is used as the key. 

```
#region ConfigureWith
var configure = Configure.With();
#endregion
```

### Snippet versioning

Snippets are versioned, these versions are used to render snippets in a tabbed manner.

<img src="tabbed_snippets.png" style='border:1px solid #000000' />

Version is of the form `Major.Minor.Patch`. If either `Minor` or `Patch` is not defined they will be rendered as an `x`. So for example Version `3.3` would be rendered as `3.3.x` and Version `3` would be rendered as `3.x`.

Snippet versions are derived in two ways

#### Version suffix on snippets

Appending a version to the end of a snippet definition as follows.

```
#region ConfigureWith 4.5
var configure = Configure.With();
#endregion
```

#### Convention based on the directory

If a snippet has no version defined then the version will be derived by walking up the directory tree until if finds a directory of the form `Name_Major.Minor.Patch`. eg

 * Snippets extracted from `docs.particular.net\Snippets\Snippets_4\TheClass.cs` would have a default version of `4`.
 * Snippets extracted from `docs.particular.net\Snippets\Snippets_4\Special_4.3\TheClass.cs` would have a default version of `4.3`.
 
### Using Snippets

The keyed snippets can then be used in any documentation `.md` file by adding the text

**&lt;!-- import KEY -->**.

Then snippets with the key (all versions) will be rendered in a tabbed manner. If there is only a single version then it will be rendered as a simple code block with no tabs.

For example 

<pre>
<code >To configure the bus call
&lt;!-- import ConfigureWith --></code>
</pre>

The resulting markdown will be will be 

    To configure the bus call
    ```
    var configure = Configure.With();
    ``` 

### Code indentation

The code snippets will do smart trimming of snippet indentation. 

For example given this snippet. 

<pre>
&#8226;&#8226;#region DataBus
&#8226;&#8226;var configure = Configure.With()
&#8226;&#8226;&#8226;&#8226;.FileShareDataBus(databusPath);
&#8226;&#8226;#endregion
</pre>

The leading two spaces (&#8226;&#8226;) will be trimmed and the result will be 

```
var configure = Configure.With()
••.FileShareDataBus(databusPath)
```

The same behavior will apply to leading tabs.

#### Do not mix tabs and spaces

If tabs and spaces are mixed there is no way for the snippets to work out what to trim.

So given this snippet 

<pre>
&#8226;&#8226;#region DataBus
&#8226;&#8226;var configure = Configure.With()
&#10137;&#10137;.FileShareDataBus(databusPath);
&#8226;&#8226;#endregion
</pre>

Where &#10137; is a tab.

The resulting markdown will be will be 

<pre>
var configure = Configure.With()
&#10137;&#10137;.FileShareDataBus(databusPath)
</pre>

Note none of the tabs have been trimmed.

### Why is explicit variable typing used instead of 'var'

This is done for two reasons

 1. Since the snippets are viewing inline to a page they lack much of the context of a full code file such as using statements. To remove the ambiguity explicit variable declaration is being used
 2. It makes it much easier to build the docs search engine when the types being used on a page can be inferred by the snippets used. 

This is enforced by Resharper rules.

### Snippets are compiled

The the code used by snippets and samples is compiled on the build server. The compilation is done against the versions of the packages referenced in the snippets project. When a snippet doesn't compile the build will break so snippets are compiling properly. Samples and snippets should not reference unreleased nugets.

#### Unreleased nugets

There are some scenarios where documentation may require unreleased or beta nugets. For example when creating a PR against documentation for a feature that is not yet released. In this case it is ok to that PR to reference an unreleased nuget and have that PR fail to build on the build server. Once the nugets have been released that PR can be merged.

## Alerts

Sometimes is necessary to draw attention to items you want to call out in a document.

This is achieved through bootstrap alerts http://getbootstrap.com/components/#alerts

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

For example this

    NOTE: Some sample note text.

Will be rendered as

    <p class="alert alert-info">
       Some sample note text.
    </p> 

### Multi-line

Sometimes it is necessary to group markdown elements inside a note. This can be done with the following syntax
 
    {{KEY:
    Inner markdown elements
    }}

For example this 

    {{NOTE:
    * Point one
    * Point Two
    }} 

Would be rendered as 

    <p class="alert alert-info">
    * Point One 
    * Point Two
    </p> 

## Headings

The first (and all top level) headers in a `.md` page should be a `h2` ie `##`. With sub-headers under it being `h2` are `h3` etc. 

## Space

* Add an empty after a heading
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

Image size can be controlled by adding a the text `width=x` to the end of the title

For example

    ![Alt text](/path/to/img.jpg "Optional title width=x")

With the minimal syntax being 

    ![](/path/to/img.jpg "width=x")

This will result in the image being resized with the following parameters

    width="x" height="auto"

It will also wrap the image in a clickable lightbox so the full image can be accessed. 

## Some Useful Characters

 * Ticks are done with `&#10004;` &#10004;
 * Crosses are done with `&#10006;` &#10006;
  
## Suggested Practices for Consistent Writing

* Spell out numbers smaller than ten (not 2,3, etc.).
* Do not use "please".
* Use these:
 * "click" (not "click on" or "press")
 * "open" (not "open up")
 * V3, V3.1	(not version 3, v.3, v3)
 * Present tense (not future tense)
 * you, NServiceBus	(not "we") 
 * to (not "in order to")
 * Next steps (not	"Where to go from here?" or "Where to now?")
 * cannot, you would, is not	 (not contractions such as can't, you'd, isn't)
 * double click, right click	(not double-click, right-click)
 * you (not developers or users)
 * backend (not	back end)
* Use these proper nouns:
 * Particular Software	not Particular 
 * Visual Studio	 not visual studio
 * NuGet	not Nuget
 * RavenDB not Raven DB
 * Fluent not fluent
 * PowerShell not powershell 
 * MVC 3 not Mvc3
 * ASP.NET not Asp.Net
 * Log4Net not log4net 
 * Intellisense	not intellisense
 * ServiceInsight not serviceInsight
 * Azure Service Bus not Azure servicebus
 * First Level Retries not First-Level-Retries

## More Info
 
 * [Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)

# Additional Resources

* [General GitHub documentation](https://help.github.com/)
* [GitHub pull request documentation](https://help.github.com/send-pull-requests/)
* [Forking a Repo](https://help.github.com/articles/fork-a-repo)
* [Using Pull Requests](https://help.github.com/articles/using-pull-requests)
