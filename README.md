# [ParticularDocs](https://docs.particular.net/)

![VerifyMaster](https://github.com/Particular/docs.particular.net/actions/workflows/verify-master.yml/badge.svg?branch=master) ![BuildProjects](https://github.com/Particular/docs.particular.net/actions/workflows/build-samples-and-snippets.yml/badge.svg?branch=master)

## How to Contribute

Before you start, ensure you have created a [GitHub account](https://github.com/join).

There are two approaches to contributing.

### Via the GitHub Web UI

For simple changes, the GitHub web UI should suffice.

 1. Find the page you want to edit on [ParticularDocs](https://docs.particular.net/).
 1. Click the `Edit Online` button. This will automatically fork the project so you can edit the file.
 1. Make the changes you require. Ensure you verify the changes in the `Preview` tab.
 1. Add a description of the changes.
 1. Click `Propose File Changes`.

### By Forking and Submitting a Pull Request

For more complex changes you should fork and then submit a pull request. This is useful if you are proposing multiple file changes

 1. [Fork](https://help.github.com/forking/) on GitHub.
 1. Clone the fork locally.
 1. Work on the feature.
 1. Push the code to GitHub.
 1. Send a Pull Request on GitHub.

For more information, see [Collaborating on GitHub](https://help.github.com/categories/63/articles) especially [using GitHub pull requests](https://help.github.com/articles/using-pull-requests).

### Running locally

The repository contains a devcontainer that has all the necessary tools to develop content for the documentation. The content is automatically rendered on http://localhost:55666.

For more information about devcontainers visit the [official documentation](https://code.visualstudio.com/docs/devcontainers/containers). Install the pre-requirements mentioned in the [getting started guide](https://code.visualstudio.com/docs/devcontainers/containers#_getting-started) and open the repository in Code (`> code .`).

> [!NOTE]
> The docstool is currently started in the foreground. While it is possible to run it in the background with `&` or the parallel execution feature of devcontainer it might be more cumbersome to inspect the log output of the tool. The downside of starting in the foreground is that Code will continuously show a spinner with "Configuring container".

### Building samples and snippets

To build all samples and snippets run `.\tools\build-samples-and-snippets.ps1` from the repository root folder.
