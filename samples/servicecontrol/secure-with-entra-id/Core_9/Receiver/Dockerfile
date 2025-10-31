FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /

COPY . .
RUN dotnet publish Receiver -c $BUILD_CONFIGURATION -o /app/publish --arch $TARGETARCH /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Receiver.dll"]