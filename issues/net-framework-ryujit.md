---
title: System.TypeInitializationException when starting
summary: Howto resolve System.TypeInitializationException: The type initializer for '<Module>' threw an exception. ---> System.AccessViolationException: Attempted
 to read or write protected memory. This is often an indication that other memory is corrupt.
tags: 
- Service Control
redirects:
related:
---


## Summary

The introduction of the .net framework v4.6 can result in a few issues that are primarily related to the new RyuJIT engine which can affect some or our platform products.

Applying the latest updates and/or hotfixes for the .net framework can resolve these issues.


## More information

Installing Visual Studio 2015 will also install .net framework v4.6 which is an in place update for all previous v4.x versions. These versions can not be deployed side by side.

There are some issues with the release version which can affect our platform products during installation or execution.

The following exception can occur:

>Unhandled Exception: System.TypeInitializationException: The type initializer for '<Module>' threw an exception. ---> System.AccessViolationException: Attempted to read or write protected memory. This is often an indication that other memory is corrupt.


### Solution 


Disablying the RyuJIT engine can temporarily resolve this issue until Microsoft has released an updated version or a hotfix.


Please look at the following documentation on how to disable the RyuJIT engine on a global or application level.

- https://github.com/Microsoft/dotnet/blob/master/docs/testing-with-ryujit.md#disable-ryujit


You will need to disable it on a global level if this issue is causing issues during installation. Disabling it in the `app.config` will make sure that the affected application will work if they are already installed.


## References

- https://github.com/Microsoft/dotnet/blob/master/docs/testing-with-ryujit.md#disable-ryujit
- http://blogs.msdn.com/b/dotnet/archive/2015/07/28/ryujit-bug-advisory-in-the-net-framework-4-6.aspx


