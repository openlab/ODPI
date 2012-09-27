Open Data Platform Installer (ODPI)
==================================================

The Open Data Platform Installer (ODPI) will allow even non-technical folks to get
a customized collection of Open Data applications up and running on Windows Azure
by filling out a few details.

How to deploy ODPI on Azure
---------------

###Storage account configuration
	Create a storage account to store blob files.
		Then create a container named "components" in the blob storage
		Once the container is created, Upload to the blob storage files stored in "/blob files"

###Solution configuration
	1. Open Visual Studio with Administrator privileges
	2. Under ODI.Azure/Roles
		(A) Open ODI then click Settings
		(B) Replace values with you storage account name and key
			
	3. Package or publish the application