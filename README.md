Open Data Platform Installer (ODPI)
===================================

About
-----

The Open Data Platform Installer (ODPI) will allow even non-technical folks to get a customized collection of Open Data applications up and running on Windows Azure by filling out a few details.


How to deploy ODPI in Azure
---------------------------

###Storage account configuration
	1. Download the [ODAF Blob Files](http://frogdidata.blob.core.windows.net/public/OGDI_DataLoader.zip) archive.
	2. If you don't have an Azure Storage account yet, create one.
	3. In your Azure Storage account, create a container named "components" in the blob storage.
	4. Uncompress the zip archive downloaded step 1 and upload all the files to the components container of your blob storage.

###Solution configuration
	1. Open Visual Studio 2012 with administrator privileges.
	2. Under ODPI.Cloud/Roles, open the ODPI role and go to the settings tab.
	3. Now replace [StorageName] and [StorageKey] by your the credentials of your Azure Storage account containing your blob files.
	4. You can now "package" and deploy your ODPI solution to Azure Cloud Services.
