# Build this image from the repository root directory:
# podman build -f samples/cosmosdb/transactions/CosmosDB_4/Dockerfile -t cosmosdb-transactions-sample-v10 .
#
# Run with your CosmosDB connection string:
# podman run --rm -e COSMOS_CONNECTION_STRING="$COSMOS_CONNECTION_STRING" cosmosdb-transactions-sample-v10
#
# Docker commands (same as above but replace 'podman' with 'docker'):
# docker build -f samples/cosmosdb/transactions/CosmosDB_4/Dockerfile -t cosmosdb-transactions-sample-v10 .
# docker run --rm -e COSMOS_CONNECTION_STRING="$COSMOS_CONNECTION_STRING" cosmosdb-transactions-sample-v10

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /app

# Set environment variable for .NET 10 preview compatibility
ENV DOTNET_SYSTEM_NET_SECURITY_NOREVOCATIONCHECKBYDEFAULT=true

# Install expect for automated testing
RUN apt-get update && apt-get install -y expect && rm -rf /var/lib/apt/lists/*

# Copy build configuration files
COPY nuget.config Directory.Build.props BannedSymbols.txt ./

# Copy solution and project files
COPY samples/cosmosdb/transactions/CosmosDB_4/ ./samples/cosmosdb/transactions/CosmosDB_4/

# Restore packages
WORKDIR /app/samples/cosmosdb/transactions/CosmosDB_4
RUN dotnet restore

# Build all projects
RUN dotnet build --no-restore SharedMessages/SharedMessages.csproj
RUN dotnet build --no-restore Client/Client.csproj
RUN dotnet build --no-restore Server/Server.csproj

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0-preview AS runtime
WORKDIR /app

# Set environment variable for .NET 10 preview compatibility
ENV DOTNET_SYSTEM_NET_SECURITY_NOREVOCATIONCHECKBYDEFAULT=true

# Install expect and process management tools
RUN apt-get update && apt-get install -y expect procps && rm -rf /var/lib/apt/lists/*

# Create .learningtransport folder for LearningTransport
RUN mkdir -p .learningtransport

# Copy built applications
COPY --from=build /app/samples/cosmosdb/transactions/CosmosDB_4/Client/bin/Debug/net10.0/ ./Client/
COPY --from=build /app/samples/cosmosdb/transactions/CosmosDB_4/Server/bin/Debug/net10.0/ ./Server/

# Copy the startup script
COPY samples/cosmosdb/transactions/CosmosDB_4/run_sample.sh ./
RUN chmod +x run_sample.sh

CMD ["./run_sample.sh"]