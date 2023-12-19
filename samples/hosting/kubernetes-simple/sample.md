---
title: Simple Kubernetes Sample
summary: Demonstrates how to host NServiceBus inside Kubernetes
reviewed: 2023-12-18
component: Core
---

This sample demonstrates a simple deployment of NServiceBus endpoints inside Kubernetes. The goal of the sample is to demonstrate hosting of endpoints inside Kubernetes in the most self-contained way possible, and therefore makes a few tradeoffs like using the learning transport and persistence to avoid having to pull in additional images from public registries.

## Prerequisites

In order to run the sample it is necessary to have Kubernetes locally installed by either using [Minikube](https://minikube.sigs.k8s.io/docs/) or [Microk8s](https://microk8s.io). For the version with the local registry, Microk8s has been proven to work in the most straightforward way on Linux.

When running on [Windows with Docker, the built-in Kubernetes cluster support](https://docs.docker.com/desktop/kubernetes/) in combination with a public [docker registry](https://hub.docker.com/) works best.

## Steps to run using .Net built-in container support & Microk8s

### Setup up Microk8s

1. [Install Microk8s](https://microk8s.io/docs/getting-started)
1. [Enable the built-in registry](https://microk8s.io/docs/registry-built-in)

The rest of this sample assumes the local registry runs on `localhost:32000`

### Build the containers

Both in the `DemoPublisher` and the `DemoSubscribe` directory execute the following command

```bash
dotnet publish -c Release /t:PublishContainer -p ContainerRegistry=localhost:32000
```

### Prepare the volume

```bash
kubectl apply -f host-pvc.yaml -f host-pv.yaml
```

### Prepare the deployment

By leveraging envsubst it is possible to substitute the `$REGISTRY` variable in the deployment file with the corresponding local registry name.

```bash
export REGISTRY="localhost:32000"
envsubst < Deployment.yaml | kubectl apply -f -
```

Alternatively replace the variable directly in the file.

## Steps to run using Dockerfile & Minikube

### Setup Docker Hub public registry and Minikube

- Setup [Docker Desktop](https://docs.docker.com/desktop/)
- Create [public Docker Hub repositories](https://docs.docker.com/docker-hub/quickstart/) for the publisher and subscriber endpoints
- Turn on [Kubernetes on Docker Desktop](https://docs.docker.com/desktop/kubernetes/)

>Note: Replace `[REGISTRY]` in all snippets below with Docker repository name.

### Log into Docker Hub

```cmd
docker login --username=[REGISTRY]
```

Enter password at prompt - typed characters will not be visibe

### Build subscriber and push to Docker Hub

Run these from the kubernetes-simple\Core_8 folder.

To build the subscriber image:

```cmd
docker build -f DemoSubscriber/Dockerfile -t [REGISTRY]/subscriber .
```

To push the subscriber image to Docker Hub

```cmd
docker push [REGISTRY]/subscriber:latest`
```

### Build publisher and push to Docker Hub

Run these from the kubernetes-simple\Core_8 folder.

To build the publisher image:

```cmd
docker build -f DemoPublisher/Dockerfile -t [REGISTRY]/publisher .
```

To push the publisher image to Docker Hub

```cmd
docker push [REGISTRY]/publisher:latest
```

### Use Minikube to setup Kubernetes

#### Create persistent volumes and persistent volume claims

```cmd
kubectl apply -f=host-pv.yaml -f=host-pvc.yaml
```

#### Create pods and services

In the `deployment.yaml` file, replace `$REGISTRY` with Docker Hub repository name.

Run these from the kubernetes-simple\Core_8 folder.

To apply the deployment:

```cmd
kubectl apply -f=deployment.yaml
```

To delete the deployment:

```cmd
kubectl delete -f=deployment.yaml
```

## Inspect the sample running

Get the pod ids:

```cmd
kubectl get pods
```

prints something like

```text
NAME                                     READY   STATUS    RESTARTS   AGE
publisher-deployment-554656c667-fppxp    1/1     Running   0          25m
subscriber-deployment-7dd5cf8c69-zs7db   1/1     Running   0          25m
```

Use the pod id to view its logs.

To follow the stream of logs:

```cmd
kubectl logs --follow [pod_id]
```

To view the logs for the past hour:

```cmd
kubectl logs --since=1h [pod_id]
```

From the publisher pod should see something like this:

```text
Publishing event [message_id]
Received response [message_id]
```

From the subscriber pod should see something like this:

```text
Received message [message_id]. Replying in 5 seconds
Replying to request [message_id] to KubernetesDemo.Publisher
```