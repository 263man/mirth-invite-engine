# docker/mirth/Dockerfile
# Build a working Mirth Connect 4.5.2.b363 image inside GitHub Codespaces
FROM eclipse-temurin:11-jre-focal

# Noninteractive install
ENV DEBIAN_FRONTEND=noninteractive
ENV MIRTH_VERSION=4.5.2.b363
ENV MIRTH_HOME=/opt/mirth-connect

# Install only required tools
RUN set -eux; \
    apt-get update; \
    apt-get install -y --no-install-recommends \
        ca-certificates curl tar unzip gzip procps; \
    rm -rf /var/lib/apt/lists/*

# Download and extract Mirth Connect tar.gz (S3 mirror, fixed version)
RUN set -eux; \
    MIRTH_URL="https://s3.amazonaws.com/downloads.mirthcorp.com/connect/${MIRTH_VERSION}/mirthconnect-${MIRTH_VERSION}-unix.tar.gz"; \
    echo "Downloading Mirth Connect from $MIRTH_URL"; \
    curl -fsSL --retry 3 --connect-timeout 15 "$MIRTH_URL" -o /tmp/mirth.tar.gz; \
    mkdir -p /opt/mirth-connect; \
    tar -xzf /tmp/mirth.tar.gz -C /opt/mirth-connect --strip-components=1; \
    rm /tmp/mirth.tar.gz; \
    chmod +x /opt/mirth-connect/mcservice /opt/mirth-connect/mcserver

# Create mirth user and setup directories
RUN set -eux; \
    groupadd -r mirth || true; \
    useradd --no-log-init -r -g mirth -d /opt/mirth-connect mirth || true; \
    mkdir -p /opt/mirth-connect/appdata /opt/mirth-connect/conf; \
    chown -R mirth:mirth /opt/mirth-connect

# Configure Mirth (disable HTTPS, set Derby DB, keystore password fix)
RUN set -eux; \
    cat > /opt/mirth-connect/conf/mirth.properties <<'EOF'
http.port=8080
https.port=0
keystore.storetype=JKS
keystore.path=conf/keystore.jks
keystore.password=changeit
database=derby
database.url=jdbc:derby:/opt/mirth-connect/appdata/mirthdb;create=true
EOF

# Set ownership for mirth.properties
RUN set -eux; \
    chown mirth:mirth /opt/mirth-connect/conf/mirth.properties

# Expose ports
EXPOSE 8080 6661

# Switch to mirth user
USER mirth

# Start Mirth Connect
WORKDIR /opt/mirth-connect
CMD ["./mcserver"]