# Based on https://medium.com/@granthair5/how-to-add-and-use-user-secrets-to-a-net-core-console-app-a0f169a8713f
param (
    [Parameter(Mandatory=$true)]
    [string]$SecretUri
)

dotnet user-secrets set KeyVault:SecretUri $SecretUri
