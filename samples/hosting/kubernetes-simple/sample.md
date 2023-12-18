---
title: Simple Kubernetes Sample
summary: Demonstrates how to host NServiceBus inside Kubernetes
reviewed: 2023-12-18
component: Core
---

This sample demonstrates a simple deployment of NServiceBus endpoints inside Kubernetes. The goal of the sample is demonstrate hosting of endpoints inside Kubernetes in the most self-contained way possible and therefore makes a few tradeoffs like using the learning transport and persistence to avoid having to pull in additional images from public registries.

## Prerequisites

In order to run the sample it is necessary to have Kubernetes locally installed by either using [Minikube](https://minikube.sigs.k8s.io/docs/) or [Microk8s](https://microk8s.io). For the version with the local registry Microk8s has been proven to work in the most straightforward way on Linux.

When running on [Windows with Docker the built-in Kubernetes cluster support](https://docs.docker.com/desktop/kubernetes/) in combination with a public docker registry works best.

## Steps to run using .Net built-in container support & Microk8s

### Setting up Microk8s

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

### Inspect the sample running

```bash
kubectl get pods
```

prints something like

```text
NAME                                     READY   STATUS    RESTARTS   AGE
publisher-deployment-554656c667-fppxp    1/1     Running   0          25m
subscriber-deployment-7dd5cf8c69-zs7db   1/1     Running   0          25m
```

use `kubectl get logs --follow` to stream the logs from Kubernetes. For example to get the logs of the subscriber use

```bash
kubectl get logs --follow subscriber-deployment-7dd5cf8c69-zs7db
```

## Steps to run using Dockerfile & Minikube

### Log into dockerhub

- `docker login --username=[docker_hub_id]`
- enter password at prompt

### Build subscriber and push to dockerhub

From the kubernetes-simple\Core_8 folder:

- `docker build -f DemoSubscriber/Dockerfile -t [docker_hub_id]/subscriber .`
- `docker push [docker_hub_id]/subscriber:latest`

### Build publisher and push to dockerhub

From the kubernetes-simple\Core_8 folder:

- `docker build -f DemoPublisher/Dockerfile -t [docker_hub_id]/publisher .`
- `docker push [docker_hub_id]/publisher:latest`

### User Minikube to setup Kubernetes

#### Create persistent volumes and persistent volume claims

- `kubectl apply -f=host-pv.yaml`
- `kubectl apply -f=host-pvc.yaml`

#### Create pods and services

Set the registry to the docker_hub_id:

`export REGISTRY=docker_hub_id`

From the kubernetes-simple\Core_8 folder:

- `kubectl apply -f=deployment.yaml`
- if need to delete use `kubectl delete -f=deployment.yaml`

### To view logs

- get the pod ids:
  - `kubectl get pods`
- get logs for a pod:
  - `kubectl logs --since=1h [pod_id]`

From the publisher pod should see:

_Publishing event [message_id]
Received response [message_id]_

From the subscriber pod should see:

_Received message [message_id]. Replying in 5 seconds_
_Replying to request [message_id] to KubernetesDemo.Publisher_