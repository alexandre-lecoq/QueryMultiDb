PRINT 'Point 1';
PRINT 'Point 1 bis';

SELECT COUNT(1) AS  [RowCount]
FROM TestTableOne;

PRINT 'Point 2';

SELECT TOP 30 CreationDate, ModificationDate
FROM TestTableOne;

PRINT 'Point 3';
