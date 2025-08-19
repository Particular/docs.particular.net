# Build and run the delayed-delivery sample
# Build context should be from the repository root
#
# Example commands:
# docker build -f samples/delayed-delivery/Core_9/Dockerfile -t delayed-delivery-core-9 .
# docker run -it --rm delayed-delivery-core-9
#
# podman build -f samples/delayed-delivery/Core_9/Dockerfile -t delayed-delivery-core-9 .
# podman run -it --rm delayed-delivery-core-9

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy required config files from repository root
COPY nuget.config .
COPY Directory.Build.props .
COPY BannedSymbols.txt .

# Copy sample solution
COPY samples/delayed-delivery/Core_9/ ./sample/

# Restore and build
WORKDIR /app/sample
RUN dotnet restore
RUN dotnet build --no-restore

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app

# Set environment variable to indicate container environment
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Install expect and process management tools
RUN apt-get update && apt-get install -y expect procps && rm -rf /var/lib/apt/lists/*

# Create .learningtransport folder for LearningTransport
RUN mkdir -p /app/.learningtransport

# Copy built applications
COPY --from=build /app/sample/Client/bin/Debug/net9.0/ ./Client/
COPY --from=build /app/sample/Server/bin/Debug/net9.0/ ./Server/
COPY --from=build /app/sample/Shared/bin/Debug/net9.0/ ./Shared/

# Copy startup script
COPY samples/delayed-delivery/Core_9/run_sample.sh ./
RUN chmod +x run_sample.sh

CMD ["./run_sample.sh"]
