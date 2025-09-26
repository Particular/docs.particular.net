# Build from repository root with:
# podman build -f samples/consumer-driven-contracts/Core_10/Dockerfile -t sample-core10 .
# docker build -f samples/consumer-driven-contracts/Core_10/Dockerfile -t sample-core10 .
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview

# Install expect for automated input
RUN apt-get update && apt-get install -y expect procps && rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /app

# Copy configuration files from repository root
COPY nuget.config ./
COPY Directory.Build.props ./
COPY BannedSymbols.txt ./

# Copy solution and project files
COPY samples/consumer-driven-contracts/Core_10/*.sln ./
COPY samples/consumer-driven-contracts/Core_10/Consumer1/*.csproj Consumer1/
COPY samples/consumer-driven-contracts/Core_10/Consumer2/*.csproj Consumer2/
COPY samples/consumer-driven-contracts/Core_10/Producer/*.csproj Producer/

# Restore packages
RUN dotnet restore

# Copy all source code
COPY samples/consumer-driven-contracts/Core_10/ .

# Restore again to ensure all packages are available after copying all files
RUN dotnet restore

# Build the solution using the latest framework
RUN dotnet build --no-restore --framework net10.0

CMD ["/app/run_sample.sh"]
