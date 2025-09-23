FROM eclipse-temurin:11-jre-focal

# Install dependencies, including the full JDK for keytool
RUN apt-get update && apt-get install -y curl tar unzip procps openjdk-11-jdk-headless \
    && rm -rf /var/lib/apt/lists/*

# Download and extract Mirth Connect
RUN curl -L -o /tmp/mirthconnect.tar.gz https://s3.amazonaws.com/downloads.mirthcorp.com/connect/4.5.2.b363/mirthconnect-4.5.2.b363-unix.tar.gz \
    && mkdir -p /opt/mirth-connect \
    && tar -xzf /tmp/mirthconnect.tar.gz -C /opt/mirth-connect --strip-components=1 \
    && rm /tmp/mirthconnect.tar.gz

# Create mirth user and permissions
RUN useradd -ms /bin/bash mirth \
    && chown -R mirth:mirth /opt/mirth-connect \
    && chmod +x /opt/mirth-connect/mcservice /opt/mirth-connect/mcserver

# Derby database directory
RUN mkdir -p /opt/mirth-connect/appdata \
    && chown mirth:mirth /opt/mirth-connect/appdata \
    && chmod 700 /opt/mirth-connect/appdata

# Generate a dummy keystore to prevent the NullPointerException on startup
RUN keytool -genkey -alias mirth -keyalg RSA -keystore /opt/mirth-connect/conf/keystore.jks \
    -storepass mirthdb -keypass mirthdb -dname "CN=localhost, OU=IT, O=Mirth, L=London, S=London, C=UK" \
    && chown mirth:mirth /opt/mirth-connect/conf/keystore.jks

# Configure Mirth, pointing to the dummy keystore but keeping HTTPS disabled
RUN echo "http.port=8080" > /opt/mirth-connect/conf/mirth.properties \
    && echo "https.port=0" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "keystore.path=conf/keystore.jks" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "keystore.password=mirthdb" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "keystore.keypassword=mirthdb" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "database=derby" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "database.url=jdbc:derby:/opt/mirth-connect/appdata/mirthdb;create=true" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "log4j2.rootLogger=INFO, stdout" >> /opt/mirth-connect/conf/mirth.properties \
    && chown mirth:mirth /opt/mirth-connect/conf/mirth.properties \
    && chmod 600 /opt/mirth-connect/conf/mirth.properties

# Run as mirth user
USER mirth
WORKDIR /opt/mirth-connect
CMD ["./mcserver"]

EXPOSE 8080 6661