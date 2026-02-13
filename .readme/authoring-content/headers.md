### Headers

Each document has a header. It is enclosed by `---` and is defined in a [YAML](https://en.wikipedia.org/wiki/YAML) document format.

The GitHub UI will [correctly render YAML](https://github.com/blog/1647-viewing-yaml-metadata-in-your-documents).

For example:

```yaml
---
title: Auditing Messages
summary: Provides built-in message auditing for every endpoint.
versions: '[4,)'
related:
- samples/custom-checks/monitoring3rdparty
redirects:
- nservicebus/overview
---
```

#### Title

```yaml
title: Auditing With NServiceBus
```

**Must be 70 characters or less**, and 50-60 characters is [recommended](https://moz.com/learn/seo/title-tag).

Required. Used for the web page title tag `<head><title>`, displayed in the page content, and displayed in search results.

**Note: When considering what is a good title keep in mind that the parent context of a given page is fairly well known by other means. ie people can see where it exists in the menu and can see where in the hierarchy it is through the breadcrumbs. So it is often not necessary to include the parent title in the current pages title. For example when documenting "Publishers name configuration", for "Azure Service Bus Transport", then "Publishers name configuration" would be a sufficient title where "Publishers name configuration in Azure Service Bus Transport" is overly verbose and partially redundant.**

#### Component

```yaml
component: Core
```

Required when using partials views, recommended also when using snippets in multiple versions. Allows the rendering engine determine what versions of the given page should be generated. Specified by providing the [component key](#component-key).

#### Versions

```yaml
versions: '[1,2)'
```

In case of components that consist of multiple packages it's also possible to explicitly specify ranges of versions for each package separately:

```yaml
versions: 'PackageA:[1,2); PackageB : [3,4); PackageC :*'
```

Optional. Used for specifying what versions the given page covers, especially relevant for features that are not available in all supported versions. The old format 'nuget_version_range' or 'package_name: nuget_version_range; package_name_2: nuget:version_range2'.

#### Reviewed

```yaml
reviewed: 2016-03-01
```

Optional. Used to capture the last date that a page was fully reviewed. Format is `yyyy-MM-dd`.

#### Summary

```yaml
summary: Provides built-in message auditing for every endpoint.
```

Optional. This section defines a short YAML string used as the page’s meta description tag (`<meta name="description" />`). It provides a concise, SEO‑friendly summary that may be displayed in search results as a snippet. The content should be under 160 characters (including spaces), accurately reflect the page’s topic, and act as a "pitch" that convinces users the page matches their needs.

#### Hidden

```yaml
hidden: true
```

Causes two things:

* Stops search engines from finding the page using a `<meta name="robots" content="noindex" />`.
* Prevents the page from being found in the docs search.

#### Preview Image

```yaml
previewImage: preview-image.png
```

Populates a feature image for the [Open Graph](http://ogp.me/) and [Twitter Card](https://developer.twitter.com/en/docs/tweets/optimize-with-cards/guides/getting-started.html) meta tags for social sharing.

The URL should be a relative URL, usually just the filename in the same directory as the article, but `../` to go up a directory is also supported. If it works in a Markdown image tag `![](relative-url.png)` then it should work for the metadata.

#### Related

```yaml
related:
- samples/custom-checks/monitoring3rdparty
```

A list of related pages for this page. These links will be rendered at the bottom of the page. Can include both samples and articles and they will be grouped as such when rendered in html.

#### Suppress Related

```yaml
suppressRelated: true
```

No related content will be displayed at the bottom of the article, including specifically included articles in the metadata, as well as any documents discovered by traversing the directory structure. This is intended for pages where tight control over the presentation of related material is desired.

#### Redirects

```yaml
redirects:
- nservicebus/overview
```

When renaming an existing article to a new name, add the `redirects:` section in the article header and specify the previous name for the article. If the old URL is linked anywhere, the new renamed article will automatically be served when the user clicks on it.

* Values specified in the `redirects` section must be lower case.
* Multiple values can be specified for the redirects, same as `tags`.
* Values are fully qualified

#### URL format for Redirects and Related

Should be the URL relative to the root with no beginning or trailing slash padding and no .md.

#### UpgradeGuides

To Mark something as an upgrade guide use `isUpgradeGuide: true`

The `upgradeGuideCoreVersions` setting can optionally be used to filter which NSB core version tab the page show up in the search results

```yaml
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
```

#### Learning Path

To mark a page as belonging to the [Particular Software Learning Path](https://particular.net/learn) use `isLearningPath: true`.

#### Calls to action

```yaml
callsToAction: ['architecture-review', 'solution-architect']
```

Calls to action are defined in `calls-to-action.yaml`.

### An example header for an article

In the following example, whenever the URLs `/servicecontrol/sc-sp` or `/servicecontrol/debugging-servicecontrol` are being requested, the given article will be rendered.

```yaml
---
title: ServicePulse Interaction
summary: 'Using ServicePulse Together'
redirects:
- servicecontrol/sc-sp
- servicecontrol/debugging-servicecontrol
related:
- servicecontrol/installation
---
```