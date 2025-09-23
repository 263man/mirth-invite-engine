FROM eclipse-temurin:11-jre-focal

# Install dependencies
RUN apt-get update && apt-get install -y curl tar && rm -rf /var/lib/apt/lists/*

# Download and extract Mirth Connect
RUN curl -L -o /tmp/mirthconnect.tar.gz https://s3.amazonaws.com/downloads.mirthcorp.com/connect/4.5.2.b363/mirthconnect-4.5.2.b363-unix.tar.gz \
    && mkdir -p /opt/mirth-connect \
    && tar -xzf /tmp/mirthconnect.tar.gz -C /opt/mirth-connect --strip-components=1 \
    && rm /tmp/mirthconnect.tar.gz

# Create mirth user and set permissions
RUN useradd -ms /bin/bash mirth \
    && chown -R mirth:mirth /opt/mirth-connect \
    && chmod +x /opt/mirth-connect/mcservice /opt/mirth-connect/mcserver

# Generate keystore in temporary location with explicit permissions
RUN keytool -genkeypair -alias mirth -keyalg RSA -keysize 2048 -validity 365 -storetype JKS \
    -keystore /tmp/keystore.jks \
    -dname "CN=Mirth, OU=Unknown, O=Unknown, L=Unknown, ST=Unknown, C=Unknown" \
    -storepass changeit -keypass changeit -noprompt \
    && echo "Keystore generated in /tmp. Verifying before chmod:" \
    && ls -l /tmp/keystore.jks \
    && chown mirth:mirth /tmp/keystore.jks \
    && chmod 600 /tmp/keystore.jks \
    && echo "Keystore permissions set in /tmp. Verifying:" \
    && ls -l /tmp/keystore.jks \
    && mv /tmp/keystore.jks /opt/mirth-connect/conf/keystore.jks \
    && echo "Keystore moved to /opt/mirth-connect/conf. Verifying final permissions:" \
    && ls -l /opt/mirth-connect/conf/keystore.jks

# Configure mirth.properties with debug logging and Derby database
RUN echo "http.port=8080" > /opt/mirth-connect/conf/mirth.properties \
    && echo "https.port=0" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "keystore.storetype=JKS" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "keystore.path=conf/keystore.jks" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "keystore.password=changeit" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "log4j.rootLogger=DEBUG,console" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "database=derby" >> /opt/mirth-connect/conf/mirth.properties \
    && echo "database.url=jdbc:derby:/opt/mirth-connect/appdata/mirthdb;create=true" >> /opt/mirth-connect/conf/mirth.properties \
    && chown mirth:mirth /opt/mirth-connect/conf/mirth.properties \
    && chmod 600 /opt/mirth-connect/conf/mirth.properties \
    && echo "mirth.properties configured. Verifying permissions:" \
    && ls -l /opt/mirth-connect/conf/mirth.properties

# Final permissions check
RUN echo "Final permissions check for all files:" \
    && chmod 600 /opt/mirth-connect/conf/keystore.jks \
    && ls -l /opt/mirth-connect/conf/keystore.jks \
    && ls -l /opt/mirth-connect/conf/mirth.properties

# Run as mirth user
USER mirth
WORKDIR /opt/mirth-connect
CMD ["./mcservice", "run"]

EXPOSE 8080 6661