The following project has four api routes. These include:

[POST]  /api/vin/upload
[PUT]   /api/vin/augment
[GET]   /api/vehicle/{vin}
[GET]   /api/vehicle


[POST]  /api/vin/upload

The first operation that can be completed by the user is use the vin/upload route. This route can support uploading up to three csv files at a time. The format of the csv file is:

integer            dealerId,
string (char(17))  vin,
date               modifiedDate,

The route parses and ensures the columns have correct headers and values. After validation the file is archived on an AWS S3 bucket or local disk (This can be changed in program.cs by injecting the DiskFileStorage class as opposed to the S3FileStorage class). After archiving the file data is parsed into and object array and stored on a MongoDB Server.


[PUT]   /api/vin/augment

This route can only be completed after the database has some values. No variables are required to pass to this route. The route invokes a service that calls the NHSTA api to collect more information on the vehicle based on the vin. This data is updated in the database.


[GET]   /api/vehicle/{vin}

Gets vehicle inofrmation based on the VIN. If the augment route has not been called the service that is invoked calls the NHSTA api. It gets, updates the record, and returns it to the consumer.

[GET]   /api/vehicle

Gets vins, dealerIds and modified date of records in the database. If the augment route has not been called the service that is invoked calls the NHSTA api with the api buk routes. It gets, updates the record, and returns it to the consumer. This route has filters for what record to start at and how many to return. DealerId and after modified date are also filterable.

Setup:

To get the project running locally you must create a connection string in appsettings.json called MongoDB. Set the value to a valid mongo database.

You can also create an S3 appsetting with the following sub values: 

Bucket | 
AccessKeyId |
Secret

If an S3 Bucket is not available update the program.cs file to inject the DiskFileStorage class as opposed to the S3FileStorage class. This will save an archived record in the WebApi/Archive folder.

An API Key is required in appsettings.json the key is ApiKey and can have any value.