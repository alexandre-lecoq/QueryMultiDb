
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

PRINT 'Print 1';

SELECT *
FROM FakeTableDoesNotExist;

PRINT 'Print 2';
