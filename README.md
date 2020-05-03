Welcome to CodeMeasurement!
===================

CodeMeasurement is a tool for that can support software development by providing specific code quality metrics.


Team members
-------------
 - Anastazja Reinisch
 - Kacper Maciejewski
 - Paweł Myszkowski
 - Sebastian Orwat

Tools needed to develop the application
-------------
 - Database - [SQL Server][3]
 - Backend and Frontend - [Microsoft Visual Studio][2]
 - Mockups design - [Figma][1]
 
Configure database 
-------------
 - For case of this project, we used free version of SQL Database offered by Azure. In order to connect to it ADO.NET library with those parameters is used:
 
	Server=tcp:codemeasurement.database.windows.net,1433;
	Initial Catalog=codemeasurement;
	Persist Security Info=False;
	User ID={your_user_id};
	Password={your_password};
	MultipleActiveResultSets=False;
	Encrypt=True;
	TrustServerCertificate=False;
	Connection Timeout=30;
 
## how to run tests

How to deploy the application to production
-------------
The application will be deployed on Azure Apps Service, when it comes to CI/CD it uses the following configuration: CI with GitHub (production branch) and App Service build service.

[1]: https://api.figma.com
[2]: https://visualstudio.microsoft.com/pl/
[3]: https://docs.microsoft.com/en-us/azure/sql-database/sql-database-single-database
