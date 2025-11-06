#FROM quay.io/pypa/manylinux_2_34_x86_64
#FROM mcr.microsoft.com/devcontainers/base:bookworm
#FROM mcr.microsoft.com/devcontainers/universal:2-focal
FROM mcr.microsoft.com/devcontainers/dotnet:9.0
#when testing and retrying dev container building, restart docker: sudo systemctl restart docker

WORKDIR /workspaces

# pgadmin4 repository setup
RUN curl -fsS https://www.pgadmin.org/static/packages_pgadmin_org.pub | sudo gpg --dearmor -o /usr/share/keyrings/packages-pgadmin-org.gpg && \
    sh -c 'echo "deb [signed-by=/usr/share/keyrings/packages-pgadmin-org.gpg] https://ftp.postgresql.org/pub/pgadmin/pgadmin4/apt/$(lsb_release -cs) pgadmin4 main" > /etc/apt/sources.list.d/pgadmin4.list && apt update' && \
    apt-get -y update

RUN curl -fsSL https://aka.ms/install-azd.sh | bash

RUN apt-get -y install pgadmin4


# Default command: Start bash to interact with the container
CMD ["/bin/bash"]