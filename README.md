# Escc.SupportWithConfidence

Find providers of social care services approved by Trading Standards. Provider data are imported from a separate system, but extra metadata may be added here.

## Extract, transform and load process

Date is exported from the source system to CSV files on a nightly basis. `Escc.SupportWithConfidence.ETL` is designed to run after that happens, and to read the data from the CSV files into the Support With Confidence website database.

The `Controller`, when instantiated, prepares data for providers. Preparing data for providers involves reading the raw provider data from the CSV into a temporary `import` table, with an id assigned to each one, then judging whether the import is valid and, if so, replacing all of the existing provider data with the new data. 

Once all the data is written to the database a 'post load' SQL operation is run which sets categories that are in use or have child categories in use to 'active'. It also ensures there's a record in the 'ProviderExtra' table for each provider.

## SQL Server data access

Data is mostly managed using stored procedures, but managing add/update/delete for categories and accreditations uses Entity Framework as that was added later. The database should have two users created using SQL authentication, matching the `SupportWithConfidenceUserRole` and `SupportWithConfidenceAdminRole` database roles. These database roles should be granted EXECUTE permission on stored procedures for the older data access, whilst the admin user should also be in the `db_ddladmin`, `db_datareader` and `db_datawriter` roles for the Entity Framework code. 

It may be appropriate to migrate existing data access from stored procedures to Entity Framework in future.