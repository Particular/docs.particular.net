# Build runtime image
FROM microsoft/dotnet:2.0-runtime
WORKDIR /Sender
COPY ./bin/Debug/netcoreapp2.0/publish .
ENTRYPOINT ["dotnet", "Sender.dll"]