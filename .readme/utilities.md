## Utilities

Under [tools](https://github.com/Particular/docs.particular.net/tree/master/tools) there are several utilities to help with the management of this repository. All are in the form of [LINQPad](https://www.linqpad.net/) scripts.

### projectStandards.linq

Remove redundant content from sln and csproj files.

### setStartup.linq

Sets the correct startup projects for every solution. This is persisted in an `.suo` file for each solution. Since `.suo` files are not committed to source control, if a re-clone is done this script will need to be re-run.