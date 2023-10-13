# Simple Kubernetes Sample

## Steps

### Log into dockerhub

- `docker login --username=[docker_hub_id]`
- enter password at prompt

### Build subscriber and push to dockerhub

From the kubernetes-simple\Core_8 folder:

- `docker build -f DemoSubscriber/Dockerfile -t [docker_hub_id]/subscriber .`
- `docker push [docker_hub_id]/subscriber:latest`

### Build publisher and push to dockerhub

From the kubernetes-simple\Core_8 folder:

- `docker build -f DemoWebApi/Dockerfile -t [docker_hub_id]/webapi .`
- `docker push [docker_hub_id]/webapi:latest`

### Create persistent volumes and persistent volume claims

- `kubectl apply -f=host-pv.yaml`
- `kubectl apply -f=host-pvc.yaml`

### Create pods and services

From the kubernetes-simple\Core_8 folder:

- `kubectl apply -f=deployment.yaml`
- if need to delete use `kubectl delete -f=deployment.yaml`

### Start the publisher service

- `minikube service publisher-service`

A screen will appear showing the exposed service ports.
A browser window will open.

### Test application

- Add `/publish` to the service address and click enter

Should see a _message published_ response

### To view logs

- get the pod ids:
  - `kubectl get pods`
- get logs for a pod:
  - `kubectl logs --since=1h [pod_id]`
  
From the publisher pod should see:
_Publishing message!
Received response [message_id]_

From the subscriber pod should see:

_Received message [message_id]. Replying in 5 seconds_  
_Replying to request [message_id] to KubernetesDemo.Publisher_
