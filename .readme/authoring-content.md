## Conventions

### Lower case  and `-` delimited

All content files (`.md`, `.png`, `.jpg`, etc.) and directories must be lower case.

All links pointing to them must be lower case.

Use a dash (`-`) to delimit filenames (e.g. `specify-endpoint-name.md`).

### Menu

The menu is a YAML text document stored at [menu/menu.yaml](menu/menu.yaml).

Any sub-items that are prefixed with the title of the parent item will have that prefix removed

Example content:

```yaml
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

### URLs

The directory structure where a `.md` exists is used to derive the URL for that document.

So a file existing at `nservicebus\logging\nlog.md` will have a URL of `https://docs.particular.net/nservicebus/logging/nlog`.

#### Index Pages

One exception to the URL rule is when a page is named `index.md`. In this case the `index.md` is omitted in the URL and only the directory structure is used.

So a file existing at `nservicebus\logging\index.md` will have a URL of `https://docs.particular.net/nservicebus/logging/`.

##### Related Pages on Index Pages

Like any page an index page can include [related pages](#related). However index pages will, by default, have all sibling and child pages included in the list of related pages. This is effectively a recursive walk of the file system for the directory the given index.md exists in.

#### Linking

Links to other documentation pages should be relative and contain the `.md` extension.

The `.md` allows links to work inside the GitHub web UI. The `.md` will be trimmed when they are finally rendered.

Given the case of editing a page located at `\nservicebus\page1.md`:

* To link to the file `nservicebus\page2.md`, use `[Page 2 Text](Page2.md)`.
* To link to the file `\servicecontrol\page3.md`, use `[Page 3 Text](/servicecontrol/page3.md)`.

Don't link to `index.md` pages, instead link to the directory. So link to `/nservicebus/logging` and NOT `/nservicebus/logging/index.md`

#### Links to 3rd parties

##### RavenDB

Avoid deep link into the RavenDB documentation since it is a maintenance pain.
For example, don't link to `https://ravendb.net/docs/article-page/3.0/Csharp/client-api//session/transaction-support/dtc-transactions#transaction-storage-recovery`.
When RavenDB 4 was released, `article-page/3.0/Csharp` became invalid and required an update.
Also, the RavenDB documentation does not maintain structure between versions. e.g. `https://ravendb.net/docs/article-page/2.0/Csharp/client-api//session/transaction-support/dtc-transactions#transaction-storage-recovery` is a 404. So we can't trust that "just change the version" will work.
Instead, link to the RavenDB docs search: `https://ravendb.net/docs/search/latest/csharp?searchTerm=THE-SEARCH-TERM`.
So for the above example it would be `https://ravendb.net/docs/search/latest/csharp?searchTerm=Transaction-storage-recovery`.

### Markdown

The site is rendered using [GitHub Flavored Markdown](https://help.github.com/articles/github-flavored-markdown)

#### [MarkdownPad](https://markdownpad.com/)

For editing markdown on the desktop (after cloning locally with Git) try [MarkdownPad](https://markdownpad.com/).

##### Markdown flavor

Ensure you enable `GitHub Flavored Markdown (Offline)` by going to

```text
Tools > Options > Markdown > Markdown Processor > GitHub Flavored Markdown (Offline)
```

Or click in the bottom left on the `M` icon to "hot-switch"

##### YAML

Don't render YAML front-matter by going to

```text
Tools > Options > Markdown > Markdown Settings
```

And checking `Ignore YAML Front-matter`

### Guidance content

Some of our documentation provides guidance for customers and prospects to make informed decisions when faced with multiple options.

For example, when a customer decides to host an endpoint in Azure, there are multiple options, each with their pros and cons. Vendor documentation on its own is often not enough in the context of creating and running a distributed system with NServiceBus.

Guidance for these decisions is valuable to our customers and is included in our public documentation.

This is not to be confused with comparisons between various vendors or technologies from various vendors. Such comparisons are contentious and are not part of our public documentation.

### Alerts

[GitHub style alert boxes](https://github.com/orgs/community/discussions/16925) are supported and rendered as [bootstrap alerts](https://getbootstrap.com/docs/5.3/components/alerts/).

### Headings

The first (and all top level) headers in a `.md` page should be a `h2` (i.e. `##`) with sub-headings under it being `h3`, `h4`, etc.

### Spaces

* Add an empty line before a heading and any other text
* Add an empty line after a heading
* Add an empty line between paragraphs

### Anchors

One addition to standard markdown is the auto creation of anchors for headings.

So if you have a heading like this:

```markdown
## My Heading
```

it will be converted to this:

```html
<h2>
  <a name="my-heading"/>
  My Heading
</h2>
```

Which means elsewhere in the page you can link to it with this:

```markdown
[Goto My Heading](#My-Heading)
```

### Some Useful Characters

* Ticks are done with `&#10004;` &#10004;
* Crosses are done with `&#10006;` &#10006;

### More Information

* [Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)

## Writing style

### Language Preferences

For consistency, prefer American English.

No personal voice. I.e. no "we", "you", "your", "our" etc.

### Version language

Avoid ambiguity.

* Range: **version X and above** and **version Y and below** and **version X to version Y**.
* Singular: **version X** and NOT **VX**.

Don't capitalize "version" unnecessarily.

* **NServiceBus version 6** and NOT **NServiceBus Version 6**
* **NServiceBus version 5 and below** and NOT **NServiceBus Version 5 and below**

Don't assume the latest version of a product is the only one being used

* Instead of "Prior to NServiceBus version 6.5, sagas could not...", say "In NServiceBus 6.4.x and below, sagas can not..."

### Embedding videos

A YouTube video can be embedded in Markdown using the `youtube` command at the beginning of a line:

Here's an example that embeds the Rick Roll video:

```md
Paragraph before…

youtube: https://www.youtube.com/watch?v=eBGIQ7ZuuiU

Paragraph after…
```

The engine will parse the video ID out of the YouTube URL and create a properly styled embed.

### Related content

- Consider what is the best place to direct the reader after they are done reading the current page. Add a link to that page at the bottom.

### Terminology

#### Bus

The word `Bus` should be avoided in documentation. Some replacements include:

* When referring to the topology, use `federated` (for which the opposite term is `centralized`)
* When referring to the NServiceBus instance, the general thing that sends or publishes messages, use `endpoint instance` or `endpoint` (when it is clear from the context that you are talking about an instance rather than a logical concept)
* When referring specifically to the `IBus` interface use `message session` or `message context` (depending if you are talking about just sending a messages from external component or from inside a handler)

The word `Bus` is allowed when a particular piece of documentation refers specifically to version 5 or below and discusses low level implementation details.

#### Core

The word `core` as a synonym for `NServiceBus` or `NServiceBus Core` should be avoided in the documentation. Prefer using `NServiceBus` or `NServiceBus package`.