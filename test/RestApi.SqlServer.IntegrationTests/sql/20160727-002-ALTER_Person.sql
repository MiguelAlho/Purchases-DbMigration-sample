/* We have decided to alter how names are stored in the system. Customers want 
seperated name components to make reporting and mailing easier... */

ALTER TABLE Person ADD FirstName varchar(100) NOT NULL DEFAULT '';
ALTER TABLE Person ADD LastName varchar(100) NOT NULL DEFAULT '';

GO

--migrate data to new columns
UPDATE PERSON
SET 
	FirstName = SUBSTRING(Name, 1, CHARINDEX(' ', Name) - 1),
	LastName = REVERSE(SUBSTRING(REVERSE(Name), 1, CHARINDEX(' ', REVERSE(Name)) - 1));

GO

--kill old column
ALTER TABLE Person DROP COLUMN Name;

