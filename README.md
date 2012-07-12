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
	2. Under ODI/Services
		(A) Open the CertificateBuilder.cs file.
			Replace [storageName] by your storage name in var url = "http://[storagename].blob.core.windows.net/components/";
			Remove the line stating with #error
		(B) Do the same in PackageBuilder.cs file
		(C) Open the CloudBackedStore.cs file
			Find var acc = "DefaultEndpointsProtocol=https;AccountName=[storagename];AccountKey=[storagekey]"
			Replace [storagename] and [storagekey] respectively by the storage name of your account and its primary access key
			Remove the line stating with #error
			
	3. Package or publish the application