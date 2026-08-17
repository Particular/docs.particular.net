# Build and run this Dockerfile from the repository root directory
# Example commands:
# podman build -f samples/cosmosdb/simple/CosmosDB_3/Dockerfile -t cosmosdb-simple .
# podman run --rm -e COSMOS_CONNECTION_STRING="$COSMOS_CONNECTION_STRING" cosmosdb-simple

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# Copy build configuration files from repository root
COPY nuget.config Directory.Build.props BannedSymbols.txt ./

# Copy the sample solution
COPY samples/cosmosdb/simple/CosmosDB_3/ ./samples/cosmosdb/simple/CosmosDB_3/

# Restore packages
WORKDIR /source/samples/cosmosdb/simple/CosmosDB_3
RUN dotnet restore --no-cache

# Build the solution (no configuration specified, using net9.0 as the latest target)
RUN dotnet build --no-restore -f net9.0
RUN dotnet publish Client/Client.csproj --no-restore -f net9.0 -o /app/client
RUN dotnet publish Server/Server.csproj --no-restore -f net9.0 -o /app/server

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app

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
COPY samples/cosmosdb/simple/CosmosDB_3/run_sample.sh ./
RUN chmod +x run_sample.sh

CMD ["./run_sample.sh"]
