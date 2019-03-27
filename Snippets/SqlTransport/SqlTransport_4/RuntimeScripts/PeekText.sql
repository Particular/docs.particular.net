startcode PeekTextSql

SELECT count(*) Id
FROM (SELECT TOP {1} * FROM {0} WITH (READPAST)) as count_table;
endcode

startcode PeekTextSql411

SELECT count(*) Id
FROM {0} WITH (READPAST);
endcode
