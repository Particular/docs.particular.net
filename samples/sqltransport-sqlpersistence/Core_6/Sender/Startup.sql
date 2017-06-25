if not exists (select  *
               from    sys.schemas
               where   name = N'sender' )
    exec('create schema sender');