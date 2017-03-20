---
title: Service Fabric Persistence Sagas
reviewed: 2017-03-20
component: ServiceFabricPersistence
---

TODO: describe saga persister [SUBJECT TO CHANGE, need to decide if a signle or multiple collections]


## Reliable collections

When using the Service Fabric Persistence with a reliable service, saga data is stored using a reliable dictionary called `sagas`.  


## Configuration

Things to list:

SagaSettings().JsonSettings().WriterCreator()
SagaSettings().JsonSettings().ReaderCreator()
