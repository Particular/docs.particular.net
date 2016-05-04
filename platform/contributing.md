---
title: How to contribute
summary: The Particular Platform is open source and takes contributions from the community.
tags:
- Platform
---

Here is a few guidelines for contributing.


## Getting Started

 * Create a [GitHub account](https://github.com/join).
 * [Find the relevant repository](https://github.com/Particular/).
 * [Create an issue](https://help.github.com/articles/creating-an-issue/) in that repository, assuming one does not already exist.
  * Clearly describe the issue including steps to reproduce when it is a bug.
  * If it is a bug make include the version.
 * Fork the repository on GitHub.


## Making Changes

 * Create a feature branch from where to base the work.
  * This is usually the develop branch since no work is done on the master branch. The master is always the latest stable release.
  * Only target release branches if the fix must be on that branch.
  * To quickly create a feature branch based on develop; `git branch fix/develop/my_contribution` then checkout the new branch with `git checkout fix/develop/my_contribution`. Avoid working directly on the `develop` branch.
 * Make commits of logical units.
 * Check for unnecessary whitespace with `git diff --check` before committing.
 * Make sure the commit messages are in the proper format.
 * Ensure tests have been added.
 * Run build.bat in the root to assure nothing else was accidentally broken.
 * There is ReSharper layer that applies coding standards.


## Submitting Changes

 * Sign the [Contributor License Agreement](http://particular.net/contributors-license-agreement-consent).
 * Push the changes to a feature branch in the fork of the repository.
 * Submit a pull request to the NServiceBus repository.


## Additional Resources

 * [General GitHub documentation](https://help.github.com/)
 * [Using pull requests](https://help.github.com/articles/using-pull-requests/)