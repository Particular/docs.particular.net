FROM mcr.microsoft.com/dotnet/sdk:9.0

# Install expect for automated input
RUN apt-get update && apt-get install -y expect procps && rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /app

# Copy configuration files
COPY nuget.config ./
COPY Directory.Build.props ./
COPY BannedSymbols.txt ./

# Copy solution and project files
COPY *.sln ./
COPY Consumer1/*.csproj Consumer1/
COPY Consumer2/*.csproj Consumer2/
COPY Producer/*.csproj Producer/

# Restore packages
RUN dotnet restore

# Copy all source code
COPY . .

# Restore again to ensure all packages are available after copying all files
RUN dotnet restore

# Build the solution using the latest framework
RUN dotnet build --no-restore --framework net9.0

CMD ["/app/run_sample.sh"]
