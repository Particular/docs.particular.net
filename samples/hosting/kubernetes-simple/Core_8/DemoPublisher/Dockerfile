FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
# USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DemoPublisher/DemoPublisher.csproj", "DemoPublisher/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "DemoPublisher/DemoPublisher.csproj"
COPY . .
WORKDIR "/src/DemoPublisher"
RUN dotnet build "DemoPublisher.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "DemoPublisher.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DemoPublisher.dll"]