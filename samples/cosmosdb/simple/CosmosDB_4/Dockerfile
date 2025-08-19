# Build and run this Dockerfile from the repository root directory
# Example commands:
# podman build -f samples/cosmosdb/simple/CosmosDB_4/Dockerfile -t cosmosdb-simple-v10 .
# podman run --rm -e COSMOS_CONNECTION_STRING="$COSMOS_CONNECTION_STRING" cosmosdb-simple-v10

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /source

# Set environment variable for .NET 10 security fix
ENV DOTNET_SYSTEM_NET_SECURITY_NOREVOCATIONCHECKBYDEFAULT=true

# Copy build configuration files from repository root
COPY nuget.config Directory.Build.props BannedSymbols.txt ./

# Copy the sample solution
COPY samples/cosmosdb/simple/CosmosDB_4/ ./samples/cosmosdb/simple/CosmosDB_4/

# Restore packages
WORKDIR /source/samples/cosmosdb/simple/CosmosDB_4
RUN dotnet restore --no-cache

# Build the solution (no configuration specified, using net10.0 as the target)
RUN dotnet build SharedMessages/SharedMessages.csproj --no-restore -f net10.0
RUN dotnet build Client/Client.csproj --no-restore -f net10.0
RUN dotnet build Server/Server.csproj --no-restore -f net10.0
RUN dotnet publish Client/Client.csproj --no-restore -f net10.0 -o /app/client
RUN dotnet publish Server/Server.csproj --no-restore -f net10.0 -o /app/server

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0-preview AS runtime
WORKDIR /app

# Set environment variable for .NET 10 security fix
ENV DOTNET_SYSTEM_NET_SECURITY_NOREVOCATIONCHECKBYDEFAULT=true

# Install expect and process management tools
RUN apt-get update && apt-get install -y \
    expect \
    procps \
    && rm -rf /var/lib/apt/lists/*

# Create .learningtransport directory for LearningTransport
RUN mkdir -p /app/.learningtransport

# Copy built applications
COPY --from=build /app/client ./client/
COPY --from=build /app/server ./server/

# Copy the startup script
COPY samples/cosmosdb/simple/CosmosDB_4/run_sample.sh ./
RUN chmod +x run_sample.sh

CMD ["./run_sample.sh"]
