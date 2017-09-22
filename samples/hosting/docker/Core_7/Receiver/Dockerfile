# Build runtime image
FROM microsoft/dotnet:runtime
WORKDIR /Receiver
COPY ./bin/Debug/netcoreapp2.0/publish .
ENTRYPOINT ["dotnet", "Receiver.dll"]