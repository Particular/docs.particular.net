Transport, versions 6 and below, uses raw connection string provided into message headers. It could result in a leak of sensitive information, f.i. logging it.

In order to prevent it, transport, versions 7 and above, allows for creating a logical name for each connection string. The name is mapped to the physical connection string, and connection strings are always referred to by their logical name.   
In the event of an error or when logging only the logical name will be used avoiding sharing of sensitive information.