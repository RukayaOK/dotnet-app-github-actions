project_folder = SimpleWorkerService
container_name = simpleworkerservice
image_name = frasersacr.azurecr.io/te/docker/$(container_name)


.PHONY: help
help:			## Displays the help.
	@fgrep -h "##" $(MAKEFILE_LIST) | fgrep -v fgrep | sed -e 's/\\$$//' | sed -e 's/##//'

.PHONY: build
build:			## Builds the dockerised app without cache.
	@docker-compose -f ./deploy/docker-compose/docker-compose.yml build --no-cache

.PHONY: start
start:			## Starts the dockerised app.
	@docker-compose -f ./deploy/docker-compose/docker-compose.yml up -d

.PHONY: stop
stop:			## Stops the dockerised app.
	@docker-compose -f ./deploy/docker-compose/docker-compose.yml down

.PHONY: restart
restart:		## Removes current dockerised app and starts again
	make stop && make clean-images && make start

.PHONY: status
status:			## Status of the dockerised app.
	docker ps 

.PHONY: logs
logs:			## Tails the docker logs
	docker logs $(container_name) -f

.PHONY: interactive
interactive:		## Start bash shell in the dockerised app
	@docker exec -it $(container_name) /bin/bash

.PHONY: clean-images
clean-images:		## Removes the docker images. [requires TAG=${value}]
	docker rmi $(container_name) -f
	docker rmi $(container_name).tests -f

.PHONY: build-remote
build-remote:	        ## Builds the docker images for remote repository [requires TAG=${value}]
	docker build . --tag $(container_name):$(TAG) -f build/Dockerfile 

.PHONY: push-remote
push-remote:		## Builds and pushes the docker images to a remote repository [requires TAG=${value}]
	docker build . --tag $(image_name):$(TAG) -f build/Dockerfile 
	docker push $(image_name):$(TAG)

.PHONY: tests
tests:			## Runs the application tests
	docker-compose -f ./deploy/docker-compose/docker-compose.yml up $(container_name).tests

.PHONY: health
health:			## Inspect health output
	docker inspect --format='{{json .State.Health}}' $(container_name) | jq
	
.PHONY: format
format:		 	## Formats code. No restore as https://github.com/dotnet/format/issues/1560
	dotnet format --no-restore --verbosity diagnostic $(project_folder)

.PHONY: clean
clean-binary:		## Removes the binaries.
	dotnet clean $(project_folder)

.PHONY: scan
scan:			## Runs checkov scan on the dockerfile
	checkov -d build --framework dockerfile
