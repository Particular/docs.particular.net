startcode Oracle_SubscriptionSubscribeSql

begin
    insert into "ENDPOINTNAMESS"
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
    when DUP_VAL_ON_INDEX then
    if :Endpoint is not null then
        update "ENDPOINTNAMESS" set
            Endpoint = :Endpoint,
            PersistenceVersion = :PersistenceVersion
        where 
            MessageType = :MessageType
            and Subscriber = :Subscriber;
    else
        ROLLBACK;
    end if;
end;

endcode
