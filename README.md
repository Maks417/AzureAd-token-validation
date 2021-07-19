# AzureAd-token-validation
 
Console application to help to retrieve and validate Bearer token with Microsoft Identity Platform (formerly Azure AD).

Follow this guide to look into the whole process: https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-client-creds-grant-flow.

The application requires the next settings from Azure Portal:

* Tenant ID
* Client ID
* Scope (Application ID URI, w/o "/.default" postfix) 
* Secret (value, not id)

![image](https://user-images.githubusercontent.com/10028262/126187513-9cbe956c-941a-4cdf-b9d2-1280f1236768.png)
