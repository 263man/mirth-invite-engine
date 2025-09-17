# Use a specific, known-good Java 11 base image
FROM eclipse-temurin:11.0.16_8-jre-focal

# Define the Mirth Connect version to build - UPDATED to a valid, downloadable version
ARG MIRTH_VERSION=4.5.2.b363
ARG ARTIFACT_URL=https://s3.amazonaws.com/downloads.mirthcorp.com/connect/${MIRTH_VERSION}/mirthconnect-${MIRTH_VERSION}-unix.tar.gz

ENV MIRTH_HOME=/opt/mirth-connect

# Install required utilities
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl ca-certificates && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /tmp

# Download and extract the official Mirth Connect tarball
RUN echo "Downloading from ${ARTIFACT_URL}" && \
    curl -fSL "${ARTIFACT_URL}" -o /tmp/mirth.tar.gz && \
    tar -xzf /tmp/mirth.tar.gz -C /tmp && \
    mv "/tmp/Mirth Connect" "${MIRTH_HOME}" && \
    rm -f /tmp/mirth.tar.gz

# Create a dedicated user for Mirth and set permissions
RUN groupadd -r mirth && useradd --no-log-init -r -g mirth -d ${MIRTH_HOME} mirth && \
    mkdir -p ${MIRTH_HOME}/appdata && \
    chown -R mirth:mirth ${MIRTH_HOME}

# Expose the necessary ports
EXPOSE 8443 6661

USER mirth
WORKDIR ${MIRTH_HOME}

# Define the command to start the server
CMD ["./mcserver"]