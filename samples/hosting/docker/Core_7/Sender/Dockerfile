FROM microsoft/dotnet:2.1-runtime AS base

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . .
WORKDIR /src/Sender
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Sender.dll"]