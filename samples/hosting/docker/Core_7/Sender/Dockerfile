# Build runtime image
FROM microsoft/dotnet:runtime
WORKDIR /Sender
COPY ./bin/Debug/netcoreapp2.0/publish .
ENTRYPOINT ["dotnet", "Sender.dll"]