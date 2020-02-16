# Azure Resource Monitoring

This project uses a Service Prinicipal stored in Key Vault to list the resources visible to said Service Principal.

## Pre-ramble

This is mainly for testing and playing around with new features in .net and Azure cloud. Specifically the aim is to handle configuration of authentication to Azure without having any secrets in configuration, stored out of the project or otherwise.

## Prerequisites;

In Azure Key Vault add a secret with the service principal information  following this json structure:

```
{
    "clientId": "<client-id>",
    "clientSecret": "<client-secret>",
    "subscriptionId": "<subscription-id>",
    "tenantId": "<tenant-id>"
}
```

Get the URI of the resource (for example `https://example-vault.vault.azure.net/secrets/service-principal-secret/`) and in PowerShell run `.\AzureResourceMonitoring\Create-UserSecrets.ps1 -SecretUri $secretUri` where `$secretUri` is the URI for the service principal secret. This will save it to your user secret store. If the key location changes, rerun the script with the new URI.
