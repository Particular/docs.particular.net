### Tutorials

Tutorials are similar to samples but optimized for new users to follow in a step-by-step fashion. Tutorials differ from samples in the following ways:

* Markdown file must be named `tutorial.md`
* No component specified in the header
* Focus on only the most recent version
* Rendered without button toolbar and component information at the top
* By default, solution download button is rendered at the end
    * An inline download button can be created instead using a `downloadbutton` directive on its own line within the tutorial markdown.
* Utilizes two collections of snippets, from the solution and also from an optional Snippets solution, allowing more granular or multi-phase snippets
* Allows use of personal voice (you/your/we/etc.) within `/tutorials` directory to foster collaborative tone with user

#### Directory structure

An example directory structure for a tutorial might look like this:

```text
{tutorial-name}/
  Snippets/ (optional)
    Snippets.sln
    Core_6
  Solution/
    {SolutionName}.sln
  tutorial.md
```

Tutorials can be grouped together in a parent directory with a normal article serving as a table of contents.

#### Multi-lesson tutorials

For tutorials chained together to form multiple lessons, navigation can be created to combine a button linking to the next lesson with the Download Solution link.

```yaml
- !!tutorial
  nextText: "Next Lesson: Sending a command"
  nextUrl: tutorials/nservicebus-step-by-step/2-sending-a-command
```

The `nextText` parameter is optional, and will default to the title of the linked page if omitted.