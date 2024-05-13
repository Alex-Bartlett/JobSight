[Go back](../README.md)

# Setup

The app requires use of a cloud bucket storage solution to store images. For the sake of development, Supabase is used. A PostgreSQL database is also required and can be set up individually, but for simplicity, a Supabase PostgreSQL database is used in this guide.

Supabase is open source, and can be self-hosted. Alternatively, a free-tier project can be created at [supabase.com](http://supabase.com), which is more than suitable for a non-production environment for this application.

Currently, the PostgreSQL connection string needs to be set in two separate configs. The Supabase API details only need to be set in one.

1. Upon creating a Supabase project, using the user interface, go to project settings. Go to Database, then select the .NET connection string. Copy this, and in a text editor, replace the password with your Supabase password.

![Supabase Setup Guide](https://alexbartlett.com/images/jobsight/supabase1.png)

2. Duplicate the *example_appsettings.json* files and name them *appsettings.json*.

	From the repository directory:

	Windows:
	```
	copy Infrastructure\example_appsettings.json Infrastructure\appsettings.json
	copy ManagementApp\example_appsettings.json ManagementApp\appsettings.json
	```

	Linux/Mac:
	```
	cp Infrastructure/example_appsettings.json Infrastructure/appsettings.json
	cp ManagementApp/example_appsettings.json ManagementApp/appsettings.json
	```

3. Open both files in a text editor and replace the ConnectionStrings.Development string with the connection string copied in step 1.
	```
	"ConnectionStrings": {
		"Development": "<<connection string here>>"
	},
	```
	You can now save and close Infrastructure\appsettings.json, but keep ManagementApp\appsettings.json open.

4. Return to the Supabase user interface, select API, then click reveal then copy on the service_role API key.

![Supabase Setup Guide](https://alexbartlett.com/images/jobsight/supabase2.png)

5. Replace the Supabase.Key string with the API key.

6. Copy the Supabase Project URL from the same page, and replace the Supabase.Url string with it.

7. Save and close the file.

8. In the terminal, cd to the JobSight/Infrastructure folder, then run the following command:
	```
	dotnet ef database update
	```

9. Return to the Supabase user interface, select Storage on the left panel, then create a new bucket called *task-images*.
![Supabase Setup Guide](https://alexbartlett.com/images/jobsight/supabase3.png)

10. In a terminal, cd to the JobSight/ManagementApp directory and run the following command:
	```
	dotnet run
	```
	You can now open the application in a browser, at the address shown in the terminal (http://localhost:5161/ by default).

11. Register an account, following the on-screen steps to verify the email address. Then, log in to the new account.

12. Since company creation and account tiers aren't implemented yet (only the JobSight host should be able to create these), you need to manually create a tier and company. Run the following query on the database:
	```postgres
	INSERT INTO "AccountTiers" ("Id", "Name") VALUES (1, 'Test Tier');

	INSERT INTO
	"Companies" ("Id", "Name", "Email", "ContactNumber", "AccountTierId")
	VALUES
	(
		1,
		'Test Company',
		'testcompany@example.com',
		'123-456-7890',
		1
	);
	```

13. Invites are also not implemented yet, so the user needs to be manually added to a company. (Users can belong to multiple companies). 
	Navigate to the Developer panel in JobSight, and copy the user id.

	Edit the following query with the user id, and run on the database:
	```postgres
		INSERT INTO "UserCompanies" ("UserId", "CompanyId") 
		VALUES (
			'User Id goes here', 
			1
		);
	```

	![JobSight Setup Guide](https://alexbartlett.com/images/jobsight/jobsight1.png)

14. Finally, select the new company in the current company drop down, and click save. There is no visual confirmation, but clicking save should submit the changes.

You can now begin using the application. To create further users/companies, repeat steps 10-14 respectively.

## Switching from Management to Site app

Currently, user roles are not properly integrated, and as such there is no control to change a user from an adminstrator to a site worker. This can be done manually with a database query:
```postgres
UPDATE "Users"
SET
  "IsSiteWorker" = true
WHERE
  "Id" = 'user id goes here';
```
Steps to retrieve the current user Id can be found on step 13 of [Setup](#setup).

## Image Upload Config

Image upload parameters can be configured in ManagementApp/appsettings.json, under the "ImageUploadConfig" section. Image processing logic currently resides in Infrastructure/ImageBucketConnector.cs.

**MaxSizeInKb** - The maximum file size that can be uploaded (note that image processing happens locally, so it will be stored in machine memory temporarily)

**AllowedExtensions** - The allowed filetypes to be uploaded.

**UrlExpirationInMinutes** - The minutes until the bucket's URLs should expire. A longer expiration requires less frequent URL generations, but is considered more insecure.

[Go back](../README.md)