# dotnet versions
ARG DOTNET_SDK_VERSION=6.0
ARG DOTNET_RUNTIME_VERSION=6.0

##################################
# BASE IMAGE -- image for service
##################################
FROM mcr.microsoft.com/dotnet/runtime:$DOTNET_RUNTIME_VERSION as base 

RUN apt-get update && \
    apt-get install lsof -y 

# non-root user details 
ARG USERNAME=builduser
ARG USER_UID=999
ARG USER_GID=$USER_UID

# add dependencies 
RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME \
    && apt-get install -y sudo \
    && echo $USERNAME ALL=\(root\) NOPASSWD:/usr/bin/dotnet > /etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME


###################################
# BUILD IMAGE -- image for service
###################################
FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_SDK_VERSION as build 

# add dependencies 
RUN apt-get update && \
    apt-get install lsof -y 

# non-root user details 
ARG USERNAME=builduser
ARG USER_UID=1000
ARG USER_GID=$USER_UID

# Create the user
RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME \
    && apt-get update \
    && apt-get install telnet -y \
    && apt-get install -y sudo \
    && echo $USERNAME ALL=\(root\) NOPASSWD:/usr/bin/dotnet > /etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME
    

#################################################
# BUILD STAGE -- build libraries and application 
#################################################
FROM build as buildservice 

WORKDIR /app

COPY ["../SimpleWorkerService/SimpleWorkerService.csproj", "SimpleWorkerService/"]
COPY ["../CustomHealthProbe/CustomHealthProbe.csproj", "CustomHealthProbe/"]
RUN dotnet restore "SimpleWorkerService/SimpleWorkerService.csproj"

# copy and build app and libraries
COPY ["../SimpleWorkerService", "SimpleWorkerService/"]
COPY ["../CustomHealthProbe", "CustomHealthProbe/"]
WORKDIR /app/SimpleWorkerService
RUN dotnet build "SimpleWorkerService.csproj" -c Release -o /app/build


#################################################################
# TEST STAGE -- optional entrypoint: docker build --target tests
#################################################################
FROM buildservice AS runtests

WORKDIR /app

COPY ["../SimpleWorkerService.Tests/SimpleWorkerService.Tests.csproj", "SimpleWorkerService.Tests/"]
RUN dotnet restore "SimpleWorkerService.Tests/SimpleWorkerService.Tests.csproj"
COPY ["../SimpleWorkerService.Tests", "SimpleWorkerService.Tests/"]
WORKDIR /app/SimpleWorkerService.Tests
RUN dotnet build --no-restore "SimpleWorkerService.Tests.csproj" 

USER $USERNAME
ENTRYPOINT ["sudo", "dotnet", "test", "--logger:trx", "--no-restore", "--no-build"]


#############################################
# PUBLISH STAGE -- publish app and libraries
#############################################
FROM buildservice AS publish
RUN dotnet publish "SimpleWorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false


##############
# FINAL IMAGE
##############
FROM base as final

# copy published application 
WORKDIR /app
COPY --from=publish /app/publish .

USER $USERNAME

EXPOSE 5009
HEALTHCHECK --interval=10s --timeout=30s --start-period=3s --retries=3 CMD ["/usr/bin/lsof", "-i", ":5009"]

# run application
ENTRYPOINT ["dotnet", "SimpleWorkerService.dll"]



