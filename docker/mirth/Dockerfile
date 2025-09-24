# Stage 1: The "First Run" stage to generate a valid configuration
FROM eclipse-temurin:11-jre-focal AS config_generator

RUN apt-get update && apt-get install -y curl tar unzip procps openjdk-11-jdk-headless \
    && rm -rf /var/lib/apt/lists/*

RUN curl -L -o /tmp/mirthconnect.tar.gz https://s3.amazonaws.com/downloads.mirthcorp.com/connect/4.5.2.b363/mirthconnect-4.5.2.b363-unix.tar.gz \
    && mkdir -p /opt/mirth-connect \
    && tar -xzf /tmp/mirthconnect.tar.gz -C /opt/mirth-connect --strip-components=1 \
    && rm /tmp/mirthconnect.tar.gz

# Run Mirth to generate config, then find and kill the Java process to stop it cleanly.
RUN cd /opt/mirth-connect && ./mcserver & \
    echo "Waiting for Mirth to generate initial config..." && \
    sleep 30 && \
    echo "Stopping Mirth..." && \
    pkill -f 'mirth-server.jar' && \
    sleep 5 # Allow time for a clean shutdown

# Stage 2: The final, clean production image
FROM eclipse-temurin:11-jre-focal

# Only install necessary runtime dependencies
RUN apt-get update && apt-get install -y procps \
    && rm -rf /var/lib/apt/lists/*

# Copy the application files AND the known-good configuration from the first stage
COPY --from=config_generator /opt/mirth-connect /opt/mirth-connect

# Create mirth user and set permissions
RUN useradd -ms /bin/bash mirth \
    && chown -R mirth:mirth /opt/mirth-connect

# Overwrite mirth.properties with a complete, known-good configuration
RUN echo "http.port = 8080" > /opt/mirth-connect/conf/mirth.properties \
    && echo "https.port = 0" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "database = derby" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "database.driver = org.apache.derby.jdbc.EmbeddedDriver" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "database.url = jdbc:derby:/opt/mirth-connect/appdata/mirthdb;create=true" >> /opt/mirth-connect/conf/mirth.properties \
    && chown mirth:mirth /opt/mirth-connect/conf/mirth.properties

# Run as mirth user
USER mirth
WORKDIR /opt/mirth-connect
CMD ["./mcserver"]

EXPOSE 8080 6661