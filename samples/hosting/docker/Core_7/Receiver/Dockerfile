#startcode Dockerfile
# Build runtime image
FROM microsoft/dotnet:2.0-runtime
WORKDIR /Receiver
COPY ./bin/Debug/netcoreapp2.0/publish .
ENTRYPOINT ["dotnet", "Receiver.dll"]
#endcode