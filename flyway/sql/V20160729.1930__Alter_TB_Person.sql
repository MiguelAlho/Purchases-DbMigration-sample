-- USE [PortoData]
-- GO

ALTER TABLE Person ADD FirstName varchar(100);
ALTER TABLE Person ADD LastName varchar(100);


--data
UPDATE dbo.Person
SET 
	FirstName = SUBSTRING(Name, 1, CHARINDEX(' ', Name) - 1),
	LastName = SUBSTRING(name, CHARINDEX(' ', name)+1, LEN(name)-(CHARINDEX(' ', name)-1))

GO

--kill old column
ALTER TABLE Person DROP COLUMN Name;


