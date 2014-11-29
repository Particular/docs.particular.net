---
title: NHibernate saga mapping
summary: TODO
tags:
- NHibernate
---

If you setup NServiceBus to use NHibernate-based persistance it will provide some default convetion to map your sagas classes to database tables. 
Additionally it will automatically create necessery database objects when ???

## Customize saga mapping

NHibernate-based persistence provide a few attributes that allow you to customize schema which will be generated:

* `TableName` - allow you to provide custom table name of your class.

	[TableName("customer_order_saga")]
	public class CustomerOrderSaga : ContainSagaData
	{
    }

* `LockMode` - this attribute describe which level of locking is used when saga data is read from database.
* `RowVersion` - this attribute is used to specify which property describe saga version. 
You can learn more about this in [NServiceBus Sagas And Concurrency](http://docs.particular.net/nservicebus/nservicebus-sagas-and-concurrency)

If that is not engough you can also map saga classes over HBM or FluentNHibernate. 
TODO: Describe how map saga over HBM or FluentNHibernate

## Disable schema update

In some cases you want to have control over creating the sagas schema.
For that reason NHibernate-based persistance provide method `DisableSchemaUpdate`.

TODO: Describe how to get SQL script for creating and updating saga schema.