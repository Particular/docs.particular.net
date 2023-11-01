startcode PeekTextSql

SELECT isnull(cast(max([RowVersion]) - min([RowVersion]) + 1 AS int), 0) Id FROM {0} WITH (nolock)
endcode
