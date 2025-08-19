# Custom Recoverability Sample - Core 9 Version
# This Dockerfile builds and runs the Custom Recoverability sample
#
# Build from the repository root directory:
# Docker: docker build -f samples/custom-recoverability/Core_9/Dockerfile -t custom-recoverability-core9 .
# Podman: podman build -f samples/custom-recoverability/Core_9/Dockerfile -t custom-recoverability-core9 .
#
# Run the container:
# Docker: docker run -it --rm custom-recoverability-core9
# Podman: podman run -it --rm custom-recoverability-core9

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Install expect for automation
RUN apt-get update && apt-get install -y expect procps && rm -rf /var/lib/apt/lists/*

# Copy repository configuration files
COPY nuget.config /src/
COPY Directory.Build.props /src/
COPY BannedSymbols.txt /src/

# Copy the sample source
COPY samples/custom-recoverability/Core_9/ /src/sample/

WORKDIR /src/sample

# Build the normal version
RUN dotnet build --framework net9.0

# Create fault scenario version by modifying the handler
RUN cp -r Server Server_Fault
RUN sed -i 's|//throw new ArgumentNullException("Uh oh - something went wrong....");|throw new ArgumentNullException("Uh oh - something went wrong....");|' Server_Fault/MyHandler.cs
RUN sed -i 's|return Task.CompletedTask;|//return Task.CompletedTask;|' Server_Fault/MyHandler.cs
RUN sed -i '1i using System;' Server_Fault/MyHandler.cs

# Build the fault version
RUN dotnet build Server_Fault/Server.csproj --framework net9.0

# Copy the run script
COPY samples/custom-recoverability/Core_9/run_sample.sh /src/run_sample.sh
RUN chmod +x /src/run_sample.sh

# Set runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0

# Install expect and process tools for runtime
RUN apt-get update && apt-get install -y expect procps && rm -rf /var/lib/apt/lists/*

# Copy built applications and run script
COPY --from=build /src/sample/ /app/
COPY --from=build /src/run_sample.sh /app/

WORKDIR /app

# Run the script by default
CMD ["./run_sample.sh"]
