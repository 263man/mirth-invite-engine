# Use the official .NET SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project file and restore dependencies
COPY ["MirthInviteEngine.csproj", "."]
RUN dotnet restore "./MirthInviteEngine.csproj"

# Copy the remaining source code and build the application
COPY . .
WORKDIR "/src/."
RUN dotnet build "./MirthInviteEngine.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

# Start the publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MirthInviteEngine.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

# Create the final, runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MirthInviteEngine.dll"]