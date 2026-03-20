FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY nuget.config .
COPY src/Acme.Shared/Acme.Shared.csproj src/Acme.Shared/
COPY src/Acme.Sales/Acme.Sales.csproj src/Acme.Sales/
RUN dotnet restore src/Acme.Sales/Acme.Sales.csproj

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Acme.Sales.dll"]
