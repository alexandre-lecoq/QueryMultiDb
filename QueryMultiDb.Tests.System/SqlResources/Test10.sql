SET STATISTICS TIME ON;

PRINT 'start';

SELECT @@version;

SELECT @@spid AS SPID;

SELECT @@servicename AS ServiceName;

SELECT @@servername AS HostName;

SELECT @@MAX_CONNECTIONS AS MaxConnections;

SELECT @@DBTS AS DatabaseTimestamp;

PRINT 'stop';
