# Use a specific, stable base image for Java 11
FROM eclipse-temurin:11.0.22_7-jdk-focal

# Argument for the Mirth Connect version
ARG MIRTH_VERSION=4.5.2
ENV MIRTH_VERSION=${MIRTH_VERSION}

# Define the canonical download URL for the OFFICIAL UNIX INSTALLER SCRIPT
ARG DOWNLOAD_URL="https://github.com/nextgenhealthcare/connect/releases/download/${MIRTH_VERSION}/mirthconnect-${MIRTH_VERSION}-unix.sh"

# Set a target directory for the installation
ENV MIRTH_HOME /opt/mirth-connect

# Install dependencies, download, install Mirth, and clean up
RUN apt-get update && apt-get install -y --no-install-recommends curl && \
    cd /tmp && \
    echo "Attempting to download Mirth Connect installer from ${DOWNLOAD_URL}" && \
    curl -fSL --retry 3 "${DOWNLOAD_URL}" -o mirthconnect-installer.sh && \
    if [ ! -s mirthconnect-installer.sh ]; then \
        echo "FATAL: Mirth Connect installer download failed."; \
        exit 1; \
    fi && \
    echo "Download successful. Running installer..." && \
    chmod +x mirthconnect-installer.sh && \
    # The installer prompts for a path; we pipe the target path to it for a silent install.
    echo "${MIRTH_HOME}" | ./mirthconnect-installer.sh && \
    echo "Installation complete. Cleaning up..." && \
    cd / && rm -rf /tmp/* && \
    apt-get purge -y --auto-remove curl && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Expose Mirth Connect ports
EXPOSE 8080 8443

WORKDIR ${MIRTH_HOME}

# The command to run Mirth Connect
CMD ["./mcservice", "run"]