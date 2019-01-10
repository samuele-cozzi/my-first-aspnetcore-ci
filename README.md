# My First ASPNET Core on GITLAB

## Introduction
My first ASPNET Core project is a R&D with the goals to:
- create a sample api project with asp.net core 2.0 and Visual Studio Code
- build and run my api project on Docker
- create my first pipeline in GITLAB
- delivery continuously my docker image to Azure k8s cluster

## Ecosystem

| System | Documentation | |
| ------ | ------ | ------ |
| ASPNET CORE |[ASPNET CORE](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.1)|  |
| Visual Studio Code |[vscode](https://code.visualstudio.com/docs)| [link](https://code.visualstudio.com/) |
| Azure Cosmos DB |[Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/)| [link](https://azure.microsoft.com/en-us/services/cosmos-db/) |
| Azure Redis Cache |[Redis Cache](https://redis.io/documentation)| [link](https://azure.microsoft.com/en-us/services/cache/) |
| Gitlab (Innersource) |[Gitlab](https://about.gitlab.com/)| [link](https://innersource.soprasteria.com/) |
| Docker Hub | [Docker](https://www.docker.com/) | [link](https://hub.docker.com/) |
| Azure Kubernetes service | [Kubernetes](https://kubernetes.io/) | [link](https://azure.microsoft.com/en-us/services/kubernetes-service/) |



## Architecture

![Architecture Diagram](https://www.dropbox.com/s/hf6bvanktg1eqnw/MyFirstASPNETCore.png?raw=1 "Architecture Diagram")


## Installation
### Prerequisites
- Git
- DOTNET Core 2.1
- Docker
- Azure Subscription with services:
    - Cosmos DB
    - Redis Cache
    - Kubernetes (AKS)

### CLI

#### Clone Repository

```shell
$ git clone https://innersource.soprasteria.com/it-dotnet/my-first-aspnet-ci.git
$ cd my-first-aspnet-ci
$ code .
```

|Path|Description|
| ------ | ------ |
| /source/FooApi | Web Api project in ASPNET CORE |
| /source/FooApi.unittest| unit test of FooApi project |
| /source/dockerfile | Docker file of FooApi project and FooApi unit test project|
| /source/FooApi.e2eTest | end to ent test of FooApi Project |
| /source/FooApi.e2etest/dockerfile | Docker file of FooApi .e2etest project |
| .gitlab-ci.yml | Configuration file of gitlab pipeline |
| k8s-stage-deploy.yml | Configuration file of kubernetes deploy on stage environment |
| k8s-prod-deploy.yml | Configuration file of kubernetes deploy on prod environment |

#### Build & Debug Solution

```shell
$ dotnet restore source
$ dotnet build source
```

Open Visual Studio Code in Debug tab, select FooApi project and run debug. The browser opens, then navigate to http://localhost:5000/api/values


#### Build and Run Docker

```shell
$ docker build -t my-first-dotnetcore-web .
$ docker run -it -p 5000:80 my-first-dotnetcore-web
```


### Gitlab Continuos Delivery

![Continuos delivery pipeline](https://www.dropbox.com/s/bn7czrtb8w2fjoy/PipelineArchitecture.PNG?raw=1)

- [Stage Environment Swagger](http://104.40.241.52/swagger)
- [Prod Environment Swagger](http://40.118.126.110/swagger)

## References
- [ASPNET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.1)
- [DOTNET Core + Docker](https://docs.docker.com/engine/examples/dotnetcore/#build-and-run-the-docker-image)
- [DOTNET Core + Gitlab](https://devblog.dymel.pl/2017/07/11/aspnetcore-docker-gitlab/)
- [Gitlab Environment + Azure](https://stackoverflow.com/questions/50749095/how-to-integrate-gitlab-ci-w-azure-kubernetes-kubectl-acr-for-deployments)
- [Kubernetes](https://thorsten-hans.com/how-to-use-a-private-azure-container-registry-with-kubernetes-9b86e67b93b6)
- [Kubernetes](https://kubernetes.io/docs/concepts/workloads/controllers/deployment/)


## License
[MIT License](https://innersource.soprasteria.com/it-dotnet/my-first-aspnet-ci/blob/master/LICENSE)