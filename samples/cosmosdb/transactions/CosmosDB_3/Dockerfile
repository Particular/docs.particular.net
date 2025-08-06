# Build this image from the repository root directory:
# podman build -f samples/cosmosdb/transactions/CosmosDB_3/Dockerfile -t cosmosdb-transactions-sample .
#
# Run with your CosmosDB connection string:
# podman run --rm -e COSMOS_CONNECTION_STRING="$COSMOS_CONNECTION_STRING" cosmosdb-transactions-sample
#
# Docker commands (same as above but replace 'podman' with 'docker'):
# docker build -f samples/cosmosdb/transactions/CosmosDB_3/Dockerfile -t cosmosdb-transactions-sample .
# docker run --rm -e COSMOS_CONNECTION_STRING="$COSMOS_CONNECTION_STRING" cosmosdb-transactions-sample

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Install expect for automated testing
RUN apt-get update && apt-get install -y expect && rm -rf /var/lib/apt/lists/*

# Copy build configuration files
COPY nuget.config Directory.Build.props BannedSymbols.txt ./

# Copy solution and project files
COPY samples/cosmosdb/transactions/CosmosDB_3/ ./samples/cosmosdb/transactions/CosmosDB_3/

# Restore packages
WORKDIR /app/samples/cosmosdb/transactions/CosmosDB_3
RUN dotnet restore

# Build all projects
RUN dotnet build --no-restore SharedMessages/SharedMessages.csproj
RUN dotnet build --no-restore Client/Client.csproj
RUN dotnet build --no-restore Server/Server.csproj

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app

# Install expect and process management tools
RUN apt-get update && apt-get install -y expect procps && rm -rf /var/lib/apt/lists/*

# Create .learningtransport folder for LearningTransport
RUN mkdir -p .learningtransport

# Copy built applications
COPY --from=build /app/samples/cosmosdb/transactions/CosmosDB_3/Client/bin/Debug/net9.0/ ./Client/
COPY --from=build /app/samples/cosmosdb/transactions/CosmosDB_3/Server/bin/Debug/net9.0/ ./Server/

# Copy the startup script
COPY samples/cosmosdb/transactions/CosmosDB_3/run_sample.sh ./
RUN chmod +x run_sample.sh

CMD ["./run_sample.sh"]
