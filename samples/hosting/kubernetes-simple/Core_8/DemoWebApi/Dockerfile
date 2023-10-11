FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
EXPOSE 5037
EXPOSE 7013

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DemoWebApi/DemoWebApi.csproj", "DemoWebApi/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "DemoWebApi/DemoWebApi.csproj"
COPY . .
WORKDIR "/src/DemoWebApi"
RUN dotnet build "DemoWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "DemoWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

WORKDIR /app/transport
WORKDIR /app/sagas

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoWebApi.dll"]