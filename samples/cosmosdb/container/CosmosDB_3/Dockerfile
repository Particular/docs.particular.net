# Build and run the CosmosDB Container sample
# Build context should be from the repository root
#
# Build examples:
# podman build -f samples/cosmosdb/container/CosmosDB_3/Dockerfile -t cosmosdb-container-sample .
# docker build -f samples/cosmosdb/container/CosmosDB_3/Dockerfile -t cosmosdb-container-sample .
#
# Run examples:
# podman run -it --rm cosmosdb-container-sample
# docker run -it --rm cosmosdb-container-sample

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Install expect and process management tools
RUN apt-get update && apt-get install -y \
    expect \
    procps \
    && rm -rf /var/lib/apt/lists/*

# Set environment variable for .NET build issues
ENV DOTNET_SYSTEM_NET_SECURITY_NOREVOCATIONCHECKBYDEFAULT=true

WORKDIR /src

# Copy build configuration files from repository root
COPY nuget.config .
COPY Directory.Build.props .
COPY BannedSymbols.txt .

# Copy the sample solution and projects
COPY samples/cosmosdb/container/CosmosDB_3/ ./samples/cosmosdb/container/CosmosDB_3/

# Restore packages
RUN cd samples/cosmosdb/container/CosmosDB_3 && dotnet restore --verbosity minimal

# Build all projects
RUN cd samples/cosmosdb/container/CosmosDB_3 && dotnet build --no-restore --verbosity minimal

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime

# Install expect and process management tools for runtime
RUN apt-get update && apt-get install -y \
    expect \
    procps \
    curl \
    && rm -rf /var/lib/apt/lists/*

# Set environment variable for .NET runtime issues
ENV DOTNET_SYSTEM_NET_SECURITY_NOREVOCATIONCHECKBYDEFAULT=true

WORKDIR /app

# Create .learningtransport directory for LearningTransport
RUN mkdir -p .learningtransport

# Copy built applications
COPY --from=build /src/samples/cosmosdb/container/CosmosDB_3/Client/bin/Debug/net9.0/ ./Client/
COPY --from=build /src/samples/cosmosdb/container/CosmosDB_3/Server/bin/Debug/net9.0/ ./Server/
COPY --from=build /src/samples/cosmosdb/container/CosmosDB_3/SharedMessages/bin/Debug/net9.0/ ./SharedMessages/

# Copy the run script
COPY samples/cosmosdb/container/CosmosDB_3/run_sample.sh ./
RUN chmod +x run_sample.sh

# Default command
CMD ["./run_sample.sh"]
