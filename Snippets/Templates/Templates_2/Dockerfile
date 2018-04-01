FROM microsoft/dotnet:2.0-runtime AS base

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY . .
WORKDIR /src/MyEndpoint
#startcode docker-publish
RUN dotnet publish -c Release -o /app
#endcode

#startcode docker-entry
FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MyEndpoint.dll"]
#endcode