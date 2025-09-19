# Use a specific, stable base image for Java 11
FROM eclipse-temurin:11.0.22_7-jdk-focal

# Argument for the Mirth Connect version, defaulting to 4.5.2
ARG MIRTH_VERSION=4.5.2.b363
ENV MIRTH_VERSION=${MIRTH_VERSION}

# Define primary and fallback download URLs
ARG PRIMARY_URL="https://s3.amazonaws.com/downloads.nextgen.com/connect/${MIRTH_VERSION}/mirthconnect-${MIRTH_VERSION}-unix.tar.gz"
ARG FALLBACK_URL="https://downloads.sourceforge.net/project/mirth-connect/Mirth%20Connect/${MIRTH_VERSION}/mirthconnect-${MIRTH_VERSION}-unix.tar.gz"

# Set working directory
WORKDIR /opt

# Download, verify, and install Mirth Connect
RUN apt-get update && apt-get install -y --no-install-recommends curl tar && \
    echo "Attempting to download Mirth Connect from Primary URL: ${PRIMARY_URL}" && \
    # Use curl with flags: fail fast (-f), show errors (-S), follow redirects (-L), retry 3 times
    # If the primary URL fails, the || operator triggers the fallback
    (curl -fsSL --retry 3 "${PRIMARY_URL}" -o mirthconnect.tar.gz || \
     (echo "Primary download failed. Attempting Fallback URL: ${FALLBACK_URL}" && \
      curl -fsSL --retry 3 "${FALLBACK_URL}" -o mirthconnect.tar.gz)) && \
    # Verify that the download was successful and the file is not empty
    if [ ! -s mirthconnect.tar.gz ]; then \
        echo "FATAL: Mirth Connect download failed from all sources."; \
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
# 8080: HTTP Admin
# 8443: HTTPS Admin
EXPOSE 8080 8443

WORKDIR /opt/mirth-connect

# The command to run Mirth Connect
CMD ["./mcservice", "run"]