FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY . .
WORKDIR /src/Sender
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Sender.dll"]