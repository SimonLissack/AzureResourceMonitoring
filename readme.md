# Azure Resource Monitoring

## Pre-ramble

The purpose of this project is to gather a list of active Azure resources for the purposes of monitoring. It's a bit of a work in progress though...

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
