
Enviroments
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-6.0
https://docs.microsoft.com/en-us/visualstudio/debugger/project-settings-for-csharp-debug-configurations-dotnetcore?view=vs-2022
https://docs.microsoft.com/en-us/visualstudio/debugger/how-to-set-debug-and-release-configurations?view=vs-2022

Purpose of launchsettings.json


Helpers
https://github.blog/changelog/2020-07-06-github-actions-manual-triggers-with-workflow_dispatch/

Env variables in pipeline
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0#set-log-level-by-command-line-environment-variables-and-other-configuration


Linting
https://docs.microsoft.com/en-us/visualstudio/code-quality/roslyn-analyzers-overview?view=vs-2022
https://www.meziantou.net/enforce-dotnet-code-style-in-ci-with-dotnet-format.htm


Docker
https://github.com/dotnet/sdk/issues/10341


Non-root
https://github.com/dotnet/dotnet-docker/issues/2249
https://devblogs.microsoft.com/dotnet/dotnet-6-is-now-in-ubuntu-2204/


Healthcheck 
https://blog.couchbase.com/docker-health-check-keeping-containers-healthy/

docker-compose/vs
https://docs.microsoft.com/en-us/visualstudio/containers/docker-compose-properties?view=vs-2022#example

----

az container create --resource-group frasers-rg --file ./deploy/aci/deploy-aci.yaml


az container show --resource-group frasers-rg --name myContainerGroup --output table

rukayaok ~/Documents/Work/dotnet/Examples/SimpleWorkerService [main] $ az container show --resource-group frasers-rg --name myContainerGroup --output table
Name              ResourceGroup    Status    Image                                              IP:ports              Network    CPU/Memory       OsType    Location
----------------  ---------------  --------  -------------------------------------------------  --------------------  ---------  ---------------  --------  ----------
myContainerGroup  frasers-rg       Running   mcr.microsoft.com/azuredocs/aci-helloworld:latest  20.237.86.23:80,8080  Public     1.0 core/1.5 gb  Linux     eastus
rukayaok ~/Documents/Work/dotnet/Examples/SimpleWorkerService [main] $ curl -I 20.237.86.23:80
HTTP/1.1 200 OK
X-Powered-By: Express
Accept-Ranges: bytes
Cache-Control: public, max-age=0
Last-Modified: Wed, 29 Nov 2017 06:40:40 GMT
ETag: W/"67f-16006818640"
Content-Type: text/html; charset=UTF-8
Content-Length: 1663
Date: Wed, 31 Aug 2022 17:13:30 GMT
Connection: keep-alive


az container show -g frasers-rg -n anotherContainer
