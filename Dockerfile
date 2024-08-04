# Use the official .NET 8.0 SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the csproj files
COPY *.csproj ./

# Restore the .NET dependencies
RUN dotnet restore

# Install dotnet-ef tool globally
RUN dotnet tool install --global dotnet-ef

# Add dotnet tools to PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy the rest of the application code
COPY . ./

# Make the entrypoint scripts executable
RUN chmod +x ./.entrypoint/entrypoint.sh ./.entrypoint/wait-for-it.sh

# Expose the application port
EXPOSE 5000

# Set the entrypoint to the script
ENTRYPOINT ["./.entrypoint/entrypoint.sh"]