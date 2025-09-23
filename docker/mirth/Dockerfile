# docker/mirth/Dockerfile
# Build a Mirth Connect image (automated, non-interactive) suitable for Codespaces.
FROM eclipse-temurin:11-jre-focal

# Use a verified, automatable Mirth release (v4.5.2 for bug fixes)
ENV MIRTH_VERSION=4.5.2.b363
ENV MIRTH_URL=https://s3.us-west-1.amazonaws.com/downloads.mirthcorp.com/connect/4.5.2.b363/mirthconnect-4.5.2.b363-unix.tar.gz

# Work dir
WORKDIR /opt

# Install tools, download tar.gz, unpack, create user, set ownership, and create mirth.properties
RUN apt-get update && apt-get install -y --no-install-recommends curl tar ca-certificates \
    && echo "Downloading Mirth Connect ${MIRTH_VERSION} from ${MIRTH_URL}" \
    && curl -fsSL "${MIRTH_URL}" -o /tmp/mirth.tar.gz \
    && tar -xzf /tmp/mirth.tar.gz -C /opt \
    && rm /tmp/mirth.tar.gz \
    && mv "/opt/Mirth Connect" /opt/mirth-connect || true \
    \
    # Create a mirth user and ensure permissions so migrations run as intended
    && groupadd -r mirth || true \
    && useradd --no-log-init -r -g mirth -d /opt/mirth-connect mirth || true \
    && mkdir -p /opt/mirth-connect/appdata /opt/mirth-connect/conf \
    && chown -R mirth:mirth /opt/mirth-connect \
    \
    # Write a minimal, valid mirth.properties (key=value format – no spaces around "=")
    && printf 'http.port=8080\nhttps.port=0\nkeystore.storetype=JKS\nkeystore.path=conf/keystore.jks\nkeystore.password=changeit\ndatabase=derby\ndatabase.url=jdbc:derby:/opt/mirth-connect/appdata/mirthdb;create=true\n' > /opt/mirth-connect/conf/mirth.properties \
    && chown mirth:mirth /opt/mirth-connect/conf/mirth.properties \
    \
    # cleanup
    && apt-get purge -y --auto-remove curl tar \
    && rm -rf /var/lib/apt/lists/*

# Set environment variables
ENV MIRTH_HOME=/opt/mirth-connect

# Expose HTTP (admin REST), and MLLP port
EXPOSE 8080 6661

# Ensure we run as the mirth user
USER mirth

WORKDIR /opt/mirth-connect

# Start Mirth (mcserver will run in foreground; mcservice may be present but mcserver is standard)
CMD ["./mcserver"]