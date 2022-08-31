[< back to README.md](../README.md)

# Build and Run this Application

## Configuration

Each project should have an appsettings.json and an appsettings.development.json where: 
- appsettings.json represents your production configuration
- appsettings.development.json represents your local configuration

If you have any additional environments these should be stored under appsettings.<environment>.json 


The keys for all your configuration should be stored in appsettings.json

Depending on features on this configuration the value of these keys should be stored

- If the 
- If the data is sensitive then 


appsettings.json - environment variables shared across all envs
appsettings.dev.json - override appsettings.json with local/dev env variables
- update launchsettings.json to follow this  

To Store Secrets 
1. Right-Click < Use Secret Manager < 
{
    "SecretValue"
}

2. This overrides appsettings.<env>.json

---
## Dependencies

---
## Tests


---
## Build and Run this application

</br>

### Fill out the Configuration 

When running this environment you will 


### Run this Application

1. Build the docker image 
    ```
    docker-compose build
    ```

2. Run the docker container
    ```
    docker-compose up -d 
    ```

3. Get the container ID 
    ```
    docker ps 
    ```

4. View the container logs 
    ```
    docker logs <container-id> -f
    ```

5. Attach to container
    ```
    docker exec -it <container-id> /bin/bash 
    ```

</br>

### Stop this Application 


1. Get the container ID 
    ```
    docker ps 
    ```

2. Stop the container 
    ```
    docker stop <container-id>
    ```

3. Delete the stopped container 
    ```
    docker container prune <container-id> -f 
    ```

</br>

### Rebuild this Application 

1. Remove the current containers (as per the above)

2. Build the docker image 
    ```
    docker-compose build
    ```

# Healthcheck 
docker inspect --format='{{json .State.Health}}' simpleworkerservice


dcproj

<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" Sdk="Microsoft.Docker.Sdk" DefaultTargets="Build">
  <PropertyGroup Label="Globals">
    <ProjectVersion>2.1</ProjectVersion>
    <DockerTargetOS>Linux</DockerTargetOS>
    <ProjectGuid>{50AD9D39-C9D5-4423-BB98-35FBD697B9FC}</ProjectGuid>
    <DockerComposeBaseFilePath>deploy/docker-compose/docker-compose</DockerComposeBaseFilePath>
  </PropertyGroup>
  <ItemGroup>
    <None Include="deploy\docker-compose\docker-compose.override.yml">
      <DependentUpon>deploy\docker-compose\docker-compose.yml</DependentUpon>
    </None>
    <None Include="deploy\docker-compose\docker-compose.yml" />
    <None Include="deploy\docker-compose\.dockerignore" />
  </ItemGroup>
</Project>
