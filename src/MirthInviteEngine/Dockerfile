# src/MirthInviteEngine/Dockerfile

# Use Debian as base instead of blocked mcr.microsoft.com
FROM debian:bullseye-slim AS base

# Install prerequisites
RUN apt-get update && apt-get install -y --no-install-recommends \
        curl ca-certificates libicu67 libssl1.1 zlib1g \
    && rm -rf /var/lib/apt/lists/*

# Install .NET SDK 8.0 manually (download tarball)
ARG DOTNET_VERSION=8.0.100
WORKDIR /usr/share/dotnet
RUN curl -fsSL https://dotnetcli.azureedge.net/dotnet/Sdk/$DOTNET_VERSION/dotnet-sdk-$DOTNET_VERSION-linux-x64.tar.gz \
    -o dotnet-sdk.tar.gz \
 && tar -xzf dotnet-sdk.tar.gz \
 && rm dotnet-sdk.tar.gz
ENV PATH="/usr/share/dotnet:${PATH}"

# Build stage
FROM base AS build
WORKDIR /src
COPY ["MirthInviteEngine.csproj", "."]
RUN dotnet restore "MirthInviteEngine.csproj"
COPY . .
RUN dotnet publish "MirthInviteEngine.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM debian:bullseye-slim AS final
RUN apt-get update && apt-get install -y --no-install-recommends \
        libicu67 libssl1.1 zlib1g \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MirthInviteEngine.dll"]
