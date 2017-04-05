startcode Oracle_SubscriptionSubscribeSql

begin
    insert into EndpointNameSS
    (
        MessageType,
        Subscriber,
        Endpoint,
        PersistenceVersion
    )
    values
    (
        :MessageType,
        :Subscriber,
        :Endpoint,
        :PersistenceVersion
    );
    commit;
exception
    when DUP_VAL_ON_INDEX
    then ROLLBACK;
end;

endcode
