# How to contribute

Before you start ensure you have

 *  Created a [GitHub account](https://github.com/signup/free)
 *  Signed the Particular [Contributor License Agreement](http://www.particular.net/contributors-license-agreement-consent).

There are two approaches to contributing

## The GitHub web UI  

For simple changes the GitHub web UI should suffice.

 * Find the page you want to edit on http://docs.particular.net/
 * Click the `Improve this doc` this will take you to the GitHub copy of the markdown for this page.
 * Click `Edit`. This will automatically fork the project so you can edit the file.
 * Make the changes you require. Ensure you verify your changes in the `Preview` tab
 * Add a description of your changes
 * Click `Propose File Changes`

## By forking and submitting a pull request

For more complex changes you should fork and then submit a pull request. This is useful if you are proposing multiple file changes

 * [Fork](http://help.github.com/forking/) on GitHub
 * Clone your fork locally
 * Work on your feature
 * Push the up to GitHub
 * Send a Pull Request on GitHub

For more information see [Collaborating on GitHub](https://help.github.com/categories/63/articles) especially [using GitHub pull requests](https://help.github.com/articles/using-pull-requests) 

# Conventions

## Page Headers

Each document has a header. It is enclosed by `---` and is defined in a [YAML](http://en.wikipedia.org/wiki/YAML) document.

The GitHub  UI will [correctly render YAML](https://github.com/blog/1647-viewing-yaml-metadata-in-your-documents)

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

Required. Used for the web page title tag `<head><title>`, displaying in the page content as `h1` and displayed in search results.

### Summary

Required. Used for the meta description tag (`<meta name="description"`) and displaying on the search results

### Tags

Optional. Used to flag the article as being part of a group of articles.

Tags are rendered in the articles content with the full list of tags being rendered at [http://docs.particular.net/tags](http://docs.particular.net/tags). Untagged articles will be rendered here [http://docs.particular.net/Tags/untagged](http://docs.particular.net/Tags/untagged)

Tags are interpreted in two pays 

1. For inclusion of urls
   * Tag will be lower cased
   * Spaces will be replaced with dashes (`-`) 
2. For display purposes 
   * Tag will be lower cased
   * Dashes (`-`) will be replaced with dashes spaces 

### Redirects

Url redirects are not currently implemented but will be included as part of the header.

## Headings

The page title is displayed as `h1`, and all subsequent headings in a documentation page should start with `h2`.

## Spacing

Please make sure to have an empty line between paragraphs, and before headings.

## Menu

The menu is a json text document stored at [Content/menu.txt](Content/menu.txt)

## Urls

The directory structure where a `.md` exists is used to derive the URL for that document. 

So a file existing at `Content\NServiceBus\Logging\NLog.md` will have a resultant url of `http://docs.particular.net/NServiceBus/Logging/Nlog`

### Index Pages

One exception to this rule is when a page is named `Index.md`. In this case the `Index.md` is omitted on the resultant url and only the directory structure used.

So a file existing at `Content\NServiceBus\Logging\Index.md` will have a resultant url of `http://docs.particular.net/NServiceBus/Logging`

### Linking

Links to other documentation pages should be relative contain the `.md` extension. 

The `.md` allows links to work inside the GitHub web UI. The `.md` will be trimmed when they are finally rendered

Given the case of editing a page located at `\Content\NServiceBus\Page1.md`.

To link to the file `\Content\NServiceBus\Page2.md` you would use `[Page 2 Text](Page2.md)`

To link to the file `\Content\ServiceControl\Page3.md` you would use `[Page 3 Text](../ServiceControl/Page3.md)`

# Markdown

The site is rendered using [GitHub Flavoured Markdown](https://help.github.com/articles/github-flavored-markdown)

## Editors
For editing markdown on your desktop using Git try the following

### [MarkdownPad](http://markdownpad.com/)

Ensure you enable *GitHub Flavoured Markdown* by going to 

    Tools > Options > Markdown > Markdown Processor > GitHub Flavoured Markdown

Or click in the bottom left no the `M` icon to "hot-switch"

Don't render YAML Front-Matter by going to  

    Tools > Options > Markdown > Markdown Settings 

And checking `Ignore YAML Front-matter`
  

## Anchors

On addition to standard markdown is the auto creating anchors for headings

So if you have a heading like this 

    ## My Heading

it will be converted to 

    <h2>
      <a name="my-heading"/>
      My Heading
    </h2>

Which means elsewhere in the page you can link to it with  

    [Goto My Heading](#My-Heading)

## Some useful characters

 * Ticks are done with `&#10004;` &#10004;
 * Crosses are done with `&#10006;` &#10006;

## More info
 
 * [Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)

# Additional Resources

* [General GitHub documentation](http://help.github.com/)
* [GitHub pull request documentation](http://help.github.com/send-pull-requests/)
* [Forking a Repo](https://help.github.com/articles/fork-a-repo)
* [Using Pull Requests](https://help.github.com/articles/using-pull-requests)
