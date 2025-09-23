# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MirthInviteEngine.csproj", "."]
RUN dotnet restore "./MirthInviteEngine.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./MirthInviteEngine.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "./MirthInviteEngine.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create the final, runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bullseye-slim AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MirthInviteEngine.dll"]