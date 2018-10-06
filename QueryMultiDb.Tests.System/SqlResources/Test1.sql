SET STATISTICS TIME ON;

PRINT 'start';

SELECT Id, ZipCode, VeryLongText, SmallBinary
FROM TestTableOne;

SELECT Age, Boolean, LongText
FROM TestTableOne;

SELECT TOP 99 Name, SmallText, CreationDate
FROM TestTableOne;

PRINT 'stop';
