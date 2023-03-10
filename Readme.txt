
REST API Etteplan

The API is implemented for the maintenance tasks associated with each device. All CRUD operations 
are implemented. Each maintenance task is associated with a device. A task cannot be added 
without binding it to a factory device. 

For adding a new task, description, severity, status and the associated deviceId are mandatory 
parameters in the body of https POST request.

PUT request can also update the task association with device

Git
https://github.com/saadbakhshi/Database.git
https://github.com/saadbakhshi/EtteplanAPI.git

Follow the following steps for execution of API:

	1. Execute the attached script in SQL server. The scripts are multi execution secure and it creates tables and runs a DML to insert data into these tables
	2. Clone the attached project file
	3. Update the connection string in appsettings.json
	4. Etteplan Rest API.postman_collection is the postman collection, import this collection to postman.
	5. Run the solution
	6. In the collection, update the https request with the URI of the machine where the solution is running
	7. Https requests for PUT and POST have the json object in the body, update these objects as per your test cases
	8. Send requests
	