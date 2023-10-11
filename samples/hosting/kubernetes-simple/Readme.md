TBA - Jo's Notes

From kubernetes-simple\Core_8:

docker build -f DemoSubscriber/Dockerfile -t joasiapalac/subscriber .
# docker run --rm --name subscriber subscriber
# docker tag subscriber joasiapalac/subscriber
docker push joasiapalac/subscriber:latest

docker build -f DemoWebApi/Dockerfile -t joasiapalac/webapi .
# docker run -p 3000:8080 -p 3001:8081 --rm --name webapi webapi
# docker tag webapi joasiapalac/webapi
docker push joasiapalac/webapi:latest

kubectl apply -f=host-pv.yaml
kubectl apply -f=host-pvc.yaml

kubectl apply -f=deployment.yaml
# kubectl delete -f=deployment.yaml

kubectl logs --since=1h subscriber-deployment-747dfb454-fjmnb
kubectl logs --since=1h publisher-deployment-747dfb454-rkqv4