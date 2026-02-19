### Markdown partials

Partials are version-specific files that contain markdown. Partials are preferable for situations where quite a bit of content differs, or there there will be multiple variants for multiple different versions. For simpler situations, such as a 1-3 line note or aside only for one range of versions, see [inline version conditionals](#inline-version-conditionals) below instead.

They are only rendered in the target page when the version filter matches the convention for a give file.

Partial Convention: `filePrefix_key_nugetAlias_version.partial.md`

Make sure to use component alias (as defined in components.yaml file) in the partial name. For most components component alias will be identical to NuGet alias, however it's not always the case, e.g. the Callbacks feature has been moved out of core package to the dedicated NServiceBus.Callbacks package, so the there are two NuGet aliases that are related to this feature, but it's still the same component and has a single component alias.

The NuGet alias in samples should match the prefix as defined by the samples solution directories.

Partials are rendered in the target page by using the following syntax

```markdown
partial: PARTIAL_KEY
```

So an example directory structure might be as follows

<img src="partials.png" style='border:1px solid #000000' />

And to include the `endpointname` partial can be pulled into `sample.md` by including.

```markdown
partial: endpointname
```

### Inline version conditionals

Inline version conditionals allow a short section of markdown to only apply to a specific version range. This is perfect for situations like an alert for all versions less than version N, where using a [partial file](#markdown-partials) might be too heavy-handed and harder to comprehend.

It's important to remember that neither partials nor inline conditionals can _**create**_ an item in the versions dropdown—only Snippets can do that.

```markdown
#if-version [7.7, )

This content only displays for versions 7.7 and up

#end-if

#if-version [, 8)

This content only displays for versions less than 8.0

#end-if

#if-version SqlTransportLegacySystemClient [4, 6)

On pages where the snippets span more than one [NuGet package alias](https://github.com/Particular/docs.particular.net/blob/version-conditionals/components/nugetAlias.txt), version expressions must include the version expression.

#end-if
```

A full NuGet-style version expression is required. `#if-version 6` is not allowed because ultimately it would not be clear if that meant version 6 and above `[6, )` or only version 6 `[6, 7)`.