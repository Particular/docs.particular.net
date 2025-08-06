# Build context should be from the repository root
# Example build commands:
# docker build -f samples/pubsub/native/Core_10/Dockerfile -t pubsub-native-core10 .
# podman build -f samples/pubsub/native/Core_10/Dockerfile -t pubsub-native-core10 .

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Set environment variable for .NET 10 preview builds
ENV DOTNET_SYSTEM_NET_SECURITY_NOREVOCATIONCHECKBYDEFAULT=true

# Copy necessary build files from repository root
COPY nuget.config .
COPY Directory.Build.props .
COPY BannedSymbols.txt .

# Copy the sample solution
COPY samples/pubsub/native/Core_10/ ./samples/pubsub/native/Core_10/

# Build the solution
WORKDIR /src/samples/pubsub/native/Core_10
RUN dotnet restore
RUN dotnet publish Publisher/Publisher.csproj -f net10.0 -o /app/publisher --no-restore
RUN dotnet publish Subscriber/Subscriber.csproj -f net10.0 -o /app/subscriber --no-restore

FROM mcr.microsoft.com/dotnet/runtime:10.0-preview AS runtime
WORKDIR /app

# Set environment variable for .NET 10 preview runtime
ENV DOTNET_SYSTEM_NET_SECURITY_NOREVOCATIONCHECKBYDEFAULT=true

# Install expect and process management tools
RUN apt-get update && apt-get install -y expect procps && rm -rf /var/lib/apt/lists/*

# Create .learningtransport directory for LearningTransport storage
RUN mkdir -p .learningtransport

# Copy published applications
COPY --from=build /app/publisher ./publisher/
COPY --from=build /app/subscriber ./subscriber/

# Copy startup script
COPY samples/pubsub/native/Core_10/run_sample.sh ./
RUN chmod +x run_sample.sh

CMD ["./run_sample.sh"]
