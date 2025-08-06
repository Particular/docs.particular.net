# Build context should be from the repository root
# Example build commands:
# docker build -f samples/pubsub/native/Core_9/Dockerfile -t pubsub-native-core9 .
# podman build -f samples/pubsub/native/Core_9/Dockerfile -t pubsub-native-core9 .

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy necessary build files from repository root
COPY nuget.config .
COPY Directory.Build.props .
COPY BannedSymbols.txt .

# Copy the sample solution
COPY samples/pubsub/native/Core_9/ ./samples/pubsub/native/Core_9/

# Build the solution
WORKDIR /src/samples/pubsub/native/Core_9
RUN dotnet restore
RUN dotnet publish Publisher/Publisher.csproj -f net9.0 -o /app/publisher --no-restore
RUN dotnet publish Subscriber/Subscriber.csproj -f net9.0 -o /app/subscriber --no-restore

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app

# Install expect and process management tools
RUN apt-get update && apt-get install -y expect procps && rm -rf /var/lib/apt/lists/*

# Create .learningtransport directory for LearningTransport storage
RUN mkdir -p .learningtransport

# Copy published applications
COPY --from=build /app/publisher ./publisher/
COPY --from=build /app/subscriber ./subscriber/

# Copy startup script
COPY samples/pubsub/native/Core_9/run_sample.sh ./
RUN chmod +x run_sample.sh

CMD ["./run_sample.sh"]
