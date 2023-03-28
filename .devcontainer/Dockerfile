# NET 6.0 is required for the docstool
# For more options see https://github.com/devcontainers/images/tree/main/src/dotnet
FROM mcr.microsoft.com/devcontainers/dotnet:0-6.0

# Required dependency for docstool libskiasharp
RUN sudo apt-get update && sudo apt-get install fontconfig -y

EXPOSE 55555