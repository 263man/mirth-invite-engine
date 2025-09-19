# Use a specific, stable base image for Java 11
FROM eclipse-temurin:11.0.22_7-jdk-focal

# Argument for the Mirth Connect version
ARG MIRTH_VERSION=4.5.2
ENV MIRTH_VERSION=${MIRTH_VERSION}

# Define the canonical download URL from the official GitHub Releases page
ARG DOWNLOAD_URL="https://github.com/nextgenhealthcare/connect/releases/download/${MIRTH_VERSION}/mirthconnect-${MIRTH_VERSION}-unix.tar.gz"

# Set working directory
WORKDIR /opt

# Download, verify, and install Mirth Connect
RUN apt-get update && apt-get install -y --no-install-recommends curl tar && \
    echo "Attempting to download Mirth Connect from Official Release URL: ${DOWNLOAD_URL}" && \
    curl -fsSL --retry 3 "${DOWNLOAD_URL}" -o mirthconnect.tar.gz && \
    if [ ! -s mirthconnect.tar.gz ]; then \
        echo "FATAL: Mirth Connect download failed from the official source."; \
        exit 1; \
    fi && \
    echo "Download successful. Extracting archive..." && \
    tar -xzf mirthconnect.tar.gz && \
    rm mirthconnect.tar.gz && \
    apt-get purge -y curl && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Set the Mirth Connect home directory environment variable
ENV MIRTH_HOME /opt/mirth-connect

# Expose Mirth Connect ports
EXPOSE 8080 8443

WORKDIR /opt/mirth-connect

# The command to run Mirth Connect
CMD ["./mcservice", "run"]