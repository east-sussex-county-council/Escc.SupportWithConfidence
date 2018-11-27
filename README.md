# Escc.SupportWithConfidence

Find providers of social care services approved by Trading Standards. Provider data are imported from a separate system, but extra metadata may be added here.

## SQL Server data access

Data is mostly managed using stored procedures, but managing add/update/delete for accreditations uses Entity Framework as that was added later. The database should have two users created using SQL authentication, matching the `SupportWithConfidenceUserRole` and `SupportWithConfidenceAdminRole` database roles. These database roles should be granted EXECUTE permission on stored procedures for the older data access, whilst the admin user should also be in the `db_ddladmin`, `db_datareader` and `db_datawriter` roles for the Entity Framework code. 

It may be appropriate to migrate existing data access from stored procedures to Entity Framework in future.