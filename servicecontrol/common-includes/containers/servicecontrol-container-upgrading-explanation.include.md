An instance is upgraded by removing the container for the old version and replacing it with a container built using the new version. However, the container should be run in [setup mode](#initial-setup) each time it is upgraded before running it to ingest messages. For example:

```shell
docker stop {CONTAINER_NAME}
docker rm {CONTAINER_NAME}
docker pull {IMAGE}:latest
docker run --rm {OPTIONS} {IMAGE}:latest --setup
docker run -d {OPTIONS} {IMAGE}:latest
```

Note that Docker can cache the `latest` tag as well as the major/minor tags (such as `5` or `5.3`) unless the tag is pulled again. To be certain, use the full version tag.
