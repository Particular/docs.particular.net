# Build from repository root:
# podman build -f samples/recoverabilitypolicytesting/Core_10/Dockerfile -t recoverabilitypolicytesting-core10 .
# docker build -f samples/recoverabilitypolicytesting/Core_10/Dockerfile -t recoverabilitypolicytesting-core10 .
#
# Run:
# podman run --rm recoverabilitypolicytesting-core10
# docker run --rm recoverabilitypolicytesting-core10

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build

# Set environment variable for .NET 10 preview issue
ENV DOTNET_SYSTEM_NET_SECURITY_NOREVOCATIONCHECKBYDEFAULT=true

# Install expect and process management tools
RUN apt-get update && apt-get install -y expect procps && rm -rf /var/lib/apt/lists/*

# Copy build configuration files
COPY nuget.config Directory.Build.props BannedSymbols.txt ./

# Copy the sample source code
COPY samples/recoverabilitypolicytesting/Core_10/ /app/sample/

WORKDIR /app/sample

# Restore dependencies
RUN dotnet restore --no-cache

# Build the sample
RUN dotnet build --no-restore

# Run the tests in the build stage (only .NET 10 to avoid missing runtime issues)
RUN dotnet test --no-build --logger:console --verbosity:normal --framework net10.0 Sample/Sample.csproj

FROM mcr.microsoft.com/dotnet/runtime:10.0-preview AS runtime

# Set environment variable for .NET 10 preview issue
ENV DOTNET_SYSTEM_NET_SECURITY_NOREVOCATIONCHECKBYDEFAULT=true

# This is a unit testing sample - tests are executed during the build stage
# The runtime stage just provides confirmation that the tests passed
RUN mkdir -p /app && echo "Custom Recoverability Policy tests completed successfully during build stage" > /app/test-results.txt

WORKDIR /app

CMD ["cat", "/app/test-results.txt"]
