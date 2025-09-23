# Use a specific, stable base image for Java 11
FROM eclipse-temurin:11.0.22_7-jdk-focal

# Argument for the Mirth Connect version (includes build number for S3 path)
ARG MIRTH_VERSION=4.5.2.b363
ENV MIRTH_VERSION=${MIRTH_VERSION}

# Define the canonical download URL from the official S3 mirror (verified pattern with build number)
ARG DOWNLOAD_URL="https://s3.amazonaws.com/downloads.mirthcorp.com/connect/${MIRTH_VERSION}/mirthconnect-${MIRTH_VERSION}-unix.tar.gz"
ARG CLI_DOWNLOAD_URL="https://s3.amazonaws.com/downloads.mirthcorp.com/connect/${MIRTH_VERSION}/mirthconnectcli-${MIRTH_VERSION}-unix.tar.gz"

# Set working directory
WORKDIR /opt

# Download, verify, and install Mirth Connect, moving files to /opt/mirth-connect
RUN apt-get update && apt-get install -y --no-install-recommends curl tar && \
    echo "Attempting to download Mirth Connect from Official S3 Mirror: ${DOWNLOAD_URL}" && \
    curl -fsSL --retry 3 "${DOWNLOAD_URL}" -o mirthconnect.tar.gz && \
    if [ ! -s mirthconnect.tar.gz ]; then \
        echo "FATAL: Mirth Connect download failed from the official source."; \
        exit 1; \
    fi && \
    echo "Download successful. Extracting archive..." && \
    tar -xzf mirthconnect.tar.gz && \
    mv "Mirth Connect" mirth-connect && \
    chmod +x /opt/mirth-connect/mcservice && \
    rm mirthconnect.tar.gz && \
    # Download and install CLI to fix migration bug
    echo "Attempting to download Mirth CLI from Official S3 Mirror: ${CLI_DOWNLOAD_URL}" && \
    curl -fsSL --retry 3 "${CLI_DOWNLOAD_URL}" -o mirthconnectcli.tar.gz && \
    if [ ! -s mirthconnectcli.tar.gz ]; then \
        echo "FATAL: Mirth CLI download failed from the official source."; \
        exit 1; \
    fi && \
    echo "Download successful. Extracting CLI archive..." && \
    tar -xzf mirthconnectcli.tar.gz && \
    mv "Mirth Connect CLI" mirth-connect-cli && \
    cp mirth-connect-cli/conf/log4j2-cli.properties /opt/mirth-connect/conf/ && \
    rm -rf mirthconnectcli.tar.gz mirth-connect-cli && \
    apt-get purge -y curl && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* && \
    # Create mirth.properties with essential settings
    mkdir -p /opt/mirth-connect/conf && \
    echo "http.port = 8080" > /opt/mirth-connect/conf/mirth.properties && \
    echo "https.port = 8443" >> /opt/mirth-connect/conf/mirth.properties && \
    echo "keystore.storetype = JKS" >> /opt/mirth-connect/conf/mirth.properties && \
    echo "keystore.path = conf/keystore.jks" >> /opt/mirth-connect/conf/mirth.properties && \
    echo "keystore.password = changeit" >> /opt/mirth-connect/conf/mirth.properties && \
    echo "database = derby" >> /opt/mirth-connect/conf/mirth.properties && \
    echo "database.url = jdbc:derby:/opt/mirth-connect/appdata/mirthdb;create=true" >> /opt/mirth-connect/conf/mirth.properties

# Set the Mirth Connect home directory environment variable
ENV MIRTH_HOME=/opt/mirth-connect

# Expose Mirth Connect ports
EXPOSE 8080 8443 6661

# Set working directory to Mirth Connect
WORKDIR /opt/mirth-connect

# The command to run Mirth Connect
CMD ["./mcservice", "run"]