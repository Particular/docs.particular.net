### Reviewing a page

If, as part of editing a page, a full review of the content is done, the [reviewed header](#reviewed) should be updated. This date is used to render the [last reviewed page](https://docs.particular.net/review).

As part of a full review, the following should be done:

* Spelling (US)
* Grammar
* Version-specific language and content is correct
* Language is concise
* All links are relevant. No 3rd party links have redirects or 404s.
* Are there any more links that can be added to improve the content
* Content is correct up to and including the current released version
* Content can benefit from having its own header so that it is picked up while searching for a related topic.
* Summary and title are adequate
* Summary adheres to the [ai friendliness](https://github.com/particular/developereducation/tree/master/seo/ai-friendliness.md] guidelines.
* Consider what is the best place to direct the reader after they are done reading the current page. Add a link to that page at the bottom.
* Update the reviewed date in the header, even if no changes were made.
* Remove [security advisories](https://docs.particular.net/security-advisories/) for no longer supported versions

## Git management/behavior

In general the quality of the git history is not important in this repository. The reason for this is that the standard usages of a clean history (blame, supporting old versions, support branches etc) do not apply to a documentation repository. As such there are several recommendations based on that:

* If pushed to GitHub **do not** re-write history. Even locally it is probably not worth the effort.
* **Do not** force push.
* Optionally merge commits immediately prior to merging a PR.

So if following the [Git pretty flow chart](http://justinhileman.info/article/git-pretty/) you should usually end in the "It's safest to let it stay ugly" end point.

## Additional Resources

* [GitHub Flow in the Browser](https://help.github.com/articles/github-flow-in-the-browser/)
* [General GitHub documentation](https://help.github.com/)
* [GitHub pull request documentation](https://help.github.com/send-pull-requests/)
* [Forking a Repo](https://help.github.com/articles/fork-a-repo)
* [Using Pull Requests](https://help.github.com/articles/using-pull-requests)
* [Markdown Table generator](https://www.tablesgenerator.com/markdown_tables)