# startcode install
dotnet new --install NServiceBus.Template.TransportAdapter.WindowsService::1.0.1-alpha*
# endcode

# startcode usage
dotnet new sc-adapter-service --name MyAdapter
# endcode


# startcode addToSolution
dotnet new sc-adapter-service --name MyAdapter
dotnet sln add "MyAdapter/MyAdapter.csproj"
# endcode