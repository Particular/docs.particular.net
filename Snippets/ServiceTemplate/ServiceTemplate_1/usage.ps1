# startcode install
dotnet new --install NServiceBus.Template.WindowsService::1.0.1-alpha*
# endcode

# startcode usage
dotnet new nsbservice --name MyWindowsService
# endcode

# startcode addToSolution
dotnet new nsbservice --name MyWindowsService
dotnet sln add "MyWindowsService/MyWindowsService.csproj"
# endcode