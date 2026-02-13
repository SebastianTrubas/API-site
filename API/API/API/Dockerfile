# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore (optional but good practice)
RUN dotnet restore API.csproj

# Publish
RUN dotnet publish API.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Set URL variable for many hosts
ENV ASPNETCORE_URLS=http://+:$PORT

ENTRYPOINT ["dotnet", "API.dll"]
