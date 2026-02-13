### Markdown includes

Markdown includes are pulled into the document prior to passing the content through the markdown conversion.

#### Defining an include

Add a file anywhere in the docs repository that is suffixed with `.include.md`. For example, the file might be named `theKey.include.md`.

#### Using an include

Add the following to the markdown:

```markdown
include: theKey
```