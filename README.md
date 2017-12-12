# Sitecore Release Notes

This is the source code for the site running on https://sitecorereleasenotes.com.

The site indexes the release notes of Sitecore releases from https://dev.sitecore.net and allows you
to easily search through them for issue numbers etc. to see if something has been fixed/changed and when.

> **NOTE**: This is currently not much more than a simple proof-of-concept site and 
> could use a lot of refactoring and more features

## Azure services

The site runs completely in Azure, using the following Azure services:

- Functions (Scheduled crawling/indexing)
- Search
- Web App
- Application Insights

Most of the services can use the free tiers or are very cheap to run.