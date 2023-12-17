FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DemoSubscriber/DemoSubscriber.csproj", "DemoSubscriber/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "DemoSubscriber/DemoSubscriber.csproj"
COPY . .
WORKDIR "/src/DemoSubscriber"
RUN dotnet build "DemoSubscriber.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "DemoSubscriber.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DemoSubscriber.dll"]